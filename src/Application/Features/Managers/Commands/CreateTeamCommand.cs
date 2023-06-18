using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;
using MassTransit;

namespace Application.Features.Managers.Commands;
public record CreateTeamCommand : IRequest<Guid?>
{
    public Guid ManagerId { get; init; }
    public string? TeamName { get; init; }
}

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IBus _bus;

    public CreateTeamCommandHandler(ILoggerManager logger, IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(CreateTeamCommandHandler));
        _bus = bus;
    }

    public async Task<Guid?> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var manager = await _unitOfWork.Managers.GetByIdAsync(request.ManagerId, cancellationToken);
        if (manager is null)
        {
            _logger.LogWarn($"Handler::{nameof(CreateTeamCommandHandler)} - Manager with id::{request.ManagerId} was not found.");
            throw new ManagerNotFoundException(request.ManagerId);
        }
        manager.CreateTeam(request.TeamName!);
        await _unitOfWork.Teams.CreateAsync(manager.Team!, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new TeamCreatedEvent(request.ManagerId, manager.TeamId), cancellationToken);
        return manager.TeamId;
    }
}

public record TeamCreatedEvent(Guid ManagerId, Guid? TeamId);

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
        RuleFor(c => c.TeamName).NotNull().NotEmpty().WithMessage("TeamName is required.")
                                .MaximumLength(75).WithMessage("Max length for TeamName is 75 characters.")
                                .MinimumLength(4).WithErrorCode("Min length for TeamName is 4 characters.");
    }
}