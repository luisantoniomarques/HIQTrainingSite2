$(function () {

    $("#Teacher").autocomplete({
        source:
        function (request, response) {
            $.ajax({
                url: "GetTeacherByName",
                type: "POST",
                dataType: "json",
                data: {
                    teacherName: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {

                        return {
                            label: item.Name + " (" + item.Company + ")",
                            Id: item.Id,
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
            $("#TeacherId").val(ui.item.Id);
        }
    });

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




        $.ajax({
            type: "GET",
            url: "/Course/GetCourseDates",
            dataType: "json",
            data: {
                id: $("#Id").val()
            },
            success: function () {
                $('#calendar').fullCalendar({
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: ''
                    },
                    defaultView: 'month',
                    editable: true,
                    displayEventTime: false,
                    dayClick: function (date, jsEvent, view) {
                        var date = (moment.unix(moment.utc(date._d).valueOf() / 1000)).format("YYYY-MM-DD");
                        var currentDate = (moment.unix(moment.utc(new Date()).valueOf() / 1000)).format("YYYY-MM-DD");

                        if (checkAndAdd(date) && (date >= currentDate)) {
                            var name = $("#Name").val();

                            var event = addEvent(date, name);

                            $("#CourseCalendar").val(arrayDates);

                            $('#calendar').fullCalendar('renderEvent', event);
                        }
                    },
                    eventClick: function (calEvent, jsEvent, view) {
                        var date = (moment.unix(moment.utc(calEvent._start._d).valueOf() / 1000)).format("YYYY-MM-DD");

                        var index = arrayDates.indexOf(date);
                        if (index > -1) {
                            arrayDates.splice(index, 1);
                        }
                        $("#CourseCalendar").val(arrayDates);
                        $('#StartDate').val(min_date(arrayDates));

                        $("#calendar").fullCalendar('removeEvents', calEvent._id);
                    },
                    contentHeight: 'auto'
                });
            }
        });

  

    $('#StartHour').timepicker({
        'step': 30,
        timeFormat: 'HH:mm'
    });

    $('#EndHour').timepicker({
        'step': 30,
        timeFormat: 'HH:mm'
    });


    $("#ConfirmButton").click(function () {
        $.ajax({
            url: "DisabledCourseDay",
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

    $('#StartDate').datepicker();

});
