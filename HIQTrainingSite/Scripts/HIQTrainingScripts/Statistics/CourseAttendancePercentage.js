$(document).ready(function () {

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
        minLength: 2
    });

    function Graph() {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'GetCourseFullAttendance',
                data: {
                    courseId: $("#CourseId").val()
                },
                success: function (chartsdata) {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', "Type");
                    data.addColumn('number', "Percentage");

                    data.addRow(["Assitiram a mais de 70% das sessões", chartsdata.MoreThanLimit]);
                    data.addRow(["Assistiram a entre 0 e 70% das sessões", chartsdata.LimitAndBelow]);
                    data.addRow(["Não assistiram a qualquer sessão", chartsdata.NeverAttended]);

                    var options = {
                        title: "Patamares de assiduidade",
                        pieHole: 0.4,
                        sliceVisibilityThreshold: 0,
                        is3D: true
                    };

                    //options.series = {
                    //    0: {
                    //        color: 'transparent'
                    //    }
                    //}

                    var chart = new google.visualization.PieChart(document.getElementById('myGraph'));
                    chart.draw(data, options);
                },
                error: function () {
                    //swal(strings.Error, strings.ErrorMessage, "error");
                }
            });

        }


    }



})

