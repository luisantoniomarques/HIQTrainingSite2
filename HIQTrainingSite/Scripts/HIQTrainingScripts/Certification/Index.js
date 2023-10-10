$(function () {

    $('div#certificationResults').hide();

    $("#Button").autocomplete({
        source:
            function (request, response) {
                $.ajax({
                    url: "SearchUsersByName",
                    type: "POST",
                    dataType: "json",
                    data: {
                        userName: request.term
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                          
                            return {
                                label: item.StudentName,
                                email: item.StudentEmail,
                                studentId: item.StudentId
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
           
            $("#StudentName").val(ui.item.label);
        }
    });

    $('#Button').keyup(function () {
        $('div#certificationResults').hide();
        var name = $(this).val();

        $.ajax({
            url: '/Certification/_CertificationIndex?nome=' + name,
            type: 'get',
            datatype: 'json',
            success: function (data) {
                $('div#searchResults').html(data);
            }
        });

    });

});

