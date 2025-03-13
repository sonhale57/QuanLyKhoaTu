$(document).ready(function () {
    let currentPage = 1;
    let limit = $("#limitSelect").val();
    let search = $("#searchInput").val();
    let newCode = "";
    Loadlist();

    function Loadlist() {
        $.ajax({
            url: "/Member/GetLists",
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
                    <td class="align-content-center text-center">${item.statusIdentity == true ? `<a href="${item.imageIdentity}" target="_blank" class="text-decoration-none"><i class="ti ti-shield-check text-success"></i></a>` : '<i class="ti ti-alert-circle text-warning"></i>'} ${item.numberIdentity}</td>
                    <td class="text-center align-content-center">${item.countJoin}</td>
                    <td class="text-center align-content-center">${item.dateCreate}</td>
                    <td class="text-end me-3 align-content-center">
                        <button type="button" class="btn btn-sm btn-dark btn-delete" data-id="${item.id}"><i class="ti ti-printer"></i> ${item.printCount}</button>
                        <a href="#!" class="btn btn-sm btn-icon btn-outline-secondary btn-edit" 
                            data-id="${item.id}" 
                            data-name="${item.name}" 
                            data-ortherName="${item.ortherName}" 
                            data-phone="${item.phone}" 
                            data-ortherPhone="${item.ortherPhone}" 
                            data-gender="${item.gender}" 
                            data-birthDay="${item.editBirthDay}" 
                            data-code="${item.code}" ><i class="ti ti-edit"></i></a>
                        <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.id}"><i class="ti ti-eraser"></i></button>
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

    $("#btnAdd").click(function () {
        $("#memberId").val(0); // Reset ID
        $("#code").val(newCode);
        $("#code").attr("disabled", true);
        $("#name").val("");
        $("#ortherName").val("");
        $("#phone").val("");
        $("#ortherPhone").val("");
        $("#birthDay").val("");
        $("#thanhvienModal").modal("show");
    });

    $(document).on("click", ".btn-edit", function () {
        var id = $(this).data("id");
        var code = $(this).data("code");
        var gender = $(this).data("gender");
        var name = $(this).data("name");
        var ortherName = $(this).data("orthername");
        var phone = $(this).data("phone");
        var ortherPhone = $(this).data("ortherphone");
        var birthDay = $(this).data("birthday");

        $("#memberId").val(id);
        $("#code").val(code);
        $("#gender").val(gender);
        $("#code").attr("disabled", true);
        $("#name").val(name);
        $("#ortherName").val(ortherName);
        $("#phone").val(phone);
        $("#ortherPhone").val(ortherPhone);
        $("#birthDay").val(birthDay);
        $("#thanhvienModal").modal("show");
    });

    $(document).on("click", ".btn-delete", function () {
        var id = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa thành viên này?")) {
            $.ajax({
                url: "/Member/Delete",
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
        let formData = new FormData();
        formData.append("id", $("#memberId").val());
        formData.append("name", $("#name").val());
        formData.append("code", $("#code").val());
        formData.append("gender", $("#gender").val());
        formData.append("ortherName", $("#ortherName").val());
        formData.append("phone", $("#phone").val());
        formData.append("ortherPhone", $("#ortherPhone").val());
        formData.append("birthDay", $("#birthDay").val());

        let imageFile = document.getElementById("imageCCCD").files[0];
        if (imageFile) {
            formData.append("image", imageFile);
        }
        $.ajax({
            url: "/Member/Savechange",
            type: "POST",
            data: formData,
            processData: false,  // Không xử lý dữ liệu FormData
            contentType: false,  // Không đặt kiểu dữ liệu
            success: function (response) {
                if (response.success) {
                    $("#thanhvienModal").modal("hide");
                    Loadlist(); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });
    document.getElementById("imageCCCD").addEventListener("change", function (event) {
        const file = event.target.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onload = function (event) {
            const img = new Image();
            img.src = event.target.result;
            img.onload = function () {
                const canvas = document.createElement("canvas");
                const ctx = canvas.getContext("2d");

                // Xác định kích thước ảnh tối đa (giảm kích thước nhưng giữ tỉ lệ)
                const MAX_WIDTH = 800;
                const MAX_HEIGHT = 800;
                let width = img.width;
                let height = img.height;

                if (width > MAX_WIDTH || height > MAX_HEIGHT) {
                    if (width > height) {
                        height *= MAX_WIDTH / width;
                        width = MAX_WIDTH;
                    } else {
                        width *= MAX_HEIGHT / height;
                        height = MAX_HEIGHT;
                    }
                }

                canvas.width = width;
                canvas.height = height;

                // Vẽ ảnh vào canvas để nén
                ctx.drawImage(img, 0, 0, width, height);

                // Chuyển ảnh sang base64 với chất lượng 0.7 (giảm dung lượng)
                const compressedImage = canvas.toDataURL("image/jpeg", 0.7);

                // Hiển thị ảnh preview
                document.getElementById("previewImage").src = compressedImage;
                document.getElementById("previewImage").style.display = "block";

                // Chuyển base64 thành file để gửi lên server
                fetch(compressedImage)
                    .then(res => res.blob())
                    .then(blob => {
                        const fileInput = document.getElementById("imageCCCD");
                        const compressedFile = new File([blob], file.name, { type: "image/jpeg" });
                        const dataTransfer = new DataTransfer();
                        dataTransfer.items.add(compressedFile);
                        fileInput.files = dataTransfer.files;
                    });
            };
        };
    });
    $("#btnImport").click(function () {
        let fileInput = document.getElementById("excelFile");
        let file = fileInput.files[0];

        if (!file) {
            showNotification("Vui lòng chọn file Excel trước khi import!");
            return;
        }

        let formData = new FormData();
        formData.append("file", file);

        $.ajax({
            url: "/Member/ImportMembers",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                showNotification(response.message);
                if (response.success) {
                    Loadlist(); // Load lại danh sách sau khi import thành công
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi import:", error);
                showNotification("Lỗi khi import file Excel!");
            }
        });
    });
});