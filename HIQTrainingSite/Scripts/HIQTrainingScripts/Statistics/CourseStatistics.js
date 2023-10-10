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

                    if ($("#CourseId").val() != "") {
                        var id = $("div .tab-pane.active").attr("id");

                        switch (id) {
                            case "successPane":
                                successGraph();
                                break;

                            case "attendancePane":
                                attendanceGraph();
                                break;

                            case "dropoutPane":
                                dropoutGraph();
                                break;

                            case "levelsPane":
                                levelsGraph();
                                break;
                        }
                    }
                },
                minLength: 2
            });

            $('#successTab').on('shown.bs.tab', function () {
                if ($("#CourseId").val() != "") {
                    successGraph();
                }
            });

            $('#attendanceTab').on('shown.bs.tab', function () {
                if ($("#CourseId").val() != "") {
                    attendanceGraph();
                }
            });

            $('#dropoutTab').on('shown.bs.tab', function () {
                if ($("#CourseId").val() != "") {
                    dropoutGraph()
                }
            });

            $('#levelsTab').on('shown.bs.tab', function () {
                if ($("#CourseId").val() != "") {
                    levelsGraph();
                }
            });

            function successGraph() {
                google.charts.load('current', { 'packages': ['corechart'] });
                google.charts.setOnLoadCallback(drawChart);

                function drawChart() {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: 'GetCourseSuccessRate',
                        data: {
                            courseId: $("#CourseId").val()
                        },
                        success: function (chartsdata) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', "Type");
                            data.addColumn('number', "Percentage");

                            data.addRow(["Aprovados", chartsdata.SuccessRate]);
                            data.addRow(["Reprovados", chartsdata.FailureRate]);

                            var options = {
                                title: "Taxa de sucesso por formação",
                                pieHole: 0.4,
                                sliceVisibilityThreshold: 0,
                                is3D: true
                            };

                            options.series = {
                                0: {
                                    color: 'transparent'
                                }
                            }

                            var chart = new google.visualization.PieChart(document.getElementById('successGraph'));
                            chart.draw(data, options);
                        },
                        error: function () { }
                    });
                }
            }

            function attendanceGraph() {
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

                            var chart = new google.visualization.ColumnChart(document.getElementById('attendanceGraph'));

                            chart.draw(data, options);

                        },
                        error: function () { }
                    });

                };

            }

            function dropoutGraph() {
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

                            var chart = new google.visualization.PieChart(document.getElementById('dropoutGraph'));
                            chart.draw(data, options);
                        },
                        error: function () { }
                    });
                }
            }

            function levelsGraph() {
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
                                title: "Assiduidade dos formandos",
                                pieHole: 0.4,
                                sliceVisibilityThreshold: 0,
                                is3D: true
                            };

                            var chart = new google.visualization.PieChart(document.getElementById('levelsGraph'));
                            chart.draw(data, options);
                        },
                        error: function () { }
                    });

                }


            }
            
        })