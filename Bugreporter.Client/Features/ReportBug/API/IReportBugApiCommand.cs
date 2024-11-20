using System.Threading.Tasks;
using Refit;
using Bugreporter.Core.Features.ReportBug;

namespace Bugreporter.Client.Features.ReportBug.API;

public interface IReportBugApiCommand
{
    // First we define a methods that will hit our backend API
    // Task to ensure we are not blocking and are using async
    // Refit config
    
    [Post("/bugs")]
    Task<ReportBugResponse> Execute(
        [Body(true)] ReportBugRequest bug);
}