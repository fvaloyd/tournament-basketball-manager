using MediatR;
using Domain.Common;
using Domain.Players;
using FluentValidation;
using MassTransit;

namespace Application.Features.Players.Commands;
public record CreatePlayerCommand : IRequest<Guid>
{
    public PlayerPersonalInfo PlayerPersonalInfo { get; init; } = null!;
    public Position Position { get; init; }
}

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public CreatePlayerCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(CreatePlayerCommandHandler));
        _bus = bus;
    }

    public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = Player.Create(request.PlayerPersonalInfo, request.Position);
        await _unitOfWork.Players.CreateAsync(player, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new PlayerCreatedEvent(player.Id), cancellationToken);
        return player.Id;
    }
}

public record PlayerCreatedEvent(Guid PlayerId);

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(c => c.PlayerPersonalInfo).NotNull().WithMessage("Invalid PlayerPersonalInfo.");
        RuleFor(c => c.PlayerPersonalInfo.City).NotNull().WithMessage("City is required.");
        RuleFor(c => c.PlayerPersonalInfo.Country).NotNull().WithMessage("Country is required.");
        RuleFor(c => c.PlayerPersonalInfo.Code).NotNull().WithMessage("PostalCode is required.");
        RuleFor(c => c.PlayerPersonalInfo.HouseNumber).NotNull().WithMessage("HouseNumber is required.");
        RuleFor(c => c.PlayerPersonalInfo.Street).NotNull().WithMessage("Street is required.");
        RuleFor(c => c.PlayerPersonalInfo.DateOfBirth).NotNull().WithMessage("DateOfBirth is required.");
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