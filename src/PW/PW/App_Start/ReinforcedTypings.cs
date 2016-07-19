using PW.API.DTO;
using PW.Hubs;
using Reinforced.Typings.Fluent;

namespace PW
{
	public static class ReinforcedTypings
	{
		public static void Configure(ConfigurationBuilder builder)
		{
			var userVDTO = builder.ExportAsInterface<UserVDTO>().OverrideNamespace("PW")
				.WithAllProperties();
			userVDTO.WithProperty(x => x.Id).Type<string>();
			userVDTO.WithProperty(x => x.CreatedAt).Type<string>();

			var pwClient = builder.ExportAsInterface<PwClient>().OverrideNamespace("PW")
				.WithAllProperties();
			pwClient.WithProperty(x => x.UserId).Type<string>();

			var transferVDTO = builder.ExportAsInterface<TransferVDTO>().OverrideNamespace("PW")
				.WithAllProperties();
			transferVDTO.WithProperty(x => x.Id).Type<string>();
			transferVDTO.WithProperty(x => x.CreatedAt).Type<string>();
			transferVDTO.WithProperty(x => x.UserFromId).Type<string>();
			transferVDTO.WithProperty(x => x.UserToId).Type<string>();

			var transferRequestVDTO = builder.ExportAsInterface<TransferRequestVDTO>().OverrideNamespace("PW");
				//.WithAllProperties();
			transferRequestVDTO.WithProperty(x => x.Amount).Type<int>();
			transferRequestVDTO.WithProperty(x => x.RecipientsIds).Type<string[]>();
			transferRequestVDTO.WithProperty(x => x.UserFromId).Type<string>();

			var sendTransferResultVDTO = builder.ExportAsInterface<SendTransferResultVDTO>().OverrideNamespace("PW")
				.WithAllProperties();


			var authRequestVDTO = builder.ExportAsInterface<AuthRequestVDTO>().OverrideNamespace("PW")
				.WithAllProperties();

			var signupRequestVDTO = builder.ExportAsInterface<SignupRequestVDTO>().OverrideNamespace("PW")
				.WithAllProperties();

			var authData = builder.ExportAsInterface<AuthData>().OverrideNamespace("PW")
				.WithAllProperties();
			authData.WithProperty(x => x.UserId).Type<string>();
			authData.WithProperty(x => x.SessionId).Type<string>();

			var authResultVDTO = builder.ExportAsInterface<AuthResultVDTO>().OverrideNamespace("PW")
				.WithAllProperties();
			authResultVDTO.WithProperty(x => x.Data).Type<AuthData>();
		}
	}
}