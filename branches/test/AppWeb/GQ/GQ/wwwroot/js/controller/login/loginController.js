app.controller('appController', function ($scope, ajax, $uibModal) {

    BaseController($scope, $uibModal);

    $scope.data = { UsuarioNombre: "", UsuarioClave: "", ErrorShow : false };

    $scope.onLogin = function () {
        $scope.data.ErrorShow = false;//Con esto, en la carga inicial se sigue viendo y luego desaparece
        var result = ajax.post("Login", "Login", { usuario: $scope.data.UsuarioNombre, clave: $scope.data.UsuarioClave },
            function (data) {
                if (data != null) {
                    $("body").addClass("hidden");
                    window.location.href = config.baseURL;
                }
                else {
                    $scope.data.ErrorShow = true;
                }
            },
            function (data) {
                $scope.data.ErrorShow = true;
            });
    }

    $scope.recuperar = { UsuarioEmail: "" };

    $scope.onRecuperarClave = function() {
        $scope.onOpenDialog("", "", function () {
            var result = ajax.get("Login", "RecuperarClave", $scope.recuperar.UsuarioEmail,
                function (data) {
                    if (data == true) {
                        $scope.onOpenDialog("Envio correcto", "Revise su correo", function () {
                        }, "sm", "modalOk.html");  
                    }
                    else {
                        $scope.onOpenDialog("Envio Incorrecto", "Revise los datos ingresados", function () {
                        }, "sm", "modalOk.html"); 
                    }
                },
                function (data) {
                    $scope.onOpenDialog("Envio Incorrecto", "Revise los datos ingresados", function () {
                    }, "sm", "modalOk.html"); 
                });

        }, "sm", "modalRecuperarClave.html", $scope.recuperar, null, null, null);  
        
    }
    
});