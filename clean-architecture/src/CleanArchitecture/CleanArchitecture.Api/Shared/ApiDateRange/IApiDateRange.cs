namespace CleanArchitecture.Api.Shared.ApiDateRange
{
    public interface IApiDateRange<T>
    {
        T FechaInicio { get; init; }
        T FechaFin { get; init; }
    }
}
