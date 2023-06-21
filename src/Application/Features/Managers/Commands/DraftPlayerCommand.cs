using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;
using MassTransit;

namespace Application.Features.Managers.Commands;
public record DraftPlayerCommand : IRequest
{
    public Guid ManagerId { get; init; }
    public Guid PlayerId { get; init; }
}

public class DraftPlayerCommandHandler : IRequestHandler<DraftPlayerCommand>
{
    private readonly ILoggerManager _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public DraftPlayerCommandHandler(ILoggerManager logger, IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(DraftPlayerCommandHandler));
        _bus = bus;
    }

    public async Task Handle(DraftPlayerCommand request, CancellationToken cancellationToken)
    {
        var manager = await _unitOfWork.Managers.GetByIdAsync(request.ManagerId, cancellationToken);
        if (manager is null)
        {
            _logger.LogWarn($"Handler::{nameof(DraftPlayerCommandHandler)} - Manager with id::{request.ManagerId} was not found.");
            throw new ManagerNotFoundException(request.ManagerId);
        }
        var player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId, cancellationToken);
        if (player is null)
        {
            _logger.LogWarn($"Handler::{nameof(DraftPlayerCommandHandler)} - Player with id::{request.PlayerId} was not found.");
            throw new PlayerNotFoundException(request.PlayerId);
        }
        manager.DraftPlayer(player);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new PlayerDraftedEvent(request.ManagerId, request.PlayerId), cancellationToken);
    }
}

public record PlayerDraftedEvent(Guid ManagerId, Guid PlayerId);

public class DraftPlayerCommandValidator : AbstractValidator<DraftPlayerCommand>
{
    public DraftPlayerCommandValidator()
    {
        RuleFor(c => c.ManagerId).NotNull().NotEmpty().WithMessage("ManagerId is required.");
        RuleFor(c => c.PlayerId).NotNull().NotEmpty().WithMessage("PlayerId is required.");
    }
}