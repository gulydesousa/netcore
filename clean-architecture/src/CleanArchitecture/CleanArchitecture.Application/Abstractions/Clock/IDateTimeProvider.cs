
namespace CleanArchitecture.Application.Abstractions.Clock;
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current date and time.
    /// </summary>
    DateTime CurrentTime { get; }
}