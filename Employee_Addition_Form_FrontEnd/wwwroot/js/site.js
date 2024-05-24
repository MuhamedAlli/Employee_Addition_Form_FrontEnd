var updatedRow;
var dataTable;
function showSuccessMessage(message = 'Saved Succefully') {
    Swal.fire({
        icon: "success",
        text: message
    });
}
function showErrorMessage(message = 'Sorry , Sothing went Wrong!') {;
    Swal.fire({
        icon: "error",
        text: 'Sorry , Sothing went Wrong!'
    });
}
function onModelBegin() {
    $('body :submit').attr('disabled', 'disabled');
}

function onModelSuccess(row) {
    showSuccessMessage();
    $('#myModal').modal('hide');
    if (updatedRow !== undefined) {
        dataTable.row(updatedRow).remove().draw();
        updatedRow = undefined
    }

    var newRow = $(row);
    dataTable.row.add(newRow).draw();
}

function onModelComplete() {
    $('body :submit').removeAttr('disabled');
}

$(document).ready(function () {
    //render datatables
	dataTable = $('#dataTable').DataTable({
		"paging": true,             // Enable pagination
		"searching": true,          // Enable search box
		"ordering": true,           // Enable sorting
		"info": true,               // Enable table information display
		"responsive": true,          // Make table responsive
	});

	//Render bootstrap Modal
    $('.js-render-model').on('click', function () {
        var btn = $(this);

        var mymodal = $('#myModal');

        mymodal.find('#ModalLabel').text(btn.data('title'));

        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }

        $.get({
            url: btn.data('url'),
            data: {
                '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (form) {
                mymodal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(mymodal);
                mymodal.modal('show');
            },
            error: function () {
                showErrorMessage();
            }
        });

    });

    //render Delete button
    $('body').delegate('.js-delete','click', function () {
        var btn = $(this);
        console.log(btn.data('url'));
        bootbox.confirm({
            message: 'Do you really want to delete ?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        success: function (result) {
                            console.log(result);
                            var deletedRow = btn.parents('tr');
                            deletedRow.addClass('animate__animated animate__slideOutRight');
                            dataTable.row(deletedRow).remove().draw();
                            showSuccessMessage();
                        },
                        error: function (error) {
                            console.log(error)
                            showErrorMessage();
                        }
                    });
                }
            }
        });

    });

});