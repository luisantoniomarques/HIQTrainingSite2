Portal.EditCourse = new (function () {
    var context = this;
    this.base = Portal;
    this.eventList = [];
    this.idcurrentEvent = 0;
    this.currentCulture = '';
    this.backColor = null;
    this.dp = new DayPilot.Month("dp");
    this.nav = new DayPilot.Navigator("nav");
    this.eventsRemoved = new DayPilot.Calendar();
    this.token = $('input[name="__RequestVerificationToken"]').val();

    this.init = function () {
        $(document).ready(function () {
            context.dp.width = "75%"
            context.dp.theme = "calendar_default";
            context.dp.startDate = new DayPilot.Date();
            context.dp.locale = context.currentCulture;
            context.dp.eventDeleteHandling = "Enabled";
            context.dp.weekStarts = 1;


            context.dp.onBeforeEventRender = function (args) {
                args.e.resizeDisabled = true;
            };

            context.dp.onBeforeCellRender = function (args) {
                var currentmonth = context.dp.startDate.getMonth() + 1;
                args.cell.headerHtml = args.cell.headerHtml.replace(/[^0-9\s]/gi, '');
                if (args.cell.start.getMonth() + 1 !== currentmonth) {
                    args.cell.headerHtml = "";
                }
            };

            context.dp.onEventMove = function (args) {
                var canCreateEvent = context.CanMoveOrCreateEvent(args.newStart, args.newEnd)
                if (canCreateEvent) {
                    var e = context.dp.events.find(args.e.id());
                    e.data.start = args.newStart.toString();
                    e.data.end = args.newEnd.toString();
                    context.dp.events.update(e);
                    context.eventList = context.dp.events.list;
                }
                else {
                    args.preventDefault();
                }
            };

            context.dp.onEventDelete = function (args) {
                var e = context.dp.events.find(args.e.id());
                context.dp.events.remove(e);
                context.eventsRemoved.events.add(e);
                context.eventList = context.dp.events.list;
                args.preventDefault();
            };

            context.dp.onTimeRangeSelected = function (args) {
                var canCreateEvent = context.CanMoveOrCreateEvent(args.start, args.end)

                if (canCreateEvent) {
                    var currentmonth = context.dp.startDate.getMonth() + 1;
                    var startDate = new Date(args.start);
                    var endDate = new Date(args.end);
                    endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate() - 1);
                    var eventmonth = endDate.getMonth() + 1;

                    if (currentmonth === eventmonth && (startDate.getMonth() + 1) === currentmonth) {
                        var e = new DayPilot.Event({
                            start: args.start,
                            end: args.end,
                            text: '',
                            id: context.idcurrentEvent + 1,
                            backColor: context.backColor,
                            isUpdated: true
                        });

                        context.dp.events.add(e);
                        context.eventList = context.dp.events.list;
                        context.idcurrentEvent = context.idcurrentEvent + 1;
                    }
                }
            };

            context.dp.init();
            context.dp.events.list = context.eventList;
            context.dp.update();

            context.nav.showMonths = 3;
            context.nav.skipMonths = 3;
            context.nav.selectMode = "Month";
            context.nav.locale = context.currentCulture;
            context.nav.onTimeRangeSelected = function (args) {
                context.dp.startDate = args.day;
                context.dp.update();
            };

            context.nav.init();
            context.nav.select(context.visibleDateStart);
            

            $("#btnSaveChanges").click(function () {visibleDateStart
                $('#eventsRemoved').val(JSON.stringify(context.eventsRemoved.events.list));
                $('#calendarEvents').val(JSON.stringify(context.eventList));
            });
        });

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

        if ($("#IsCanceled")[0].checked) {
            var el = document.getElementById('cancelobs');

            if (el.style.display === 'none') {
                el.style.display = 'inherit';
            } else if (el.style.display === 'inherit') {
                el.style.display = 'none';
            }
        }

        //$("#calendar").hide();



        //$("#id").click(function () {
        //    $("#calendar").toggle();
        //});

    };

    this.CanMoveOrCreateEvent = function (dateStart, dateEnd) {
        var canCreateEvent = true;
        $.each(context.eventList, function (index, item) {
            if ((dateStart >= item.start && dateStart < item.end)
                || (dateEnd > item.start && dateEnd <= item.end)
                || (dateStart.value === item.start)) {
                canCreateEvent = false;
            }
        });
        return canCreateEvent;
    }
});

