$(function () {

    function addEvent(start, title, id, color, roomname) {
        var event = new Object();
        event.start = start;
        event.title = title;
        event.id = id;
        event.color = "#" + color;
        event.roomName = roomname;

        return event;
    }

    $.ajax({
        type: "POST",
        data: {},
        url: "GetFullCalendarCourse",
        dataType: "json",
        success: function (data) {
            $('#calendar').fullCalendar('destroy');
            $('#calendar').fullCalendar('render');

            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: ''
                },
                defaultView: 'month',
                editable: false,
                displayEventTime: true,
                events: $.map(data, function (item, i) {

                    var date = (moment.unix(moment.utc(data[i].Date).valueOf() / 1000)).format("YYYY-MM-DD");
                    var event = addEvent(date, data[i].CourseName, data[i].Id, data[i].DisplayColor, data[i].RoomName);

                    return event;
                }),
                eventClick: function (event) {
                    getCourseDetails(event.id);
                },
                height: 650,
            });
        }
    });


    function getCourseDetails(id) {

        $.ajax({
            type: "GET",
            url: "../Course/GetCourseById",
            dataType: "json",
            data: {
                id: id
            },
            success: function (data) {
                var row = '';
                var displaycolor = '';
                if (data) {
                    console.log(data);
                    switch (data.StatusDescription) {
                        case "Concluído":
                            displaycolor = "#2267d6";
                            break;
                        case "Cancelada":
                            displaycolor = "#c40000";
                            break;
                        case "Por Iniciar":
                            displaycolor = "#e6ac00";
                            break;
                        case "A decorrer":
                            displaycolor = "#008900";
                            break;
                    }
                    var date = (moment.unix(moment.utc(data.StartDate).valueOf() / 1000)).format("YYYY-MM-DD");
                    
                    $('#courseName').text(data.Name);
                    $('#courseEntityName').text(data.Entity);
                    $('#courseTeacher').text(data.Teacher);
                    $('#courseLevel').text(data.Level);
                    $('#courseDate').text(date);
                    $('#courseEffort').text(data.Effort);
                    $('#courseStatusDescription').text(data.StatusDescription);
                }
                
            },
        });
        $('#myModalShowCourseDetails').modal();
    }
});
