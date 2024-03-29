using FluentValidation;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

public class ReservarAlquilerCommandValidator : AbstractValidator<ReservarAlquilerCommand>
{
    public ReservarAlquilerCommandValidator()
    {
        RuleFor(x => x.userId).NotEmpty();
        RuleFor(x => x.vehiculoId).NotEmpty();
        RuleFor(x => x.fechaInicio).NotEmpty().GreaterThanOrEqualTo(x => x.fechaFin);
    }
}

