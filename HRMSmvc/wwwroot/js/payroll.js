$(document).ready(function () {
    $('#PayrollTable').DataTable(
        {
        //    searching: false
        });

    var yearEl = $('#year');
    var monthEl = $('#month');

    //var table = $('#PayrollTable').DataTable();
    $('#PayrollTable_filter').hide()

    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        var selectedYear = parseInt(yearEl.val(), 10);
        var selectedMonth = parseInt(monthEl.val(), 10);

        var rowYear = parseInt(data[1], 10);
        var rowMonth = parseInt(data[2], 10);

        if (!isNaN(selectedYear) && !isNaN(selectedMonth)) {
            return rowYear === selectedYear && rowMonth === selectedMonth;
        }

        if (!isNaN(selectedYear) && isNaN(selectedMonth)) {
            return rowYear === selectedYear;
        }

        if (isNaN(selectedYear) && !isNaN(selectedMonth)) {
            return rowMonth === selectedMonth;
        }
        return true;
    });

    yearEl.on('input', function () {
        $('#PayrollTable').DataTable().draw();
    });

    monthEl.on('input', function () {
        $('#PayrollTable').DataTable().draw();
    });


    //Generate Payroll Form
    
    $('#genpayrollForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this);

        var id = $('#userId').val();

        $.ajax({
            type: 'POST',
            url: '/Admin/Payroll/GeneratePayroll',
            data: formData,               // Use FormData instead of serialize()
            processData: false,           // Required for FormData
            contentType: false,
            //dataType: 'json', // Expect JSON response
            success: function (response) {
                console.log('Response:', response); // For debugging
                if (response.success) {                                //Changes Data to LOGIN
                    // Show success message with SweetAlert
                    Swal.fire({
                        title: "Payroll Generated!",
                        icon: "success",
                        confirmButtonText: "OK",
                    }).then((result) => {
                        // If user clicks 'OK', redirect to login page
                        if (result.isConfirmed) {
                            window.location.href = '/Admin/Payroll/ViewPayroll?userId=' + id;
                        }
                    });
                } else {
                    // If registration fails, show the error message from the server
                    Swal.fire({
                        title: 'Generation Failed!',
                        text: response.message || 'Please try again.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error('Error occurred:', error);
                Swal.fire({
                    title: 'Update Failed!',
                    text: 'An unexpected error occurred. Please try again later.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });
    


});
