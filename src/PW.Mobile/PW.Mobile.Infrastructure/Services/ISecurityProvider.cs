namespace PW.Mobile.Infrastructure.Services
{
	public interface ISecurityProvider
	{
		string CalculateMD5(params object[] data);
	}
}