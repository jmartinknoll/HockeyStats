public class Result
{
    private List<string> _errors = new();
    private List<string> _warnings = new();

    public bool Success { get; set; }
    public int Code { get; init; } = 0;

    public string ErrorMessage
    {
        get => _errors.Count == 0 ? "" : _errors.First();
        init
        {
            if (!string.IsNullOrEmpty(value))
                _errors.Add(value);
        }
    }

    public List<string> Warnings
    {
        get => _warnings;
        set => _warnings = value;
    }

    public List<string> Errors
    {
        get => _errors;
        set => _errors = value;
    }

    public Result(bool success, int code = 0, string errorMessage = "")
    {
        Success = success;
        Code = code;
        ErrorMessage = errorMessage;
    }

    public static Result Succeeded()
    {
        return new Result(true);
    }

    public static Result Failed(string error)
    {
        return new Result(false) { ErrorMessage = error };
    }

    public static Result Failed(List<string> errors)
    {
        return new Result(false) { Errors = errors };
    }
}

public class Result<T> : Result
{
    public T? Value { get; set; }

    public Result(bool success, T value, int code = 0, string errorMessage = "")
        : base(success, code, errorMessage)
    {
        Value = value;
    }

    public Result(bool success, T value)
        : base(success)
    {
        Value = value;
    }

    public static Result<T> Succeeded(T value)
    {
        return new Result<T>(true, value);
    }

    public static new Result<T> Failed(string error)
    {
        return new Result<T>(false, default!) { ErrorMessage = error };
    }

    public static new Result<T> Failed(List<string> errors)
    {
        return new Result<T>(false, default!) { Errors = errors };
    }
}
