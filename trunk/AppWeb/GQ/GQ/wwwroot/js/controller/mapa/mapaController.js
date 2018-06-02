﻿try {
    for (var i = 0; i < mapMarkers.length; i++) {
        mapMarkers[i].setMap(null);
    }
}
catch (error) { }
mapMarkers = [];

app.controller('appController', function ($scope, NgTableParams, ajax, $uibModal, $rootScope, NgMap) {

    $scope.googleKey = "AIzaSyB-ACuHbgpwOm0fvOJ-v9w15Kq4-dt9WDo";
    $scope.googleMapsUrl = "https://maps.google.com/maps/api/js?key=" + $scope.googleKey;

    $scope.over = "";

    $scope.onSelectedNodo = function (nodoId) {
        //Logica para cuando seleccione un punto

        //if (nodoId !== null && $scope.filterObject.selected !== null) {
        //    if ($scope.filterObject.selected.tipoSeleccion === 1) {
        //        $scope.selectedNodo = nodoId;
        //        //$scope.onApply();
        //    }
        //}
    }

    $scope.onOverNodo = function (nodoId) {
        //if (nodoId !== null && $scope.filterObject.selected !== null) {
        //    if ($scope.filterObject.selected.tipoSeleccion === 1) {
        //        $scope.over = nodoId;
        //        //$scope.onApply();
        //    }
        //}

        $scope.over = nodoId;
    }

    $scope.onSelectedLinea = function (event, lineaId) {
        //Logica para cuando seleccione una linea

        //if (lineaId !== null && $scope.filterObject.selected !== null) {
        //    if ($scope.filterObject.selected.tipoSeleccion === 2) {
        //        $scope.selectedNodo = lineaId;
        //    }
        //}
    }

    $scope.onOverLinea = function (event, lineaId) {
        //if (lineaId !== null && $scope.filterObject.selected !== null) {
        //    if ($scope.filterObject.selected.tipoSeleccion === 2) {
        //        $scope.over = lineaId;
        //    }
        //}

        $scope.over = lineaId;
    }

    $scope.onOut = function (event, lineaId) {
        $scope.over = "";
        //$scope.onApply();
    }

    $scope.markers = [];
    $scope.shapes = [];

    $scope.ClearMarkers = function () {
        if ($scope.markers.length > 0) {
            for (var i = 0; i < $scope.markers.length; i++) {
                $scope.markers[i].setMap(null);
            }
        }
        $scope.markers = [];
    }

    $scope.AddMarkers = function (data) {
        var markers = [];
        var shapes = [];

        for (var i = 0; i < data.length; i++) {
            if (data[i].type === "marker") {
                markers.push(data[i]);
            }
            else {
                shapes.push(data[i]);
            }
        }
        $scope.shapes = shapes;

        mapMarkers = [];
        for (var i = 0; i < markers.length; i++) {
            var marker = new MarkerWithLabel({
                id: markers[i].id,
                position: new google.maps.LatLng(markers[i].position[0], markers[i].position[1]),
                map: $scope.map,
                title: markers[i].label,
                labelContent: markers[i].label,
                labelAnchor: new google.maps.Point(10, 22),
                labelClass: "map-Label", // the CSS class for the label
                icon: {
                    path: google.maps.SymbolPath.CIRCLE,
                    scale: 4,
                    strokeColor: markers[i].fillColor,
                    fillColor: 'yellow',
                    fillOpacity: 1.0,
                    strokeWeight: 2
                }
            });
            mapMarkers.push(marker);
            google.maps.event.addListener(marker, 'click', function (event) {
                try {
                    var id = event.ta !== undefined ? event.ta.currentTarget.title : event.currentTarget.title;
                    $scope.onSelectedNodo(id);
                } catch (e) {

                }

            })
            google.maps.event.addListener(marker, 'mouseover', function (event) {
                try {
                    var id = event.ta !== undefined ? event.ta.currentTarget.title : event.currentTarget.title;
                    $scope.onOverNodo(id);
                } catch (e) {

                }

            })
            google.maps.event.addListener(marker, 'mouseout', function (event) {
                try {
                    var id = event.ta !== undefined ? event.ta.currentTarget.title : event.currentTarget.title;
                    $scope.onOut(null, id);
                } catch (e) {

                }
            })
        }

        $scope.markers = mapMarkers;

        $scope.UpdateMap();

       //$scope.onApply();
    }

    $scope.UpdateMap = function () {
        try {
            google.maps.visualRefresh = true;
            google.maps.event.trigger($scope.map, 'resize');
        }
        catch (error) { }
    }

    NgMap.getMap().then(function (map) {
        $scope.map = map;
        var isDefinedMarkerWithLabel = false;
        try {
            isDefinedMarkerWithLabel = MarkerWithLabel != null;
        }
        catch (error) {
        }
        if (!isDefinedMarkerWithLabel) {
            $scope.GetMarkerWithLabel();
        }
    }, 2000);

    $scope.GetMarkerWithLabel = function (functionreturn) {
        functionreturn = ((functionreturn === undefined || functionreturn == null) ? function () { } : functionreturn);
        jQuery.ajax({
            url: config.baseURL + "lib/ng-map/MarkerWithLabel.js",
            dataType: 'script',
            success: function (data) {
                try {
                    eval(data);
                    $scope.UpdateMap();
                }
                catch (e) {
                }
                functionreturn();
            },
            async: true
        });
    }   

    $scope.ObtenerMarcadoresMapa = function () {
        ajax.get("Mapa", "BuscarMarcadores", null, function (data) {
            $scope.GraficarMarcadores(data);
        }, $scope.onDefaultReturnFault);
    };

    $scope.GraficarMarcadores = function (data) {
        $scope.ClearMarkers();
        var existMarkerWithLabel = false;
        try {
            existMarkerWithLabel = MarkerWithLabel != null;
        }
        catch (e) {
        }
        if (existMarkerWithLabel) {
            $scope.AddMarkers(data);
        }
        else {
            $scope.GetMarkerWithLabel(function () {
                $scope.AddMarkers(data);
            });
        }
    }
   
    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);
});
registerController('AngularJSApp', 'appController');