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

    public CreateManagerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = Manager.Create(request.ManagerPersonalInfo);
        await _unitOfWork.Managers.CreateManagerAsync(manager, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return manager.Id;
    }
}

public class CreateManagerCommandValidator : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        RuleFor(c => c.ManagerPersonalInfo).NotNull().WithMessage("Invalid ManagerPersonalInfo.");
        RuleFor(c => c.ManagerPersonalInfo.Address).NotNull().WithMessage("Address is required.");
        RuleFor(c => c.ManagerPersonalInfo.DateOfBirht).NotNull().WithMessage("DateOfBirth is required.");
        RuleFor(c => c.ManagerPersonalInfo.Email).EmailAddress().WithMessage("Email should be a valid email address.");
        RuleFor(c => c.ManagerPersonalInfo.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required.")
                                                    .MaximumLength(75).WithMessage("Max length for FirstName is 75 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for FirstName is 2 characters.");
        RuleFor(c => c.ManagerPersonalInfo.LastName).NotNull().NotEmpty().WithMessage("LastName is required.")
                                                    .MaximumLength(100).WithMessage("Max length for LastName is 100 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for LastName is 2 characters.");
    }
}
