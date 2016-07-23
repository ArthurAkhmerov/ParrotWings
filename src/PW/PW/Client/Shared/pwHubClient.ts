module PW {
	declare var $: any;

	export class SignalrEvents {
		eventName: string;
		callback: any;
	}

	export class PwHubClient {
		private pwHubProxy: any;
		constructor(private $rootScope: ng.IRootScopeService,
			private $q: ng.IQService,
			private securityProvider: ISecurityProvider) {
		}

		public on(events: SignalrEvents[], callback) {
			var connection = $.hubConnection();
			var signalRChatHub = connection.chatHub;
			this.pwHubProxy = connection.createHubProxy('pwHub');
			var rootTemp = this.$rootScope;

			events.forEach(e => {
				this.pwHubProxy.on(e.eventName, function (msg) {
					var args = arguments;

					rootTemp.$apply(function () {
						e.callback.apply(this.chatHubProxy, args);
					});
				});
			});

			connection.start().done(function () { callback.apply() });
		}

		public joinUser(userId: string) {
			var dto: any = {
				ClientId: "",
				UserId: userId,
			};

			this.pwHubProxy.invoke('joinUser', dto);

		}

		public sendTransfer(dto: ITransferRequestVDTO): ng.IPromise<ISendTransferResultVDTO> {
			var deferred = this.$q.defer();
			
			var result = this.pwHubProxy.invoke('sendTransfer', dto)
				.then((result: ISendTransferResultVDTO) => {
					deferred.resolve(result);
				});
			return deferred.promise;
		}

		private getAllMethods(object) {
			return Object.getOwnPropertyNames(object);
		}
	}
	angular.module("PW")
		.factory("PW.PwHubClient", ["$rootScope", "$q", "PW.SecurityProvider",
			($rootScope, $q, securityProvider) => new PwHubClient($rootScope, $q, securityProvider)]);
}