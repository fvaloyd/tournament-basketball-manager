using MediatR;
using Domain.Common;
using FluentValidation;
using AutoMapper;
using Domain.Organizers.Exceptions;
using Shared;

namespace Application.Features.Organizers.Queries;
public record GetTournamentMatchesQuery : IRequest<IEnumerable<MatchResponse>>
{
    public Guid OrganizerId { get; set; }
}

public class GetTournamentMatchesQueryHandler : IRequestHandler<GetTournamentMatchesQuery, IEnumerable<MatchResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetTournamentMatchesQueryHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger, IMapper mapper)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(GetTournamentMatchesQueryHandler));
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchResponse>> Handle(GetTournamentMatchesQuery request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(GetOrganizerQueryHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        var matches = organizer.GetTournamentMatches();
        return _mapper.Map<IEnumerable<MatchResponse>>(matches);
    }
}

public class GetTournamentMatchesQueryValidator : AbstractValidator<GetTournamentMatchesQuery>
{
    public GetTournamentMatchesQueryValidator()
    {
        RuleFor(c => c.OrganizerId).NotNull().NotEmpty().WithMessage("OrganizerId is required.");
    }
}