var PW;
(function (PW) {
    var MakeTransferModalController = (function () {
        function MakeTransferModalController($rootScope, $scope, $uibModalInstance, userService, transferService, transfer) {
            var _this = this;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.$uibModalInstance = $uibModalInstance;
            this.userService = userService;
            this.transferService = transferService;
            this.transfer = transfer;
            $scope.ok = function () { return _this.ok(); };
            $scope.cancel = function () { return _this.cancel(); };
            $scope.ShowUsersList = function (enabled) { return _this.ShowUsersList(enabled); };
            $scope.selectUser = function (user) { return _this.selectUser(user); };
            $scope.unselectUser = function (user) { return _this.unselectUser(user); };
            $scope.unselectedUsers = function () { return _this.unselectedUsers(); };
            $scope.recipientInput = "";
            $scope.amount = 0;
            $scope.selectedUsers = [];
            $scope.$watch('recipientInput', function (newVal, oldVal) {
                if (newVal.length > 1) {
                    _this.userService.loadUsersBySearchText(newVal)
                        .then(function (data) {
                        _this.$scope.users = data.filter(function (x) { return x.id != _this.userService.getCurrentUser().id; });
                    });
                }
            });
            if (transfer) {
                $scope.amount = transfer.data.amount;
                $scope.selectedUsers = [this.userService.getUserById(transfer.data.userToId)];
            }
            $scope.users = this.userService.getUsers().filter(function (x) { return x.id != _this.userService.getCurrentUser().id; });
        }
        MakeTransferModalController.prototype.ok = function () {
            var _this = this;
            if (!(this.$scope.amount > 0)) {
                this.$scope.errorMessage = "Amount must be greater than 0";
                return;
            }
            if (!this.$scope.selectedUsers || this.$scope.selectedUsers.length == 0) {
                this.$scope.errorMessage = "Choose recipient";
                return;
            }
            var currentUser = this.userService.getCurrentUser();
            if (currentUser.balance < this.$scope.amount * this.$scope.selectedUsers.length) {
                this.$scope.errorMessage = "Invalid balance";
                return;
            }
            var selectedUsers = this.$scope.selectedUsers;
            var dto = {
                amount: this.$scope.amount,
                recipientsIds: selectedUsers.map(function (x) { return x.id; }),
                userFromId: this.userService.getCurrentUser().id,
            };
            this.transferService.sendTransfer(dto).then(function (result) {
                if (result.success) {
                    _this.$uibModalInstance.close();
                }
                else {
                    _this.$scope.errorMessage = "The operation has not been completed. " + result.message;
                }
            });
        };
        MakeTransferModalController.prototype.cancel = function () {
            this.$uibModalInstance.dismiss();
        };
        MakeTransferModalController.prototype.unselectedUsers = function () {
            return _.difference(this.$scope.users, this.$scope.selectedUsers);
        };
        MakeTransferModalController.prototype.ShowUsersList = function (enabled) {
            this.$scope.showUsers = enabled;
        };
        MakeTransferModalController.prototype.selectUser = function (user) {
            if (this.$scope.selectedUsers.filter(function (x) { return x.id === user.id; }).length === 0)
                this.$scope.selectedUsers.push(user);
        };
        MakeTransferModalController.prototype.unselectUser = function (user) {
            this.$scope.selectedUsers = this.$scope.selectedUsers.filter(function (x) { return x.id !== user.id; });
        };
        return MakeTransferModalController;
    })();
    PW.MakeTransferModalController = MakeTransferModalController;
    angular.module('PW').controller('PW.MakeTransferModalController', ["$rootScope", "$scope", "$uibModalInstance", "PW.UserService", "PW.TransferService", "transfer",
        function ($rootScope, $scope, $uibModalInstance, userService, transferService, transfer) { return new MakeTransferModalController($rootScope, $scope, $uibModalInstance, userService, transferService, transfer); }]);
})(PW || (PW = {}));
//# sourceMappingURL=makeTransferModalController.js.map