using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;
using MassTransit;

namespace Application.Features.Managers.Commands;
public record ReleasePlayerCommand : IRequest
{
    public Guid ManagerId { get; init; }
    public Guid PlayerId { get; init; }
}

public class ReleasePlayerCommandHandler : IRequestHandler<ReleasePlayerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IBus _bus;

    public ReleasePlayerCommandHandler(ILoggerManager logger, IUnitOfWorkFactory unitOfWorkFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(ReleasePlayerCommandHandler));
        _bus = bus;
    }

    public async Task Handle(ReleasePlayerCommand request, CancellationToken cancellationToken)
    {
        var manager = await _unitOfWork.Managers.GetByIdAsync(request.ManagerId, cancellationToken);
        if (manager is null)
        {
            _logger.LogWarn($"Handler::{nameof(ReleasePlayerCommandHandler)} - Manager with id::{request.ManagerId} was not found.");
            throw new ManagerNotFoundException(request.ManagerId);
        }
        manager.ReleasePlayer(request.PlayerId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new PlayerReleasedEvent(request.ManagerId, request.PlayerId), cancellationToken);
    }
}

public record PlayerReleasedEvent(Guid ManagerId, Guid PlayerId);

public class ReleasePlayerCommandValidator : AbstractValidator<ReleasePlayerCommand>
{
    public ReleasePlayerCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
        RuleFor(c => c.PlayerId).Empty().WithMessage("PlayerId is required.");
    }
}