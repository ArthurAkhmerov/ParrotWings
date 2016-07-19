var PW;
(function (PW) {
    var MakeTransferModalInstanceController = (function () {
        function MakeTransferModalInstanceController($rootScope, $scope, $uibModalInstance) {
            var _this = this;
            this.$rootScope = $rootScope;
            this.$scope = $scope;
            this.$uibModalInstance = $uibModalInstance;
            $scope.ok = function () { return _this.ok(); };
            $scope.cancel = function () { return _this.cancel(); };
        }
        MakeTransferModalInstanceController.prototype.ok = function () {
            this.$uibModalInstance.close();
        };
        MakeTransferModalInstanceController.prototype.cancel = function () {
            this.$uibModalInstance.dismiss();
        };
        return MakeTransferModalInstanceController;
    })();
    PW.MakeTransferModalInstanceController = MakeTransferModalInstanceController;
    angular.module('PW').controller('PW.MakeTransferModalInstanceController', ["$rootScope", "$scope", "$uibModalInstance",
        function ($rootScope, $scope, $uibModalInstance) { return new MakeTransferModalInstanceController($rootScope, $scope, $uibModalInstance); }]);
})(PW || (PW = {}));
