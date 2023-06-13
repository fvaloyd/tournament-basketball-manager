using MediatR;
using Domain.Common;
using FluentValidation;
using Application.Features.Organizers.DTOs;
using AutoMapper;

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
        var matches = await _unitOfWork.Organizers.GetTournamentMatches(request.OrganizerId, cancellationToken);
        return _mapper.Map<IEnumerable<MatchResponse>>(matches);
    }
}

public class GetTournamentMatchesQueryValidator : AbstractValidator<GetTournamentMatchesQuery>
{
    public GetTournamentMatchesQueryValidator()
    {
        RuleFor(c => c.OrganizerId).Empty().WithMessage("OrganizerId is required.");
    }
}