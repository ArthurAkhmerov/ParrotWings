module PW {
    export class Config {
        constructor(private $stateProvider: ng.ui.IStateProvider, private $urlRouterProvider: ng.ui.IUrlRouterProvider) {
            this.init();
        }

        private init(): void {
        }
    }
}

moment.locale('en');

angular.module("PW")
    .config(["$stateProvider", "$urlRouterProvider", ($stateProvider, $urlRouterProvider) => {
        return new PW.Config($stateProvider, $urlRouterProvider);
    }]);