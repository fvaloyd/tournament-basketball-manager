using MediatR;
using Domain.Common;
using Domain.Services;
using FluentValidation;
using Domain.Organizers.Exceptions;

namespace Application.Features.Organizers.Commands;
public record MatchTeamsCommand : IRequest
{
    public Guid OrganizerId { get; set; }
}

public class MatchTeamsCommandHandler : IRequestHandler<MatchTeamsCommand>
{
    private readonly ITeamMatchMaker _teamMatchMaker;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public MatchTeamsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger, ITeamMatchMaker teamMatchMaker)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(MatchTeamsCommandHandler));
        _logger = logger;
        _teamMatchMaker = teamMatchMaker;
    }

    public async Task Handle(MatchTeamsCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(MatchTeamsCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        organizer.MatchTeams(_teamMatchMaker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public class MatchTeamsCommandValidator : AbstractValidator<MatchTeamsCommand>
{
    public MatchTeamsCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
    }
}