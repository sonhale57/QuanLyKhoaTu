$(document).ready(function () {
    // Hiển thị loader
    function showLoader() {
        $("#bedMatrixContainer").html(`<div class="text-center mt-3">
                                            <div class="spinner-border text-success me-3" role="status" style="width:20px;height:20px;">
                                                <span class="visually-hidden"> Loading...</span>
                                             </div>Đang tải dữ liệu...</div>`);
    }showLoader();

    // Ẩn loader
    function hideLoader() {
        $("#bedMatrixContainer").html('');
    }
    function GetListZen() {
        $.ajax({
            url: "/KhoaTu/GetListKhoaTu",
            type: "GET",
            data: {},
            success: function (data) {
                var html = "";
                $.each(data.zens.result, function (index, item) {
                    html += `<option value="${item.id}">${item.name}</td>`;
                });
                $("#selectKhoatu").html(html);
            }
        });
    } GetListArea();

    function GetListArea() {
        $.ajax({
            url: "/Bed/GetListArea",
            type: "GET",
            data: {},
            success: function (data) {
                var html = "";
                $.each(data.areas.result, function (index, item) {
                    html += `<option value="${item.id}">${item.name}</td>`;
                });
                $("#selectArea").html(html);
            }
        });
    } GetListZen();

    $("#selectArea").change(function () {
        loadBedMatrix();
    });

    $("#selectKhoatu").change(function () {
        loadBedMatrix();
    });

    // Đợi cả hai phương thức chạy xong rồi mới load bed matrix
    $.when(GetListZen(), GetListArea()).done(function () {
        setTimeout(() => { // Chờ 500ms để đảm bảo dữ liệu đã được cập nhật
            hideLoader();
            loadBedMatrix();
        }, 500);
    });
    function loadBedMatrix() {
        let courseId = $("#selectKhoatu").val();
        let areaId = $("#selectArea").val();
        if (!courseId || !areaId) return;
        $.ajax({
            url: "/KhoaTu/GetBedMatrix",
            type: "GET",
            data: {
                areaId: areaId,
                courseId: courseId
            },
            success: function (response) {
                hideLoader();
                if (response.success) {
                    let html = `<table class="table table-bordered text-center" style="border-collapse: unset;border-spacing: 10px; ">`;
                    html += `<tbody>`;

                    response.data.forEach(row => {
                        html += `<tr>`;
                        row.beds.forEach(bed => {
                            let color = bed.isAvailable ? "bg-light" : "bg-danger text-white";
                            if (bed.isAvailable) {
                                if (bed.available) {
                                    html += `<td class="bg-success text-white align-content-center" style="border:1px solid #d5d5d5; min-width:150px;"  data-bs-toggle="dropdown" aria-expanded="false">
                                                ${bed.name}
                                                <div class="border-dashed border-t pt-3 mt-2"> </div>
                                                ${bed.memberName}<br/>${bed.memberOrtherName}
                                                            <div class="btn-reveal-trigger position-static">
                                                                <button class="btn btn-sm dropdown-toggle dropdown-caret-none text-white" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><i class="ti ti-dots"></i></button>
                                                                <div class="dropdown-menu dropdown-menu-end py-2" style="">
                                                                    <a class="dropdown-item btn-changeBed" href="#!" data-memberid="${bed.occupiedBy}" data-membername="${bed.memberName}" data-courseid="${courseId}" data-currentbed="${bed.name}">Đổi chỗ ngủ</a>
                                                                    <div class="dropdown-divider"></div>
                                                                    <a class="dropdown-item text-danger btn-Capnhatve" href="#!" data-memberid="${bed.occupiedBy}" data-courseid="${courseId}" data-membername="${bed.memberName}" data-currentbed="${bed.name}" data-guidienthoai="${bed.guidienthoai}" data-guicccd="${bed.guicccd}">Cập nhật về</a>
                                                                </div>
                                                          </div>
                                            </td>`;
                                } else {
                                    html += `<td class="align-content-center" style="border:1px solid #d5d5d5; min-width:90px;">
                                                ${bed.name}
                                                <div class="border-dashed border-t pt-3 mt-2"></div>
                                                <div class="btn-reveal-trigger position-static">
                                                                <button class="btn btn-sm dropdown-toggle dropdown-caret-none" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><i class="ti ti-plus"></i></button>
                                                                <div class="dropdown-menu dropdown-menu-end py-2" style="">
                                                                    <a class="dropdown-item btn-registrationNew" href="#!" data-bedid="${bed.id}" data-bedname="${bed.name}" data-courseid="${courseId}">+ Thành viên mới</a>
                                                                    <div class="dropdown-divider"></div>
                                                                    <a class="dropdown-item btn-registrationOld text-success" href="#!" data-bedid="${bed.id}" data-bedname="${bed.name}" data-courseid="${courseId}">+ Thành viên có sẵn</a>
                                                                </div>
                                                            </div>
                                            </td>`;
                                }
                            }
                            else {
                                html += `<td class="${color} align-content-center" style="min-width:75px;" title="Giường: ${bed.name}">
                                        ${bed.name}
                                        <p class="border-dashed border-t pt-3 mt-2">X</p>
                                </td>`;
                            }
                        });
                        html += `</tr>`;
                    });

                    html += `</tbody></table>`;
                    $("#bedMatrixContainer").html(html);
                }
            },
            error: function () {
                hideLoader();
                $("#bedMatrixContainer").html('<div class="text-danger text-center">Lỗi khi tải dữ liệu.</div>');
            }
        });
    }

    //Đăng ký cho thành viên mới
    $(document).on("click", ".btn-registrationNew", function () {
        var bedId = $(this).data("bedid");
        var bedName = $(this).data("bedname");
        var courseId = $(this).data("courseid");

        $("#dknew_courseId").val(courseId);
        $("#dknew_bedId").val(bedId);
        $("#dknew_bedName").val(bedName);

        $("#dknew_code").val("");
        $("#dknew_name").val("");
        $("#dknew_ortherName").val("");
        $("#dknew_birthDay").val("");
        $("#dknew_phone").val("");
        $("#dknew_ortherPhone").val("");

        $.ajax({
            url: "/KhoaTu/GetnewCode",
            type: "GET",
            data: {  },
            success: function (data) {
                $("#dknew_code").val(data.newCode);
            }
        });
        $("#dangkymoiModal").modal("show");
    });
    $("#btnSubmitDKNew").click(function () {
        let formData = new FormData();
        formData.append("courseId", $("#dknew_courseId").val());
        formData.append("bedId", $("#dknew_bedId").val());
        formData.append("name", $("#dknew_name").val());
        formData.append("code", $("#dknew_code").val());
        formData.append("gender", $("#dknew_gender").val());
        formData.append("ortherName", $("#dknew_ortherName").val());
        formData.append("phone", $("#dknew_phone").val());
        formData.append("ortherPhone", $("#dknew_ortherPhone").val());
        formData.append("birthDay", $("#dknew_birthDay").val());
        formData.append("receivePhone", $("#dknew_guidienthoai").prop("checked"));
        formData.append("receiveCCCD", $("#dknew_guicccd").prop("checked"));
        let imageFile = document.getElementById("dknew_imageCCCD").files[0];
        if (imageFile) {
            formData.append("image", imageFile);
        }
        $.ajax({
            url: "/KhoaTu/SubmitDKNew",
            type: "POST",
            data: formData,
            processData: false,  // Không xử lý dữ liệu FormData
            contentType: false,  // Không đặt kiểu dữ liệu
            success: function (response) {
                if (response.success) {
                    $("#dangkymoiModal").modal("hide");
                    loadBedMatrix($("#areaSelect").val()); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });

    $(document).on("click", ".btn-registrationOld", function () {
        var bedId = $(this).data("bedid");
        var bedName = $(this).data("bedname");
        var courseId = $(this).data("courseid");

        $("#dkold_memberCode").val("");
        $("#dkold_memberName").val("");
        $("#dkold_memberOrtherName").val("");

        $("#dkold_bedId").val(bedId);
        $("#dkold_bedName").val(bedName);
        $("#dkold_courseId").val(courseId);

        $.ajax({
            url: "/KhoaTu/GetListMember",
            type: "GET",
            data: { courseId: courseId },
            success: function (data) {
                var html = "<option>-- Chọn thành viên</option>";
                $.each(data.members, function (index, item) {
                    html += `<option value="${item.id}" data-name="${item.name}" data-orthername="${item.ortherName}"  data-code="${item.code}">${item.text}</td>`;
                });
                $("#dkold_memberId").html(html);
            }
        });
        $("#dangkycuModal").modal("show");
    });

    $("#dkold_memberId").change(function () {
        var selectedOption = $("#dkold_memberId option:selected"); // Lấy option đang được chọn
        var Name = selectedOption.data("name"); // Lấy data-code
        var Code = selectedOption.data("code"); // Lấy data-code
        var OrtherName = selectedOption.data("orthername"); // Lấy data-orthername

        $("#dkold_memberCode").val(Code);
        $("#dkold_memberName").val(Name);
        $("#dkold_memberOrtherName").val(OrtherName);
    });

    $("#btnSubmitDKOld").click(function () {
        $.ajax({
            url: "/KhoaTu/submitRegistrationOld",
            type: "POST",
            data: {
                memberId: $("#dkold_memberId").val(),
                courseId: $("#dkold_courseId").val(),
                bedId: $("#dkold_bedId").val(),
                receivePhone: $("#dkold_guidienthoai").prop("checked"),
                receiveCCCD: $("#dkold_guicccd").prop("checked")
            },
            success: function (response) {
                if (response.success) {
                    $("#dangkycuModal").modal("hide");
                    loadBedMatrix($("#areaSelect").val()); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });

    //Cập nhật đổi chỗ
    $(document).on("click", ".btn-changeBed", function () {
        var memberId = $(this).data("memberid");
        var memberName = $(this).data("membername");
        var courseId = $(this).data("courseid");
        var currentBed = $(this).data("currentbed");

        $("#doicho_memberId").val(memberId);
        $("#doicho_memberName").val(memberName);
        $("#doicho_currentBed").val(currentBed);
        $("#doicho_courseId").val(courseId);

        $.ajax({
            url: "/Bed/GetListArea",
            type: "GET",
            data: {},
            success: function (data) {
                var html = "<option>-- Chọn khu vực --</option>";
                $.each(data.areas.result, function (index, item) {
                    html += `<option value="${item.id}">${item.name}</td>`;
                });
                $("#doicho_areaId").html(html);
            }
        });
        $("#doicho_areaId").change(function () {
            let areaId = $("#doicho_areaId").val();
            GetListBed_Change(courseId, areaId);
        });
        $("#doichoModal").modal("show");
    });
    $("#btnchangeBed").click(function () {
        $.ajax({
            url: "/KhoaTu/submitchangeBed",
            type: "POST",
            data: {
                memberId: $("#doicho_memberId").val(),
                courseId: $("#doicho_courseId").val(),
                bedId: $("#doicho_bedId").val()
            },
            success: function (response) {
                if (response.success) {
                    $("#doichoModal").modal("hide");
                    loadBedMatrix($("#areaSelect").val());
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });

    //Cập nhật về
    $(document).on("click", ".btn-Capnhatve", function () {
        var memberId = $(this).data("memberid");
        var courseId = $(this).data("courseid");
        var memberName = $(this).data("membername");
        var currentBed = $(this).data("currentbed");
        var guidienthoai = $(this).data("guidienthoai");
        var guicccd = $(this).data("guicccd");

        $("#capnhatve_memberName").val(memberName);
        $("#capnhatve_currentBed").val(currentBed);

        $("#capnhatve_memberId").val(memberId);
        $("#capnhatve_courseId").val(courseId);
        $("#capnhatve_guidienthoai").prop("checked", guidienthoai);
        $("#capnhatve_guicccd").prop("checked", guicccd);

        $("#capnhatveModal").modal("show");
    });
    $("#btnsubmitVe").click(function () {
        $.ajax({
            url: "/KhoaTu/submitDive",
            type: "POST",
            data: {
                memberId: $("#capnhatve_memberId").val(),
                courseId: $("#capnhatve_courseId").val()
            },
            success: function (response) {
                if (response.success) {
                    $("#capnhatveModal").modal("hide");
                    loadBedMatrix($("#areaSelect").val()); 
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });
});
function GetListBed_Change(courseId,areaId) {
    $.ajax({
        url: "/KhoaTu/GetListBed_Change",
        type: "GET",
        data: {
            courseId, areaId
        },
        success: function (data) {
            var html = "";
            $.each(data.beds, function (index, item) {
                html += `<option value="${item.id}">${item.name}</td>`;
            });
            $("#doicho_bedId").html(html);
        }
    });
}