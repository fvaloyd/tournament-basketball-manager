using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;

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

    public DraftPlayerCommandHandler(ILoggerManager logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
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
    }
}

public class DraftPlayerCommandValidator : AbstractValidator<DraftPlayerCommand>
{
    public DraftPlayerCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
        RuleFor(c => c.PlayerId).Empty().WithMessage("PlayerId is required.");
    }
}