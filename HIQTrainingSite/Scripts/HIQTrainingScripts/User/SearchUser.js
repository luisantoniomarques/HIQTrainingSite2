$("#Name").autocomplete({
    source:
    function (request, response) {
        var companyId = $('#SelectedCompanyId').val();
        if (companyId) {
            $.ajax({
                url: "/student/SearchUsersByName",
                type: "GET",
                dataType: "json",
                data: {
                    companyId: companyId,
                    userName: request.term
                },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name,
                            Email: item.Email,
                            CompanyName: item.CompanyName
                        };
                    }))
                }
            })
        }
    },
    minLength: 2,
    select: function (event, ui) {
        event.preventDefault();
        $(this).val(ui.item.label);
        $("#Email").val(ui.item.Email);
    }
});

