($scope.showBuscando = false);
($scope.showSeleccioneAlerta = true);
($scope.showGrafico = false);
($scope.pasos = [{ id: 0, label: "0 - Año-Semana-Dia Tipico-Bloque" }, { id: 1, label: "1 - Año" }, { id: 2, label: "2 - Año-Semana" }]);
($scope.filtroValores = [{ id: 0, label: "Min" }, { id: 1, label: "Percentil25" }, { id: 2, label: "Promedio" }, { id: 3, label: "Percentil75" }, { id: 4, label: "Max" }]);
($scope.filterObject = {selectedPaso: $scope.pasos[2], selectedfiltroValores: $scope.filtroValores[2]});

($scope.ObtenerExcel = function () {
    var fileName = "Excel_" + moment().format('YYYYMMDD_HHmmss') + ".xlsx";
    if ($scope.filtro != null) {
        var Ejecutar = {
            GraficoId: $scope.item.id,
            Metodo: "GetExcel",
            Parametros: [$scope.schema, $scope.filtro]
        }
        ajax.postDownloadFile("Grafico", "Ejecutar", Ejecutar, fileName, "xlsx", function (data) {

            $scope.ChartCollection = data;
        }, $scope.onDefaultReturnFault);
    }
});

($scope.ObtenerPDF = function () {
    var fileName = "Pdf_" + moment().format('YYYYMMDD_HHmmss') + ".pdf";
    if ($scope.filtro != null) {
        var Ejecutar = {
            GraficoId: $scope.item.id,
            Metodo: "GetPDF",
            Parametros: [$scope.schema, $scope.filtro]
        }
        ajax.postDownloadFile("Grafico", "Ejecutar", Ejecutar, fileName, "pdf", function (data) {

            $scope.ChartCollection = data;
        }, $scope.onDefaultReturnFault);
    }
});

($scope.ObtenerGraficos = function () {
    //$scope.schema = viene como parametro 
    $scope.filtro = 'FiltroX';   
    $scope.showGrafico = false;
    $scope.ChartCollection = { charts: [] };
    if ($scope.filtro != null) {        
        $scope.showSeleccioneAlerta = false;
        $scope.showBuscando = true;

        var Ejecutar = {
            GraficoId: $scope.item.id,
            Metodo: "ObtenerGraficos",
            Parametros: [$scope.schema, $scope.filtro]
        }
        ajax.post("Grafico", "Ejecutar", Ejecutar, function (data) { 
            $scope.ChartCollection = data;
            $scope.showGrafico = true;
            $scope.showBuscando = false;
        }, $scope.onDefaultReturnFault);
    }
});
($scope.ObtenerGraficos());

($scope.options = {
    "chart": {
        "type": "multiChart",
        "height": 500,
        "margin": {
            "top": 100,
            "right": 20,
            "bottom": 75,
            "left": 50
        },
        "useVoronoi": false,
       "clipEdge": true,
       "duration": 100,
       "useInteractiveGuideline": true,
        "xAxis": {
            "rotateLabels": 26,
            "axisLabel": "UnidadesX",
            "showMaxMin": false,
            "tickFormat": function(d){
                return $scope.ChartCollection.labelsX[d.toString()]
            }
        },
        "yAxis1": {
            "axisLabel": "UnidadesY",
            "axisLabelDistance": -10,
            "tickFormat": function(d){
                return d3.format(".1f")(d);
            }
        },
        "legend": {
            "align": false
        }
    }
});