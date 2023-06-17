using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;
using MassTransit;

namespace Application.Features.Managers.Commands;
public record DissolveTeamCommand : IRequest
{
    public Guid ManagerId { get; init; }
}

public class DissolveTeamCommandHandler : IRequestHandler<DissolveTeamCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IBus _bus;

    public DissolveTeamCommandHandler(ILoggerManager logger, IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(DissolveTeamCommandHandler));
        _bus = bus;
    }

    public async Task Handle(DissolveTeamCommand request, CancellationToken cancellationToken)
    {
        var manager = await _unitOfWork.Managers.GetByIdAsync(request.ManagerId, cancellationToken);
        if (manager is null)
        {
            _logger.LogWarn($"Handler::{nameof(DissolveTeamCommandHandler)} - Manager with id::{request.ManagerId} was not found.");
            throw new ManagerNotFoundException(request.ManagerId);
        }
        manager.DissolveTheTeam();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new TeamDissolvedEvent(request.ManagerId), cancellationToken);
    }
}

public record TeamDissolvedEvent(Guid ManagerId);

public class DissolveTeamCommandValidator : AbstractValidator<DissolveTeamCommand>
{
    public DissolveTeamCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
    }
}