using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;
using MassTransit;

namespace Application.Features.Organizers.Commands;
public record DiscardTeamCommand : IRequest
{
    public Guid OrganizerId { get; init; }
    public Guid TeamId { get; init; }
}

public class DiscardTeamCommandHandler : IRequestHandler<DiscardTeamCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IBus _bus;

    public DiscardTeamCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger, IBus bus)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(DiscardTeamCommandHandler));
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(DiscardTeamCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(DiscardTeamCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        organizer.DiscardTeam(request.TeamId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new TeamDiscardedEvent(request.OrganizerId, request.TeamId), cancellationToken);
    }
}

public record TeamDiscardedEvent(Guid OrganizerId, Guid TeamId);

public class DiscardTeamCommandValidator : AbstractValidator<DiscardTeamCommand>
{
    public DiscardTeamCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
        RuleFor(c => c.TeamId).Empty().WithMessage("TeamId is required.");
    }
}