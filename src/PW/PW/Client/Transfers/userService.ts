module PW {

	export interface IUserService {
		loadUsers(): ng.IPromise<IUserVDTO[]>;
		loadUsersBySearchText(searchText: string): ng.IPromise<IUserVDTO[]>;
		getUsers(): IUserVDTO[];
		getUserById(id: string): IUserVDTO;
		getCurrentUser(): IUserVDTO;
		addUser(user: IUserVDTO): void;
		joinUser(userId: string);
		updateUserState(userId: string, state: string): void;
		loadUserData(): ng.IPromise<IUserVDTO>;
		loadUserById(userId: string): ng.IPromise<IUserVDTO>;
	}

	export class UserService implements IUserService {
		private users: IUserVDTO[] = [];
		private currentUser: IUserVDTO;

		constructor(private $q: ng.IQService, private pwApiClient: IPwApiClient, private pwHubClient: PwHubClient, private authService: IAuthService) {

		}

		public loadUsers(): ng.IPromise<IUserVDTO[]> {
			var deferred = this.$q.defer<IUserVDTO[]>();

			this.pwApiClient.getUsers()
				.success((data: IUserVDTO[]) => {
					this.users = _.sortBy(data, x => x.createdAt);
					deferred.resolve(this.users);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}

		public loadUsersBySearchText(searchText: string): ng.IPromise<IUserVDTO[]> {
			var deferred = this.$q.defer<IUserVDTO[]>();

			this.pwApiClient.getUsersBySearchText(searchText)
				.success((data: IUserVDTO[]) => {
					this.users = _.sortBy(data, x => x.createdAt);
					deferred.resolve(this.users);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}

		public loadUserData(): ng.IPromise<IUserVDTO> {
			var deferred = this.$q.defer<IUserVDTO>();

			var currentUserId = this.authService.getCurrentUserId();
			var token = this.authService.getTokenData();
			if (token) {
				this.pwApiClient.getUserById(currentUserId)
					.success((data: IUserVDTO) => {
						this.currentUser = data;
						deferred.resolve(this.currentUser);
					})
					.error((reason) => deferred.reject(reason));
			} else {
				deferred.reject();
			}
			return deferred.promise;
		}

		public loadUserById(userId: string): ng.IPromise<IUserVDTO> {
			var deferred = this.$q.defer<IUserVDTO>();

			this.pwApiClient.getUserById(userId)
				.success((dto: IUserVDTO) => {
					this.addUser(dto);
					deferred.resolve(dto);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}

		public loadUsersByFilter(username: string): ng.IPromise<IUserVDTO[]> {
			var deferred = this.$q.defer<IUserVDTO[]>();

			this.pwApiClient.getUsers()
				.success((data: IUserVDTO[]) => {
					this.users = _.sortBy(data, x => x.createdAt);
					deferred.resolve(this.users);
				})
				.error((reason) => deferred.reject(reason));

			return deferred.promise;
		}


		public getUsers(): IUserVDTO[] { return this.users; }

		public getUserById(id: string): IUserVDTO {
			if (!this.users) return null;
			if (this.users.filter(x => x.id === id).length === 0) return null;

			return this.users.filter(x => x.id === id)[0];
		}

		public getCurrentUser(): IUserVDTO {
			return this.currentUser;
		}

		public addUser(user: IUserVDTO): void {
			this.users.push(user);
		}

		public joinUser(userId: string) {
			this.pwHubClient.joinUser(userId);
		}

		public updateUserState(userId: string, state: string): void {
			this.users.filter(x => x.id === userId).forEach(x => { x.state = state; });
		}
	}

	angular.module("PW")
		.factory('PW.UserService',
		["$q", "PW.PwApiClient", "PW.PwHubClient", "PW.AuthService",
			($q, pwApiClient, pwHubClient, authService) => new UserService($q, pwApiClient, pwHubClient, authService)]);
}