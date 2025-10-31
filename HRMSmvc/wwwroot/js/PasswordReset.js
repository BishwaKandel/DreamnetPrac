$(document).ready(function () {
    function showLoadingSpinner() {
        document.getElementById("loading-container").style.display = "block";
    }

    function hideLoadingSpinner() {
        document.getElementById("loading-container").style.display = "none";
    }

    $('#forgotPasswordform').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
        },
        messages: {
            Email: {
                required: "Email is required.",
                email: "Please enter a valid email address."
            },
        },
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        errorPlacement: function (error, element) {
            error.addClass('text-danger');
            error.insertAfter(element);
        },
        submitHandler: function (form) {
            showLoadingSpinner();
            $.ajax({
                type: 'POST',
                url: "/Auth/ForgotPassword",
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
                            if (result.isConfirmed) {
                                window.location.href = '/Auth/Login';
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

    $('#resetPwdform').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            NewPassword: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: '[name="NewPassword"]'
            }
        },
        messages: {
            Email: {
                required: "Email is required.",
                email: "Please enter a valid email address."
            },
            NewPassword: {
                required: "New password is required.",
                minlength: "Password must be at least 6 characters long."
            },
            ConfirmPassword: {
                required: "Please confirm your password.",
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
            showLoadingSpinner();
            $.ajax({
                type: 'POST',
                url: "/Auth/ResetPassword",
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
                            if (result.isConfirmed) {
                                window.location.href = '/Auth/Login';
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

});