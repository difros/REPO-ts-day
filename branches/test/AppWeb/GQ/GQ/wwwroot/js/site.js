var rp = null;

if ($(".main-sidebar").length === 1) {
    app.controller('mainSidebarController', function ($scope, ajax, $sce, $uibModal, $route, $routeParams, $location) {
        BaseController($scope, $uibModal);

        $scope.deliberatelyTrustDangerousSnippet = function (htmlText) {
            return $sce.trustAsHtml(htmlText);
        };

        $scope.menues = [];
        ajax.post("Menu", "Buscar", null, function (data) {

            for (var i = 0; i < data.length; i++) {
                //idioma menu header
                if (data[i].menuPosition == "90-00-00" ) {
                    data[i].nombre = lenguaje.menu_90_00_00;
                }
                for (var j = 0; j < data[i].child.length; j++) {
                    //idioma menu body
                    switch (data[i].child[j].menuPosition) {
                        case "90-10-00":
                            //usuario
                            data[i].child[j].nombre = lenguaje.menu_90_10_00;
                            break;
                        case "90-20-00":
                            //perfil
                            data[i].child[j].nombre = lenguaje.menu_90_20_00;
                            break;
                        case "90-30-00":
                            //grafico
                            data[i].child[j].nombre = lenguaje.menu_90_30_00;
                            break;
                    }

                    rp.when('/' + data[i].child[j].menuUrl, {
                        templateUrl: config.baseURL + data[i].child[j].menuUrl
                    })
                }
            }

            rp.when('/', {
                templateUrl: config.baseURL + "Home.html"
            })

            $scope.menues = data;

            if (window.location.href.indexOf('#') == -1) {
                window.location.href = config.baseURL + '#!/';
            }
            else if (window.location.href.substr(window.location.href.length - 1, 1) != "/") {
                window.location.href = window.location.href + "/";
            } else {
                window.location.href = config.baseURL;
            }
        });

        $scope.getPath = function (value) {
            return '#!' + value;
        }

        setTimeout(function () {
            $(".main-sidebar").removeClass("hidden");
        }, 1000);

        $scope.$route = $route;
        $scope.$location = $location;
        $scope.$routeParams = $routeParams;

    });
}

app.controller('LoadingPageController', function ($scope) {
    BaseController($scope, null);
    $scope.showCargando = true;
    $scope.$on('showLoadingPage', function (newValue, oldValue) {
        $scope.showCargando = oldValue;
        $scope.onApply();
    });
});

app.config(function ($routeProvider, $locationProvider) {
    rp = $routeProvider;
});
