using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace ScriptingApiSandbox;

internal static class GuiHelper
{
    public static Window ActiveWindow
    {
        get
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                throw new InvalidOperationException();

            var activeWindow = desktop.Windows.FirstOrDefault(x => x.IsActive);

            return activeWindow ?? desktop.Windows[^1];
        }
    }

    // https://github.com/AvaloniaUI/Avalonia/issues/4810 

    public static void ShowDialogSync(this Window window, Window parent)
    {
        using var source = new CancellationTokenSource();
        
        var task = window.ShowDialog(parent);

        // ReSharper disable once AccessToDisposedClosure
        task.ContinueWith(_ => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
    
        Dispatcher.UIThread.MainLoop(source.Token);
    }

    public static T ShowDialogSync<T>(this Window window, Window parent)
    {
        using var source = new CancellationTokenSource();

        var task = window.ShowDialog<T>(parent);

        // ReSharper disable once AccessToDisposedClosure
        task.ContinueWith(_ => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());

        Dispatcher.UIThread.MainLoop(source.Token);
        return task.Result;
    }
}