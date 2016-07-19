module PW {
	export class Duration {
		public from: moment.Moment;
		public to: moment.Moment;
	}

	export class PagingData {
		public skip: number;
		public take: number;
	}

	export interface IPwApiClient {
		signIn(dto: IAuthRequestVDTO): ng.IHttpPromise<IAuthResultVDTO>;
		signUp(dto: ISignupRequestVDTO): ng.IHttpPromise<IAuthResultVDTO>;
		getTransfers(userId: string, duration: Duration, paging: PagingData, sessionId: string): ng.IHttpPromise<ITransferVDTO[]>;
		getTransfersByUserTo(userId: string, usernameTo: string, duration: Duration, paging: PagingData, sessionId: string): ng.IHttpPromise<ITransferVDTO[]>;
		getTransfersCount(userId: string, duration: Duration, sessionId: string): ng.IHttpPromise<number>;
		getTransfersCountByUserTo(userId: string, usernameTo: string, duration: Duration, sessionId: string): ng.IHttpPromise<number>;
		getUserById(id: string): ng.IHttpPromise<IUserVDTO>;
		getUsers(): ng.IHttpPromise<IUserVDTO[]>;
		getUsersBySearchText(searchText: string): ng.IHttpPromise<IUserVDTO[]>;
	}
	export class PwApiClient implements IPwApiClient {

		public static get DATE_FORMAT(): string { return "YYYY-MM-DD HH:mm"; }

		constructor(private $http: ng.IHttpService, private securityProvider: ISecurityProvider) {
		}

		public signIn(dto: IAuthRequestVDTO): ng.IHttpPromise<IAuthResultVDTO> {
			return this.$http.post(`/api/auth`, dto);
		}

		public signUp(dto: ISignupRequestVDTO): ng.IHttpPromise<IAuthResultVDTO> {
			return this.$http.post(`/api/auth/signup`, dto);
		}

		public getTransfers(userId: string, duration: Duration, paging: PagingData, sessionId: string): ng.IHttpPromise<ITransferVDTO[]> {

			var hash = this.securityProvider.calculateMD5(userId,  sessionId);

			return this.$http.get(`/api/transfer?userId=${userId}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&hash=${hash}
					&skip=${paging.skip}&take=${paging.take}`);
		}

		public getTransfersByUserTo(userId: string, usernameTo: string, duration: Duration, paging: PagingData, sessionId: string): ng.IHttpPromise<ITransferVDTO[]> {
			var hash = this.securityProvider.calculateMD5(userId, sessionId);

			return this.$http.get(`/api/transfer?userId=${userId}&usernameTo=${usernameTo}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&hash=${hash}
					&skip=${paging.skip}&take=${paging.take}`);
		}

		public getTransfersCount(userId: string, duration: Duration, sessionId: string): ng.IHttpPromise<number> {
			var hash = this.securityProvider.calculateMD5(userId, sessionId);

			return this.$http.get(`/api/transfer/count?userId=${userId}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&hash=${hash}`);
		}

		public getTransfersCountByUserTo(userId: string, usernameTo: string, duration: Duration, sessionId: string): ng.IHttpPromise<number> {
			var hash = this.securityProvider.calculateMD5(userId, sessionId);

			return this.$http.get(`/api/transfer/count?userId=${userId}&usernameTo=${usernameTo}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&hash=${hash}`);
		}

		public getUserById(id: string): ng.IHttpPromise<IUserVDTO> {
			return this.$http.get(`/api/user?id=${id}`);
		}

		public getUsers(): ng.IHttpPromise<IUserVDTO[]> {
			return this.$http.get(`/api/user`);
		}

		public getUsersBySearchText(searchText: string): ng.IHttpPromise<IUserVDTO[]> {
			return this.$http.get(`/api/user?searchText=${searchText}`);
		}
	}

	angular.module("PW")
		.factory("PW.PwApiClient", ["$http", "PW.SecurityProvider", ($http, securityProvider) => new PwApiClient($http, securityProvider)]);

}
