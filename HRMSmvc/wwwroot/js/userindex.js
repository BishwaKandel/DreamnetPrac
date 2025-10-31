$(document).ready(function () {
    $('#editForm').validate({
        rules: {
            Name: {
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
            Address: {
                required: true,
                minlength: 5
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
            // Serialize form data (since it's text inputs only)
            var formData = $(form).serialize();

            // Send the AJAX request
            $.ajax({
                type: 'POST',
                url: '/client/Dashboard/Edit',
                data: formData,
                success: function (response) {
                    console.log('Response:', response);
                    if (response.success) {
                        Swal.fire({
                            title: "Details updated!",
                            icon: "success",
                            confirmButtonText: "OK",
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/client/Dashboard/UserIndex?mode=1';
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Update Failed!',
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
        





    $('#changepasswordForm').validate({
        rules: {
            CurrentPassword: {
                required: true,
                minlength: 6
            },
            NewPassword: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#NewPassword"
            }
        },
        messages: {
            CurrentPassword: {
                required: "Current password is required.",
                minlength: "Current password must be at least 6 characters."
            },
            NewPassword: {
                required: "New password is required.",
                minlength: "New password must be at least 6 characters."
            },
            ConfirmPassword: {
                required: "Please confirm your new password.",
                equalTo: "Passwords do not match."
            }
        },
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        errorPlacement: function (error, element) {
            error.addClass('text-danger');
            error.insertAfter(element);
        },
        submitHandler: function (form) {
            var formData = new FormData(form);
            $.ajax({
                type: 'POST',
                url: $(form).attr('action'),
                data: formData,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: "Password Changed!",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/client/Dashboard/UserIndex?mode=2';
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Password Change Failed!',
                            text: response.message || 'Please try again.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error occurred:', error);
                    Swal.fire({
                        title: 'Password Change Failed!',
                        text: 'An unexpected error occurred. Please try again later.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });
        }
    });

    $("#DOB").flatpickr({
        dateFormat: "Y-m-d",  // Format of the selected date
        maxDate: "today",     // Prevent selecting future dates
        allowInput: true      // Allow the user to type the date manually
    });
    let cropper;
    let selectedFile;

    // Trigger file input
    $("#uploadBtn").click(function (e) {
        e.preventDefault();
        $("#changeFile").click();
    });

    // Open modal after file selection
    $("#changeFile").change(function (e) {
        const file = e.target.files[0];
        if (!file) return;

        selectedFile = file;
        const reader = new FileReader();
        reader.onload = function (event) {
            $("#cropImage").attr("src", event.target.result);
            $("#cropModal").modal("show");
        };
        reader.readAsDataURL(file);
    });

    // Initialize Cropper when modal opens
    $("#cropModal").on("shown.bs.modal", function () {
        cropper = new Cropper(document.getElementById("cropImage"), {
            aspectRatio: 1,
            viewMode: 1,
            movable: false,
            zoomable: true,
            rotatable: false,
            scalable: false
        });
    }).on("hidden.bs.modal", function () {
        if (cropper) cropper.destroy();
        cropper = null;
    });

    // Handle crop and upload
    $("#cropAndUploadBtn").click(function () {
        if (!cropper) return;

        const canvas = cropper.getCroppedCanvas({
            width: 300,
            height: 300,
        });

        // Create a circular mask on the canvas
        const circleCanvas = document.createElement("canvas");
        const ctx = circleCanvas.getContext("2d");
        const size = 300;

        circleCanvas.width = size;
        circleCanvas.height = size;

        ctx.beginPath();
        ctx.arc(size / 2, size / 2, size / 2, 0, Math.PI * 2);
        ctx.closePath();
        ctx.clip();

        ctx.drawImage(canvas, 0, 0, size, size);

        // Convert circular canvas to blob
        circleCanvas.toBlob(function (blob) {
            const formData = new FormData();
            formData.append("Id", $("#Id").val());
            formData.append("formFile", blob, selectedFile.name);

            $.ajax({
                url: "/Client/Dashboard/ChangePhoto",
                method: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function () {
                    $("#cropModal").modal("hide");
                    Swal.fire("Updated!", "Profile photo updated successfully.", "success").then(() => {
                        window.location.href = "/Client/Dashboard?mode=1";
                    });
                },
                error: function () {
                    Swal.fire("Error!", "Unable to upload image.", "error");
                }
            });
        });
    });



    $("#deleteBtn").on("click", function () {
        var userId = $("input[name='Id']").val(); // get the hidden Id

        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor:  "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    url: 'client/Dashboard/DeletePhoto',
                    data: { id: userId },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: "Deleted!",
                                text: "Your profile picture has been deleted.",
                                icon: "success"
                            }).then(() => {
                                // Clear the image preview and reload the page
                                $("#preview").attr("src", "");
                                window.location.href = "/Client/Dashboard?mode=1";
                            });
                        } else {
                            Swal.fire({
                                title: 'Delete Failed!',
                                text: response.message || 'Please try again.',
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            title: 'Delete Failed!',
                            text: 'An unexpected error occurred. Please try again later.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            }
        });
    });


    //Checkin Button
    $("#checkinBtn").on("click", function () {
        $.ajax({
            type: 'POST',
            url: '/client/Dashboard/CheckIn', 
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: "Success",
                        text: "Your attendance has been marked.",
                        icon: "success"
                    })
                    .then(() => {
                        window.location.href = "/Client/Dashboard?mode=0";
                    });
                } else {
                    Swal.fire({
                        title: 'CheckIn Failed!',
                        text: response.message || 'Please try again.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    title: 'CheckIn Failed!',
                    text: 'An unexpected error occurred. Please try again later.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    //Checkout Button
    $("#checkoutBtn").on("click", function () {
        $.ajax({
            type: 'POST',
            url: '/client/Dashboard/CheckOut',
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: "Success",
                        text: "Your are Checked out",
                        icon: "success"
                    })
                        .then(() => {
                            window.location.href = "/Client/Dashboard?mode=0";
                        });
                } else {
                    Swal.fire({
                        title: 'Checkout Failed!',
                        text: response.message || 'Please try again.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    title: 'Checkout Failed!',
                    text: 'An unexpected error occurred. Please try again later.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    $('#AttendanceTable').DataTable(
        {
            responsive: true

        });

    
});

