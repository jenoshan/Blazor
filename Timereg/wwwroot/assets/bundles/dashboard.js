"use strict";

(function () {

    function dashboardVM() {
        let self = this;

       
        self.exportReport = function (filename, bytesBase64) {
            var link = document.createElement('a');
            link.download = filename;
            link.href = "data:application/octet.stream;base64," + bytesBase64;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }

        self.createDoughnutChart = function (data) {
            /* Donut Chart */
            try {
                var chart = document.getElementById('total-cost-assignment-cost-donut');
                self.doughnutChart = echarts.init(chart);
                self.doughnutChartRefreshData(data);
            } catch (e) {
                console.log(e.name + ': ' + e.message);
            }
        }

        self.doughnutChartRefreshData = function (data) {
            try {
                if (!self.doughnutChart)
                    self.createDoughnutChart(data);
                if (self.doughnutChart && data) {
                    self.doughnutChart.setOption({
                        tooltip: {
                            trigger: "item",
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        calculable: !0,
                        legend: {
                            x: "center",
                            y: "bottom",
                            textStyle: { color: '#9aa0ac' },
                            data: Object.keys(data)
                        },
                        series: [{
                            name: "Projectname",
                            type: "pie",
                            radius: ["35%", "55%"],
                            itemStyle: {
                                normal: {
                                    label: {
                                        show: !0
                                    },
                                    labelLine: {
                                        show: !0
                                    }
                                },
                                emphasis: {
                                    label: {
                                        show: !0,
                                        position: "center",
                                        textStyle: {
                                            fontSize: "14",
                                            fontWeight: "normal"
                                        }
                                    }
                                }
                            },
                            data: $(Object.keys(data)).map((i, costType) => { return { value: data[costType], name: costType }; })
                        }]
                    });
                }
            } catch (e) {
                console.log(e.name + ': ' + e.message);
            }

        }

        self.createMorisLineChart = function (data) {
            try {
                if (data && data.length > 0) {
                    let dataKeys = Object.keys(data[0]).filter(k => k != "year");
                    self.assignmentPerYearLineChart = Morris.Line({
                        element: 'assignment-per-year',
                        data: data,
                        xkey: 'year',
                        ykeys: dataKeys,
                        labels: $(dataKeys).map((i, v) => v.charAt(0).toUpperCase() + v.slice(1)),
                        pointSize: 3,
                        fillOpacity: 0,
                        //pointStrokeColors: ['#222222', '#cccccc', '#f96332'],
                        behaveLikeLine: true,
                        gridLineColor: '#e0e0e0',
                        lineWidth: 2,
                        hideHover: 'auto',
                        //lineColors: ['#222222', '#20B2AA', '#f96332'],
                        resize: true
                    });
                }
            } catch (e) {
                console.log(e.name + ': ' + e.message);
            }

        }

        self.morisLineChartRefreshData = function (data) {
            try {
                if (data && data.length > 0) {
                    self.assignmentPerYearLineChart.setData(data);
                }
            } catch (e) {
                console.log(e.name + ': ' + e.message);
            }

        }
    };

    window.DashboardVM = new dashboardVM();
})();
