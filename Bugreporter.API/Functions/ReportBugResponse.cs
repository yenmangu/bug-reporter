using System;

namespace Bugreporter.API.Functions;

public class ReportBugResponse
{
    public string Id { get; set; }
    public string summary { get; set; }
    public string description { get; set; }
}
