$(function () {

    var token = $('input[name="__RequestVerificationToken"]').val();
    

    var arrayDates = [];//Contem todas as datas 
    var newArray = [];//Os que foram adicionados/removidos no edit 
    var auxDate = null;
    var id = null;
    var theEvent = null;

    function addEvent(start, title, id, color) {
        var event = new Object();
        event.start = start;
        event.title = title;
        event.id = id;
        event.color = color;

        return event;
    }

    function checkAndAdd(date) { //vai verificar se o nome da formacao (e id) ja existem no array 
        var found = arrayDates.some(function (el) {
            return (el === date);
        });
        if (!found) { arrayDates.push(date); return true }//se ainda nao existir adiciona ao array
    }

    function checkAndRemove(date) {
        //   var found = arrayDates.some(function(el))


    }

    function min_date(all_dates) {

        var min_dt = all_dates[0],
            min_dtObj = new Date(all_dates[0]);

        all_dates.forEach(function (dt, index) {
            if (new Date(dt) < min_dtObj) {
                min_dt = dt;
                min_dtObj = new Date(dt);
            }
        });

        return min_dt;
    }



   

        $.ajax({
            type: "GET",
            data: { id: $("#Id").val() },
            url: "/Course/GetCourseDates",
            dataType: "json",
            success: function (data) {
                $('#calendar').fullCalendar({
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month'
                    },
                    defaultView: 'month',
                    editable: true,
                    displayEventTime: false,
                    events: $.map(data, function (item, i) {
                        var date = (moment.unix(moment.utc(data[i].Date).valueOf() / 1000)).format("YYYY-MM-DD");

                        if (data[i].Status == 0) {
                            var event = addEvent(date, $("#Name").val(), data[i].Id, '#8B0000');
                        } else {
                            var event = addEvent(date, $("#Name").val(), data[i].Id);
                        }

                        arrayDates.push(event.start);

                        return event;
                    }),
                    contentHeight: 'auto'
                });
            }
        });

    $("#ConfirmButton").click(function () {
        $.ajax({
            url: "/Course/DisabledCourseDay",
            type: "POST",
            dataType: "json",
            data: {
                courseId: $("#Id").val(),
                date: auxDate,
                observation: $("#CalendarDetails_Observation").val()
            },
            success: function (data) {
                theEvent.color = '#8B0000';
                $('#calendar').fullCalendar('refetchEvents');

                $('#fullCalModal').modal('hide');

            }
        });

    });
});
