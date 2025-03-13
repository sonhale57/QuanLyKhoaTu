$(document).ready(function () {
    let currentPage = 1;
    let limit = $("#limitSelect").val();
    let search = $("#searchInput").val();
    let newCode = "";
    Loadlist();

    function Loadlist() {
        $.ajax({
            url: "/Member/GetRemoveLists",
            type: "GET",
            data: { searchText: search, page: currentPage, limit: limit },
            success: function (data) {
                var html = "";
                $.each(data.members, function (index, item) {
                    html += `<tr>
                    <td class="text-center align-content-center">${index + 1}</td>
                    <td class="text-center align-content-center">${item.code}</td>
                    <td class="align-content-center">${item.name}</td>
                    <td class="align-content-center">${item.ortherName}</td>
                    <td class="text-center align-content-center">${item.year}</td>
                    <td class="align-content-center text-center">${item.phone}</td>
                    <td class="align-content-center text-center">${item.statusIdentity == true ? '<i class="ti ti-shield-check text-success"></i>' : '<i class="ti ti-shield-exclamation text-warning"></i>'} ${item.numberIdentity}</td>
                    <td class="text-center align-content-center">${item.countJoin}</td>
                    <td class="text-center align-content-center">${item.dateCreate}</td>
                    <td class="text-end me-3 align-content-center">
                        <button type="button" class="btn btn-sm btn-outline-dark btn-restore" data-id="${item.id}"><i class="ti ti-refresh"></i> Khôi phục</button>
                    </td>
                </tr>`;
                });
                $("#thanhvienTable").html(html);
                $("#currentPage").text(data.currentPage);
                newCode = data.newCode;
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
        Loadlist();
    });

    // 3️⃣ Xử lý khi thay đổi số lượng user mỗi trang
    $("#limitSelect").change(function () {
        limit = $(this).val();
        currentPage = 1; // Reset về trang 1
        Loadlist();
    });

    // 4️⃣ Chuyển trang trước
    $("#prevPage").click(function () {
        if (currentPage > 1) {
            currentPage--;
            Loadlist();
        }
    });

    // 5️⃣ Chuyển trang sau
    $("#nextPage").click(function () {
        currentPage++;
        Loadlist();
    });

    $(document).on("click", ".btn-restore", function () {
        var id = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn khôi phục thành viên này?")) {
            $.ajax({
                url: "/Member/Restore",
                type: "POST",
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        Loadlist();
                        showNotification(response.message);
                    } else {
                        showNotification(response.message);
                    }
                }
            });
        }
    });
});