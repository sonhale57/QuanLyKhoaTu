$(document).ready(function () {
    let currentPage = 1;
    let limit = $("#limitSelect").val();
    let search = $("#searchInput").val();
    loadUsers(); 

    // Load danh sách User
    function loadUsers() {
        $.ajax({
            url: "/Users/GetUsers",
            type: "GET",
            data: { searchText: search, page: currentPage, limit: limit },
            success: function (data) {
                var html = "";
                $.each(data.users, function (index, item) {
                    html += `<tr>
                    <td class="text-center align-content-center">${index + 1}</td>
                    <td class="align-content-center">${item.name}</td>
                    <td class="align-content-center">${item.ortherName}</td>
                    <td class="text-center align-content-center">${item.phone}</td>
                    <td class="text-center align-content-center">${item.username}</td>
                    <td class="text-center align-content-center">${item.role}</td>
                    <td class="text-center align-content-center">
                        <label class="switch">
                            <input type="checkbox" class="toggle-enable" data-id="${item.id}" ${item.enable == true ? "checked" : ""}>
                            <span class="slider round"></span>
                        </label>
                    </td>
                    <td class="text-end me-3 align-content-center">
                        <a href="#!" class="btn btn-sm btn-icon btn-outline-secondary btn-edit" 
                            data-id="${item.id}" 
                            data-name="${item.name}" 
                            data-orthername="${item.ortherName}" 
                            data-username="${item.username}" 
                            data-phone="${item.phone}" 
                            data-role="${item.adminRole}"><i class="ti ti-edit"></i></a>
                        <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.id}"><i class="ti ti-eraser"></i></button>
                    </td>
                </tr>`;
                });
                $("#userTable").html(html);
                $("#currentPage").text(data.currentPage);

                // Ẩn nút "Trước" nếu đang ở trang 1
                if (data.currentPage === 1) {
                    $("#prevPage").parent().addClass("disabled");
                } else {
                    $("#prevPage").parent().removeClass("disabled");
                }

                // Ẩn nút "Sau" nếu đang ở trang cuối
                if (data.currentPage === data.totalPages) {
                    $("#nextPage").parent().addClass("disabled");
                } else {
                    $("#nextPage").parent().removeClass("disabled");
                }
            }
        });
    }
    // 2️⃣ Xử lý tìm kiếm khi nhập vào ô input
    $("#searchInput").on("input", function () {
        search = $(this).val();
        currentPage = 1; // Reset về trang 1
        loadUsers();
    });

    // 3️⃣ Xử lý khi thay đổi số lượng user mỗi trang
    $("#limitSelect").change(function () {
        limit = $(this).val();
        currentPage = 1; // Reset về trang 1
        loadUsers();
    });

    // 4️⃣ Chuyển trang trước
    $("#prevPage").click(function () {
        if (currentPage > 1) {
            currentPage--;
            loadUsers();
        }
    });

    // 5️⃣ Chuyển trang sau
    $("#nextPage").click(function () {
        currentPage++;
        loadUsers();
    });

    // Mở Modal thêm/sửa User
    $("#btnAddUser").click(function () {
        $("#userId").val(0); // Reset ID
        $("#name").val("");
        $("#ortherName").val("");
        $("#username").val("");
        $("#username").attr("disabled", false);
        $("#phone").val("");
        $("#password").val("");
        $("#adminRole").val("false");
        $("#userModal").modal("show");
    });
    // Sửa User
    $(document).on("click", ".btn-edit", function () {
        var id = $(this).data("id");
        var name = $(this).data("name");
        var orthername = $(this).data("orthername");
        var username = $(this).data("username");
        var phone = $(this).data("phone");
        var adminRole = $(this).data("role");
        $("#userId").val(id);
        $("#name").val(name);
        $("#ortherName").val(orthername);
        $("#username").val(username);
        $("#phone").val(phone);
        $("#username").attr("disabled", true);
        $("#password").val(""); // Không hiển thị mật khẩu
        $("#adminRole").val(adminRole.toString());
        $("#userModal").modal("show");
    });

    // Xóa User
    $(document).on("click", ".btn-delete", function () {
        var id = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa user này?")) {
            $.ajax({
                url: "/Users/DeleteUser",
                type: "POST",
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        loadUsers();
                        showNotification("Đã xóa thông tin người dùng!");
                    }
                    else {
                        showNotification(response.message);
                    }
                }
            });
        }
    });
    $(document).on("change", ".toggle-enable", function () {
        var id = $(this).data("id");
        var isChecked = $(this).prop("checked");

        $.ajax({
            url: "/Users/ToggleEnable",
            type: "POST",
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    showNotification("Cập nhật trạng thái thành công!");
                } else {
                    showNotification("Có lỗi xảy ra, vui lòng thử lại!");
                }
            },
            error: function () {
                showNotification("Lỗi kết nối đến server!");
            }
        });
    });
    // Lưu User
    $("#btnSaveUser").click(function () {
        $.ajax({
            url: "/Users/SaveUser",
            type: "POST",
            data: {
                id: $("#userId").val(),
                name: $("#name").val(),
                phone: $("#phone").val(),
                orthername: $("#ortherName").val(),
                username: $("#username").val(),
                password: $("#password").val(),
                adminRole: $("#adminRole").val()
            },
            success: function (response) {
                if (response.success) {
                    $("#userModal").modal("hide");
                    loadUsers(); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });
});