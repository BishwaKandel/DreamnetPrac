$(document).ready(function () {

    function showLoadingSpinner() {
        $('#loading-container').fadeIn()
       /* document.getElementById("loading-container").style.display = "block";*/
    }

    function hideLoadingSpinner() {
        document.getElementById("loading-container").style.display = "none";
    }

    $('#EmployeeTable').DataTable(
        {
            responsive: true

        });
    $('#DeptTable').DataTable(
        {
            responsive: true

        });
    $('#admninAttendanceTable').DataTable(
        {
            responsive: true

        });


    $(document).on('click', '#saveEmployeesBtn', function () {
        var deptId = $("#departmentId").val();
        var employeeIds = $("#empSelect").val(); // array of selected IDs

        console.log("Clicked Save! Dept:", deptId, "Employee IDs:", employeeIds); // DEBUG
        $.ajax({
            url: '/Admin/Department/AddEmployeesToDepartment',
            type: 'POST',
            traditional: true,
            dataType: 'json',
            data: {
                departmentId: deptId,
                employeeIds: employeeIds
            },
            success: function (response) {
                Swal.fire("Success", response.message, "success");

                // Properly close modal
                var modalEl = document.getElementById("AddModal");
                var modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();

            },
            error: function (xhr) {
                console.error(xhr.responseText);
                Swal.fire("Error", "Something went wrong.", "error");
            }
        });
    });


    $("#JoiningDate").flatpickr({
        dateFormat: "Y-m-d",  // Format of the selected date
        allowInput: true      // Allow the user to type the date manually
    });

    $('#AdminEditForm').validate({
        rules: {
            Name: {
                required: true,
                minlength: 3
            },
            UserName: {
                required: true,
                minlength: 3
            },
            Email: {
                required: true,
                email: true
            },
            PhoneNumber: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            DOB: {
                required: true,
                date: true
            },
            JoiningDate: {
                required: true,
                date: true
            },
            Position: {
                required: true
            },
            Salary: {
                required: true,
                number: true,
                min: 1000
            },
            Address: {
                required: true,
                minlength: 5
            },
            Password: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#yourPassword"
            }
        },
        messages: {
            Name: {
                required: "Name is required.",
                minlength: "Name should be at least 3 characters."
            },
            Email: {
                required: "Email is required.",
                email: "Please enter a valid email address."
            },
            PhoneNumber: {
                required: "Phone number is required.",
                digits: "Phone number must contain only digits.",
                minlength: "Phone number must be 10 digits.",
                maxlength: "Phone number must be 10 digits."
            },
            DOB: {
                required: "Date of birth is required.",
                date: "Please enter a valid date."
            },
            JoiningDate: {
                required: "Joining date is required.",
                date: "Please enter a valid date."
            },
            Position: {
                required: "Position is required."
            },
            Salary: {
                required: "Salary is required.",
                number: "Please enter a valid number.",
                min: "Salary must be at least 1000."
            },
            Address: {
                required: "Address is required.",
                minlength: "Address must be at least 5 characters long."
            }
        },
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        errorPlacement: function (error, element) {
            error.addClass('text-danger');
            error.insertAfter(element);
        },
        submitHandler: function (form) {
            // Determine the action URL dynamically
            var actionUrl = $(form).attr('action'); // This will be "/Admin/Dashboard/Create" or "/Admin/Dashboard/Edit"
            showLoadingSpinner();
            $.ajax({
                type: 'POST',
                url: actionUrl,
                data: $(form).serialize(),
                success: function (response) {
                    hideLoadingSpinner();
                    if (response && response.success) {
                        
                        Swal.fire({
                            title: "Success!",
                            text: response.message,
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function (result) {
                            // Check if the user clicked "OK"
                            if (result.isConfirmed) {
                                // Redirect to dashboard after clicking "OK"
                                window.location.href = '/Admin/Dashboard/Index';
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Failed!',
                            text: response ? response.message : 'Please try again.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    hideLoadingSpinner();
                    console.error('Error occurred:', error);
                    Swal.fire({
                        title: 'Error!',
                        text: 'An unexpected error occurred.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });
        }
    });


    //Mark active Button
    $(document).on("click", ".activeBtn", function () {
        var userId = $(this).data("id");
       
        $.ajax({
            success: function (response) {
                Swal.fire({
                    title: "Are you sure?",
                    //text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes !"
                }).then((result) => {
                    if (result.isConfirmed) {
                        showLoadingSpinner();
                        $.ajax({
                            type: 'POST',
                            url: '/admin/Dashboard/ChangeStatus',
                            data: { id : userId },
                            success: function (response) {
                                hideLoadingSpinner();
                                Swal.fire({
                                    title: "Success!",
                                    text: response.message,
                                    icon: "success",
                                    confirmButtonText: "OK"
                                }).then(function () {
                                    window.location.href = '/Admin/Dashboard/Index';
                                });
                            },
                            error: function (xhr, status, error) {
                                console.error('Error occurred ', error);
                                hideLoadingSpinner();
                                Swal.fire({
                                    title: 'Error!',
                                    text: 'An unexpected error occurred',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    } else {
                        window.location.href = '/Admin/Dashboard/Index';
                    }
                });
            },
            error: function (xhr, status, error) {
                hideLoadingSpinner();
                console.error('Error occurred while fetching leave data:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'An unexpected error occurred.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });


    //delete employee button
    $(document).on("click", ".deleteEmpBtn", function () {
        var userId = $(this).data("id");
        $.ajax({
            success: function (response) {
                Swal.fire({
                    title: "Are you sure?",
                    //text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#d33",
                    cancelButtonColor: "#3085d6",
                    confirmButtonText: "Yes !"
                }).then((result) => {
                    if (result.isConfirmed) {
                        showLoadingSpinner();
                        $.ajax({
                            type: 'POST',
                            url: '/admin/Dashboard/Delete',
                            data: { id: userId },
                            success: function (response) {
                                hideLoadingSpinner();

                                Swal.fire({
                                    title: "Success!",
                                    text: response.message,
                                    icon: "success",
                                    confirmButtonText: "OK"
                                }).then(function () {
                                    window.location.href = '/Admin/Dashboard/Index';
                                });
                            },
                            error: function (xhr, status, error) {
                                console.error('Error occurred :', error);
                                hideLoadingSpinner();

                                Swal.fire({
                                    title: 'Error!',
                                    text: 'An unexpected error occurred.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    } else {
                        hideLoadingSpinner();

                        window.location.href = '/Admin/Dashboard/Index';

                    }
                });
            },
            error: function (xhr, status, error) {
                hideLoadingSpinner();
                console.error('Error occurred while fetching leave data:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'An unexpected error occurred.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    $(document).on("click", ".deleteDeptBtn", function () {
        var deptId = $(this).data("id");
        $.ajax({
            success: function (response) {
                Swal.fire({
                    title: "Are you sure?",
                    //text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#d33",
                    cancelButtonColor: "#3085d6",
                    confirmButtonText: "Yes !"
                }).then((result) => {
                    if (result.isConfirmed) {
                        showLoadingSpinner();
                        $.ajax({
                            type: 'POST',
                            url: '/admin/Department/Delete',
                            data: { deptId: deptId },
                            success: function (response) {
                                hideLoadingSpinner();
                                Swal.fire({
                                    title: "Success!",
                                    text: response.message,
                                    icon: "success",
                                    confirmButtonText: "OK"
                                }).then(function () {
                                    window.location.href = '/Admin/Department/Index';
                                });
                            },
                            error: function (xhr, status, error) {
                                hideLoadingSpinner();
                                console.error('Error occurred :', error);
                                Swal.fire({
                                    title: 'Error!',
                                    text: 'An unexpected error occurred.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    } else {
                        window.location.href = '/Admin/Department/Index';            
                    }
                });
            },
            error: function (xhr, status, error) {
                hideLoadingSpinner();
                console.error('Error occurred while fetching leave data:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'An unexpected error occurred.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });


    //$(document).on("click", "#AddButton", function () {
    //    var deptId = $(this).data("id"); // Get deptId from button
    //    console.log("Clicked Dept ID:", deptId);
    //    $.ajax({
    //        url: '/Admin/Department/GetEmployeesByDepartmentId',
    //        type: 'GET',
    //        data: { id: deptId },
    //        success: function (response) {
    //            $('#Add').remove();
    //            $("body").append(response);
    //            var modal = new bootstrap.Modal(document.getElementById('Add'), {
    //            });

    //            $('.empSelect').select2({
    //                placeholder: "Select employees",
    //                allowClear: true, width: '100%',
    //                dropdownParent: $("#Add")
    //            });
    //        }, error: function (xhr, status, error)
    //        {
    //            console.error("Error loading employees:", error);
    //            alert("Could not load employees for this department.");
    //        }
    //    });
    //});

    AddButton = function (deptId) {
        $.ajax({
            url: '/Admin/Department/GetEmpExceptDeptId',
            type: 'GET',
            data: { id: deptId },
            success: function (response) {
                $('#AddModal').remove();
                $("body").append(response);
                $('#AddModal').modal('toggle');

                $('.empSelect').select2({
                    placeholder: "Select employees",
                    allowClear: true, width: '100%',
                    dropdownParent: $("#AddModal")
                });
            }, error: function (xhr, status, error) {
                console.error("Error loading employees:", error);
                alert("Could not load employees for this department.");
            }
        });
    }


    // Department Edit Form
    //$('#DeptEditForm123').on('submit', function (event) {
    //    event.preventDefault(); // Prevent normal form submission
    //    var isValid = $(this).valid();  // Check if the form is valid using jQuery Validation
    //    if (!isValid) {
    //        return; // If the form is invalid, do not submit the form
    //    }

    //    // Determine the action URL dynamically
    //    var actionUrl = $(this).attr('action'); // This will be "/Admin/Dashboard/Create" or "/Admin/Dashboard/Edit"

    //    $.ajax({
    //        type: 'POST',
    //        url: actionUrl,
    //        data: $(this).serialize(),
    //        success: function (response) {
    //            if (response.success) {
    //                Swal.fire({
    //                    title: "Success!",
    //                    text: response.message, // Display the message from the response
    //                    icon: "success",
    //                    confirmButtonText: "OK"
    //                }).then(function (result) {
    //                    if (result.isConfirmed) {
    //                        // Redirect to department index page after clicking OK
    //                        window.location.href = '/Admin/Department/Index';
    //                    }
    //                });
    //            } else {
    //                Swal.fire({
    //                    title: 'Failed!',
    //                    text: response.message || 'Please try again.',
    //                    icon: 'error',
    //                    confirmButtonText: 'OK'
    //                });
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('Error occurred:', error);
    //            Swal.fire({
    //                title: 'Error!',
    //                text: 'An unexpected error occurred.',
    //                icon: 'error',
    //                confirmButtonText: 'OK'
    //            });
    //        }
    //    });
    //});




    //Department Form Validation
    $("#DeptEditForm").validate({
        rules: {
            Name: {
                required: true,
                minlength: 5
            },
            Location: {
                required: true,
                minlength: 5
            },
            Description: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            Name: {
                required: "Please enter a department name",
                minlength: "Name must be at least 5 characters long"
            },
            Location: {
                required: "Please enter the location",
                minlength: "Location must be at least 5 characters long"
            },
            Description: {
                required: "Please enter a description",
                minlength: "Description must be at least 5 characters long"
            }
        },
        errorClass: "text-danger",
        errorPlacement: function (error, element) {
            error.insertAfter(element); // place error below each input
        },
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            // Optional: You can do additional custom validation here before submission
            /*event.preventDefault();*/ // Prevent normal form submission
            //var isValid = $(this).valid();  // Check if the form is valid using jQuery Validation
            //if (!isValid) {
            //    return; // If the form is invalid, do not submit the form
            //}

            // Determine the action URL dynamically
            var actionUrl = $(form).attr('action'); // This will be "/Admin/Dashboard/Create" or "/Admin/Dashboard/Edit"
            showLoadingSpinner();
            $.ajax({
                type: 'POST',
                url: actionUrl,
                data: $(form).serialize(),
                success: function (response) {
                    hideLoadingSpinner();
                    if (response.success) {
                        Swal.fire({
                            title: "Success!",
                            text: response.message, // Display the message from the response
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function (result) {
                            if (result.isConfirmed) {
                                // Redirect to department index page after clicking OK
                                window.location.href = '/Admin/Department/Index';
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Failed!',
                            text: response.message || 'Please try again.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error occurred:', error);
                    hideLoadingSpinner();
                    Swal.fire({
                        title: 'Error!',
                        text: 'An unexpected error occurred.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });
        }
    });

    

    $(document).on("click", "#logoutBtn", function (e) {
        showLoadingSpinner();
        $.ajax({
            type: "POST",            
            url: "/Auth/Logout",     
            success: function (response) {
                hideLoadingSpinner();
                Swal.fire({
                    title: "Logged out!",
                    icon: "success",
                    confirmButtonText: "OK"
                }).then(function () {
                    window.location.href = '/Auth/login';  
                });
            },
            error: function (xhr, status, error) {
                console.error("Error occurred during logout:", error);
                hideLoadingSpinner();
                Swal.fire({
                    title: "Error!",
                    text: "An unexpected error occurred during logout.",
                    icon: "error",
                    confirmButtonText: "OK"
                });
            }
        });
    });





});

