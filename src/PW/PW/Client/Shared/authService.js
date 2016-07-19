var PW;
(function (PW) {
    var Auth = (function () {
        function Auth(userId, sessionId, isSignedIn) {
            if (isSignedIn === void 0) { isSignedIn = true; }
            this.userId = userId;
            this.sessionId = sessionId;
            this.isSignedIn = isSignedIn;
        }
        return Auth;
    })();
    PW.Auth = Auth;
    var AuthService = (function () {
        function AuthService($q, $cookies, pwApiClient, pwHubClient) {
            this.$q = $q;
            this.$cookies = $cookies;
            this.pwApiClient = pwApiClient;
            this.pwHubClient = pwHubClient;
            var userId = this.$cookies.get("userId");
            var sessionId = this.$cookies.get("sessionId");
            if (userId && sessionId) {
                this.auth = new Auth(userId, sessionId);
            }
        }
        AuthService.prototype.signIn = function (email, password) {
            var _this = this;
            var deferred = this.$q.defer();
            var authRequestDto = {
                email: email,
                password: password
            };
            this.pwApiClient.signIn(authRequestDto).success(function (result) {
                if (result && result.success) {
                    _this.auth = new Auth(result.data.userId, result.data.sessionId);
                    _this.$cookies.put("userId", _this.auth.userId, { expires: moment().add("month", 3).toDate(), path: "/" });
                    _this.$cookies.put("sessionId", _this.auth.sessionId, { expires: moment().add("month", 3).toDate(), path: "/" });
                }
                else {
                    _this.auth = null;
                }
                deferred.resolve(result);
            }).error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        AuthService.prototype.signUp = function (name, email, password) {
            var deferred = this.$q.defer();
            var signUpRequestDto = {
                name: name,
                email: email,
                password: password
            };
            this.pwApiClient.signUp(signUpRequestDto).success(function (result) {
                deferred.resolve(result);
            }).error(function (reason) { return deferred.reject(reason); });
            return deferred.promise;
        };
        AuthService.prototype.signOut = function () {
            this.$cookies.remove("userId");
            this.$cookies.remove("sessionId");
            this.auth = null;
        };
        AuthService.prototype.isSignedIn = function () {
            return this.auth && this.auth.isSignedIn;
        };
        AuthService.prototype.getAuth = function () {
            return this.auth;
        };
        return AuthService;
    })();
    PW.AuthService = AuthService;
    angular.module("PW").factory("PW.AuthService", ["$q", "$cookies", "PW.PwApiClient", "PW.PwHubClient",
        function ($q, $cookies, pwApiClient, pwHubClient) { return new AuthService($q, $cookies, pwApiClient, pwHubClient); }]);
})(PW || (PW = {}));
//# sourceMappingURL=authService.js.map