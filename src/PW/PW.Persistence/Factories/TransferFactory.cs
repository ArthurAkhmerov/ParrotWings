using PW.Domain;
using PW.Persistence.DTO;
using PW.Persistence.Repositories;

namespace PW.Persistence.Factories
{
	public class TransferFactory: IFactory<Transfer, TransferPDTO>
	{
		private readonly UserFactory _userFactory = new UserFactory();

		public Transfer CreateEntity(TransferPDTO dto)
		{
			return new Transfer(dto.Id, _userFactory.CreateEntity(dto.UserFrom),_userFactory.CreateEntity(dto.UserTo), dto.Amount, dto.CreatedAt);
		}

		public TransferPDTO CreateDTO(Transfer e)
		{
			return new TransferPDTO
			{
				Id = e.Id,
				UserFromId = e.UserFrom.Id,
				UserToId = e.UserTo.Id,
				Amount = e.Amount,
				CreatedAt = e.CreatedAt
			};
		}
	}
}