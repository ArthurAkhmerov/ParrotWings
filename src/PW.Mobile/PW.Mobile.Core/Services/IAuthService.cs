using System.Threading.Tasks;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core.Services
{
	public interface IAuthService
	{
		Task<Auth> SignIn(string email, string password);
		Task<AuthResultVDTO> SignUp(string name, string email, string password);
		void SignOut();
		bool IsSignedIn();
		Auth GetAuth();
	}

}