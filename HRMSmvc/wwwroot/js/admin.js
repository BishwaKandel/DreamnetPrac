$(document).ready(function () {

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
    
        $('#AdminEditForm').submit(function (event) {
            event.preventDefault(); // Prevent normal form submission
            var formData = new FormData(this);

            // Determine the action URL dynamically
            var actionUrl = $(this).attr('action'); // This will be "/Admin/Dashboard/Create" or "/Admin/Dashboard/Edit"

            $.ajax({
                type: 'POST',
                url: actionUrl,
                data: formData,               // Use FormData instead of serialize()
                processData: false,           // Required for FormData
                contentType: false,
                success: function (response) {
                    if (response && response.success) {
                        Swal.fire({
                            title: "Success!",
                            text: response.message,
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function () {
                            window.location.href = '/Admin/Dashboard/Index';
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
                    console.error('Error occurred:', error);
                    Swal.fire({
                        title: 'Error!',
                        text: 'An unexpected error occurred.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });
        });
        $(document).on("click", ".delete-btn", function () {
        var id = $(this).data("id");

        if (confirm("Are you sure you want to delete this Department?")) {
            $.ajax({
                type: 'POST',
                url: '@Url.Action("Department", "Delete")',
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        alert(response.message);
                        location.reload(); // refresh table
                    } else {
                        alert(response.message);
                    }
                },
                error: function () {
                    alert("Something went wrong!");
                }
            });
        }
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
    $('#DeptEditForm').submit(function (event) {
        event.preventDefault(); // Prevent normal form submission
        var formData = new FormData(this);

        // Determine the action URL dynamically
        var actionUrl = $(this).attr('action'); // This will be "/Admin/Dashboard/Create" or "/Admin/Dashboard/Edit"

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: formData,               // Use FormData instead of serialize()
            processData: false,           // Required for FormData
            contentType: false,
            success: function (response) {
                if (response && response.success) {
                    Swal.fire({
                        title: "Success!",
                        text: response.message,
                        icon: "success",
                        confirmButtonText: "OK"
                    }).then(function () {
                        window.location.href = '/Admin/Department/Index';
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
                console.error('Error occurred:', error);
                Swal.fire({
                    title: 'Error!',
                    text: 'An unexpected error occurred.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

});

