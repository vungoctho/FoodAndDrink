angular.module('fad.chefRegister', ['ngSanitize', 'fad.common', 'fad.resources', 'fad.filters', 'fad.directives'])
.controller('fadChefRegisterController', [
    '$scope', '$http', '$rootScope', 'resourcesHelper', 'fadConst',
    function ($scope, $http, $rootScope, resourcesHelper, fadConst) {
        $scope.chef = {};
        $scope.isValidate = false;

        $scope.chefRegister = function () {
            if ($scope.chefRegisterForm.$valid) {
                //Post the model to the API
                var postData = {
                    FirstName: $scope.chef.firstName,
                    LastName: $scope.chef.lastName,
                    Email: $scope.chef.email,
                    Phone: $scope.chef.phoneNumber,
                };
                var api = fadConst.urls.chef;                
                resourcesHelper.post(api + "RegisterNewChef", postData, function (chefId) {
                    console.log(chefId);
                });
            }
            $scope.isValidate = true;
        }
    }
]);