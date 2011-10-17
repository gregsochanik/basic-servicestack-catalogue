using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace catalogue.Database.Queries
{
	public class DatabaseQuery : IDatabaseQuery
	{
		private readonly ConnectionStringSettings _connectionString;

		public DatabaseQuery() {
			_connectionString = ConfigurationManager.ConnectionStrings["Catalogue"];
			if (_connectionString == null)
				throw new ArgumentException("Required connection string \"Catalogue\" is missing");
		}

		public IDataReader ExecuteQuery(IQuery command) {
			var connection = new SqlConnection(_connectionString.ConnectionString);
			var cmd = new SqlCommand(command.Command, connection);
			foreach (var parameter in command.Parameters) {
				cmd.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
			}
			connection.Open();
			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}
	}
}