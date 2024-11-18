using System;

namespace Bugreporter.Core.Features.ReportBug;

public class ReportBugRequest
{
    public string Summary { get; set; }
    public string Description { get; set; }
}