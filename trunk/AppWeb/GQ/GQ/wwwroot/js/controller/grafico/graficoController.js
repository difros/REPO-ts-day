app.controller('appController', function ($scope, NgTableParams, ajax, $uibModal, $rootScope) {

    $scope.show = false;

    $scope.aceHtml = {
        workerPath: '/lib/ace',
        theme: 'chrome',
        require: ['ace/ext/language_tools'],
        advanced: {
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true
        },
        mode: "html",
    };

    $scope.aceJS = {
        workerPath: '/lib/ace',
        theme: 'chrome',
        require: ['ace/ext/language_tools'],
        advanced: {
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true
        },
        mode: "javascript",
    };

    $scope.aceCS = {
        workerPath: '/lib/ace',
        theme: 'chrome',
        require: ['ace/ext/language_tools'],
        advanced: {
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true
        },
        mode: "csharp",
    };

    $scope.options = {};
    $scope.data = null;
    $scope.template = "";

    CrudController($scope, NgTableParams, ajax, $uibModal, "Grafico");

    $scope.filterCondition.Nombre = "con";
    $scope.filterCondition.Descripcion = "con";
    $scope.filterCondition.Folder = "con";

    $scope.onAgregarAfter = function () {
        $scope.show = false;
        $scope.script = "";
        $scope.template = "";
        $scope.selecteditem.estado = "A";
        return true;
    };

    $scope.onModificarAfter = function () {
        $scope.show = false;
        $scope.script = "";
        $scope.template = "";
        $scope.GetGrafico();
        return true;
    };

    $scope.onExecuteTest = function () {
        $scope.script = $scope.selecteditem.scritp;
        $scope.template = $scope.selecteditem.template;
        //$scope.selecteditem.extrasEscenario = "gq_base";
        $scope.show = true;
    }

    $scope.GetGrafico = function () {
        ajax.get("Grafico", "GetGrafico", $scope.selecteditem.id, function (data) {
            $scope.selecteditem = data;
        }, $scope.onDefaultReturnFault);
    }

    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);
});
registerController('AngularJSApp', 'appController');