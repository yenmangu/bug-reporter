using System;

namespace Bugreporter.API.Features.ReportBug;

public class ReportedBug
{
    public string Id { get; }
    public string Summary { get; }
    public string Description { get; }

    public ReportedBug(string id, string summary, string description)
    {
        Id = id;
        Summary = summary;
        Description = description;
    }
}
