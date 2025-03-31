using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScriptingApi;
using ScriptingApiSandbox.Dialog;

namespace ScriptingApiSandbox.MainWindow;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Script { get; set; } =
        """
        def show_modal():
            dlg = Dialog("ModalDialog Sample")
            
            name = dlg.Text("Enter your name", "no-name")
            dlg.Button("ABC").Clicked += lambda s, e: print("ABC clicked")
            dlg.Button("DEF").Clicked += lambda s, e: print("DEF clicked")
            
            close = dlg.Group(Horizontal)
            close.Button("Close").Clicked += lambda s, e: dlg.Close(DialogResult.Ok)
            close.Button("Close:Ok").Clicked += lambda s, e: dlg.Close(DialogResult.Ok)
            close.Button("Close:Cancel").Clicked += lambda s, e: dlg.Close(DialogResult.Cancel)
            
            result = dlg.ShowModal()
            print(f"result: {result}")
            
            if(result == DialogResult.Ok):
               print(name.Text)
               
        def show_modeless():
            dlg = Dialog("ModelessDialog Sample", Horizontal)
            
            left = dlg.Group(Vertical);
            dlg.Separator();
            right = dlg.Group(Vertical);
            
            name = left.Text("Enter your name", "no-name")
            intValue = left.Int("Int", 10, -100, 100);
            floatValue = left.Float("Float", 20, -100, 100);
            boolValue = left.Bool("Bool", True);
            
            name.TextChanged += lambda s, e: print(f"Name changed: {name.Text}")
            intValue.ValueChanged += lambda s, e: print(f"Int changed: {intValue.Value}")
            floatValue.ValueChanged += lambda s, e: print(f"Float changed: {floatValue.Value}")
            boolValue.ValueChanged += lambda s, e: print(f"Bool changed: {boolValue.Value}")
            
            left.Separator();
            left.Label("ラベル");
            
            items = ["Apple", "Banana", "Orange", "Peach", "Melon", "Grape", "Strawberry"]
            left.Choice(*items).SelectedChanged += lambda s, e: print(f"Choice changed: {s.SelectedIndex}, {s.SelectedItem}")
            
            left.Button("ABC").Clicked += lambda s, e: print(f"ABC clicked: {name.Text}")
            
            xyz = left.Group(Horizontal)
            xyz.Bool("X", False)
            xyz.Bool("Y", False)
            xyz.Bool("Z", False)
            
            pos = right.GroupBox("Rotate", Vertical)
            pos.Float("X", 0, -360, 360);
            pos.Float("Y", 0, -360, 360);
            pos.Float("Z", 0, -360, 360);
            
            tab = right.Tab()
            p0 = tab.Page("Page0")
            p1 = tab.Page("Page1")
            p2 = tab.Page("Page2")
            
            p0.Text("Text", "text")
            p0.Bool("Width", True)
            p0.Bool("Height", False)
            p1.Text("Data", "000")
            p2.Float("Float", 20, -100, 100);
            
            dlg.Closed += lambda s, e: print(f"Dialog closed: {dlg.DialogResult}, {name.Text}, {intValue.Value}, {floatValue.Value}, {boolValue.Value}")
            dlg.Show()
        """;

    [ObservableProperty]
    public partial string AllFunctions { get; set; } = "";

    public ReadOnlyObservableCollection<string> Stdout { get; }

    private readonly ScriptContext _scriptContext = new();
    private readonly ObservableCollection<string> _stdout = new();
    private readonly IDisposable _standardOutputListener;

    public MainWindowViewModel()
    {
        Stdout = new ReadOnlyObservableCollection<string>(_stdout);

        _standardOutputListener = Observable.FromEventPattern<EventHandler<StandardOutputEventArgs>, StandardOutputEventArgs>(
                h => h,
                h => _scriptContext.StandardOutput += h,
                h => _scriptContext.StandardOutput -= h)
            .Select(e => e.EventArgs)
            .Subscribe(e => _stdout.Add(e.Text));

        _scriptContext.SetType<DialogImpl>("Dialog");
        _scriptContext.SetType<DialogResult>(nameof(DialogResult));
        _scriptContext.SetType<Orientation>(nameof(Orientation));

        foreach (var e in Enum.GetValues<Orientation>())
            _scriptContext.SetVariable(e.ToString(), e);

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
    private void ShowModeless()
    {
        ExecuteScript();
        _scriptContext.CallFunction("show_modeless");
    }
}