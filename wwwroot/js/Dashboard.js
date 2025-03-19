$(document).ready(function () {
    let search = $("#searchInput").val();
    let currentSort = "date"; // Lưu trạng thái sắp xếp hiện tại
    Loadlist(currentSort);

    function Loadlist(sort) {
        currentSort = sort; // Cập nhật trạng thái sắp xếp hiện tại
        $.ajax({
            url: "/Home/Loadlist",
            type: "GET",
            data: { searchText: search, sort: currentSort },
            success: function (data) {
                var html = "";
                $.each(data.data, function (index, item) {
                    html += `<tr>
                    <td class="text-center align-content-center">${index + 1}</td>
                    <td class="text-center align-content-center">${item.code}</td>
                    <td class="align-content-center">${item.name}</td>
                    <td class="align-content-center">${item.ortherName}</td>
                    <td class="text-center align-content-center">${item.gender}</td>
                    <td class="text-center align-content-center">${item.year}</td>
                    <td class="text-center align-content-center">${item.phone}</td>
                    <td class="text-center align-content-center">${item.ortherPhone}</td>
                    <td class="align-content-center text-center">${item.bed}</td>
                    <td class="text-center align-content-center">${item.dateCreate}</td>
                    <td class="text-center align-content-center">${item.status}</td>
                </tr>`;
                });
                $("#dashboardTable").html(html);
            }
        });
    }

    // 2️⃣ Xử lý tìm kiếm khi nhập vào ô input
    $("#searchInput").on("input", function () {
        search = $(this).val();
        Loadlist();
    });
    // 3️⃣ Xử lý click vào dropdown sắp xếp
    $(".dropdown-menu a").click(function () {
        let selectedSort = $(this).attr("href").split("'")[1]; // Lấy giá trị từ href (vd: 'status', 'date'...)
        Loadlist(selectedSort);
    });

    function Onload() {
        $.ajax({
            url: "/Home/Onload",
            type: "GET",
            data: {},
            success: function (data) {
                $("#courseName").html(data.courseName);
                $("#count_khoatu").html(data.count_khoatu);
                $("#count_thanhvien").html(data.count_tongthanhvien);
                $("#count_thamgia").html(data.count_thamgia);
                $("#count_dave").html(data.count_dave);
                $("#count_controng").html(data.count_controng);
                $("#count_tongchongu").html(data.count_tongchongu);
            }
        });
    } Onload();
});