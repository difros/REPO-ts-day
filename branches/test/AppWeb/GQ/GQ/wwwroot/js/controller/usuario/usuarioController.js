app.controller('appController', function ($scope, NgTableParams, ajax, $uibModal, $rootScope) {

    $scope.roles = [];
    CrudController($scope, NgTableParams, ajax, $uibModal, "Usuario");

    $scope.filterCondition.Nombre = "con";
    $scope.filterCondition.Apellido = "con";
    $scope.filterCondition.NombreUsuario = "con";
    $scope.filterCondition.Email = "con";

    $scope.getPerfiles = function () {
        ajax.post("Usuario", "GetPerfiles", null, function (data) {
            $scope.roles = data;
        });
    };

    $scope.onAgregarAfter = function () {
        $scope.selecteditem.estado = "A";
        $scope.selecteditem.requiereClave = "N";
        $scope.selecteditem.perfilId = $scope.roles[0].perfilId;
        return true;
    };

    $scope.onModificarAfter = function () {
        $scope.showButtonAdd = false;
    };

    $scope.getPerfiles();

    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);

});
registerController('AngularJSApp', 'appController');
