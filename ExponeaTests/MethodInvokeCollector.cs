using Exponea;

namespace ExponeaTests;

public class MethodInvokeCollector : IMethodChannelConsumerPlatformSpecific
{
    private struct MethodInvoke
    {
        public readonly string MethodName;
        public readonly string? InputData;

        public MethodInvoke(string methodName, string? inputData)
        {
            this.MethodName = methodName;
            this.InputData = inputData;
        }
    }
    
    /**
     * Simple wrapper for MethodMauiResult, used only for Testing purpose.
     * Main reason is to contain an exception for next `_methodResults` searching.
     */
    private class MethodMauiResultWithError : MethodMauiResult
    {
        public MethodMauiResultWithError(bool success, string? data, Exception error) : base(success, data, error.Message)
        {
            this.Exception = error;
        }

        public Exception Exception { get; set; }
    }
    
    /**
     * Simple wrapper for MethodMauiResultForView, used only for Testing purpose.
     * Main reason is to contain an exception for next `_uiMethodResults` searching.
     */
    private class MethodMauiResultForViewWithError : MethodMauiResultForView
    {
        public MethodMauiResultForViewWithError(bool success, IView? data, Exception error) : base(success, data, error.Message)
        {
            this.Exception = error;
        }

        public Exception Exception { get; set; }
    }

    private readonly List<MethodInvoke> _invokedMethods = new();
    private readonly Dictionary<string, MethodMauiResult?> _methodResults = new();
    private readonly Dictionary<string, MethodMauiResultForView?> _uiMethodResults = new();
    public MethodMauiResult? DefaultMethodResult { get; set; }
    public MethodMauiResultForView? DefaultMethodResultForView { get; set; }

    public MethodMauiResult InvokeMethod(string method, string? data)
    {
        _invokedMethods.Add(new MethodInvoke(method, data));
        return FindMethodResultForMethod(method);
    }
    
    public MethodMauiResultForView InvokeUiMethod(string method, string? data)
    {
        _invokedMethods.Add(new MethodInvoke(method, data));
        return FindMethodResultForUiMethod(method);
    }

    public void InvokeMethodAsync(string method, string? data, Action<MethodMauiResult, Exception?> action)
    {
        _invokedMethods.Add(new MethodInvoke(method, data));
        Task.Run(() =>
        {
            MethodMauiResult result;
            Exception? exception = null;
            try
            {
                result = FindMethodResultForMethod(method);
            }
            catch (Exception e)
            {
                exception = e;
                result = new MethodMauiResult(false, "", $"Registered result for {method} failed");
            }
            action.Invoke(result, exception);
        });
    }

    private MethodMauiResultForView FindMethodResultForUiMethod(string method)
    {
        var result = _uiMethodResults.GetValueOrDefault(method, null)
                     ?? DefaultMethodResultForView
                     ?? new MethodMauiResultForView(false, null, $"Registered result for {method} not found");
        if (result is MethodMauiResultForViewWithError { Exception: not null } resultWithError )
        {
            throw resultWithError.Exception;
        }
        return result;
    }

    private MethodMauiResult FindMethodResultForMethod(string method)
    {
        var result = _methodResults.GetValueOrDefault(method, null)
                     ?? DefaultMethodResult
                     ?? new MethodMauiResult(false, "", $"Registered result for {method} not found");
        if (result is MethodMauiResultWithError { Exception: not null } resultWithError )
        {
            throw resultWithError.Exception;
        }
        return result;
    }

    public void Clear()
    {
        _invokedMethods.Clear();
        _methodResults.Clear();
        _uiMethodResults.Clear();
    }

    public void VerifyMethodCalled(
        string methodName,
        int calledExactly = -1,
        int calledAtLeast = 1,
        int calledAtMost = -1)
    {
        var calledTimes = _invokedMethods.FindAll(invoke => invoke.MethodName == methodName).Count;
        if (calledExactly >= 0)
        {
            Assert.That(
                calledExactly,
                Is.EqualTo(calledTimes),
                "Method " + methodName + " is called " + calledTimes + " times, but expected to be called " + calledExactly + " times"
            );
        }
        if (calledAtLeast >= 0)
        {
            Assert.That(
                calledAtLeast,
                Is.LessThanOrEqualTo(calledTimes),
                "Method " + methodName + " is called " + calledTimes + " times, but expected to be called more than " + calledAtLeast + " times"
            );
        }
        if (calledAtMost >= 0)
        {
            Assert.That(
                calledAtMost,
                Is.GreaterThanOrEqualTo(calledTimes),
                "Method " + methodName + " is called " + calledTimes + " times, but expected to be called less than " + calledAtMost + " times"
            );
        }
    }

    public void VerifyMethodInput(
        string methodName,
        string? equalsTo)
    {
        List<MethodInvoke> methodInvokes = _invokedMethods.FindAll(invoke => invoke.MethodName == methodName);
        Assert.That(
            methodInvokes.Count,
            Is.GreaterThan(0),
            "Method " + methodName + " has not been called at all"
        );
        var inputMatched = methodInvokes.Any(invoke => invoke.InputData == equalsTo);
        if (inputMatched)
        {
            // ok
            return;
        }
        // Failure, but for single method call we can write more details
        if (methodInvokes.Count == 1)
        {
            Assert.That(
                methodInvokes[0].InputData,
                Is.EqualTo(equalsTo),
                "Method " + methodName + " input mis-matched"
            );
            return;
        }
        Assert.That(
            methodInvokes.Any(invoke => invoke.InputData == equalsTo),
            "Method " + methodName + " was called " + methodInvokes.Count + " times but no input matched"
        );
    }

    public void RegisterSuccessMethodResult(string methodName, string? response)
    {
        _methodResults[methodName] = new MethodMauiResult(
            true, response, ""
        );
    }

    public void RegisterFailureMethodResult(string methodName, string error)
    {
        _methodResults[methodName] = new MethodMauiResult(
            false, null, error
        );
    }

    public void RegisterFailureMethodResult(string methodName, Exception error)
    {
        _methodResults[methodName] = new MethodMauiResultWithError(
            false, null, error
        );
    }

    public void RegisterDefaultMethodResult(MethodMauiResult defaultResult)
    {
        this.DefaultMethodResult = defaultResult;
    }
}