using FluentValidation;
using System.Globalization;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

public class ReservarAlquilerCommandValidator : AbstractValidator<ReservarAlquilerCommand>
{
    public ReservarAlquilerCommandValidator()
    {
        RuleFor(x => x.userId).NotEmpty();
        RuleFor(x => x.vehiculoId).NotEmpty();
        RuleFor(x => x.fechaInicio).NotEmpty();
        RuleFor(x => x.fechaFin).NotEmpty();
        //Que inicio y fin sean coherente
        RuleFor(x => x).Custom((command, context) =>
        {
            if (command.fechaInicio >= command.fechaFin)
            {
                context.AddFailure("La fecha de inicio debe ser anterior a la fecha de fin");
            }
        });
    }

}

