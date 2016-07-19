module PW {

	export class TransfersController {
		private usernameInput: string;
		private dateInput: string;
		private currentDuration: DatepickerOptions;
		private duration: Duration;
		private errorMessage: string;
		private transfers: Transfer[];
		private transfersCount: number;
		private page: number;
		private allPagesCount: number;

		public static get PAGE_SIZE(): number { return 5; }

		constructor(private $uibModal: ng.ui.bootstrap.IModalService,
			private $timeout: ng.ITimeoutService,
			private notification: INotificationService,
			private pwHubClient: PwHubClient,
			private pwApiClient: PwApiClient,
			private authService: IAuthService,
			private userService: IUserService, private transferService: ITransferService) {

			this.currentDuration = {
				startDate: moment().add(-2, "month"),
				endDate: moment(),
			};

			this.setupEvents(() => {
				var auth = this.authService.getAuth();
				if (auth.isSignedIn) {
					this.userService.joinUser(auth.userId, auth.sessionId);
				}
			});

			this.userService.loadUserData().then(data => {
				this.applyFilter();
			});
		}


		public updateCurrentDuration() {
			if (this.currentDuration.startDate != undefined && this.currentDuration.startDate != null) {
				this.duration = {
					from: this.currentDuration.startDate.add(-1, 'day').endOf('day'),
					to: this.currentDuration.endDate.endOf('day')
				};
			}
		}

		public applyFilter() {
			this.updateCurrentDuration();
			if (this.usernameInput && this.usernameInput.trim() != '') {
				this.pwApiClient.getTransfersCountByUserTo(this.getCurrentUser().id, this.usernameInput, this.duration)
					.success(result => this.transfersCount = result);
			} else {
				this.pwApiClient.getTransfersCount(this.getCurrentUser().id, this.duration)
					.success(result => this.transfersCount = result);
			}

			this.goToPage(1);
		}

		public goToPage(page: number) {
			this.page = page;
			//this.transfers = _.take(_.drop(this.transferService.getTransfers(), 3 * (page - 1)), 3);
			var paging = {
				skip: TransfersController.PAGE_SIZE * (page - 1),
				take: TransfersController.PAGE_SIZE
			};
			if (this.usernameInput && this.usernameInput.trim() != '') {
				this.transferService.loadTransfersByUserTo(this.usernameInput, this.duration, paging)
					.then(data => {
						this.transfers = this.transferService.getTransfers();
					});
			} else {
				this.transferService.loadTransfers(this.duration, paging)
					.then(data => {
						this.transfers = this.transferService.getTransfers();
					});
			}

		}

		public getPages() {
			var input = [];
			if (!this.transferService.getTransfers()) return input;

			this.allPagesCount = Math.ceil(this.transfersCount / TransfersController.PAGE_SIZE);

			if (this.page <= 12) {
				if (this.allPagesCount > 12) {
					for (var i = 1; i <= 12; i++) {
						input.push(i);
					}
				}  else {
					for (var i = 1; i <= this.allPagesCount; i++) {
						input.push(i);
					}
				}
			} else {
				for (var i = this.page - 11; i <= this.page; i++) {
					input.push(i);
				}
			}


			return input;
		};

		public prev() {
			if (this.page > 1) {
				this.goToPage(this.page - 1);
			}
		}
		public next() {
			if (this.page < this.allPagesCount) {
				this.goToPage(this.page + 1);
			}
		}

		private setupEvents(callback: any): void {
			var events: SignalrEvents[] = [];

			events.push(<SignalrEvents>{
				eventName: 'userJoined',
				callback: (user: IUserVDTO) => {
					var currentUser = this.userService.getCurrentUser();
					if (user.id != currentUser.id) {
						var userEmail = user.email;
						if (this.userService.getUserById(user.id) !== null) {
							this.userService.updateUserState(user.id, "Online");
						} else {
							this.userService.addUser(user);
						}
					}
				}
			});
			events.push(<SignalrEvents>{
				eventName: 'userLeft',
				callback: (user: IUserVDTO) => {
					this.userService.updateUserState(user.id, "Offline");
				}
			});

			events.push(<SignalrEvents>{
				eventName: 'transferSent',
				callback: (dto: ITransferVDTO) => {

					this.transferService.addTransfer(dto);
					var addedTransfer = this.transferService.getTransferById(dto.id);
					if (addedTransfer.isIncoming) {
						this.notification.success(`new transfer(${addedTransfer.data.amount}) from ${addedTransfer.data.userFromName}`);
					} else {
						this.notification.success("your transfer has been sent");
					}

					if (this.page == 1) {
						this.transfers.unshift(addedTransfer);
					}

				}
			});

			this.pwHubClient.on(events, callback);
		}

		public getCurrentUser(): IUserVDTO {
			return this.userService.getCurrentUser();
		}

		public showMakeTransferModal(transfer: Transfer): void {
			var modalInstance = this.$uibModal.open({
				templateUrl: '/Client/Transfers/makeTransferModal.html',
				controller: 'PW.MakeTransferModalController',
				size: 'lg',
				resolve: {
					transfer: () => transfer
					//channel: () => null
				}
			});

			modalInstance.result
				.then((result) => { })
				.catch(() => { });
		}

		private filterTransfers(): Transfer[] {
			if (this.transferService.getTransfers()) {
				if (this.usernameInput && this.usernameInput.trim() != '') {
					return this.transferService.getTransfers()
						.filter(x => (x.data.userFromName.indexOf(this.usernameInput) != -1
							|| x.data.userToName.indexOf(this.usernameInput) != -1)
							&& moment(x.data.createdAt, 'YYYY-MM-DD').isAfter(this.duration.from)
							&& moment(x.data.createdAt, 'YYYY-MM-DD').isBefore(this.duration.to));
				} else if (this.duration) {
					return this.transferService.getTransfers()
						.filter(x => moment(x.data.createdAt, 'YYYY-MM-DD').isAfter(this.duration.from)
							&& moment(x.data.createdAt, 'YYYY-MM-DD').isBefore(this.duration.to));
				}
				return this.transferService.getTransfers();
			}
		}
	}

	angular.module("PW")
		.controller("PW.TransfersController",
		["$uibModal", "$timeout", "PW.NotificationService", "PW.PwHubClient", "PW.PwApiClient", "PW.AuthService", "PW.UserService", "PW.TransferService",
			($uibModal, $timeout, notificationService, pwHubClient, pwApiClient, authService, userService, transfersService) =>
				new TransfersController($uibModal, $timeout, notificationService, pwHubClient, pwApiClient, authService, userService, transfersService)]);
}