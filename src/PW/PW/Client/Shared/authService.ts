﻿module PW {

	export class Auth {
		constructor(public userId: string, public sessionId: string, public isSignedIn: boolean = true) { }
	}

	export interface IAuthService {
		signIn(email: string, password: string): ng.IPromise<IAuthResultVDTO>;
		signUp(name: string, email: string, password: string): ng.IPromise<IAuthResultVDTO>;
		signOut(): void;
		isSignedIn(): boolean;
		getAuth(): Auth;
	}

	export class AuthService implements IAuthService {
		private auth: Auth;

		constructor(private $q: ng.IQService, private $cookies: ng.cookies.ICookiesService, private pwApiClient: IPwApiClient, private pwHubClient: PwHubClient) {
			var userId :string= this.$cookies.get("userId");
			var sessionId: string = this.$cookies.get("sessionId");

			if (userId && sessionId) {
				this.auth = new Auth(userId, sessionId);
			}
		}

		public signIn(email: string, password: string): ng.IPromise<IAuthResultVDTO> {
			var deferred = this.$q.defer();

			var authRequestDto: IAuthRequestVDTO = {
				email: email,
				password: password
			};

			this.pwApiClient.signIn(authRequestDto).success((result) => {
				if (result && result.success) {
					this.auth = new Auth(result.data.userId, result.data.sessionId);

					this.$cookies.put("userId", this.auth.userId, { expires: moment().add("month", 3).toDate(), path: "/" });
					this.$cookies.put("sessionId", this.auth.sessionId, { expires: moment().add("month", 3).toDate(), path: "/" });
				} else {
					this.auth = null;
				}
				deferred.resolve(result);

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
			this.$cookies.remove("userId");
			this.$cookies.remove("sessionId");
			this.auth = null;
		}

		public isSignedIn(): boolean {
			return this.auth && this.auth.isSignedIn;
		}

		public getAuth(): Auth {
			return this.auth;
		}
	}

	angular.module("PW").factory("PW.AuthService",
		["$q", "$cookies", "PW.PwApiClient", "PW.PwHubClient",
			($q, $cookies, pwApiClient, pwHubClient) => new AuthService($q, $cookies, pwApiClient, pwHubClient)]);
}