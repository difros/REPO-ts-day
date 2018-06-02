app.controller('appController', function ($scope, NgTableParams, ajax, $uibModal, $rootScope) {

    CrudController($scope, NgTableParams, ajax, $uibModal, "Formulario");

    $scope.filterCondition.Texto = "con";
    $scope.filterCondition.Opcion = "=|T";
    $scope.filterCondition.IdUsuario = "=|T";
    
    $scope.filterObject.Opcion = "T";
    $scope.filterObject.IdUsuario = "T";

    $scope.listaArchivos = [];
    $scope.filePresent = false;
    $scope.mostrarImagen = false;

    $scope.listaUsuarios = [];

    //datePicker -start
    $scope.popup = {};
    $scope.popup.opened = false;
    $scope.openDatePicker = function () {
        $scope.popup.opened = !$scope.popup.opened;
    };
    //datePicker -end

    //timePicker -start
    $scope.hstep = 1;
    $scope.mstep = 5;
    $scope.ismeridian = false;
    $scope.toggleMode = function () {
        $scope.ismeridian = !$scope.ismeridian;
    };
    //timePicker -end

    $scope.onAgregarAfter = function () {
        $scope.selecteditem.estado = "A";
        $scope.selecteditem.hora = new Date();
        $scope.deleteArchivo();
        return true;
    };

    $scope.onModificarAfter = function () {
        $scope.selecteditem.fecha = new Date($scope.selecteditem.fecha);
        angular.element("input[type='file']").val(null);
        if ($scope.selecteditem.urlArchivo != null) {
            $scope.filePresent = true;
            $scope.mostrarImagen = true;
        } else {
            $scope.filePresent = false;
            $scope.mostrarImagen = false;
        }
        $scope.showButtonAdd = false;
    };

    $scope.onGuardar = function () {
        if ($scope.onGuardarBefore()) {
            $scope.onOpenDialog("Guardar", "¿Está seguro de Guardar?", function () {
                ajax.postUploadList("Formulario", "Guardar", $scope.selecteditem, $scope.listaArchivos, function (data) {
                    $scope.onGuardarAfter();
                    $scope.onCancelar();
                }, $scope.onDefaultReturnFault);
            }, "sm", "modalYesNo.html", null, null, "fa-question-circle-o", "");
        }
    };

    $scope.fileNameChanged = function (file, name) {
        var ext = file.files[0].type;
        arr=['image/jpeg','image/png','image/gif','image/bmp']
        if ($.inArray(ext,arr)==-1) {
            $scope.onOpenDialog(lenguaje.default_title_error, "No se acepta el tipo de archivo. Únicamente imágenes con los siguientes formatos: jpg, png, bmp o gif", null, null, "modalOk.html", null, null, "fa-exclamation", "modal-danger");
            $scope.deleteArchivo();
            return false;
        }
        $scope.mostrarImagen = false;
        $scope.listaArchivos[name] = file.files;
        $scope.filePresent = true;
        $scope.onApply();
    }

    $scope.deleteArchivo = function () {
        angular.element("input[type='file']").val(null);
        if ($scope.selecteditem.urlArchivo != null) {
            $scope.selecteditem.deletePicture = true;
        }
        $scope.mostrarImagen = false;
        $scope.filePresent = false;
        $scope.onApply();
    }

    $scope.BuscarUsuarios = function () {
        ajax.post("Usuario", "GetUsuarios", null, function (data) {
            $scope.listaUsuarios = data;
            $scope.listaUsuarios.sort();
        });
    };

    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);

    $scope.BuscarUsuarios();
});
registerController('AngularJSApp', 'appController');
