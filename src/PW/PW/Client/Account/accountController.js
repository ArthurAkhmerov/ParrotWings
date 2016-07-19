var PW;
(function (PW) {
    var Error = (function () {
        function Error() {
        }
        return Error;
    })();
    PW.Error = Error;
    var AccountController = (function () {
        function AccountController(authService) {
            this.authService = authService;
            this.inputErrors = [];
            this.authErrors = [];
            this.serverErrors = [];
            this.name = "";
            this.email = "";
            this.password = "";
            this.confirmPassword = "";
            authService.signOut();
        }
        AccountController.prototype.signIn = function () {
            var _this = this;
            this.authErrors = [];
            this.serverErrors = [];
            if (!this.checkForSignInErrors()) {
                this.authService.signIn(this.email, this.password).then(function (result) {
                    if (result.success) {
                        location.href = location.origin + "/transfer";
                    }
                    else {
                        _this.authErrors.push({ code: "serverError", message: result.message });
                    }
                }).catch(function (reason) {
                    _this.serverErrors.push({ code: "server", message: reason });
                });
            }
        };
        AccountController.prototype.signUp = function () {
            var _this = this;
            this.authErrors = [];
            this.serverErrors = [];
            if (!this.checkForSignUpErrors()) {
                this.authService.signUp(this.name, this.email, this.password).then(function (result) {
                    if (result.success) {
                        location.href = location.origin + "/account/signin";
                    }
                    else {
                        _this.authErrors.push({ code: "auth", message: result.message });
                    }
                }).catch(function (reason) {
                    _this.serverErrors.push({ code: "server", message: reason });
                });
            }
        };
        AccountController.prototype.checkForSignInErrors = function () {
            this.inputErrors = [];
            if (this.email.trim() == "") {
                this.inputErrors.push({
                    code: "email", message: "Email cannot be empty"
                });
            }
            else if (!this.validateEmail(this.email)) {
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
        };
        AccountController.prototype.checkForSignUpErrors = function () {
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
            }
            else if (!this.validateEmail(this.email)) {
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
        };
        AccountController.prototype.validateEmail = function (email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        };
        return AccountController;
    })();
    PW.AccountController = AccountController;
    angular.module("PW")
        .controller("PW.AccountController", ["PW.AuthService", function (authService) { return new AccountController(authService); }]);
})(PW || (PW = {}));
//# sourceMappingURL=accountController.js.map