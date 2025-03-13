$(document).ready(function () {
    let currentPage = 1;
    let limit = $("#limitSelect").val();
    let search = $("#searchInput").val();
    GetListArea();

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
                $("#areaSelect").html(html);
                loadBedMatrix($("#selectArea").val());
            }
        });
    }
    $("#selectArea").change(function () {
        loadBedMatrix($(this).val());
    });

    function loadBedMatrix(areaId) {
        $.ajax({
            url: "/Bed/GetBedMatrix",
            type: "GET",
            data: { areaId: areaId },
            success: function (response) {
                if (response.success) {
                    let html = `<table class="table table-bordered text-center" style="border-collapse: unset;border-spacing: 10px; ">`;
                    let maxColumns = Math.max(...response.data.map(row => row.beds.length));
                    html += `<tbody>`;

                    response.data.forEach(row => {
                        html += `<tr>`;
                        row.beds.forEach(bed => {
                            let color = bed.isAvailable ? "bg-light" : "bg-danger text-white";
                            if (bed.isAvailable) {
                                html += `<td class="${color} align-content-center" style="border:1px solid #d5d5d5;">
                                    <div class="row g-3 justify-content-between">
                                        <div class="col-auto">${bed.name}</div>
                                        <div class="col-auto">
                                            <a href="#!" class="text-secondary btn-edit"
                                                data-id="${bed.id}" 
                                                data-name="${bed.name}" 
                                                data-rownumber="${bed.rowNumber}" 
                                                data-areaid="${bed.areaId}" 
                                                data-isavailable="${bed.isAvailable}" 
                                                data-bednumber="${bed.bedNumber}" ><i class="ti ti-edit"></i></a>
                                        </div>
                                    </div>
                                    <p class="border-dashed border-t pt-3 mt-2">Trống</p>
                                </td>`;
                            }
                            else {
                                html += `<td class="${color} align-content-center" title="Giường: ${bed.name}">
                                        <div class="row g-3 justify-content-between">
                                            <div class="col-auto">${bed.name}</div>
                                            <div class="col-auto">
                                                <a href="#!" class="text-white btn-edit"
                                                    data-id="${bed.id}" 
                                                    data-name="${bed.name}" 
                                                    data-rownumber="${bed.rowNumber}" 
                                                    data-isavailable="${bed.isAvailable}" 
                                                    data-areaid="${bed.areaId}" 
                                                    data-bednumber="${bed.bedNumber}" ><i class="ti ti-edit"></i></a>
                                            </div>
                                        </div>
                                        <p class="border-dashed border-t pt-3 mt-2">X</p>
                                </td>`;
                            }
                        });
                        html += `</tr>`;
                    });

                    html += `</tbody></table>`;
                    $("#bedMatrixContainer").html(html);
                }
            }
        });
    }

    $("#btnAdd").click(function () {
        $("#bedId").val(0); // Reset ID
        $("#name").val("");
        $("#rowNumber").val("");
        $("#bedNumber").val("");
        $("#chonguModal").modal("show");
    });
    $(document).on("click", ".btn-edit", function () {
        var id = $(this).data("id");
        var name = $(this).data("name");
        var rowNumber = $(this).data("rownumber");
        var bedNumber = $(this).data("bednumber");
        var isAvailable = $(this).data("isavailable");
        var areaSelect = $(this).data("areaid");

        $("#bedId").val(id);
        $("#name").val(name);
        $("#rowNumber").val(rowNumber);
        $("#bedNumber").val(bedNumber);
        $("#isAvailable").val(isAvailable+"");
        $("#areaSelect").val(areaSelect + "");
        console.log("area" + areaSelect);
        $("#chonguModal").modal("show");
    });
    $("#btnSave").click(function () {
        $.ajax({
            url: "/Bed/Savechange",
            type: "POST",
            data: {
                id: $("#bedId").val(),
                name: $("#name").val(),
                rowNumber: $("#rowNumber").val(),
                bedNumber: $("#bedNumber").val(),
                isAvailable: $("#isAvailable").val(),
                areaId: $("#areaSelect").val()
            },
            success: function (response) {
                if (response.success) {
                    $("#chonguModal").modal("hide");
                    loadBedMatrix($("#areaSelect").val()); // Load lại danh sách
                    showNotification("Đã cập nhật thành công!");
                } else {
                    showNotification(response.message);
                }
            }
        });
    });
});