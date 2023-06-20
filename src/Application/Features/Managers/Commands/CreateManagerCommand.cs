using MediatR;
using Domain.Common;
using Domain.Managers;
using FluentValidation;
using MassTransit;

namespace Application.Features.Managers.Commands;
public record CreateManagerCommand : IRequest<Guid>
{
    public ManagerPersonalInfo ManagerPersonalInfo { get; init; } = null!;
}

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public CreateManagerCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(CreateManagerCommandHandler));
        _bus = bus;
    }

    public async Task<Guid> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = Manager.Create(request.ManagerPersonalInfo);
        await _unitOfWork.Managers.CreateAsync(manager, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new ManagerCreatedEvent(manager.Id), cancellationToken);
        return manager.Id;
    }
}

public record ManagerCreatedEvent(Guid ManagerId);

public class CreateManagerCommandValidator : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        RuleFor(c => c.ManagerPersonalInfo).SetValidator(new ManagerPersonalInfoValidator());
    }
}

public class ManagerPersonalInfoValidator : AbstractValidator<ManagerPersonalInfo>
{
    public ManagerPersonalInfoValidator()
    {
        RuleFor(c => c.City)
            .NotNull().WithMessage("City is required.");
        RuleFor(c => c.Country)
            .NotNull().WithMessage("Country is required.");
        RuleFor(c => c.Code)
            .NotNull().WithMessage("PostalCode is required.");
        RuleFor(c => c.HouseNumber)
            .NotNull().WithMessage("HouseNumber is required.");
        RuleFor(c => c.Street)
            .NotNull().WithMessage("Street is required.");
        RuleFor(c => c.DateOfBirth)
            .NotNull().WithMessage("DateOfBirth is required.");
        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Email should be a valid email address.");
        RuleFor(c => c.FirstName)
            .NotNull()
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(75).WithMessage("Max length for FirstName is 75 characters.")
            .MinimumLength(2).WithMessage("Min length for FirstName is 2 characters.");
        RuleFor(c => c.LastName)
            .NotNull()
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100).WithMessage("Max length for LastName is 100 characters.")
            .MinimumLength(2).WithMessage("Min length for LastName is 2 characters.");
    }
}