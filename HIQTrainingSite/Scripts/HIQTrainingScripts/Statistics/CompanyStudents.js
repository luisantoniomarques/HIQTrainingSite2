$(document).ready(function () {

            $('#clearButton').on('click', function () {
                $("#CompanyId option:eq(0)").prop("selected", true);
                $("#CourseType option:eq(0)").prop("selected", true);
                $("#SelectedMonth option:eq(0)").prop("selected", true);
                $("#SelectedYear option:eq(0)").prop("selected", true);
            });

            google.charts.load('current', { 'packages': ['corechart'] });
            //google.charts.setOnLoadCallback(drawChart);

            function monthYearChart(data) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'GetCompanyStudentsByPeriod',
                    data: data,
                    success: function (chartsdata) {
                        var data = new google.visualization.DataTable();
                        data.addColumn('string', "Mês");
                        data.addColumn('number', "Número de formandos");

                        for (var i = 0; i < chartsdata.length; i++) {
                            //data.addRow([1, 1]);
                            var week = "Semana " + (i +1);
                            var value = chartsdata[i];
                            data.addRow([week, value]);
                        }

                        var options = {
                            title: "",
                            isStacked: false,
                            hAxis: {
                                title: "Semanas"
                            },
                            vAxis: {
                                title: "Número de formandos"
                            }
                        };

                        var chart = new google.visualization.ColumnChart(document.getElementById('myGraph'));
                        chart.draw(data, options);
                    },
                    error: function () {

                    }
                });
            }

            function yearChart(data) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'GetCompanyStudentsByPeriod',
                    data: data,
                    success: function (chartsdata) {
                        var data = new google.visualization.DataTable();
                        data.addColumn('string', "Mês");
                        data.addColumn('number', "Número de formandos");

                        for (var i = 0; i < chartsdata.length; i++) {
                            //data.addRow([1, 1]);
                            var month = getMonthName(i + 1);
                            var value = chartsdata[i];
                            data.addRow([month, value]);
                        }

                        var options = {
                            title: "",
                            isStacked: false,
                            hAxis: {
                                title: "Meses"
                            },
                            vAxis: {
                                title: "Número de formandos"
                            }
                        };

                        var chart = new google.visualization.ColumnChart(document.getElementById('myGraph'));
                        chart.draw(data, options);
                    },
                    error: function () {

                    }
                });
            }

            function courseTypeChart(data) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'GetCompanyStudentsByCourseType',
                    data: data,
                    success: function (chartsdata) {
                        var data = new google.visualization.DataTable();
                        data.addColumn('string', "Tipo");
                        data.addColumn('number', "Número de formandos");
                        data.addRow([$('#CourseType').val(), chartsdata]);


                        var options = {
                            title: "",
                            isStacked: false,
                            hAxis: {
                                title: "Tipo de formação"
                            },
                            vAxis: {
                                title: "Número de formandos"
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

            function getMonthName(number){

                switch (number) {
                    case 1:
                        return "Jan";
                        break;
                    case 2:
                        return "Fev";
                        break;
                    case 3:
                        return "Mar";
                        break;
                    case 4:
                        return "Abr";
                        break;
                    case 5:
                        return "Mai";
                        break;
                    case 6:
                        return "Jun";
                        break;
                    case 7:
                        return "Jul";
                        break;
                    case 8:
                        return "Ago";
                        break;
                    case 9:
                        return "Set";
                        break;
                    case 10:
                        return "Out";
                        break;
                    case 11:
                        return "Nov";
                        break;
                    case 12:
                        return "Dez";
                        break;
                    default:
                        return "Not recognized";
                }
            }

            $('#dateButton').click(function () {

                var id = $('#CompanyId').val();
                var month = $('#SelectedMonth').val();
                var year = $('#SelectedYear').val();
                var data;

                if(id !== ""){
                    if (month !== "" && year !== "") {
                        data = {
                            companyId: id,
                            month: month,
                            year: year
                        }
                        monthYearChart(data)
                    }
                    else if (year !== "") {
                        data = {
                            companyId: id,
                            month: -1,
                            year: year
                        }
                        yearChart(data)
                    }
                    else if (month !== ""){
                        data = {
                            companyId: id,
                            month: month,
                            year: -1
                        }
                        monthYearChart(data)
                    }
                }

            });

            $('#typeButton').click(function () {

                var id = $('#CompanyId').val();
                var type = $('#CourseType').val();

                var data = {
                    companyId: id,
                    courseType: type
                };

                if (id !== "" && type !== "") courseTypeChart(data);

            });

        });