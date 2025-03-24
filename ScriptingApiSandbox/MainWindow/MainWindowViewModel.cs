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
    public partial string Script { get; set; } = """
                                                 def show_modal():
                                                     d = Dialog("Dialog Test")
                                                     
                                                     name = d.Text("Enter your name", "no-name")
                                                     d.Button("ABC").Clicked += lambda s, e: print("ABC clicked")
                                                     
                                                     result = d.ShowModal()
                                                     
                                                     print(f"result: {result}")
                                                     
                                                     if(result == DialogResult.Ok):
                                                        print(name.Text)
                                                 """;


//      public partial string Script { get; set; } = """
//                                                   print(sample.Name)
//                                                   sample.Name = 'IronPython'
//                                                   sample.SayHello()
//
//                                                   print(StaticMethod())
//                                                   print(StaticClass.StaticMethod())
//                                                   print(Color.Red)
//
//                                                   print(Math.Abs(-123))
//                                                   print(sample.Color == Color.Green)
//                                                   print(sample.Color)
//                                                   print(type(Color))
//
//
//
//
//
//                                                   notifier = Notifier()
//
//                                                   # イベントハンドラを定義して購読
//                                                   def on_notify(sender, args):
//                                                       print("Event received!")
//
//                                                   notifier.OnNotify += on_notify
//
//                                                   # イベントを発火
//                                                   notifier.Notify()  # "Notify called!" と "Event received!" が表示される
//
//                                                   def add(a, b):
//                                                       return a + b
//
//                                                   def greet(name):
//                                                       return f'Hello, {name}!'
//                                                       
//                                                   def show_modal():
//                                                       d = Dialog("Dialog Test")
//                                                       
//                                                       name = d.Text("Enter your name", "no-name")
//                                                       d.Button("ABC").Clicked += lambda s, e: print("ABC clicked")
//                                                       
//                                                       result = d.ShowModal()
//                                                       
//                                                       print(result)
//                                                       print(name.Text)
//                                                   """;


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

    [RelayCommand]
    private void ExecuteScript()
    {
        _stdout.Clear();
        _scriptContext.Execute(Script);

        AllFunctions = string.Join("\n", _scriptContext.AllFunctionNames);
    }

    [RelayCommand]
    private void Add()
    {
        ExecuteScript();
        var result = _scriptContext.CallFunction("add", 10, 20);

        Console.WriteLine(result);
    }

    [RelayCommand]
    private void Greet()
    {
        ExecuteScript();
        var result = _scriptContext.CallFunction("greet", "Yoshihiro");

        Console.WriteLine(result);
    }

    [RelayCommand]
    private void ShowModal()
    {
        ExecuteScript();
        _scriptContext.CallFunction("show_modal");
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