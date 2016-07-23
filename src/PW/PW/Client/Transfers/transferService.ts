module PW {

	export class Transfer {
		constructor(public data: ITransferVDTO, public isIncoming: boolean, public isNew: boolean = false) { }
	}

	export interface ITransferService {
		getTransfers(): Transfer[];
		sendTransfer(dto: ITransferRequestVDTO): ng.IPromise<ISendTransferResultVDTO>;
		addTransfer(dto: ITransferVDTO);
		getTransferById(id: string): Transfer;
		loadTransfers(duration: Duration, paging: PagingData): ng.IPromise<Transfer[]>;
		loadTransfersByUserTo(usernameTo: string, duration: Duration, paging: PagingData): ng.IPromise<Transfer[]>;
	}

	export class TransferService implements ITransferService {
		private transfers: Transfer[] = [];

		constructor(private $q: ng.IQService,
			private $timeout: ng.ITimeoutService,
			private pwApiClient: IPwApiClient, private pwHubClient: PwHubClient,
			private authService: IAuthService,
			private userService: IUserService) {

		}

		private createTransfer(dto: ITransferVDTO, isNew: boolean = false): Transfer {
			var currentUser = this.userService.getCurrentUser();
			if (currentUser.id == dto.userToId) {
				return new Transfer(dto, true, isNew);
			} else {
				return new Transfer(dto, false, isNew);
			}
		}

		public loadTransfers(duration: Duration, paging: PagingData): ng.IPromise<Transfer[]> {
			var deferred = this.$q.defer<Transfer[]>();
			var currentUserId = this.authService.getCurrentUserId();
			var token = this.authService.getTokenData();
			
			this.pwApiClient.getTransfers(currentUserId, duration, paging, token.access_token)
				.success((data: ITransferVDTO[]) => {
					var dtos = _.sortBy(data, x => x.createdAt);
					this.transfers = _.map(data, x => this.createTransfer(x));
					this.transfers = _.sortBy(this.transfers, x => x.data.createdAt).reverse();
					deferred.resolve(this.transfers);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}

		public loadTransfersByUserTo(usernameTo: string, duration: Duration, paging: PagingData): ng.IPromise<Transfer[]> {
			var deferred = this.$q.defer<Transfer[]>();
			var currentUserId = this.authService.getCurrentUserId();
			var token = this.authService.getTokenData();
			this.pwApiClient.getTransfersByUserTo(currentUserId, usernameTo, duration, paging, token.access_token)
				.success((data: ITransferVDTO[]) => {
					var dtos = _.sortBy(data, x => x.createdAt);
					this.transfers = _.map(data, x => this.createTransfer(x));
					this.transfers = _.sortBy(this.transfers, x => x.data.createdAt).reverse();
					deferred.resolve(this.transfers);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}

		public getTransfers(): Transfer[] {
			return this.transfers.splice(0);
		}

		public getTransferById(id: string): Transfer {
			if (!this.transfers) return null;
			if (this.transfers.filter(x => x.data && x.data.id === id).length === 0) return null;

			return this.transfers.filter(x => x.data && x.data.id === id)[0];
		}

		public sendTransfer(dto: ITransferRequestVDTO): ng.IPromise<ISendTransferResultVDTO> {
			var deferred = this.$q.defer();

			this.pwHubClient.sendTransfer(dto)
				.then(result => deferred.resolve(result))
				.catch(reason => deferred.reject(reason));

			return deferred.promise;
		}

		public addTransfer(dto: ITransferVDTO): void {
			var newTransfer = this.createTransfer(dto, true);
			this.$timeout(() => newTransfer.isNew = false, 10000);

			this.transfers.unshift(newTransfer);
			this.transfers = _.sortBy(this.transfers, x => x.data.createdAt).reverse();

			var currentuser = this.userService.getCurrentUser();
			if (newTransfer.isIncoming) {
				currentuser.balance += newTransfer.data.amount
			} else {
				currentuser.balance -= newTransfer.data.amount
			}
		}
	}

	angular.module("PW")
		.factory('PW.TransferService',
		["$q", "$timeout", "PW.PwApiClient", "PW.PwHubClient", "PW.AuthService", "PW.UserService", 
			($q, $timeout, pwApiClient, pwHubClient, authService, userService) => new TransferService($q,$timeout, pwApiClient, pwHubClient, authService, userService)]);
}