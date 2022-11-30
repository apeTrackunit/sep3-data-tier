using Model;

namespace Sep3DataTier.Repository;

public interface IReportDao
{
    Task<IEnumerable<Model.Report>> GetAsync();
}