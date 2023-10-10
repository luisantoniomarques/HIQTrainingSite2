﻿if ($("#checkboxInternal")[0].checked == true) {
    $('#formularioStudent').css('display', "block");

    
}

if ($("#checkboxExternal")[0].checked == true) {
    $('#formularioStudent').css('display', "block");
    $("#Name").css('display', "none");

}

function StartautoComplete() {
    var selectedCompanyId = $("#SelectedCompanyId").val();

    $("#Name").on('input', function () {
        $("#Email").val("");
    });

    $("#SelectedCompanyId").on("change", function () {
        $("#Name").val("");
        $("#Email").val("");
    });


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

}







var showForm = function () {
    var objclick = this;
    if (objclick.id === "checkboxInternal" && objclick.checked == true) {
        $('#formularioStudent').css('display', "block");
        $('#EmailPartialStudent').attr('readonly', true);
        $("#checkboxExternal").prop('checked', false);
        getCompanies(false);
        $("#isExternal").val("false");

        StartautoComplete();
    }
    else {
        $('#formularioStudent').css('display', "none");
    }
    if (objclick.id === "checkboxExternal" && objclick.checked == true) {
        $('#EmailPartialStudent').attr('readonly', false);
        $('#formularioStudent').css('display', 'block');
        $("#checkboxInternal").prop('checked', false);


        getCompanies(true);
        $("#isExternal").val("true");

        $("#Name")[0].autocomplete = "off";


    }

    if ($("#checkboxInternal")[0].checked == false && $("#checkboxExternal")[0].checked == false) {
        $('#formularioStudent').css('display', 'none');

    }
};


function getCompanies(CompanieValue) {
    var options = $("#SelectedCompanyId");
    $.ajax({
        url: '/student/SearchExternalCompany',
        type: 'GET',
        data: { external: CompanieValue },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            options.empty();
            $.each(data, function () {
                options.append($("<option/>").val(this.Id).text(this.Name));
            });
        }
    })
}




$("#checkboxInternal").on("click", showForm);
$("#checkboxExternal").on("click", showForm);


