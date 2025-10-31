$(document).ready(function () {
    function showLoadingSpinner() {
        $('#loading-container').fadeIn()
        /* document.getElementById("loading-container").style.display = "block";*/
    }

    function hideLoadingSpinner() {
        document.getElementById("loading-container").style.display = "none";
    }


    $('#userLeaveTable').DataTable(
        {
            responsive: true
        });

    // Initialize End Date first, so it's ready for updating
    var endPicker = $("#EndDate").flatpickr({
        dateFormat: "Y-m-d",
        allowInput: true,
        minDate: "today"
    });

    // Initialize Start Date
    var startPicker = $("#StartDate").flatpickr({
        dateFormat: "Y-m-d",
        allowInput: true,
        minDate: "today",

        onChange: function (selectedDates, dateStr, instance) {
            // When start date changes, update the End Date minDate
            if (selectedDates.length > 0) {
                endPicker.set('minDate', dateStr);
            }
        }
    });

    $('#leavetable').DataTable(
        {
            responsive: true

        });



    //Leave form Validation

    $('#LeaveForm').validate({
        rules: {
            Reason: {
                required: true,
                minlength: 5
            },
            LeaveType: {
                required: true
            },
            StartDate: {
                required: true,
                date: true
            },
            EndDate: {
                required: true,
                date: true,
            },
            Description: {
                required: true,
                minlength: 10,
                maxlength: 500
            }
        },
        messages: {
            Reason: {
                required: "Please enter a reason for your leave.",
                minlength: "Reason must be at least 5 characters long."
            },
            LeaveType: {
                required: "Please select a leave type."
            },
            StartDate: {
                required: "Please select a start date.",
                date: "Please enter a valid date."
            },
            EndDate: {
                required: "Please select an end date.",
                date: "Please enter a valid date.",
            },
            Description: {
                required: "Please enter a brief description.",
                minlength: "Description must be at least 10 characters.",
                maxlength: "Description cannot exceed 500 characters."
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
            showLoadingSpinner();
            $.ajax({
                type: 'POST',
                url: '/client/Leave/LeaveApply',
                data: $(form).serialize(), // Use FormData instead of serialize()
                success: function (response) {
                    hideLoadingSpinner();
                    if (response && response.success) {
                        Swal.fire({
                            title: "Leave Applied Successfully!",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function () {
                            window.location.href = '/client/Leave/MyLeaves';
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



    $(document).on("click", ".ApproveBtn", function () {
        var leaveRequestId = $(this).data("id");

        // Confirm the action before proceeding with the AJAX request
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, approve it!"
        }).then((result) => {
            if (result.isConfirmed) {
                showLoadingSpinner();
                // Proceed with the approval AJAX call
                $.ajax({
                    type: 'POST',
                    url: '/admin/Leave/ApproveLeave',
                    data: { leaveRequestId: leaveRequestId },
                    success: function (approveResponse) {
                        hideLoadingSpinner();
                        Swal.fire({
                            title: "Success!",
                            text: approveResponse.message,
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function () {
                            window.location.href = '/Admin/Leave/ViewAllLeaves';
                        });
                    },
                    error: function (xhr, status, error) {
                        hideLoadingSpinner();
                        console.error('Error occurred while approving leave:', error);
                        Swal.fire({
                            title: 'Error!',
                            text: 'An unexpected error occurred while approving leave.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            }
        }).catch((err) => {
            console.error("Error in Swal:", err);
        });
    });

    ////Reject Leave
    //$(document).on("click", ".RejectBtn", function () {
    //    var leaveRequestId = $(this).data("id");

    //    $.ajax({
    //        success: function (response) {
    //            Swal.fire({
    //                title: "Are you sure?",
    //                text: "You won't be able to revert this!",
    //                icon: "warning",
    //                showCancelButton: true,
    //                confirmButtonColor: "#3085d6",
    //                cancelButtonColor: "#d33",
    //                confirmButtonText: "Yes, reject it!"
    //            }).then((result) => {
    //                if (result.isConfirmed) {
    //                    $.ajax({
    //                        type: 'POST',
    //                        url: '/admin/Leave/RejectLeave',
    //                        data: { leaveRequestId: leaveRequestId },
    //                        success: function (approveResponse) {
    //                            Swal.fire({
    //                                title: "Success!",
    //                                text: approveResponse.message,
    //                                icon: "success",
    //                                confirmButtonText: "OK"
    //                            }).then(function () {
    //                                window.location.href = '/Admin/Leave/ViewAllLeaves';
    //                            });
    //                        },
    //                        error: function (xhr, status, error) {
    //                            console.error('Error occurred while rejecting leave:', error);
    //                            Swal.fire({
    //                                title: 'Error!',
    //                                text: 'An unexpected error occurred while rejecting leave.',
    //                                icon: 'error',
    //                                confirmButtonText: 'OK'
    //                            });
    //                        }
    //                    });
    //                } else {
    //                    Swal.fire({
    //                        title: 'Cancelled',
    //                        text: 'Leave approval has been cancelled.',
    //                        icon: 'info',
    //                        confirmButtonText: 'OK'
    //                    });
    //                }
    //            });
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('Error occurred while fetching leave data:', error);
    //            Swal.fire({
    //                title: 'Error!',
    //                text: 'An unexpected error occurred.',
    //                icon: 'error',
    //                confirmButtonText: 'OK'
    //            });
    //        }
    //    });
    //});

    // Open modal when Reject button is clicked
    $(document).on("click", ".RejectBtn", function () {
        var leaveRequestId = $(this).data("id");
        $("#leaveRequestId").val(leaveRequestId);
        $("#rejectReason").val('');
        $("#RejectModal").modal("show");
    });

    // Confirm rejection inside modal
    $(document).on("click", "#confirmRejectBtn", function () {
        var leaveRequestId = $("#leaveRequestId").val();
        var rejectReason = $("#rejectReason").val().trim();
        showLoadingSpinner();

        if (rejectReason === "") {
            $("#rejectError").show();
            return;
        }

        $("#rejectError").hide();

        $.ajax({
            type: "POST",
            url: "/admin/Leave/RejectLeave",
            data: { leaveRequestId: leaveRequestId, rejectionReason: rejectReason },
            success: function (response) {
                hideLoadingSpinner();
                $("#RejectModal").modal("hide");
                Swal.fire({
                    title: "Rejected!",
                    text: response.message || "Leave has been rejected successfully.",
                    icon: "success"
                }).then(() => window.location.reload());
            },
            error: function () {
                hideLoadingSpinner();
                Swal.fire({
                    title: "Error!",
                    text: "Something went wrong while rejecting the leave.",
                    icon: "error"
                });
            }
        });
    });



});
