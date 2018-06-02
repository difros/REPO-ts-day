app.controller('appController', function ($scope, NgTableParams, ajax, $uibModal, $rootScope) {

    $scope.roles = [];
    CrudController($scope, NgTableParams, ajax, $uibModal, "SMTP");

    $scope.filterCondition = { Estado: 'x' };

    $scope.filterCondition.Nombre = "con";
    $scope.filterCondition.NombreFrom = "con";
    $scope.filterCondition.User = "con";
    $scope.filterCondition.Pass = "con";
    $scope.filterCondition.Host = "con";
    $scope.filterCondition.Port = "con";
    $scope.filterCondition.UseDefaultCredentials = "con";
    $scope.filterCondition.EnableSsl = "con";
    $scope.filterCondition.EMailFrom = "con";

   
    $scope.onAgregarAfter = function () {
        $scope.selecteditem.Id = $scope.roles[0].Id;
        return true;
    };

    $scope.onModificarAfter = function () {
        $scope.showButtonAdd = false;
    };

    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);
});
registerController('AngularJSApp', 'appController');