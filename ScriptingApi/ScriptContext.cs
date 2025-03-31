using IronPython.Hosting;
using IronPython.Runtime.Types;
using Microsoft.Scripting.Hosting;

namespace ScriptingApi;

public sealed class ScriptContext
{
    public IEnumerable<string> AllFunctionNames
    {
        get
        {
            foreach (var item in _scope.GetItems())
                if (item.Value is IronPython.Runtime.PythonFunction)
                    yield return item.Key;
        }
    }

    public event EventHandler<StandardOutputEventArgs>? StandardOutput;

    private readonly ScriptEngine _pythonEngine = Python.CreateEngine();
    private readonly ScriptScope _scope;

    public ScriptContext()
    {
        _scope = CreateScope();
    }

    public void SetVariable(string name, object? obj)
    {
        _scope.SetVariable(name, obj);
    }

    public void SetType<T>(string name)
    {
        SetType(name, typeof(T));
    }

    public void SetType(string name, Type type)
    {
        _scope.SetVariable(name, DynamicHelpers.GetPythonTypeFromType(type));
    }
    
    public void SetEnum<T>() where T : struct, Enum 
    {
        SetType<T>(typeof(T).Name);
        
        foreach (var e in Enum.GetValues<T>())
            SetVariable(e.ToString(), e);
    }
    
    public void RemoveVariable(string name)
    {
        _scope.RemoveVariable(name);
    }

    public dynamic GetVariable(string name)
    {
        return _scope.GetVariable(name);
    }

    public void Execute(string code)
    {
        ClearAllFunctions();
        
        try
        {
#if false
            var source = _pythonEngine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.Statements);
            var compiled = source.Compile();
            compiled.Execute(_scope);
#else
            _pythonEngine.Execute(code, _scope);
#endif

        }
        catch (Exception ex)
        {
            InvokeStandardOutput($"エラー: {ex.Message}");
            InvokeStandardOutput($"例外の種類: {ex.GetType().FullName}");
            InvokeStandardOutput($"スタックトレース: {ex.StackTrace}");
        }
    }
    
    public void ClearAllFunctions()
    {
        foreach (var function in AllFunctionNames.ToArray())
            RemoveVariable(function);
    }

    public object? CallFunction(string functionName)
    {
        return CallFunctionInternal(() => _scope.GetVariable(functionName)());
    }

    public object? CallFunction<T0>(string functionName, T0 arg0)
    {
        return CallFunctionInternal(() => _scope.GetVariable(functionName)(arg0));
    }

    public object? CallFunction<T0, T1>(string functionName, T0 arg0, T1 arg1)
    {
        return CallFunctionInternal(() => _scope.GetVariable(functionName)(arg0, arg1));
    }

    public void InvokeStandardOutput(string output)
    {
        StandardOutput?.Invoke(this, new StandardOutputEventArgs(output));
    }

    private ScriptScope CreateScope()
    {
        var scope = _pythonEngine.CreateScope();
        scope.SetVariable("__custom_print__", new Action<string>(InvokeStandardOutput));

        try
        {
            _pythonEngine.Execute("""
                                  import sys

                                  class CustomOutput:
                                      def write(self, text):
                                          if text and text.strip():
                                              __custom_print__(text)
                                      def flush(self):
                                          pass

                                  sys.stdout = CustomOutput()
                                  sys.stderr = CustomOutput()
                                  """,
                scope);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"初期化エラー: {ex.Message}");
        }

        return scope;
    }

    private static object? CallFunctionInternal(Func<object?> func)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling function: {ex.Message}");
            return null;
        }
    }
}

public class StandardOutputEventArgs(string text) : EventArgs
{
    public readonly string Text = text;
}