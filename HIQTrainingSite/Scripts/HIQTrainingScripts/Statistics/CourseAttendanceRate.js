$(function () {

    $("#CourseName").autocomplete({
        source:
            function (request, response) {
                $.ajax({
                    url: "GetCourseNamesByName",
                    type: "POST",
                    dataType: "json",
                    data: {
                        courseName: request.term
                    },
                    success: function (data) {
                        response($.map(data, function (item) {

                            return {
                                label: item.Name,
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
            $("#CourseId").val(ui.item.Id);

            if ($("#CourseId").val() != null) {
                Graph();
            }
        }, 
        minLength:2
    });


    function Graph() {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'GetCourseAttendanceRate',
                data: {
                    courseId: $("#CourseId").val()
                },
                success: function (chartsdata) {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', "Date");
                    data.addColumn('number', "Presenças");
                    data.addColumn('number', "Faltas");
                    data.addColumn('number', "Faltas Justificadas");

                    for (var i = 0; i < chartsdata.length; i++) {
                        var date = moment.unix(parseInt(chartsdata[i].Date.substring(6)) / 1000).format("DD/MM/YYYY");
                        data.addRow([date,
                            ((chartsdata[i]) ? (chartsdata[i].Attended) : 0),
                            ((chartsdata[i]) ? (chartsdata[i].NotAttended) : 0),
                            ((chartsdata[i]) ? (chartsdata[i].Justified) : 0)]);
                    }

                    var options = {
                        title: "",
                        isStacked: true,
                        hAxis: {
                            title: "Dias"
                        },
                        vAxis: {
                            title: "Presenças"
                        }
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('myGraph'));

                    chart.draw(data, options);

                },
                error: function () {
                    //swal(strings.Error, strings.ErrorMessage, "error");
                }
            });

        };

    }

});

