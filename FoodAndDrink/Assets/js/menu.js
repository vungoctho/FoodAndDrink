angular.module('fad.menu', ['ngSanitize'])
.controller('fadMenuController', [
    '$scope',
    function ($scope) {
        /* DATA */
        $scope.selectedFood = {
            imageUrl: null,
            displayName: null,
            description: null,
            price: null
        };
        $scope.orderList = [];
        $scope.totalOrderPrice = 0;

        /* PUBLIC FUNCTION */

        $scope.showForm = function ($event, id, imageUrl, displayName, description, price) {
            //Stop anchor link
            $event.preventDefault();
            $scope.selectedFood.id = parseInt(id);
            $scope.selectedFood.imageUrl = imageUrl;
            $scope.selectedFood.displayName = displayName;
            $scope.selectedFood.description = description;
            $scope.selectedFood.price = price;
            toggleModal(true);
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
            toggleModal(false);
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

       
        var toggleModal = function (show) {
            var cmd = show ? 'open' : 'close';
            //Show the modal and attach event listeners once each
            $('#foodChooseForm')
                .foundation('reveal', cmd)
                .one('closed.fndtn.reveal', function () {
                });
        };
    }
]);