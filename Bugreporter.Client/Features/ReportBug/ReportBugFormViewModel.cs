using System.Windows.Input;
using Bugreporter.Client.Shared;

namespace Bugreporter.Client.Features.ReportBug;

public class ReportBugFormViewModel : ViewModelBase
{
    private string? _summary;

    public string Summary
    {
        get
        {
            if (_summary != null)
            {
                return _summary;
            }
            return null!;
        }
        set
        {
            _summary = value;
            OnPropertyChanged(nameof(Summary));
        }
    }

    private string? _description;

    public string Description

    {
        get
        {
            if (_description != null)
            {
                return _description;
            }
            return null!;
        }
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public ICommand ReportBugCommand { get; }

    public ReportBugFormViewModel()
    {
        ReportBugCommand = new ReportBugCommand(this);
    }
}