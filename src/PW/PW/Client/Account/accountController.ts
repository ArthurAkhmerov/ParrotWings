module PW {

	export class Error {
		public code: string;
		public message: string;
	}

	export class AccountController {
		private inputErrors: Error[] = [];
		private authErrors: Error[] = [];
		private serverErrors: Error[] = [];

		private name: string = "";
		private email: string = "";
		private password: string = "";
		private confirmPassword: string = "";

		constructor(private authService: IAuthService) {
			authService.signOut();
		}

		public signIn(): void {
			this.authErrors = [];
			this.serverErrors = [];
			if (!this.checkForSignInErrors()) {
				this.authService.signIn(this.email, this.password).then(result => {
					if (result.success) {
						location.href = location.origin + "/transfer";
					} else {
						this.authErrors.push({ code: "serverError", message: result.message });
					}
				}).catch(reason => {
					this.serverErrors.push({ code: "server", message: reason });
				});
			}
		}

		public signUp(): void {
			this.authErrors = [];
			this.serverErrors = [];
			if (!this.checkForSignUpErrors()) {
				this.authService.signUp(this.name, this.email, this.password).then(result => {
					if (result.success) {
						location.href = location.origin + "/account/signin";
					} else {
						this.authErrors.push({ code: "auth", message: result.message });
					}
				}).catch(reason => {
					this.serverErrors.push({ code: "server", message: reason });
				});
			}
		}

		private checkForSignInErrors(): boolean {
			this.inputErrors = [];

			if (this.email.trim() == "") {
				this.inputErrors.push({
					code: "email", message: "Email cannot be empty"
				});
			} else if (!this.validateEmail(this.email)) {
				this.inputErrors.push({
					code: "email", message: "Email address is not valid"
				});
			}
			if (this.password.trim() == "") {
				this.inputErrors.push({
					code: "password", message: "Password cannot be empty"
				});
			}

			return this.inputErrors.length > 0;
		}

		private checkForSignUpErrors(): boolean {
			this.inputErrors = [];

			if (this.name.trim() == "") {
				this.inputErrors.push({
					code: "name", message: "Name cannot be empty"
				});
			}
			if (this.email.trim() == "") {
				this.inputErrors.push({
					code: "email", message: "Email cannot be empty"
				});
			} else if (!this.validateEmail(this.email)) {
				this.inputErrors.push({
					code: "email", message: "Email address is not valid"
				});
			}
			if (this.password.trim() == "") {
				this.inputErrors.push({
					code: "password", message: "Password cannot be empty"
				});
			}
			if (this.password != this.confirmPassword) {
				this.inputErrors.push({
					code: "password", message: "Your password and confirmation password do not match"
				});
			}

			return this.inputErrors.length > 0;
		}

		private validateEmail(email): boolean {
			var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
			return re.test(email);
		}
	}

	angular.module("PW")
		.controller("PW.AccountController",
		["PW.AuthService", (authService) => new AccountController(authService)]);
}