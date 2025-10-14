$(document).ready(function () {
    $('#editForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this);
        // Prevent default form submission
        
        // Send the AJAX request
        $.ajax({
            type: 'POST',
            url: '/client/Dashboard/Edit',
            //data: $('#editForm').serialize(),
            data: formData,               // Use FormData instead of serialize()
            processData: false,           // Required for FormData
            contentType: false,
            //dataType: 'json', // Expect JSON response
            success: function (response) {
                console.log('Response:', response); // For debugging
                if (response.success) {                                //Changes Data to LOGIN
                    // Show success message with SweetAlert
                    Swal.fire({
                        title: "Details updated!",
                        icon: "success",
                        confirmButtonText: "OK",
                    }).then((result) => {
                        // If user clicks 'OK', redirect to login page
                        if (result.isConfirmed) {
                            window.location.href = '/client/Dashboard/UserIndex';
                        }
                    });
                } else {
                    // If registration fails, show the error message from the server
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
    });
    $('#changepasswordForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this);
        // Prevent default form submission

        // Send the AJAX request
        $.ajax({
            type: 'POST',
            url: '/client/Dashboard/ChangePassword',
            //data: $('#editForm').serialize(),
            data: formData,               // Use FormData instead of serialize()
            processData: false,           // Required for FormData
            contentType: false,
            dataType: 'json', // Expect JSON response
            success: function (response) {
                console.log('Response:', response); // For debugging
                if (response.success) {                                //Changes Data to LOGIN
                    // Show success message with SweetAlert
                    Swal.fire({
                        title: "Password Changed!",
                        icon: "success",
                        confirmButtonText: "OK",
                    }).then((result) => {
                        // If user clicks 'OK', redirect to login page
                        if (result.isConfirmed) {
                            window.location.href = '/client/Dashboard/UserIndex';
                        }
                    });
                } else {
                    // If registration fails, show the error message from the server
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
    });
    $("#DOB").flatpickr({
        dateFormat: "Y-m-d",  // Format of the selected date
        maxDate: "today",     // Prevent selecting future dates
        allowInput: true      // Allow the user to type the date manually
    });
    $('#uploadBtn').click(function (event) {
        event.preventDefault(); // Prevent the default anchor behavior
        $('#changeFile').click(); // Trigger the file input click
    });
    //$("#customFile").on("change", function () { // For Register Page
    //    const file = this.files[0]; // get the selected file
    //    if (file) {
    //        const reader = new FileReader(); // create a FileReader
    //        reader.onload = function (e) {
    //            $("#preview").attr("src", e.target.result); // set the img src to file content
    //        }
    //        reader.readAsDataURL(file);            // read the file as data URL
    //    }
    //});
    $("#changeFile").on("change", function () {
        const file = this.files[0];
        if (file) {
            // Preview image
            const reader = new FileReader();
            reader.onload = function (e) {
                $("#preview").attr("src", e.target.result);
            }
            reader.readAsDataURL(file);

            //  Use the whole form (includes hidden Id + file input)
            var formData = new FormData($("#photoForm")[0]);

            $.ajax({
                type: 'POST',
                url: '/client/Dashboard/ChangePhoto',   // matches your MVC controller
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    console.log('Response:', response);
                    if (response.success) {
                        Swal.fire({
                            title: "Picture updated!",
                            icon: "success",
                            confirmButtonText: "OK",
                        }).then((result) => {
                            if (result.isConfirmed) {

                                //window.location.href = '/Client/Dashboard/UserIndex';
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
    $("#deleteBtn").on("click", function () {
        var userId = $("input[name='Id']").val(); // get the hidden Id

        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    url: 'client/Dashboard/DeletePhoto',
                    data: { id: userId }, // send Id as plain data
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: "Deleted!",
                                text: "Your profile picture has been deleted.",
                                icon: "success"
                            });
                            $("#preview").attr("src", ""); // clear the image preview
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
            url: 'client/Dashboard/CheckIn', 
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: "Welcome",
                        text: "Your attendance has been marked.",
                        icon: "success"
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
            url: 'client/Dashboard/CheckOut',
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: "Thankyou",
                        text: "Your are Checked out",
                        icon: "success"
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


    
});

