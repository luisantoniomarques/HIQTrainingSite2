$(function () {

    var token = $('input[name="__RequestVerificationToken"]').val();

    $("#Teacher").autocomplete({
        source:
            function (request, response) {
                $.ajax({
                    url: "GetTeacherByName",
                    type: "GET",
                    dataType: "json",
                    data: {
                        teacherName: request.term
                    },
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Name,
                                Id: item.Id,
                                email: item.Email
                            };
                        }))
                    }, 
                    error: function (response) {
                        console.log(response);
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
            $("#TeacherId").val(ui.item.Id);
            $("#TeacherEmail").val(ui.item.email);
        },
        minLength: 2
    });


    $("#Teacher").on('input', function () {
        $("#TeacherId").val('');
        $("#TeacherEmail").val('');
    });


   // var arrayDates = [];

   // function addEvent(start, title, id, color) {
   //     var event = new Object();
   //     event.start = start;
   //     event.title = title;
   //     event.id = id;
   //     event.color = color;

   //     return event;
   // }

   // function checkAndAdd(date) { //vai verificar se o nome da formacao (e id) ja existem no array 
   //     var found = arrayDates.some(function (el) {
   //         return (el === date);
   //     });
   //     if (!found) { arrayDates.push(date); return true }//se ainda nao existir adiciona ao array
   // }

   // function min_date(all_dates) {

   //     var min_dt = all_dates[0],
			//min_dtObj = new Date(all_dates[0]);

   //     all_dates.forEach(function (dt, index) {
   //         if (new Date(dt) < min_dtObj) {
   //             min_dt = dt;
   //             min_dtObj = new Date(dt);
   //         }
   //     });

   //     return min_dt;
   // }

   // $('#calendar').fullCalendar({
   //     header: {
   //         left: 'prev,next today',
   //         center: 'title',
   //         right: ''
   //     },
   //     defaultView: 'month',
   //     editable: true,
   //     displayEventTime: false,
   //     dayClick: function (date, jsEvent, view) {
   //         var date = (moment.unix(moment.utc(date._d).valueOf() / 1000)).format("YYYY-MM-DD");
   //         var currentDate = (moment.unix(moment.utc(new Date()).valueOf() / 1000)).format("YYYY-MM-DD");

   //         if (checkAndAdd(date) && (date >= currentDate)) {
   //             var name = $("#Name").val();

   //             var event = addEvent(date, name);

   //             $("#CourseCalendar").val(arrayDates);

   //             $('#calendar').fullCalendar('renderEvent', event);
   //         }
   //     },
   //     eventClick: function (calEvent, jsEvent, view) {
   //         var date = (moment.unix(moment.utc(calEvent._start._d).valueOf() / 1000)).format("YYYY-MM-DD");

   //         var index = arrayDates.indexOf(date);
   //         if (index > -1) {
   //             arrayDates.splice(index, 1);
   //         }
   //         $("#CourseCalendar").val(arrayDates);
   //         $('#StartDate').val(min_date(arrayDates));

   //         $("#calendar").fullCalendar('removeEvents', calEvent._id);
   //     },
   //     contentHeight: 'auto'
   // });

    $('#StartHour').timepicker({
        'step': 30,
        timeFormat: 'HH:mm',
        defaultTime: '19:00'
    });

    $('#EndHour').timepicker({
        'step': 30,
        timeFormat: 'HH:mm',
        defaultTime: '21:00'
    });

    $('#StartDate').datepicker();
    $("#StartDate").datepicker("option", "dateFormat", "yy-mm-dd");

});
