using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;

namespace Application.Features.Organizers.Commands;
public record FinishTournamentCommand : IRequest
{
    public Guid OrganizerId { get; init; }
}

public class FinishTournamentCommandHandler : IRequestHandler<FinishTournamentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public FinishTournamentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(FinishTournamentCommandHandler));
        _logger = logger;
    }
    public async Task Handle(FinishTournamentCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(FinishTournamentCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        organizer.FinishTournament();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public class FinishTournamentCommandValidator : AbstractValidator<FinishTournamentCommand>
{
    public FinishTournamentCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
    }
}