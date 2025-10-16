$(document).ready(function () {
   
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


    $('#LeaveForm').submit(function (event) {
        event.preventDefault(); 
        var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: '/client/Leave/LeaveApply',
            data: formData,               // Use FormData instead of serialize()
            processData: false,           // Required for FormData
            contentType: false,
            success: function (response) {
                if (response && response.success) {
                    Swal.fire({
                        title: "Leave Applied Sucessfully!",
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

    $(document).on("click", ".ApproveBtn", function () {
        var leaveRequestId = $(this).data("id");

        $.ajax({
            success: function (response) {
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
                        $.ajax({
                            type: 'POST',
                            url: '/admin/Leave/ApproveLeave',
                            data: { leaveRequestId: leaveRequestId },
                            success: function (approveResponse) {
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
                                console.error('Error occurred while approving leave:', error);
                                Swal.fire({
                                    title: 'Error!',
                                    text: 'An unexpected error occurred while approving leave.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Cancelled',
                            text: 'Leave approval has been cancelled.',
                            icon: 'info',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            },
            error: function (xhr, status, error) {
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

    //Reject Leave
    $(document).on("click", ".RejectBtn", function () {
        var leaveRequestId = $(this).data("id");

        $.ajax({
            success: function (response) {
                Swal.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, reject it!"
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            url: '/admin/Leave/RejectLeave',
                            data: { leaveRequestId: leaveRequestId },
                            success: function (approveResponse) {
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
                                console.error('Error occurred while rejecting leave:', error);
                                Swal.fire({
                                    title: 'Error!',
                                    text: 'An unexpected error occurred while rejecting leave.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Cancelled',
                            text: 'Leave approval has been cancelled.',
                            icon: 'info',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            },
            error: function (xhr, status, error) {
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


});
