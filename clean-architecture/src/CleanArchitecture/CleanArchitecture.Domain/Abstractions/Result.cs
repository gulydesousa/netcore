using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Domain.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess; 
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    //Se crea un constructor protegido para que solo las clases que 
    //hereden de Result puedan crear instancias
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
        IsSuccess = isSuccess;
        Error = error;
    }

   
    public static Result<TValue> Success<TValue>(TValue value)
                                => new (true, Error.None, value);


    public static Result<TValue> Failure<TValue>(Error error)
                                => new (false, error, default!);
                               
    public static Result<TValue> Create<TValue>(TValue value)
                                => (Result<TValue>)(value is not null
                                ? Success(value)
                                :Failure(Error.NullValue));
}

public class Result<TValue> : Result
{
    private readonly TValue _value;

    protected internal Result(bool isSuccess, Error error, TValue value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    //Value se invoca así: result.Value
    [NotNull] //El valor no puede ser nulo
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("El resultado del valor de error no es admisible.");

    //Se crea un constructor protegido para que solo las 
    //clases que hereden de Result puedan crear instancias
    public static implicit operator Result<TValue>(TValue value) => Create(value);

    
}

