$(function () {

    
    $("#Name").on('input', function () {
        $("#Email").val("");
    });

 
    $("#Name").autocomplete({
        source:
        function (request, response) {
            
                $.ajax({
                    url: "SearchExternalUsersByName",
                    type: "GET",
                    dataType: "json",
                    data: {
                        userName: request.term
                    },
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Name,
                                Email: item.Email
                            };
                        }))
                    }
                })
            
        },
        minLength: 2,
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
            $("#Email").val(ui.item.Email);
        }
    });

});
