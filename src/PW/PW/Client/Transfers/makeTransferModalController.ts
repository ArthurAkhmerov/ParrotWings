module PW {

	export class MakeTransferModalController {

		constructor(private $rootScope: ng.IRootScopeService, private $scope: any,
			private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
			private userService: IUserService,
			private transferService: ITransferService,
			private transfer: Transfer) {
			$scope.ok = () => this.ok();
			$scope.cancel = () => this.cancel();
			$scope.ShowUsersList = (enabled: boolean) => this.ShowUsersList(enabled);
			$scope.selectUser = (user: IUserVDTO) => this.selectUser(user);
			$scope.unselectUser = (user: IUserVDTO) => this.unselectUser(user);
			$scope.unselectedUsers = () => this.unselectedUsers();
			$scope.recipientInput = "";
			
			$scope.amount = 0;
			$scope.selectedUsers = [];

			$scope.$watch('recipientInput', (newVal, oldVal) => {
				if (newVal.length > 1) {
					this.userService.loadUsersBySearchText(newVal)
						.then(data => {
							this.$scope.users = data.filter(x => x.id != this.userService.getCurrentUser().id);
						});
				}
			});

			if (transfer) {
				$scope.amount = transfer.data.amount;
				$scope.selectedUsers = [this.userService.getUserById(transfer.data.userToId)];
			}
			$scope.users =  this.userService.getUsers().filter(x => x.id != this.userService.getCurrentUser().id);
		}

		
		public ok() {
			if (!(this.$scope.amount > 0)) {
				this.$scope.errorMessage = "Amount must be greater than 0";
				return;
			}

			if (!this.$scope.selectedUsers || this.$scope.selectedUsers.length == 0) {
				this.$scope.errorMessage = "Choose recipient";
				return;
			}

			var currentUser = this.userService.getCurrentUser();

			if (currentUser.balance < this.$scope.amount * this.$scope.selectedUsers.length) {
				this.$scope.errorMessage = "Invalid balance";

				return;
			}

			var selectedUsers: IUserVDTO[] = this.$scope.selectedUsers;
			var dto = <ITransferRequestVDTO>{
				amount: this.$scope.amount,
				recipientsIds: selectedUsers.map(x => x.id),
				userFromId: this.userService.getCurrentUser().id,
			};

			
				
			this.transferService.sendTransfer(dto).then(result => {
				if (result.success) {
					this.$uibModalInstance.close();
				} else {
					this.$scope.errorMessage = "The operation has not been completed. " + result.message;
				}

			});
		}

		public cancel() {
			this.$uibModalInstance.dismiss();
		}

		public unselectedUsers() {
			return _.difference(this.$scope.users, this.$scope.selectedUsers);
		}


		public ShowUsersList(enabled: boolean) {
		
			this.$scope.showUsers = enabled;
		}

		public selectUser(user: IUserVDTO) {
			if (this.$scope.selectedUsers.filter(x => x.id === user.id).length === 0)
				this.$scope.selectedUsers.push(user);
		}

		public unselectUser(user: IUserVDTO) {
			this.$scope.selectedUsers = this.$scope.selectedUsers.filter(x => x.id !== user.id);
		}

	}

	angular.module('PW').controller('PW.MakeTransferModalController',
		["$rootScope", "$scope", "$uibModalInstance", "PW.UserService", "PW.TransferService","transfer",
			($rootScope, $scope, $uibModalInstance, userService, transferService, transfer) => new
				MakeTransferModalController($rootScope, $scope, $uibModalInstance, userService, transferService, transfer)]);
}
