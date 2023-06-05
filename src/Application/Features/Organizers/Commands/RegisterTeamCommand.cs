using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;

namespace Application.Features.Organizers.Commands;
public record RegisterTeamCommand : IRequest
{
    public Guid OrganizerId { get; init; }
    public Guid TeamId { get; init; }
}

public class RegisterTeamCommandHandler : IRequestHandler<RegisterTeamCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public RegisterTeamCommandHandler(IUnitOfWork unitOfWork, ILoggerManager logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(RegisterTeamCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(RegisterTeamCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        var team = await _unitOfWork.Teams.GetByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
        {
            _logger.LogWarn($"Handler::{nameof(RegisterTeamCommandHandler)} - Team with id::{request.TeamId} was not found.");
            throw new TeamNotFoundException(request.TeamId);
        }
        organizer.RegisterTeam(team!);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public class RegisterTeamCommandValidator : AbstractValidator<RegisterTeamCommand>
{
    public RegisterTeamCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
        RuleFor(c => c.TeamId).Empty().WithMessage("OrganizerId is required.");
    }
}