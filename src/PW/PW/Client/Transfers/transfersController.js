var PW;
(function (PW) {
    var TransfersController = (function () {
        function TransfersController($uibModal, $timeout, notification, pwHubClient, pwApiClient, authService, userService, transferService) {
            var _this = this;
            this.$uibModal = $uibModal;
            this.$timeout = $timeout;
            this.notification = notification;
            this.pwHubClient = pwHubClient;
            this.pwApiClient = pwApiClient;
            this.authService = authService;
            this.userService = userService;
            this.transferService = transferService;
            this.currentDuration = {
                startDate: moment().add(-2, "month"),
                endDate: moment(),
            };
            this.setupEvents(function () {
                var auth = _this.authService.getAuth();
                if (auth.isSignedIn) {
                    _this.userService.joinUser(auth.userId, auth.sessionId);
                }
            });
            this.userService.loadUserData().then(function (data) {
                _this.applyFilter();
            });
        }
        Object.defineProperty(TransfersController, "PAGE_SIZE", {
            get: function () { return 5; },
            enumerable: true,
            configurable: true
        });
        TransfersController.prototype.updateCurrentDuration = function () {
            if (this.currentDuration.startDate != undefined && this.currentDuration.startDate != null) {
                this.duration = {
                    from: this.currentDuration.startDate.add(-1, 'day').endOf('day'),
                    to: this.currentDuration.endDate.endOf('day')
                };
            }
        };
        TransfersController.prototype.applyFilter = function () {
            var _this = this;
            this.updateCurrentDuration();
            if (this.usernameInput && this.usernameInput.trim() != '') {
                this.pwApiClient.getTransfersCountByUserTo(this.getCurrentUser().id, this.usernameInput, this.duration)
                    .success(function (result) { return _this.transfersCount = result; });
            }
            else {
                this.pwApiClient.getTransfersCount(this.getCurrentUser().id, this.duration)
                    .success(function (result) { return _this.transfersCount = result; });
            }
            this.goToPage(1);
        };
        TransfersController.prototype.goToPage = function (page) {
            var _this = this;
            this.page = page;
            //this.transfers = _.take(_.drop(this.transferService.getTransfers(), 3 * (page - 1)), 3);
            var paging = {
                skip: TransfersController.PAGE_SIZE * (page - 1),
                take: TransfersController.PAGE_SIZE
            };
            if (this.usernameInput && this.usernameInput.trim() != '') {
                this.transferService.loadTransfersByUserTo(this.usernameInput, this.duration, paging)
                    .then(function (data) {
                    _this.transfers = _this.transferService.getTransfers();
                });
            }
            else {
                this.transferService.loadTransfers(this.duration, paging)
                    .then(function (data) {
                    _this.transfers = _this.transferService.getTransfers();
                });
            }
        };
        TransfersController.prototype.getPages = function () {
            var input = [];
            if (!this.transferService.getTransfers())
                return input;
            this.allPagesCount = Math.ceil(this.transfersCount / TransfersController.PAGE_SIZE);
            if (this.page <= 12) {
                if (this.allPagesCount > 12) {
                    for (var i = 1; i <= 12; i++) {
                        input.push(i);
                    }
                }
                else {
                    for (var i = 1; i <= this.allPagesCount; i++) {
                        input.push(i);
                    }
                }
            }
            else {
                for (var i = this.page - 11; i <= this.page; i++) {
                    input.push(i);
                }
            }
            return input;
        };
        ;
        TransfersController.prototype.prev = function () {
            if (this.page > 1) {
                this.goToPage(this.page - 1);
            }
        };
        TransfersController.prototype.next = function () {
            if (this.page < this.allPagesCount) {
                this.goToPage(this.page + 1);
            }
        };
        TransfersController.prototype.setupEvents = function (callback) {
            var _this = this;
            var events = [];
            events.push({
                eventName: 'userJoined',
                callback: function (user) {
                    var currentUser = _this.userService.getCurrentUser();
                    if (user.id != currentUser.id) {
                        var userEmail = user.email;
                        if (_this.userService.getUserById(user.id) !== null) {
                            _this.userService.updateUserState(user.id, "Online");
                        }
                        else {
                            _this.userService.addUser(user);
                        }
                    }
                }
            });
            events.push({
                eventName: 'userLeft',
                callback: function (user) {
                    _this.userService.updateUserState(user.id, "Offline");
                }
            });
            events.push({
                eventName: 'transferSent',
                callback: function (dto) {
                    _this.transferService.addTransfer(dto);
                    var addedTransfer = _this.transferService.getTransferById(dto.id);
                    if (addedTransfer.isIncoming) {
                        _this.notification.success("new transfer(" + addedTransfer.data.amount + ") from " + addedTransfer.data.userFromName);
                    }
                    else {
                        _this.notification.success("your transfer has been sent");
                    }
                    if (_this.page == 1) {
                        _this.transfers.unshift(addedTransfer);
                    }
                }
            });
            this.pwHubClient.on(events, callback);
        };
        TransfersController.prototype.getCurrentUser = function () {
            return this.userService.getCurrentUser();
        };
        TransfersController.prototype.showMakeTransferModal = function (transfer) {
            var modalInstance = this.$uibModal.open({
                templateUrl: '/Client/Transfers/makeTransferModal.html',
                controller: 'PW.MakeTransferModalController',
                size: 'lg',
                resolve: {
                    transfer: function () { return transfer; }
                }
            });
            modalInstance.result
                .then(function (result) { })
                .catch(function () { });
        };
        TransfersController.prototype.filterTransfers = function () {
            var _this = this;
            if (this.transferService.getTransfers()) {
                if (this.usernameInput && this.usernameInput.trim() != '') {
                    return this.transferService.getTransfers()
                        .filter(function (x) { return (x.data.userFromName.indexOf(_this.usernameInput) != -1
                        || x.data.userToName.indexOf(_this.usernameInput) != -1)
                        && moment(x.data.createdAt, 'YYYY-MM-DD').isAfter(_this.duration.from)
                        && moment(x.data.createdAt, 'YYYY-MM-DD').isBefore(_this.duration.to); });
                }
                else if (this.duration) {
                    return this.transferService.getTransfers()
                        .filter(function (x) { return moment(x.data.createdAt, 'YYYY-MM-DD').isAfter(_this.duration.from)
                        && moment(x.data.createdAt, 'YYYY-MM-DD').isBefore(_this.duration.to); });
                }
                return this.transferService.getTransfers();
            }
        };
        return TransfersController;
    })();
    PW.TransfersController = TransfersController;
    angular.module("PW")
        .controller("PW.TransfersController", ["$uibModal", "$timeout", "PW.NotificationService", "PW.PwHubClient", "PW.PwApiClient", "PW.AuthService", "PW.UserService", "PW.TransferService",
        function ($uibModal, $timeout, notificationService, pwHubClient, pwApiClient, authService, userService, transfersService) {
            return new TransfersController($uibModal, $timeout, notificationService, pwHubClient, pwApiClient, authService, userService, transfersService);
        }]);
})(PW || (PW = {}));
//# sourceMappingURL=transfersController.js.map