using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reactive.Bindings;
using ScriptingApi;
using ScriptingApiSandbox.Dialog;

namespace ScriptingApiSandbox.MainWindow;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Script { get; set; } =
        """
        def show_modal():
            d = Dialog("ModalDialog Test")
            
            name = d.Text("Enter your name", "no-name")
            d.Button("ABC").Clicked += lambda s, e: print("ABC clicked")
            d.Button("DEF").Clicked += lambda s, e: print("DEF clicked")
            
            result = d.ShowModal()
            print(f"result: {result}")
            
            if(result == DialogResult.Ok):
               print(name.Text)
               
        def show():
            d = Dialog("Dialog Test")
            
            name = d.Text("Enter your name", "no-name")
            intValue = d.Int("Int", 10, -100, 100);
            floatValue = d.Float("Float", 20, -100, 100);
            boolValue = d.Bool("Bool", True);
            
            
            
            name.TextChanged += lambda s, e: print(f"Name changed: {name.Text}")
            intValue.ValueChanged += lambda s, e: print(f"Int changed: {intValue.Value}")
            floatValue.ValueChanged += lambda s, e: print(f"Float changed: {floatValue.Value}")
            boolValue.ValueChanged += lambda s, e: print(f"Bool changed: {boolValue.Value}")
            
            d.Button("ABC").Clicked += lambda s, e: print(f"ABC clicked: {name.Text}")
            d.Closed += lambda s, e: print(f"Dialog closed: {d.DialogResult}, {name.Text}, {intValue.Value}, {floatValue.Value}, {boolValue.Value}")
            
            d.Show()
        """;


    [ObservableProperty]
    public partial string AllFunctions { get; set; } = "";


    public ReadOnlyReactiveCollection<string> Stdout { get; }

    private readonly ScriptContext _scriptContext = new();
    private readonly ReactiveCollection<string> _stdout;
    private readonly CompositeDisposable _trash = new();

    public MainWindowViewModel()
    {
        _stdout = new ReactiveCollection<string>().AddTo(_trash);
        Stdout = _stdout.ToReadOnlyReactiveCollection().AddTo(_trash);

        Observable.FromEventPattern<EventHandler<StandardOutputEventArgs>, StandardOutputEventArgs>(
                h => h,
                h => _scriptContext.StandardOutput += h,
                h => _scriptContext.StandardOutput -= h)
            .Select(e => e.EventArgs)
            .Subscribe(e =>
            {
                _stdout.Add(e.Text);
            })
            .AddTo(_trash);


        var sample = new SampleObject(_scriptContext);
        _scriptContext.SetVariable("sample", sample);
        _scriptContext.SetVariable("StaticMethod", new Func<string>(StaticClass.StaticMethod));
        _scriptContext.SetType(nameof(StaticClass), typeof(StaticClass));
        _scriptContext.SetType<Color>(nameof(Color));
        _scriptContext.SetType(nameof(Math), typeof(Math));
        _scriptContext.SetType<Notifier>(nameof(Notifier));

        _scriptContext.SetType<DialogImpl>("Dialog");
        _scriptContext.SetType<DialogResult>(nameof(DialogResult));

        ExecuteScript();
    }

    private void ExecuteScript()
    {
        _stdout.Clear();
        _scriptContext.Execute(Script);

        AllFunctions = string.Join("\n", _scriptContext.AllFunctionNames);
    }

    [RelayCommand]
    private void ShowModal()
    {
        ExecuteScript();
        _scriptContext.CallFunction("show_modal");
    }

    [RelayCommand]
    private void Show()
    {
        ExecuteScript();
        _scriptContext.CallFunction("show");
    }
}

public enum Color
{
    Red,
    Green,
    Blue
}

public static class StaticClass
{
    public static string StaticMethod()
    {
        return "This is a static method.";
    }
}

public class SampleObject(ScriptContext scriptContext)
{
    public string Name { get; set; } = "DefaultName";

    public Color Color { get; set; } = Color.Green;

    public void SayHello()
    {
        scriptContext.InvokeStandardOutput($"Hello from {Name}!");
    }
}

public class Notifier
{
    public event EventHandler? OnNotify;

    public void Notify()
    {
        Console.WriteLine("Notify called!");
        OnNotify?.Invoke(this, EventArgs.Empty);
    }
}