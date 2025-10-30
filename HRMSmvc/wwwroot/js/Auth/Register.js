$(document).ready(function () {
    function showLoadingSpinner() {
        document.getElementById("loading-container").style.display = "block";
    }

    function hideLoadingSpinner() {
        document.getElementById("loading-container").style.display = "none";
    }

    // Initialize Flatpickr
    $("#DOB").flatpickr({
        dateFormat: "Y-m-d",
        maxDate: "today",
        allowInput: true
    });
    $("#JoiningDate").flatpickr({
        dateFormat: "Y-m-d",
        allowInput: true
    });

    $.validator.setDefaults({
        ignore: [] // Don’t ignore hidden inputs like file fields
    });

    // Register form validation + AJAX
    $('#registerForm').validate({
        rules: {
            Name: { required: true, minlength: 3 },
            UserName: { required: true, minlength: 3 },
            Position: { required: true },
            DOB: { required: true, date: true },
            JoiningDate: { required: true, date: true },
            PhoneNumber: { required: true, digits: true, minlength: 10, maxlength: 10 },
            Address: { required: true, minlength: 5 },
            Email: { required: true, email: true },
            Password: { required: true, minlength: 6 },
            ConfirmPassword: { required: true, equalTo: "#yourPassword" }
        },
        messages: {
            Name: { required: "Please enter your Name.", minlength: "Name must be at least 3 characters long." },
            UserName: { required: "Please enter your Username.", minlength: "Username must be at least 3 characters long." },
            Position: { required: "Please enter your Position." },
            DOB: { required: "Please enter your Date of Birth.", date: "Please enter a valid date." },
            JoiningDate: { required: "Please enter your Joining Date.", date: "Please enter a valid date." },
            PhoneNumber: { required: "Please enter your Phone Number.", digits: "Only digits are allowed.", minlength: "Phone Number must be 10 digits.", maxlength: "Phone Number must be 10 digits." },
            Address: { required: "Please enter your Address.", minlength: "Address must be at least 5 characters long." },
            Email: { required: "Please enter your Email.", email: "Please enter a valid Email address." },
            Password: { required: "Please enter your Password.", minlength: "Password must be at least 6 characters long." },
            ConfirmPassword: { required: "Please confirm your Password.", equalTo: "Passwords do not match." }
        },
        errorClass: "is-invalid",
        validClass: "is-valid",
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("text-danger mt-1");
            if (element.hasClass("custom-file-input")) {
                error.insertAfter(element.closest(".custom-file"));
            } else {
                error.insertAfter(element);
            }
        },
        // On successful validation submitHandler
        submitHandler: function (form) {
            var actionUrl = $(form).attr('action') || '@Url.Action("Register", "Auth")';
            showLoadingSpinner();

            // Use FormData for possible file uploads
            var formData = new FormData(form);
            $.ajax({
                type: 'POST',
                url: actionUrl,
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    hideLoadingSpinner();
                    if (response && response.success) {
                        Swal.fire({
                            title: "Registration successful!",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function (result) {
                            if (result.isConfirmed) {
                                window.location.href = '/Auth/Login';
                            }
                        });
                    } else {
                        Swal.fire({
                            title: "Registration Failed!",
                            text: response ? response.message : "Please try again.",
                            icon: "error",
                            confirmButtonText: "OK"
                        });
                    }
                },
                error: function (xhr, status, error) {
                    hideLoadingSpinner();
                    console.error("Error occurred:", error);
                    Swal.fire({
                        title: "Error!",
                        text: "An unexpected error occurred. Please try again later.",
                        icon: "error",
                        confirmButtonText: "OK"
                    });
                }
            });
        }
    });
    let regCropper;
    let regSelectedFile;

    // Trigger file input
    $("#registerUploadBtn").click(function (e) {
        e.preventDefault();
        $("#registerFile").click();
    });

    // Show modal when a file is selected
    $("#registerFile").change(function (e) {
        const file = e.target.files[0];
        if (!file) return;

        regSelectedFile = file;
        const reader = new FileReader();
        reader.onload = function (event) {
            $("#registerCropImage").attr("src", event.target.result);
            $("#registerCropModal").modal("show");
        };
        reader.readAsDataURL(file);
    });

    // Initialize cropper on modal open
    $("#registerCropModal").on("shown.bs.modal", function () {
        regCropper = new Cropper(document.getElementById("registerCropImage"), {
            aspectRatio: 1,
            viewMode: 1,
            movable: false,
            zoomable: true,
            rotatable: false,
            scalable: false
        });
    }).on("hidden.bs.modal", function () {
        if (regCropper) regCropper.destroy();
        regCropper = null;
    });

    // Handle Crop & Set
    $("#registerCropAndSetBtn").click(function () {
        if (!regCropper) return;

        const canvas = regCropper.getCroppedCanvas({
            width: 300,
            height: 300
        });

        // Create circular mask
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

        // Convert circular crop to blob and replace file input
        circleCanvas.toBlob(function (blob) {
            const croppedFile = new File([blob], regSelectedFile.name, { type: "image/png", lastModified: Date.now() });

            // Use DataTransfer to update hidden file input
            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(croppedFile);
            document.getElementById("registerFile").files = dataTransfer.files;

            // Update preview
            const previewUrl = URL.createObjectURL(blob);
            $("#registerPreview").attr("src", previewUrl);

            $("#registerCropModal").modal("hide");
        }, "image/png");
    });

});
