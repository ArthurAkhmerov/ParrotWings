using PW.Domain;
using PW.Persistence.DTO;

namespace PW.Persistence.Factories
{
	public class SessionFactory: IFactory<Session, SessionPDTO>
	{
		public Session CreateEntity(SessionPDTO dto)
		{
			return new Session(dto.Id, dto.UserId, dto.CreatedAt, dto.LastUsage);
		}

		public SessionPDTO CreateDTO(Session e)
		{
			return new SessionPDTO
			{
				Id = e.Id,
				UserId = e.UserId,
				CreatedAt = e.CreatedAt,
				LastUsage = e.LastUsage
			};
		}
	}
}