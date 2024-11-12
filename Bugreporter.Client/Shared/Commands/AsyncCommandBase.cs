using System.Windows.Input;

namespace Bugreporter.Client.Shared.Commands;

public abstract class AsyncCommandBase : ICommand
{
    private bool _isExecuting;
    private readonly Action<Exception> _onException;

    public bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            _isExecuting = value;
            // If this has no subscribers, it will not `Invoke`
            OnExecuteChanged();
        }
    }

    public AsyncCommandBase(Action<Exception> onException = null)
    {
        _onException = onException;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return !IsExecuting;
    }


    public async void Execute(object parameter)
    {
        IsExecuting = true;
        try
        {
            await ExecuteAsync(parameter);
        }
        catch (Exception e)
        {
            _onException?.Invoke(e);
        }
        IsExecuting = false;
    }

    // Abstract modifier to indicate this method is to be overridden in the
    // inheriting class.
    // In order to use, the class itself must be identified as abstract


    protected abstract Task ExecuteAsync(object parameter);

    protected void OnExecuteChanged()
    {
        // This refers to the instance of this class, and when inherited, the inheriting class instance
        // So the CanExecuteChanged subscribers know which command instance raised/triggered the event
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}