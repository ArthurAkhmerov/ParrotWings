var PW;
(function (PW) {
    var SignalrEvents = (function () {
        function SignalrEvents() {
        }
        return SignalrEvents;
    })();
    PW.SignalrEvents = SignalrEvents;
    var PwHubClient = (function () {
        function PwHubClient($rootScope, $q, securityProvider) {
            this.$rootScope = $rootScope;
            this.$q = $q;
            this.securityProvider = securityProvider;
        }
        PwHubClient.prototype.on = function (events, callback) {
            var _this = this;
            var connection = $.hubConnection();
            var signalRChatHub = connection.chatHub;
            this.pwHubProxy = connection.createHubProxy('pwHub');
            var rootTemp = this.$rootScope;
            events.forEach(function (e) {
                _this.pwHubProxy.on(e.eventName, function (msg) {
                    var args = arguments;
                    rootTemp.$apply(function () {
                        e.callback.apply(this.chatHubProxy, args);
                    });
                });
            });
            connection.start().done(function () { callback.apply(); });
        };
        PwHubClient.prototype.joinUser = function (userId, sessionId) {
            var dto = {
                ClientId: "",
                UserId: userId,
            };
            var hash = this.securityProvider.calculateMD5(dto, sessionId);
            this.pwHubProxy.invoke('joinUser', dto, hash);
        };
        PwHubClient.prototype.sendTransfer = function (dto, sessionId) {
            var deferred = this.$q.defer();
            var hash = this.securityProvider.calculateMD5(dto, sessionId);
            var result = this.pwHubProxy.invoke('sendTransfer', dto, hash)
                .then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        PwHubClient.prototype.getAllMethods = function (object) {
            return Object.getOwnPropertyNames(object);
        };
        return PwHubClient;
    })();
    PW.PwHubClient = PwHubClient;
    angular.module("PW")
        .factory("PW.PwHubClient", ["$rootScope", "$q", "PW.SecurityProvider",
        function ($rootScope, $q, securityProvider) { return new PwHubClient($rootScope, $q, securityProvider); }]);
})(PW || (PW = {}));
//# sourceMappingURL=pwHubClient.js.map