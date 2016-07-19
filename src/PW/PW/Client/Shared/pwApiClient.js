var PW;
(function (PW) {
    var Duration = (function () {
        function Duration() {
        }
        return Duration;
    })();
    PW.Duration = Duration;
    var PagingData = (function () {
        function PagingData() {
        }
        return PagingData;
    })();
    PW.PagingData = PagingData;
    var PwApiClient = (function () {
        function PwApiClient($http, securityProvider) {
            this.$http = $http;
            this.securityProvider = securityProvider;
        }
        Object.defineProperty(PwApiClient, "DATE_FORMAT", {
            get: function () { return "YYYY-MM-DD HH:mm"; },
            enumerable: true,
            configurable: true
        });
        PwApiClient.prototype.signIn = function (dto) {
            return this.$http.post("/api/auth", dto);
        };
        PwApiClient.prototype.signUp = function (dto) {
            return this.$http.post("/api/auth/signup", dto);
        };
        PwApiClient.prototype.getTransfers = function (userId, duration, paging) {
            return this.$http.get("/api/transfer?userId=" + userId + "\n\t\t\t\t\t&from=" + duration.from.format(PwApiClient.DATE_FORMAT) + "&to=" + duration.to.format(PwApiClient.DATE_FORMAT) + "\n\t\t\t\t\t&skip=" + paging.skip + "&take=" + paging.take);
        };
        PwApiClient.prototype.getTransfersByUserTo = function (userId, usernameTo, duration, paging) {
            return this.$http.get("/api/transfer?userId=" + userId + "&usernameTo=" + usernameTo + "\n\t\t\t\t\t&from=" + duration.from.format(PwApiClient.DATE_FORMAT) + "&to=" + duration.to.format(PwApiClient.DATE_FORMAT) + "\n\t\t\t\t\t&skip=" + paging.skip + "&take=" + paging.take);
        };
        PwApiClient.prototype.getTransfersCount = function (userId, duration) {
            return this.$http.get("/api/transfer/count?userId=" + userId + "\n\t\t\t\t\t&from=" + duration.from.format(PwApiClient.DATE_FORMAT) + "&to=" + duration.to.format(PwApiClient.DATE_FORMAT));
        };
        PwApiClient.prototype.getTransfersCountByUserTo = function (userId, usernameTo, duration) {
            return this.$http.get("/api/transfer/count?userId=" + userId + "&usernameTo=" + usernameTo + "\n\t\t\t\t\t&from=" + duration.from.format(PwApiClient.DATE_FORMAT) + "&to=" + duration.to.format(PwApiClient.DATE_FORMAT));
        };
        PwApiClient.prototype.getUserById = function (id) {
            return this.$http.get("/api/user?id=" + id);
        };
        PwApiClient.prototype.getUsers = function () {
            return this.$http.get("/api/user");
        };
        PwApiClient.prototype.getUsersBySearchText = function (searchText) {
            return this.$http.get("/api/user?searchText=" + searchText);
        };
        return PwApiClient;
    })();
    PW.PwApiClient = PwApiClient;
    angular.module("PW")
        .factory("PW.PwApiClient", ["$http", "PW.SecurityProvider", function ($http, securityProvider) { return new PwApiClient($http, securityProvider); }]);
})(PW || (PW = {}));
//# sourceMappingURL=pwApiClient.js.map