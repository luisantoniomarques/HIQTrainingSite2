Portal.Notes = new (function () {
    var context = this;
    this.base = Portal;

    this.eventList = [];

    this.idcurrentEvent = 0;
    this.currentCulture = '';
    this.eventsRemoved = new DayPilot.Calendar();
    this.eventAction = null;
    this.eventArgs = null;
    this.userCreated = null;
    this.visibleDateStart = null;
    this.barColor = null;

    this.dp = new DayPilot.Calendar("dp");
    this.nav = new DayPilot.Navigator("nav");

    this.messagesText = {
        confirmDeleteText: '',
        confirmClientDeleteText: '',
        confirmSaveText: '',
        invalidDataFieldError: '',
        invalidEmail: ''
    };

    this.init = function () {
        $(document).ready(function () {
            context.dp.viewType = "Week";
            context.dp.width = "100%"
            context.dp.businessBeginsHour = 8;
            context.dp.locale = context.currentCulture;
            context.dp.theme = "calendar_default";
            context.dp.businessEndsHour = 22;

            context.dp.height = "";
            context.dp.heightSpec = 'BusinessHoursNoScroll';

            context.dp.timeFormat = "Clock24Hours";
            context.dp.eventDeleteHandling = "Enabled";
            context.dp.weekStarts = 1;
            context.dp.headerDateFormat = "d dddd";
            context.dp.timeHeaderCellDuration = 30;

            $('#errorDate').text("");

            context.dp.onBeforeTimeHeaderRender = function (args) {
                var hour = args.header.hours + ":" + args.header.minutes;
                var position = hour.indexOf(":", hour.indexOf(":") + 1);
                args.header.html = hour.substr(0, position);
            };

            context.dp.onBeforeEventRender = function (args) {
                if (args.e.UserCreated !== context.userCreated) {
                    args.data.deleteDisabled = true;
                    args.e.moveDisabled = true;
                    args.e.resizeDisabled = true;
                }
            };


            context.dp.onEventDelete = function (args) {
                context.eventArgs = args;
                context.eventAction = "delete";
                $(".actionDelete").css({ "display": "block" });
                $(".actionAdd").css({ "display": "none" });
                $("#myModal").modal();
            };

            context.dp.onEventMoved = function (args) {
                context.eventArgs = args;
                if (context.eventArgs.e.data.UserCreated === context.userCreated) {
                    context.onEventPositionChanged();
                }
            };


            context.dp.onEventResized = function (args) {
                context.eventArgs = args;
                if (context.eventArgs.e.data.UserCreated === context.userCreated) {
                    context.onEventPositionChanged();
                }
            };

            context.dp.onTimeRangeSelected = function (args) {
                context.eventArgs = args;
                context.eventAction = "add";
                $('#eventText').val();
                $(".actionDelete").css({ "display": "none" });
                $(".actionAdd").css({ "display": "block" });
                $("#myModal").modal();
            };


            context.dp.onEventClick = function (args) {
                context.eventArgs = args;
                if (context.eventArgs.e.data.UserCreated === context.userCreated) {
                    context.eventAction = "edit";
                    $(".actionDelete").css({ "display": "none" });
                    $(".actionAdd").css({ "display": "block" });
                    $('#eventText').val(context.eventArgs.e.text());
                    $("#myModal").modal();
                }
            };

            context.dp.init();
            context.dp.events.list = context.eventList;
            context.dp.update();

            context.nav.showMonths = 3;
            context.nav.skipMonths = 3;
            context.nav.selectMode = "week";
            context.nav.weekStarts = 1;
            context.nav.locale = context.currentCulture;
            context.nav.onTimeRangeSelected = function (args) {
                context.dp.startDate = args.day;
                context.dp.update();
                context.loadEvents();
            };

            context.nav.init();
            context.nav.select(context.visibleDateStart);


            $('#ModalConfirmButton').click(function (e) {

                if (context.eventAction === "delete") {
                    var e = context.dp.events.find(context.eventArgs.e.id());

                    e.data.isDeleted = true;
                    context.dp.events.update(e);

                    context.eventsRemoved.events.add(e);
                    context.dp.events.remove(e);

                    context.eventList = context.dp.events.list;
                    context.eventArgs.preventDefault();
                }

                if (context.eventAction === "add") {
                    var text = $('#eventText').val();
                    context.dp.clearSelection();

                    if (!text) return;
                    var e = new DayPilot.Event({
                        start: context.eventArgs.start,
                        end: context.eventArgs.end,
                        id: context.idcurrentEvent + 1,
                        resource: context.eventArgs.resource,
                        EventStart: context.eventArgs.start,
                        EventEnd: context.eventArgs.end,
                        isDeleted: false,
                        isUpdated: false,
                        text: text,
                        barColor: context.barColor,
                        deleteDisabled: false
                    });

                    context.dp.events.add(e);
                    context.eventList = context.dp.events.list;
                    context.idcurrentEvent = context.idcurrentEvent + 1;
                }

                if (context.eventAction === "edit") {
                    var text = $('#eventText').val();
                    if (text != context.eventArgs.e.text() || text != null) {
                        context.eventArgs.e.data.text = text;
                        context.eventArgs.e.data.isUpdated = true;
                        context.dp.events.update(context.eventArgs.e);
                        context.eventList = context.dp.events.list;
                    }
                }
                $('#myModal .close').click();
            });


            $('#ModalReturnButton').click(function (e) {
                $('#myModal .close').click();
            });

            $("#btnSearchEvents").click(function () {

                if ($('#dpDateStart').val()) {
                    if (IsDate($('#dpDateStart').val())) {
                        context.dp.startDate = $('#dpDateStart').datepicker('getDate').toISOString();
                        context.dp.update();
                        context.loadEvents();
                    }
                    } else {
                        //Modal para mostrar erro
                        $(".actionDelete").css({ "display": "none" });
                        $(".actionAdd").css({ "display": "none" });
                        $(".MessageError").css({ "display": "block" });
                        $("#ModalConfirmButton").css({ "display": "none" });
                        $("#myModal").modal();
                    }


                });

            $("#btnSaveChanges").click(function () {
                $('#token').val($('input[name="__RequestVerificationToken"]').val());
                $('#eventsRemoved').val(JSON.stringify(context.eventsRemoved.events.list));
                $('#noteList').val(JSON.stringify(context.eventList));
                $('#startDate').val(context.dp.visibleStart());
                $('#endDate').val(context.dp.visibleEnd());
            });

            $("#btnSearchClean").click(function () {
                $('#errorDate').text("");
            });

            $('#dpDateStart').datepicker({ dateFormat: 'dd-mm-yy' }).val();
        });
    };

    this.onEventPositionChanged = function () {
        var e = context.dp.events.find(context.eventArgs.e.id());
        e.data.start = context.eventArgs.newStart.toString();
        e.data.end = context.eventArgs.newEnd.toString();
        e.data.EventStart = context.eventArgs.newStart;
        e.data.EventEnd = context.eventArgs.newEnd;
        e.data.isUpdated = true;
        context.dp.events.update(e);
        context.eventList = context.dp.events.list;
    }


    this.loadEvents = function () {
        var start = context.dp.visibleStart();
        var end = context.dp.visibleEnd();

        $.post("events",
            {
                startDate: start.toString(),
                endDate: end.toString()
            },
            function (data) {
                context.dp.events.list = data.events;
                context.idcurrentEvent = data.lastEventId;
                context.dp.update();
                context.nav.select(start);
            });
    }

});


function IsDate(txtDate) {

    var rxDatePattern = /^(\d{1,2})(\.|-)(\d{1,2})(\.|-)(\d{4})$/; //Declare Regex

    return txtDate.match(rxDatePattern); //Returns true iff. currval is a valid date
};

