using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotElegible = 
    new Error("Review.NotElegible"
            , "Review no elegible porque el alquiler no ha sido completado.");
}