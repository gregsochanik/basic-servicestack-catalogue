using System.Collections.Generic;

namespace catalogue.Database.Queries
{
	public interface IQuery {
		string Command { get; }
		IDictionary<string, object> Parameters { get; } 
	}
}