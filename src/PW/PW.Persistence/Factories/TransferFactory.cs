using PW.Domain;
using PW.Persistence.DTO;
using PW.Persistence.Repositories;

namespace PW.Persistence.Factories
{
	public class TransferFactory: IFactory<Transfer, TransferPDTO>
	{
		public Transfer CreateEntity(TransferPDTO dto)
		{
			return new Transfer(dto.Id, dto.UserFromId, dto.UserToId, dto.Amount, dto.CreatedAt);
		}

		public TransferPDTO CreateDTO(Transfer e)
		{
			return new TransferPDTO
			{
				Id = e.Id,
				UserFromId = e.UserFromId,
				UserToId = e.UserToId,
				Amount = e.Amount,
				CreatedAt = e.CreatedAt
			};
		}
	}
}