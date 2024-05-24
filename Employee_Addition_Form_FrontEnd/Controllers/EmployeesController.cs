using Employee_Addition_Form_FrontEnd.Models;
using Employee_Addition_Form_FrontEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Employee_Addition_Form_FrontEnd.Consts;
using System.Collections.Generic;
using Employee_Addition_Form_FrontEnd.Filters;
using Employee_Addition_Form_FrontEnd.Enums;

namespace Employee_Addition_Form_FrontEnd.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly HttpClient _httpClient;

		public EmployeesController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var list = new List<Employee>();
			var result = await _httpClient.GetAsync(ConstValues.Url + "GetAll");
			Debug.WriteLine("======Status Code========" + result.StatusCode);

			if (!result.IsSuccessStatusCode)
			{
				return View(list);
			}
			Debug.WriteLine("==============" + result.Content.ToString());
			var responseBody = await result.Content.ReadAsStringAsync();
			var response = JsonSerializer.Deserialize<successResponseViewModel<IEnumerable<Employee>>>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			return View(response.Data);
		}

		[HttpGet]
		[AjaxOnly]
		public IActionResult Create()
		{
			return PartialView("_EmployeeForm");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeViewModel viewModel)
		{

			if (!ModelState.IsValid)
				return BadRequest();

			var content = new StringContent(JsonSerializer.Serialize(viewModel), Encoding.UTF8, "application/json");

			var response = await _httpClient
				.PostAsync(ConstValues.Url + "Create", content);

			if (!response.IsSuccessStatusCode)
				return BadRequest();


			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<successResponseViewModel<Employee>>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			var createdEmployee = new Employee
			{
				Id = result.Data.Id,
				Name = result.Data.Name,
				JobRole = result.Data.JobRole,
				Gender = result.Data.Gender,
				StartDate = result.Data.StartDate,
				Notes = result.Data.Notes
			};

			return PartialView("_EmployeeRow", createdEmployee);
		}

		[HttpGet]
		[AjaxOnly]
		public async Task<IActionResult> Edit(int id)
		{
			var response = await _httpClient
				.GetAsync(ConstValues.Url + "GetById/" + id);

			var responseBody = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
				return BadRequest();

			var result = JsonSerializer.Deserialize<successResponseViewModel<Employee>>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			if (result.Data is null)
				return NotFound();

			var Role = (JobRole)Enum.Parse(typeof(JobRole), result.Data.JobRole);
			var Gender = (Gender)Enum.Parse(typeof(Gender), result.Data.Gender);

			var viewModel = new EmployeeViewModel
			{
				Id = result.Data.Id,
				Name = result.Data.Name,
				JobRole = (int)Role,
				Gender = (int)Gender,
				StartDate = result.Data.StartDate,
				Notes = result.Data.Notes
			};

			return PartialView("_EmployeeForm", viewModel);
		}

		[HttpPost()]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
		{

			if (!ModelState.IsValid)
				return BadRequest();
			var employee = new
			{
				Name = viewModel.Name,
				JobRole = viewModel.JobRole,
				Gender = viewModel.Gender,
				StartDate = viewModel.StartDate,
				Notes = viewModel.Notes
			};

			var content = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

			var response = await _httpClient
				.PostAsync(ConstValues.Url + "Update/?id="+viewModel.Id, content);

			if (!response.IsSuccessStatusCode)
				return BadRequest();


			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<successResponseViewModel<Employee>>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			var updatedEmployee = new Employee
			{
				Id = result.Data.Id,
				Name = result.Data.Name,
				JobRole = result.Data.JobRole,
				Gender = result.Data.Gender,
				StartDate = result.Data.StartDate,
				Notes = result.Data.Notes
			};

			return PartialView("_EmployeeRow", updatedEmployee);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var response = await _httpClient
				.PostAsync(ConstValues.Url + "Delete?id="+id,null);

			if (!response.IsSuccessStatusCode)
				return BadRequest();

			return Ok();
		}
	}
}
