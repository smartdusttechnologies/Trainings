using MultiDatabase.Data;
namespace MultiDatabase.Repository.Interface
{
    public interface IDbContextFactory
    {
        ApplicationDbContext GetDbContext(string connectionstring, string provider);
    }
}
