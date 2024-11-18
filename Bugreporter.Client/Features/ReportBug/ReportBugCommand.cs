using System;
using System.Threading.Tasks;
using Bugreporter.Client.Features.ReportBug.API;
using Bugreporter.Client.Shared.Commands;
using Bugreporter.Core.Features.ReportBug;
using Microsoft.Maui.Controls;

namespace Bugreporter.Client.Features.ReportBug;

public class ReportBugCommand : AsyncCommandBase
{
    private readonly ReportBugFormViewModel _viewModel;
    private readonly IReportBugApiCommand _reportBugApiCommand;

    public ReportBugCommand(ReportBugFormViewModel viewModel
        , IReportBugApiCommand? reportBugApiCommand)
    {
        _viewModel = viewModel;
        _reportBugApiCommand = reportBugApiCommand;
    }


    protected override async Task ExecuteAsync(object parameter)
    {
        // We have access to the _viewModel property, and therefore the fields,
        // because we are injecting ViewModel through the constructor.

        ReportBugRequest request = new()
        {
            Summary = _viewModel.Summary, Description = _viewModel.Description
        };
        try
        {
            ReportBugResponse response =
                await _reportBugApiCommand.Execute(request);
            await Application.Current.MainPage.DisplayAlert(
                "Success", $"Successfully reported bug #{response.Id}", "OK"
            );
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error", "Failed to report bug"
                , "Ok"
            );
        }
    }
}