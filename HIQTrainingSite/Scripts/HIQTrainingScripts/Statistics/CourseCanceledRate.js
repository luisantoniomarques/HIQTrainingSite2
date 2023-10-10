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
                url: 'GetCourseCanceledRate',
                data: {
                    courseId: $("#CourseId").val()
                },
                success: function (chartsdata) {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', "Type");
                    data.addColumn('number', "Percentage");

                    data.addRow(["Cancelados", chartsdata.CanceledRate]);
                    data.addRow(["Outro estado", chartsdata.OtherRate]);

                    var options = {
                        title: "Taxa de cancelamento por formação",
                        pieHole: 0.4,
                        sliceVisibilityThreshold: 0,
                        is3D: true
                    };

                    options.series = {
                        0: {
                            color: 'transparent'
                        }
                    }

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

