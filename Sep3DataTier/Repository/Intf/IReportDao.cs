using Model;

namespace Sep3DataTier.Repository;

public interface IReportDao
{
    Task<IEnumerable<Model.Report>> GetAsync();
    Task<Model.Report> CreateAsync(Model.Report report);
    Task<string> UpdateReviewAsync(string reportId, string status);
}