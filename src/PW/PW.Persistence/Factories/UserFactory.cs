using PW.Domain;
using PW.Persistence.DTO;

namespace PW.Persistence.Factories
{
	public class UserFactory: IFactory<User, UserPDTO>
	{
		public User CreateEntity(UserPDTO dto)
		{
			return new User(dto.Id, dto.Username,dto.Email, dto.Salt, dto.HashedPassword, dto.CreatedAt, (Role)dto.Role, (State)dto.State, dto.Balance);
		}

		public UserPDTO CreateDTO(User e)
		{
			return new UserPDTO
			{
				Id = e.Id,
				Username = e.Username,
				Email = e.Email,
				Salt = e.Salt,
				HashedPassword = e.HashedPassword,
				CreatedAt = e.CreatedAt,
				Role = (int)e.Role,
				State = (int)e.State,
				Balance = e.Balance
			};
		}
	}
}