using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;
using MassTransit;

namespace Application.Features.Organizers.Commands;
public record FinishTournamentCommand : IRequest
{
    public Guid OrganizerId { get; init; }
}

public class FinishTournamentCommandHandler : IRequestHandler<FinishTournamentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IBus _bus;

    public FinishTournamentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger, IBus bus)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(FinishTournamentCommandHandler));
        _logger = logger;
        _bus = bus;
    }
    public async Task Handle(FinishTournamentCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(FinishTournamentCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        var managerIds = organizer.Tournament!.Teams.Select(t => t.ManagerId).ToList();
        organizer.FinishTournament();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _bus.Publish(new TournamentFinishedEvent(request.OrganizerId, managerIds), cancellationToken);
    }
}

public record TournamentFinishedEvent(Guid OrganizerId, IEnumerable<Guid> ManagerIds);

public class FinishTournamentCommandValidator : AbstractValidator<FinishTournamentCommand>
{
    public FinishTournamentCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
    }
}