using Model;

namespace Sep3DataTier.Repository;

public interface IReportDao
{
    Task<IEnumerable<Model.Report>> GetAsync(string email, bool approved);
    Task<Model.Report> CreateAsync(Model.Report report);
    Task<string> UpdateReviewAsync(string reportId, string status);
}