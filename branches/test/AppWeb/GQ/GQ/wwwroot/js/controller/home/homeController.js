app.controller('appController', function ($scope, $rootScope, $uibModal) {
    BaseController($scope, $uibModal);

    setTimeout(function () {
        $rootScope.$broadcast('showLoadingPage', false);
    }, 1000);
    registerController('AngularJSApp', 'appController');
});

