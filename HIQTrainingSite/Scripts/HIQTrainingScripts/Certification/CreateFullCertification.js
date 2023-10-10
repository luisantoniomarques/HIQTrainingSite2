$(function () {

    $("#StudentName").autocomplete({
        source:
            function (request, response) {
                $.ajax({
                    url: "GetStudentsByName",
                    type: "POST",
                    dataType: "json",
                    data: {
                        userName: request.term
                    },
                    success: function (data) {
                        response($.map(data, function (item) {

                            return {
                                label: item.Name,
                                email: item.Email, 
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
            $("#StudentEmail").val(ui.item.email);
            $("#StudentId").val(ui.item.id);
        }
    });

    $("#Name").autocomplete({
        source:
            function (request, response) {
                $.ajax({
                    url: "SearchCertificationByName",
                    type: "POST",
                    dataType: "json",
                    data: {
                        certificationName: request.term
                    },
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
            $("#CertifictionTypeId").val(ui.item.id);

        }
    });

    $("#Name").keyup(function () {
        $("#Code").val('');
    });

    $("#StudentName").keyup(function () {
        $("#StudentEmail").val('');
    });

});

