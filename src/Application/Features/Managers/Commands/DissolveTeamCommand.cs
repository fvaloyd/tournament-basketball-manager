using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;

namespace Application.Features.Managers.Commands;
public record DissolveTeamCommand : IRequest
{
    public Guid ManagerId { get; init; }
}

public class DissolveTeamCommandHandler : IRequestHandler<DissolveTeamCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public DissolveTeamCommandHandler(ILoggerManager logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
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
    }
}

public class DissolveTeamCommandValidator : AbstractValidator<DissolveTeamCommand>
{
    public DissolveTeamCommandValidator()
    {
        RuleFor(c => c.ManagerId).Empty().WithMessage("ManagerId is required.");
    }
}