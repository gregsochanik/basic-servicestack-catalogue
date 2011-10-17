namespace catalogue.Database.Repositories
{
	public interface IDbRepository<T>
	{
		T Get(int id);
	}
}