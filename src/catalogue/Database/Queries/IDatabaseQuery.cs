using System.Data;

namespace catalogue.Database.Queries
{
	public interface IDatabaseQuery
	{
		IDataReader ExecuteQuery(IQuery command);
	}
}