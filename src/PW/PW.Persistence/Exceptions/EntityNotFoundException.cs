using System;

namespace PW.Persistence.Exceptions
{
	public class EntityNotFoundException : ApplicationException
	{
		public EntityNotFoundException(string keyRepresentation, string entity) : base($"Not found {entity} with key {keyRepresentation}") { }
	}
}