using Model;

namespace Sep3DataTier.Repository;

public interface IReportEfcDao
{
    Task<IEnumerable<Model.Report>> GetAsync();
}