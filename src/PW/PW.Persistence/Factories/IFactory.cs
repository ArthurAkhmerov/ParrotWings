namespace PW.Persistence.Factories
{
	public interface IFactory<TE, TDTO>
	{
		TE CreateEntity(TDTO dto);
		TDTO CreateDTO(TE e);
	}
}