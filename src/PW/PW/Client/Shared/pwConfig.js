var PW;
(function (PW) {
    var Config = (function () {
        function Config($stateProvider, $urlRouterProvider) {
            this.$stateProvider = $stateProvider;
            this.$urlRouterProvider = $urlRouterProvider;
            this.init();
        }
        Config.prototype.init = function () {
        };
        return Config;
    })();
    PW.Config = Config;
})(PW || (PW = {}));
moment.locale('en');
angular.module("PW")
    .config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {
        return new PW.Config($stateProvider, $urlRouterProvider);
    }]);
//# sourceMappingURL=pwConfig.js.map