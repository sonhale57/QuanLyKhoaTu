// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showNotification(notificationText) {
    var toastEl = $(".toast");
    var toast = new bootstrap.Toast(toastEl, {
        animation: true,
        autohide: true,
        delay: 3000
    });
    $("#notificationText").html(notificationText);
    toast.show();
}
window.addEventListener('DOMContentLoaded', event => {
    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

function submitChangePassword() {
    let password = document.getElementById("floatingPassword").value;
    let confirm = document.getElementById("floatingConfirm").value;
    if (password != confirm) {
        setTimeout(function () {
            alert("Mật khẩu không trùng khớp!");
        }, 2000);
    }
    else {
        $.ajax({
            url: '/Account/SubmitChangePassword',
            type: 'POST',
            data: {
                'password': document.getElementById("floatingPassword").value
            },
            success: function (response) {
                if (response.success) {
                    $('#changePasswordModal').modal('hide');
                    setTimeout(function () {
                        alert(response.message);
                    }, 2000);
                } else {
                    setTimeout(function () {
                        alert(response.message);
                    }, 2000);
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi đăng nhập:" + error);
            }
        });
    }
}