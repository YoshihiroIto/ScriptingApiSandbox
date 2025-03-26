﻿using System;
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
            dlg = Dialog("ModalDialog Test")
            
            name = dlg.Text("Enter your name", "no-name")
            dlg.Button("ABC").Clicked += lambda s, e: print("ABC clicked")
            dlg.Button("DEF").Clicked += lambda s, e: print("DEF clicked")
            
            result = dlg.ShowModal()
            print(f"result: {result}")
            
            if(result == DialogResult.Ok):
               print(name.Text)
               
        def show():
            dlg = Dialog("Dialog Test")
            
            name = dlg.Text("Enter your name", "no-name")
            intValue = dlg.Int("Int", 10, -100, 100);
            floatValue = dlg.Float("Float", 20, -100, 100);
            boolValue = dlg.Bool("Bool", True);
            
            name.TextChanged += lambda s, e: print(f"Name changed: {name.Text}")
            intValue.ValueChanged += lambda s, e: print(f"Int changed: {intValue.Value}")
            floatValue.ValueChanged += lambda s, e: print(f"Float changed: {floatValue.Value}")
            boolValue.ValueChanged += lambda s, e: print(f"Bool changed: {boolValue.Value}")
            
            dlg.Label("ラベル");
            
            dlg.Button("ABC").Clicked += lambda s, e: print(f"ABC clicked: {name.Text}")
            
            g = dlg.Group(Orientation.Horizontal)
            g.Bool("X", False)
            g.Bool("Y", False)
            g.Bool("Z", False)
            
            tab = dlg.Tab()
            
            p0 = tab.Page("Page0")
            p1 = tab.Page("Page1")
            p2 = tab.Page("Page2")
            
            p0.Text("Text", "text")
            p0.Bool("X", False)
            p0.Bool("Y", False)
            p1.Text("T0", "000")
            p2.Float("Float", 20, -100, 100);
            
            dlg.Closed += lambda s, e: print(f"Dialog closed: {dlg.DialogResult}, {name.Text}, {intValue.Value}, {floatValue.Value}, {boolValue.Value}")
            
            dlg.Show()
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

        _scriptContext.SetType<DialogImpl>("Dialog");
        _scriptContext.SetType<DialogResult>(nameof(DialogResult));
        _scriptContext.SetType<Orientation>(nameof(Orientation));

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
