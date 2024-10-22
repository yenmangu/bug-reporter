using System;

namespace Bugreporter.API.Functions;

public class ReportBugRequest
{
    public string Summary { get; set; }
    public string Description { get; set; }
}
