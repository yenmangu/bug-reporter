using System;
using System.ComponentModel;
using Bugreporter.Client.Features.ReportBug;
using Bugreporter.Client.Shared;

namespace Bugreporter.Client.Pages.ReportBug;

public class ReportBugViewModel : ViewModelBase
{
    public ReportBugFormViewModel ReportBugFormViewModel { get; }

    public ReportBugViewModel(ReportBugFormViewModel reportBugFormViewModel)
    {
        ReportBugFormViewModel = reportBugFormViewModel;
    }
}