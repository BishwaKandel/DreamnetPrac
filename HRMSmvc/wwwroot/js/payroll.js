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


    $('#genpayrollForm').validate({
        rules: {
            Year: {
                required: true,
                digits: true,
                validYear: true
            },
            Month: {
                required: true,
                digits: true,
                validMonth: true
            },
            BasicSalary: {
                required: true,
                number: true,
                min: 0,
                validNetSalary: true // apply custom net salary rule
            },
            Allowances: {
                required: true,
                number: true,
                min: 0,
                validNetSalary: true // apply custom net salary rule
            },
            Deductions: {
                required: true,
                number: true,
                min: 0,
                validNetSalary: true // apply custom net salary rule
            }
        },
        messages: {
            Year: {
                required: "Please enter the year.",
                digits: "Year must contain only digits.",
                validYear: "Please enter a valid year (e.g., 2025)."
            },
            Month: {
                required: "Please enter the month.",
                digits: "Month must contain only digits.",
                validMonth: "Month must be between 1 and 12."
            },
            BasicSalary: {
                required: "Please enter the basic salary.",
                number: "Basic salary must be a valid number.",
                min: "Basic salary cannot be negative."
            },
            Allowances: {
                required: "Please enter allowances.",
                number: "Allowances must be a valid number.",
                min: "Allowances cannot be negative."
            },
            Deductions: {
                required: "Please enter deductions.",
                number: "Deductions must be a valid number.",
                min: "Deductions cannot be negative."
            }
        },
        errorClass: "is-invalid",
        validClass: "is-valid",
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("invalid-feedback");
            if (element.parent(".input-group").length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            var id = $('#userId').val();

            $.ajax({
                type: 'POST',
                url: '/Admin/Payroll/GeneratePayroll',
                data: $(form).serialize(),
                success: function (response) {
                    console.log('Response:', response); // For debugging
                    if (response.success) {
                        // Show success message with SweetAlert
                        Swal.fire({
                            title: "Payroll Generated!",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then((result) => {
                            // If user clicks 'OK', redirect to login page
                            if (result.isConfirmed) {
                                window.location.href = '/Admin/Payroll/ViewPayroll?userId=' + id;
                            }
                        });
                    } else {
                        // If generation fails, show the error message from the server
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
        }
    });
    $.validator.addMethod("validYear", function (value, element) {
        return this.optional(element) || /^(19|20)\d{2}$/.test(value);
    }, "Please enter a valid 4-digit year.");

    // Custom method to validate month (1–12)
    $.validator.addMethod("validMonth", function (value, element) {
        var month = parseInt(value, 10);
        return this.optional(element) || (month >= 1 && month <= 12);
    }, "Please enter a valid month (1–12).");

    $.validator.addMethod("validNetSalary", function (value, element) {
        var basic = parseFloat($("#BasicSalary").val()) || 0;
        var allowance = parseFloat($("#Allowances").val()) || 0;
        var deduction = parseFloat($("#Deductions").val()) || 0;

        var net = basic + allowance - deduction;
        return net >= 0; // valid only if non-negative
    }, "Net Salary cannot be negative.");

});
