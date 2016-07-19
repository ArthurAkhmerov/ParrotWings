namespace PW.Domain.Infrastructure
{
	public interface ISecurityProvider
	{
		string CalculateMD5(params object[] data);
		string CalculateSHA256(params object[] data);
		string CreateSalt(int size = 10);
	}
}