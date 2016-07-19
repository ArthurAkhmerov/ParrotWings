using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MvvmCross.Core.ViewModels;
using PW.Mobile.API;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class SignupViewModel : MvxViewModel
	{
		private readonly IAuthService _authService;
		private IList<Error> errors;

		public SignupViewModel(IAuthService authService)
		{
			_authService = authService;
		}

		public void Init()
		{
			Name = string.Empty;
			Email = string.Empty;
			Password = string.Empty;
			ConfirmPassword = string.Empty;
			MessageInfo = string.Empty;
			IsLoading = false;
			errors = new List<Error>();
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				SetProperty(ref _name, value);
				SignUpCommand.RaiseCanExecuteChanged();
			}
		}

		private string _email;
		public string Email
		{
			get { return _email; }
			set
			{
				SetProperty(ref _email, value);
				SignUpCommand.RaiseCanExecuteChanged();
			}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				SetProperty(ref _password, value);
				SignUpCommand.RaiseCanExecuteChanged();
			}
		}

		private string _confirmPassword;
		public string ConfirmPassword
		{
			get { return _confirmPassword; }
			set
			{
				SetProperty(ref _confirmPassword, value);
				SignUpCommand.RaiseCanExecuteChanged();
			}
		}

		private string _messageInfo;
		public string MessageInfo
		{
			get { return _messageInfo; }
			set { SetProperty(ref _messageInfo, value); }
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				SetProperty(ref _isLoading, value);
				SignUpCommand.RaiseCanExecuteChanged();
			}
		}

		private MvxCommand _signUpCommand;
		public IMvxCommand SignUpCommand
		{
			get
			{
				return _signUpCommand = _signUpCommand ?? new MvxCommand(async () =>
				{
					var errorExists = CheckForErrors();
					if (!errorExists)
					{
						MessageInfo = "Signing...";
						IsLoading = true;
						var result = await _authService.SignUp(Name, Email, Password);
						IsLoading = false;

						if (result != null && result.Success)
						{
							MessageInfo = String.Empty;
							ShowViewModel<LoginViewModel>();
							Close(this);
						}
						else
						{
							var error = new Error
							{
								Type = ErrorType.ServerError,
								Message = result.Message
							};
							errors.Add(error);
							MessageInfo = result.Message;
						}
					}
					else
					{
						MessageInfo = errors.First().Message;
					}
					
				}, CanSignUp);
			}
		}

		public bool CanSignUp()
		{
			if (IsLoading) return false;
			if (string.IsNullOrWhiteSpace(Name)) return false;
			if (string.IsNullOrWhiteSpace(Email)) return false;
			if (string.IsNullOrWhiteSpace(Password)) return false;
			if (string.IsNullOrWhiteSpace(ConfirmPassword)) return false;

			return true;
		}

		private bool CheckForErrors()
		{
			errors.Clear();
			if (!Password.Equals(ConfirmPassword))
			{
				var error = new Error
				{
					Type = ErrorType.InputError,
					Message = "Your password and confirmation password do not match"
				};
				errors.Add(error);

			}

			if (!Validator.EmailIsValid(Email))
			{
				var error = new Error
				{
					Type = ErrorType.InputError,
					Message = "Email address is not valid"
				};
				errors.Add(error);
			}

			return errors.Any();
		}


		//		private checkForSignUpErrors(): boolean {
		//			this.inputErrors = [];

		//			if (this.name.trim() == "") {
		//				this.inputErrors.push({
		//					code: "name", message: "Name cannot be empty"
		//				});
		//			}
		//			if (this.email.trim() == "") {
		//	this.inputErrors.push({
		//		code: "email", message: "Email cannot be empty"
		//				});
		//} else if (!this.validateEmail(this.email)) {
		//	this.inputErrors.push({
		//		code: "email", message: "Email is not valid"
		//				});
		//}
		//			if (this.password.trim() == "") {
		//	this.inputErrors.push({
		//		code: "password", message: "Password cannot be empty"
		//				});
		//}
		//			if (this.password != this.confirmPassword) {
		//	this.inputErrors.push({
		//		code: "password", message: "Your password and confirmation password do not match"
		//				});
		//}

		//			return this.inputErrors.length > 0;
		//}
	}
}