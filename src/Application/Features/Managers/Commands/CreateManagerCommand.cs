using MediatR;
using Domain.Common;
using Domain.Managers;
using FluentValidation;

namespace Application.Features.Managers.Commands;
public record CreateManagerCommand : IRequest<Guid>
{
    public ManagerPersonalInfo ManagerPersonalInfo { get; init; } = null!;
}

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateManagerCommandHandler(IUnitOfWorkFactory unitOfWorkFactory) => _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(CreateManagerCommandHandler));

    public async Task<Guid> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = Manager.Create(request.ManagerPersonalInfo);
        await _unitOfWork.Managers.CreateAsync(manager, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return manager.Id;
    }
}

public class CreateManagerCommandValidator : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        RuleFor(c => c.ManagerPersonalInfo).NotNull().WithMessage("Invalid ManagerPersonalInfo.");
        RuleFor(c => c.ManagerPersonalInfo.City).NotNull().WithMessage("City is required.");
        RuleFor(c => c.ManagerPersonalInfo.Country).NotNull().WithMessage("Country is required.");
        RuleFor(c => c.ManagerPersonalInfo.Code).NotNull().WithMessage("PostalCode is required.");
        RuleFor(c => c.ManagerPersonalInfo.HouseNumber).NotNull().WithMessage("HouseNumber is required.");
        RuleFor(c => c.ManagerPersonalInfo.Street).NotNull().WithMessage("Street is required.");
        RuleFor(c => c.ManagerPersonalInfo.DateOfBirth).NotNull().WithMessage("DateOfBirth is required.");
        RuleFor(c => c.ManagerPersonalInfo.Email).EmailAddress().WithMessage("Email should be a valid email address.");
        RuleFor(c => c.ManagerPersonalInfo.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required.")
                                                    .MaximumLength(75).WithMessage("Max length for FirstName is 75 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for FirstName is 2 characters.");
        RuleFor(c => c.ManagerPersonalInfo.LastName).NotNull().NotEmpty().WithMessage("LastName is required.")
                                                    .MaximumLength(100).WithMessage("Max length for LastName is 100 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for LastName is 2 characters.");
    }
}
