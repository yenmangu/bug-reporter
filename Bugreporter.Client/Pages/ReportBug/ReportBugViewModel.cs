using System;
using System.ComponentModel;
using Bugreporter.Client.Shared;

namespace Bugreporter.Client.Pages.ReportBug;

public class ReportBugViewModel : ViewModelBase
{
    private string _value = string.Empty;
    public string Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }
}
