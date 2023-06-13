using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;

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

    public ReleasePlayerCommandHandler(ILoggerManager logger, IUnitOfWorkFactory unitOfWorkFactory)
    {
        _logger = logger;
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(ReleasePlayerCommandHandler));
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
    }
}

public class ReleasePlayerCommandValidator : AbstractValidator<ReleasePlayerCommand>
{
    public ReleasePlayerCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
        RuleFor(c => c.PlayerId).Empty().WithMessage("PlayerId is required.");
    }
}