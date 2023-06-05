using MediatR;
using Domain.Common;
using Domain.Players;
using FluentValidation;

namespace Application.Features.Players.Commands;
public record CreatePlayerCommand : IRequest<Guid>
{
    public PlayerPersonalInfo PlayerPersonalInfo { get; init; } = null!;
    public Position Position { get; init; }
}

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = Player.Create(request.PlayerPersonalInfo, request.Position);
        await _unitOfWork.Players.CreateAsync(player, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return player.Id;
    }
}

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(c => c.PlayerPersonalInfo).NotNull().WithMessage("Invalid PlayerPersonalInfo.");
        RuleFor(c => c.PlayerPersonalInfo.Address).NotNull().WithMessage("Address is required.");
        RuleFor(c => c.PlayerPersonalInfo.DateOfBirht).NotNull().WithMessage("DateOfBirth is required.");
        RuleFor(c => c.PlayerPersonalInfo.Email).EmailAddress().WithMessage("Email should be a valid email address.");
        RuleFor(c => c.PlayerPersonalInfo.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required.")
                                                    .MaximumLength(75).WithMessage("Max length for FirstName is 75 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for FirstName is 2 characters.");
        RuleFor(c => c.PlayerPersonalInfo.LastName).NotNull().NotEmpty().WithMessage("LastName is required.")
                                                    .MaximumLength(100).WithMessage("Max length for LastName is 100 characters.")
                                                    .MinimumLength(2).WithMessage("Min length for LastName is 2 characters.");
        RuleFor(c => c.PlayerPersonalInfo.Weight).GreaterThan(30f).WithMessage("The weight could not be less than 30 kg.")
                                                .LessThan(200f).WithMessage("The weight could not be greater than 200kg.");
        RuleFor(c => c.PlayerPersonalInfo.Height).GreaterThan(1f).WithMessage("The height could not be less than 1m.")
                                                .LessThan(2.5f).WithMessage("The height could not be greater than 2.5m.");
        RuleFor(c => c.Position).IsInEnum();
    }
}