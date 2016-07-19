var PW;
(function (PW) {
    var SecurityProvider = (function () {
        function SecurityProvider() {
        }
        SecurityProvider.prototype.calculateMD5 = function () {
            var content = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                content[_i - 0] = arguments[_i];
            }
            var dataAsString = JSON.stringify(content);
            var hash = md5(dataAsString);
            return hash;
        };
        return SecurityProvider;
    })();
    PW.SecurityProvider = SecurityProvider;
    angular.module("PW")
        .factory("PW.SecurityProvider", [function () { return new SecurityProvider(); }]);
})(PW || (PW = {}));
//# sourceMappingURL=securityProvider.js.map