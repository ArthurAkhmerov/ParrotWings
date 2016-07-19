var PW;
(function (PW) {
    var Transfer = (function () {
        function Transfer(data, isIncoming, isNew) {
            if (isNew === void 0) { isNew = false; }
            this.data = data;
            this.isIncoming = isIncoming;
            this.isNew = isNew;
        }
        return Transfer;
    })();
    PW.Transfer = Transfer;
    var TransferService = (function () {
        function TransferService($q, $timeout, pwApiClient, pwHubClient, authService, userService) {
            this.$q = $q;
            this.$timeout = $timeout;
            this.pwApiClient = pwApiClient;
            this.pwHubClient = pwHubClient;
            this.authService = authService;
            this.userService = userService;
            this.transfers = [];
        }
        TransferService.prototype.createTransfer = function (dto, isNew) {
            if (isNew === void 0) { isNew = false; }
            var currentUser = this.userService.getCurrentUser();
            if (currentUser.id == dto.userToId) {
                return new Transfer(dto, true, isNew);
            }
            else {
                return new Transfer(dto, false, isNew);
            }
        };
        TransferService.prototype.loadTransfers = function (duration, paging) {
            var _this = this;
            var deferred = this.$q.defer();
            var currentUser = this.userService.getCurrentUser();
            this.pwApiClient.getTransfers(currentUser.id, duration, paging)
                .success(function (data) {
                var dtos = _.sortBy(data, function (x) { return x.createdAt; });
                _this.transfers = _.map(data, function (x) { return _this.createTransfer(x); });
                _this.transfers = _.sortBy(_this.transfers, function (x) { return x.data.createdAt; }).reverse();
                deferred.resolve(_this.transfers);
            })
                .error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        TransferService.prototype.loadTransfersByUserTo = function (usernameTo, duration, paging) {
            var _this = this;
            var deferred = this.$q.defer();
            var currentUser = this.userService.getCurrentUser();
            this.pwApiClient.getTransfersByUserTo(currentUser.id, usernameTo, duration, paging)
                .success(function (data) {
                var dtos = _.sortBy(data, function (x) { return x.createdAt; });
                _this.transfers = _.map(data, function (x) { return _this.createTransfer(x); });
                _this.transfers = _.sortBy(_this.transfers, function (x) { return x.data.createdAt; }).reverse();
                deferred.resolve(_this.transfers);
            })
                .error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        TransferService.prototype.getTransfers = function () {
            return this.transfers.splice(0);
        };
        TransferService.prototype.getTransferById = function (id) {
            if (!this.transfers)
                return null;
            if (this.transfers.filter(function (x) { return x.data && x.data.id === id; }).length === 0)
                return null;
            return this.transfers.filter(function (x) { return x.data && x.data.id === id; })[0];
        };
        TransferService.prototype.sendTransfer = function (dto) {
            var deferred = this.$q.defer();
            this.pwHubClient.sendTransfer(dto, this.authService.getAuth().sessionId)
                .then(function (result) { return deferred.resolve(result); })
                .catch(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        TransferService.prototype.addTransfer = function (dto) {
            var newTransfer = this.createTransfer(dto, true);
            this.$timeout(function () { return newTransfer.isNew = false; }, 10000);
            this.transfers.unshift(newTransfer);
            this.transfers = _.sortBy(this.transfers, function (x) { return x.data.createdAt; }).reverse();
            var currentuser = this.userService.getCurrentUser();
            if (newTransfer.isIncoming) {
                currentuser.balance += newTransfer.data.amount;
            }
            else {
                currentuser.balance -= newTransfer.data.amount;
            }
        };
        return TransferService;
    })();
    PW.TransferService = TransferService;
    angular.module("PW")
        .factory('PW.TransferService', ["$q", "$timeout", "PW.PwApiClient", "PW.PwHubClient", "PW.AuthService", "PW.UserService",
        function ($q, $timeout, pwApiClient, pwHubClient, authService, userService) { return new TransferService($q, $timeout, pwApiClient, pwHubClient, authService, userService); }]);
})(PW || (PW = {}));
//# sourceMappingURL=transferService.js.map