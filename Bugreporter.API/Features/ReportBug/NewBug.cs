using System;

namespace Bugreporter.API.Features.ReportBug;

public class NewBug
{
    // readonly at the moment
    public string Summary { get; }
    public string Description { get; }

    public NewBug(string summary, string description)
    {
        Summary = summary;
        Description = description;
    }
}
