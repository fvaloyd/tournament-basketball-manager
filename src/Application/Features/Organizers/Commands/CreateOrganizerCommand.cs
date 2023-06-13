using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers;

namespace Application.Features.Organizers.Commands;
public record CreateOrganizerCommand : IRequest<Guid>
{
    public OrganizerPersonalInfo OrganizerPersonalInfo { get; init; } = null!;
}

public class CreateOrganizerCommandHandler : IRequestHandler<CreateOrganizerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrganizerCommandHandler(IUnitOfWorkFactory unitOfWorkFactory) => _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(CreateOrganizerCommandHandler));

    public async Task<Guid> Handle(CreateOrganizerCommand request, CancellationToken cancellationToken)
    {
        var organizer = Organizer.Create(request.OrganizerPersonalInfo);
        await _unitOfWork.Organizers.CreateAsync(organizer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return organizer.Id;
    }
}

public class CreateOrganizerCommandValidator : AbstractValidator<CreateOrganizerCommand>
{
    public CreateOrganizerCommandValidator()
    {
        RuleFor(c => c.OrganizerPersonalInfo).NotNull().WithMessage("Invalid OrganizerPersonalInfo.");
        RuleFor(c => c.OrganizerPersonalInfo.City).NotNull().WithMessage("City is required.");
        RuleFor(c => c.OrganizerPersonalInfo.Country).NotNull().WithMessage("Country is required.");
        RuleFor(c => c.OrganizerPersonalInfo.Code).NotNull().WithMessage("PostalCode is required.");
        RuleFor(c => c.OrganizerPersonalInfo.HouseNumber).NotNull().WithMessage("HouseNumber is required.");
        RuleFor(c => c.OrganizerPersonalInfo.Street).NotNull().WithMessage("Street is required.");
        RuleFor(c => c.OrganizerPersonalInfo.DateOfBirth).NotNull().WithMessage("DateOfBirth is required.");
        RuleFor(c => c.OrganizerPersonalInfo.Email).EmailAddress().WithMessage("Email should be a valid email address.");
        RuleFor(c => c.OrganizerPersonalInfo.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required.")
                                                    .MaximumLength(75).WithMessage("Max length for FirstName is 75 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for FirstName is 2 characters.");
        RuleFor(c => c.OrganizerPersonalInfo.LastName).NotNull().NotEmpty().WithMessage("LastName is required.")
                                                    .MaximumLength(100).WithMessage("Max length for LastName is 100 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for LastName is 2 characters.");
    }
}