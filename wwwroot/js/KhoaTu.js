$(document).ready(function () {
    let currentPage = 1;
    let limit = $("#limitSelect").val();
    let search = $("#searchInput").val();
    Loadlist();

    function Loadlist() {
        $.ajax({
            url: "/KhoaTu/GetLists",
            type: "GET",
            data: { searchText: search, page: currentPage, limit: limit },
            success: function (data) {
                var html = "";
                $.each(data.zens, function (index, item) {
                    html += `<tr>
                    <td class="text-center align-content-center">${index + 1}</td>
                    <td class="align-content-center">${item.name}</td>
                    <td class="align-content-center text-center">${item.from}</td>
                    <td class="text-center align-content-center">${item.to }</td>
                    <td class="text-center align-content-center">${item.countJoin}</td>
                    <td class="text-center align-content-center">${item.status}</td>
                    <td class="text-end me-3 align-content-center">
                        <a href="#!" class="btn btn-sm btn-icon btn-outline-secondary btn-edit" 
                            data-id="${item.id}" 
                            data-name="${item.name}" 
                            data-fromdate="${item.fromdate}" 
                            data-todate="${item.todate}" ><i class="ti ti-edit"></i></a>
                        <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.id}"><i class="ti ti-eraser"></i></button>
                    </td>
                </tr>`;
                });
                $("#khoatuTable").html(html);
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

    $("#btnAdd").click(function () {
        $("#zensId").val(0); // Reset ID
        $("#name").val("");
        $("#fromdate").val("");
        $("#todate").val("");
        $("#khoatuModal").modal("show");
    });

    $(document).on("click", ".btn-edit", function () {
        var id = $(this).data("id");
        var name = $(this).data("name");
        var fromdate = $(this).data("fromdate");
        var todate = $(this).data("todate");

        $("#zensId").val(id);
        $("#name").val(name);
        $("#fromdate").val(fromdate);
        $("#todate").val(todate);
        $("#khoatuModal").modal("show");
    });

    $(document).on("click", ".btn-delete", function () {
        var id = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa khóa tu này?")) {
            $.ajax({
                url: "/KhoaTu/Delete",
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

    $("#btnSave").click(function () {
        $.ajax({
            url: "/KhoaTu/Savechange",
            type: "POST",
            data: {
                id: $("#zensId").val(),
                name: $("#name").val(),
                fromdate: $("#fromdate").val(),
                todate: $("#todate").val()
            },
            success: function (response) {
                if (response.success) {
                    $("#khoatuModal").modal("hide");
                    Loadlist(); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });
});