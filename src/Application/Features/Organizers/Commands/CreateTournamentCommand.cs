using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;

namespace Application.Features.Organizers.Commands;
public record CreateTournamentCommand : IRequest<Guid>
{
    public Guid OrganizerId { get; init; }
    public string TournamentName { get; init; } = string.Empty;
}

public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public CreateTournamentCommandHandler(ILoggerManager logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(CreateTournamentCommandHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        organizer.CreateTournament(request.TournamentName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return organizer.TournamentId;
    }
}

public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
{
    public CreateTournamentCommandValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
        RuleFor(c => c.TournamentName).NotNull().NotEmpty().WithMessage("TournamentName is requried.")
            .MaximumLength(75).WithMessage("Max length for TournamentName is 75 characters.")
            .MinimumLength(2).WithMessage("Min length for TournamentName is 2 characters.");
    }
}