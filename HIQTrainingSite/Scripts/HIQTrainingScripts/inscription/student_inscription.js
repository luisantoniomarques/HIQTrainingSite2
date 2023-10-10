
//*********************verificar de as dropdown estão checked*********//

if ($("#checkboxInternal2")[0].checked == true) {
    $('#formularioStudent2').css('display', "block");
 
}

if ($("#checkboxExternal2")[0].checked == true) {
    $('#formularioStudent2').css('display', "block");
    $("#Name").css('display', "none");
}

function StartautoComplete2() {
    var selectedCompanyId = $("#SelectedCompanyId").val();

    $("#studentName").on('input', function () {
        $("#EmailPartialStudent").val("");
    });

    $("#SelectedCompanyId").on("change", function () {
        $("#Name").val("");
        $("#EmailPartialStudent").val("");
    });


    $("#studentName").autocomplete({
        source:
        function (request, response) {
            var companyId = $('#Companies_SelectedCompanyId').val();
            if (companyId) {
                $.ajax({
                    url: "/student/SearchUsersByName",
                    type: "GET",
                    dataType: "json",
                    data: {
                        companyId: companyId,
                        userName: request.term,
                        createPartialStudent: null,
                        duplicateName: null,
                        isExternal: null
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
            $("#EmailPartialStudent").val(ui.item.Email);
        }
    });

}



var showForm2 = function () {
    var objclick = this;
    if (objclick.id === "checkboxInternal2" && objclick.checked == true) {
        $("#studentName").val("");
        $("#EmailPartialStudent").val("");
        $("#PhoneNumber").val("");
        $("#Observation").val("");
        $('#EmailPartialStudent').attr('readonly', true);
        $('#formularioStudent2').css('display', "block");      
        $("#checkboxExternal2").prop('checked', false);
        getCompanies2(false);
        $("#isExternalbyInscription").val("false");
        StartautoComplete2();
    }
    else {
        $('#formularioStudent2').css('display', "none");
    }
    if (objclick.id === "checkboxExternal2" && objclick.checked == true) {
        $("#studentName").val("");
        $("#EmailPartialStudent").val("");
        $("#PhoneNumber").val("");
        $("#Observation").val("");

        $('#EmailPartialStudent').attr('readonly', false);
        $('#formularioStudent2').css('display', 'block');
        $("#checkboxInternal2").prop('checked', false);


        getCompanies2(true);
        $("#isExternalbyInscription").val("true");

        $("#Name")[0].autocomplete = "off";


    }

    if ($("#checkboxInternal2")[0].checked == false && $("#checkboxExternal2")[0].checked == false) {
        $('#formularioStudent2').css('display', 'none');

    }
};


function getCompanies2(CompanieValue) {
    var options = $("#Companies_SelectedCompanyId");
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




$("#checkboxInternal2").on("click", showForm2);
$("#checkboxExternal2").on("click", showForm2);


// When a certain button is clicked, post your <form> data and URL to the Controller
$("#SaveData").click(function () {
    // Serialize your form and add your location
    var formdata = $("#FormInscriptionId").serialize() + "&urlofRequest=" + escape(window.location.href);
    // Make your AJAX call
    $.ajax({
        url: '/Inscription/SaveTempData',
        data: formdata,
        success: function (data) {
            // Do something here when it is finished
        }
    });
});




