module PW {

	export interface IAuthService {
		signIn(email: string, password: string): ng.IPromise<TokenData>;
		signUp(name: string, email: string, password: string): ng.IPromise<IAuthResultVDTO>;
		signOut(): void;
		isSignedIn(): boolean;
		getTokenData(): TokenData;
		getCurrentUserId(): string;
	}

	export class AuthService implements IAuthService {
		private token: TokenData;
		private currentUserId: string;

		constructor(private $q: ng.IQService, private $cookies: ng.cookies.ICookiesService, private pwApiClient: IPwApiClient, private pwHubClient: PwHubClient) {
			var pwCookie: string = this.$cookies.get("pw");
			var ucCookie: string = this.$cookies.get("cu");
			
			if (pwCookie && ucCookie) {
				this.token = JSON.parse(pwCookie);
				this.currentUserId = ucCookie;
			}
		}

		public signIn(email: string, password: string): ng.IPromise<TokenData> {
			var deferred = this.$q.defer();

			var authRequestDto: IAuthRequestVDTO = {
				email: email,
				password: password
			};

			this.pwApiClient.getToken(email, password).success(result => {
				this.token = result;

				this.pwApiClient.getUsersBySearchText(email).success(data => {
					if (data && data.length > 0) {
						this.currentUserId = data[0].id;
						this.$cookies.put("pw", JSON.stringify(this.token), { expires: moment().add("month", 3).toDate(), path: "/" });
						this.$cookies.put("cu", this.currentUserId, { expires: moment().add("month", 3).toDate(), path: "/" });
						deferred.resolve(this.token);
					} else {
						deferred.reject();
					}
				}).catch(() => { deferred.reject() });

			}).error(reason => deferred.reject(reason));


			return deferred.promise;
		}

		public signUp(name: string, email: string, password: string): ng.IPromise<IAuthResultVDTO> {
			var deferred = this.$q.defer();

			var signUpRequestDto: ISignupRequestVDTO = {
				name: name,
				email: email,
				password: password
			};

			this.pwApiClient.signUp(signUpRequestDto).success((result) => {
				deferred.resolve(result);
			}).error(reason => deferred.reject(reason));

			return deferred.promise;
		}

		public signOut(): void {
			this.$cookies.remove("pw");
			this.token = null;
		}

		public isSignedIn(): boolean {
			return this.token != undefined && this.token != null;
		}

		public getTokenData(): TokenData {
			return this.token;
		}

		public getCurrentUserId(): string {
			return this.currentUserId;
		}
	}

	angular.module("PW").factory("PW.AuthService",
		["$q", "$cookies", "PW.PwApiClient", "PW.PwHubClient",
			($q, $cookies, pwApiClient, pwHubClient) => new AuthService($q, $cookies, pwApiClient, pwHubClient)]);
}