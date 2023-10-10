$(function () {

    $("#Name").autocomplete({
        source:
        function (request, response) {
            $("#Code").val("");
            $.ajax({
                url: "/Certification/SearchCertificationByName",
                type: "GET",
                dataType: "json",
                data: {
                    certificationName: request.term
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        console.log(item);
                        return {
                            label: item.Name,
                            code: item.Code,
                            id: item.Id
                        };
                    }))
                }
            })
        },

        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
        },
        select: function (event, ui) {
            event.preventDefault();

            $(this).val(ui.item.label);
            $("#Code").val(ui.item.code);
            $("#CertificationTypeId").val(ui.item.id);

        }
    });
});

