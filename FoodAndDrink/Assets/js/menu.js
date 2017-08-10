angular.module('fad.menu', ['ngSanitize'])
.controller('fadMenuController', [
    '$scope', '$http', '$rootScope',
    function ($scope, $http, $rootScope) {
        /* DATA */
        $scope.foodChooseFormId = '#foodChooseForm';
        $scope.checkoutFormId = '#checkoutForm';
        $scope.selectedFood = {
            imageUrl: null,
            displayName: null,
            description: null,
            price: null
        };
        $scope.orderList = [];
        $scope.totalOrderPrice = 0;
        $scope.deliveryInfor = {};
        $scope.isValidate = false;
        $scope.isChecking = false;
        /* PUBLIC FUNCTION */

        $scope.showForm = function ($event, id, imageUrl, displayName, description, price) {
            //Stop anchor link
            $event.preventDefault();
            $scope.selectedFood.id = parseInt(id);
            $scope.selectedFood.imageUrl = imageUrl;
            $scope.selectedFood.displayName = displayName;
            $scope.selectedFood.description = description;
            $scope.selectedFood.price = price;
            toggleModal($scope.foodChooseFormId, true);            
            
        };

        $scope.chooseFood = function (food) {            
            if (food.id === $scope.selectedFood.id) {
                var foundItem = _.find($scope.orderList, function (item) { return item.id === food.id });
                if (!!foundItem) {
                    foundItem.amount += 1;
                    calculateTotalOrderPrice();
                } else {
                    food.amount = 1;
                    $scope.orderList.push(_.clone(food));
                }

            }
            toggleModal($scope.foodChooseFormId, false);
        }

        $scope.removeOrderItem = function (itemId)
        {
            $scope.orderList = _.reject($scope.orderList, function (item) { return item.id === itemId});
        }

        $scope.adjustAmount = function (itemId, amount) {
            var foundItem = _.find($scope.orderList, function (item) { return item.id === itemId });
            foundItem.amount += amount;
            if (foundItem.amount <= 0) {
                $scope.removeOrderItem(itemId);
            }
            else {
                calculateTotalOrderPrice();
            }
        }

        $scope.checkout = function ($event) {
            $event.preventDefault();
            $scope.isValidate = false;
            toggleModal($scope.checkoutFormId, true);
        }

        $scope.sendOrderFood = function () {            
            if ($scope.checkoutForm.$valid)
            {
                var orderDetails = _.map($scope.orderList, function (item) {
                    return {
                        FoodId: item.id,
                        FoodName: item.displayName,
                        FoodDescription: item.description,
                        Price: item.price,
                        Amount: item.amount
                    }
                });
                
                //Post the model to the API
                var postData = {
                    record: {
                        FullName: $scope.deliveryInfor.fullName,
                        Email: $scope.deliveryInfor.email,
                        Phone: $scope.deliveryInfor.phoneNumber,
                        Address: $scope.deliveryInfor.address,
                        City: $scope.deliveryInfor.city,
                        OrderDetails: orderDetails
                    }
                };

                var url = '/umbraco/api/FoodOrder/post';
                $scope.isChecking = true;
                //Post the data to the server
                $http.post(url, postData).then(
                    function (result) {
                        $scope.isChecking = false;
                        resetFormAfterSubmit();
                    },
                    //AJAX errors
                    function (response) {
                        $scope.isChecking = false;
                        resetFormAfterSubmit();
                    }
                );
                toggleModal($scope.checkoutFormId, false);
            }
            $scope.isValidate = true;            
        }

        $scope.cancelOrderFood = function(){
            toggleModal($scope.checkoutFormId, false);
        }

        /* WATCH FUNCTION */
        $scope.$watchCollection("orderList", function (newValue, oldValue) {
            if (!!newValue) {
                calculateTotalOrderPrice();
            }
        }, true);
        
        /* PRIVATE FUNCTION */
        var calculateTotalOrderPrice = function () {
            var total = 0;
            _.each($scope.orderList, function (item) {
                total += (item.amount * item.price);
            })
            $scope.totalOrderPrice = total;
        }

       
        var toggleModal = function (formId, show) {
            var cmd = show ? 'open' : 'close';
            //Show the modal and attach event listeners once each
            $(formId)
                .foundation('reveal', cmd)
                .one('closed.fndtn.reveal', function () {
                });
        };

        var resetFormAfterSubmit = function () {
            $scope.orderList = [];
            $scope.totalOrderPrice = 0;
            $scope.deliveryInfor = {};
        };
    }
]);