using MultiDatabase.Data;
namespace MultiDatabase.Repository.Interface
{
    public interface IDbContextFactory
    {
        ApplicationDbContext CreateApplicationDbContext();
        Application2DbContext CreateApplication2DbContext();
       
    }
}
