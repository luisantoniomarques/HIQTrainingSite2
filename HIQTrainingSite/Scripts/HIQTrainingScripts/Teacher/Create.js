
function StartautoComplete() {
        var selectedCompanyId = $("#SelectedCompanyId").val();

        $("#Name").on('input', function () {
            $("#Email").val("");
        });

        $("#SelectedCompanyId").on("change", function () {
            $("#Name").val("");
            $("#Email").val("");
        });


        $("#NameTeacher").autocomplete({
            source:
            function (request, response) {
                var companyId = $('#SelectedCompanyId').val();
                if (companyId) {
                    $.ajax({
                        url: "/Teacher/SearchUsersByName",
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
        $('#formularioTeacher').css('display', "block");
        $('#Email').attr('readonly', true);
        $("#checkboxExternal").prop('checked', false);
        getCompanies(false);
        $("#isExternal").val("false"); 
        StartautoComplete();
    }
    else {
        $('#formularioTeacher').css('display', "none");    
    }
    if (objclick.id === "checkboxExternal" && objclick.checked == true) {

        $('#Email').attr('readonly', false);
        $('#formularioTeacher').css('display', 'block');
        $("#checkboxInternal").prop('checked', false);  

        getCompanies(true);
        $("#isExternal").val("true");
  
 
    }

    if ($("#checkboxInternal")[0].checked == false && $("#checkboxExternal")[0].checked == false) {
        $('#formularioTeacher').css('display', 'none');
    
    }
};


function getCompanies(CompanieValue) {
    var options = $("#SelectedCompanyId");
    $.ajax({
        url: '/Teacher/SearchExternalCompany',
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



    // When a certain button is clicked, post your <form> data and URL to the Controller
    $("#SaveData").click(function () {
        // Serialize your form and add your location
        var formdata = $("#FormCourseId").serialize() + "&urlofRequest=" + escape(window.location.href);
        // Make your AJAX call
        $.ajax({
            url: '/Teacher/SaveTempData',
            data: formdata,
            success: function (data) {
                // Do something here when it is finished
            }
        });
    });
