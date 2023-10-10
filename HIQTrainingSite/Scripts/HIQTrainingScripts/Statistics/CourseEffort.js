$(document).ready(function () {

    $('#clearButton').on('click', function () {
        $("#CourseType option:eq(0)").prop("selected", true);
        $("#SelectedMonth option:eq(0)").prop("selected", true);
        $("#SelectedYear option:eq(0)").prop("selected", true);
    });

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart(url, data) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: url,
            data: data,
            success: function (chartsdata) {
                var data = new google.visualization.DataTable();
                data.addColumn('string', "Carga");
                data.addColumn('number', "Número de formações");

                for (var i = 0; i < chartsdata.EffortsCount; i++) {
                    //data.addRow([1, 1]);
                    var efforts = chartsdata.Efforts[i] + " horas";
                    var repetitions = chartsdata.EffortsRepetition[i];
                    data.addRow([efforts, repetitions]);
                }

                var options = {
                    title: "",
                    isStacked: false,
                    hAxis: {
                        title: "Horas de carga"
                    },
                    vAxis: {
                        title: "Número de formações"
                    }                  
                };

                //options.series = {
                //    0: {
                //        color: 'transparent'
                //    }
                //}

                var chart = new google.visualization.ColumnChart(document.getElementById('myGraph'));
                chart.draw(data, options);
            },
            error: function () {
                //swal(strings.Error, strings.ErrorMessage, "error");
            }
        });
    }

    $('#dateButton').click(function Graph() {

        var date = new Date($('#Date').val());
        var month = $('#SelectedMonth').val();
        var year = $('#SelectedYear').val();

        if (month === "") month = -1;
        if (year === "") year = -1;

        var data = {
            month: month,
            year: year
        };

        if (month !== -1 || year !== -1)
        {
            drawChart('GetCourseEffortByPeriod', data);
        }
        
    });

    $('#typeButton').click(function Graph() {

        var type = $('#CourseType').val();

        var data = {courseType: type};

        if (type !== "") drawChart('GetCourseEffortByCourseType', data);

    });
});

