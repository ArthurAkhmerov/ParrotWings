namespace PW.Mobile.Core.Model
{
	public class Error
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public ErrorType Type { get; set; } 
	}

	public enum ErrorType
	{
		InputError, ServerError, AuthError
	}
}