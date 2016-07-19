var PW;
(function (PW) {
    var UserService = (function () {
        function UserService($q, pwApiClient, pwHubClient, authService) {
            this.$q = $q;
            this.pwApiClient = pwApiClient;
            this.pwHubClient = pwHubClient;
            this.authService = authService;
            this.users = [];
        }
        UserService.prototype.loadUsers = function () {
            var _this = this;
            var deferred = this.$q.defer();
            this.pwApiClient.getUsers()
                .success(function (data) {
                _this.users = _.sortBy(data, function (x) { return x.createdAt; });
                deferred.resolve(_this.users);
            })
                .error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        UserService.prototype.loadUsersBySearchText = function (searchText) {
            var _this = this;
            var deferred = this.$q.defer();
            this.pwApiClient.getUsersBySearchText(searchText)
                .success(function (data) {
                _this.users = _.sortBy(data, function (x) { return x.createdAt; });
                deferred.resolve(_this.users);
            })
                .error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        UserService.prototype.loadUserData = function () {
            var _this = this;
            var deferred = this.$q.defer();
            var auth = this.authService.getAuth();
            if (auth && auth.isSignedIn) {
                this.pwApiClient.getUserById(auth.userId)
                    .success(function (data) {
                    _this.currentUser = data;
                    deferred.resolve(_this.currentUser);
                })
                    .error(function (reason) { return deferred.reject(reason); });
            }
            else {
                deferred.reject();
            }
            return deferred.promise;
        };
        UserService.prototype.loadUsersByFilter = function (username) {
            var _this = this;
            var deferred = this.$q.defer();
            this.pwApiClient.getUsers()
                .success(function (data) {
                _this.users = _.sortBy(data, function (x) { return x.createdAt; });
                deferred.resolve(_this.users);
            })
                .error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        UserService.prototype.getUsers = function () { return this.users; };
        UserService.prototype.getUserById = function (id) {
            if (!this.users)
                return null;
            if (this.users.filter(function (x) { return x.id === id; }).length === 0)
                return null;
            return this.users.filter(function (x) { return x.id === id; })[0];
        };
        UserService.prototype.getCurrentUser = function () {
            return this.currentUser;
        };
        UserService.prototype.addUser = function (user) {
            this.users.push(user);
        };
        UserService.prototype.joinUser = function (userId, sessionId) {
            this.pwHubClient.joinUser(userId, sessionId);
        };
        UserService.prototype.updateUserState = function (userId, state) {
            this.users.filter(function (x) { return x.id === userId; }).forEach(function (x) { x.state = state; });
        };
        return UserService;
    })();
    PW.UserService = UserService;
    angular.module("PW")
        .factory('PW.UserService', ["$q", "PW.PwApiClient", "PW.PwHubClient", "PW.AuthService",
        function ($q, pwApiClient, pwHubClient, authService) { return new UserService($q, pwApiClient, pwHubClient, authService); }]);
})(PW || (PW = {}));
//# sourceMappingURL=userService.js.map