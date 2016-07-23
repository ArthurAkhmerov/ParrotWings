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
		getToken(username: string, password: string): ng.IHttpPromise<TokenData>;
		signIn(dto: IAuthRequestVDTO): ng.IHttpPromise<IAuthResultVDTO>;
		signUp(dto: ISignupRequestVDTO): ng.IHttpPromise<IAuthResultVDTO>;
		getTransfers(userId: string, duration: Duration, paging: PagingData, token: string): ng.IHttpPromise<ITransferVDTO[]>;
		getTransfersByUserTo(userId: string, usernameTo: string, duration: Duration, paging: PagingData, token: string): ng.IHttpPromise<ITransferVDTO[]>;
		getTransfersCount(userId: string, duration: Duration, token: string): ng.IHttpPromise<number>;
		getTransfersCountByUserTo(userId: string, usernameTo: string, duration: Duration, token: string): ng.IHttpPromise<number>;
		getUserById(id: string): ng.IHttpPromise<IUserVDTO>;
		getUsers(): ng.IHttpPromise<IUserVDTO[]>;
		getUsersBySearchText(searchText: string): ng.IHttpPromise<IUserVDTO[]>;
	}

	export class TokenResponseVDTO {
		public data: TokenData;
	}

	export class TokenData {
		public access_token: string;
		public expires_in: number;
		public token_type: string;
		
	}



	export class PwApiClient implements IPwApiClient {

		public static get DATE_FORMAT(): string { return "YYYY-MM-DD HH:mm"; }

		constructor(private $http: ng.IHttpService, private $resource: ng.resource.IResourceService, private securityProvider: ISecurityProvider) {
		}

		public getToken(username: string, password: string): ng.IHttpPromise<TokenData> {
			this.$http.defaults.headers.post['Content-Type'] = "application/x-www-form-urlencoded";
			return this.$http({
				url: "/token",
				method: "POST",
				data: `grant_type=password&username=${username}&password=${password}`,
				file: {}
			});
			
		}

		public signIn(dto: IAuthRequestVDTO): ng.IHttpPromise<IAuthResultVDTO> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";
			return this.$http.post(`/api/auth`, dto);
		}

		public signUp(dto: ISignupRequestVDTO): ng.IHttpPromise<IAuthResultVDTO> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";
			return this.$http.post(`/api/auth/signup`, dto);
		}

		public getTransfers(userId: string, duration: Duration, paging: PagingData, token: string): ng.IHttpPromise<ITransferVDTO[]> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";

			return this.$http.get(`/api/transfer?userId=${userId}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&skip=${paging.skip}&take=${paging.take}`,
					{headers: { "Authorization": "Bearer " + token}});
		}

		public getTransfersByUserTo(userId: string, usernameTo: string, duration: Duration, paging: PagingData, token:string): ng.IHttpPromise<ITransferVDTO[]> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";

			return this.$http.get(`/api/transfer?userId=${userId}&usernameTo=${usernameTo}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}
					&skip=${paging.skip}&take=${paging.take}`,
					{headers: { "Authorization": "Bearer " + token } });
		}

		public getTransfersCount(userId: string, duration: Duration, token: string): ng.IHttpPromise<number> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";


			return this.$http.get(`/api/transfer/count?userId=${userId}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}`,
				{headers: { "Authorization": "Bearer " + token } });
		}

		public getTransfersCountByUserTo(userId: string, usernameTo: string, duration: Duration, token: string): ng.IHttpPromise<number> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";


			return this.$http.get(`/api/transfer/count?userId=${userId}&usernameTo=${usernameTo}
					&from=${duration.from.format(PwApiClient.DATE_FORMAT)}&to=${duration.to.format(PwApiClient.DATE_FORMAT)}`,
				{headers: { "Authorization": "Bearer " + token } });
		}

		public getUserById(id: string): ng.IHttpPromise<IUserVDTO> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";

			return this.$http.get(`/api/user?id=${id}`);
		}

		public getUsers(): ng.IHttpPromise<IUserVDTO[]> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";
			return this.$http.get(`/api/user`);
		}

		public getUsersBySearchText(searchText: string): ng.IHttpPromise<IUserVDTO[]> {
			this.$http.defaults.headers.post['Content-Type'] = "application/json";
			return this.$http.get(`/api/user?searchText=${searchText}`);
		}
	}

	angular.module("PW")
		.factory("PW.PwApiClient", ["$http", "$resource", "PW.SecurityProvider",
			($http, $resource, securityProvider) => new PwApiClient($http, $resource, securityProvider)]);

}
