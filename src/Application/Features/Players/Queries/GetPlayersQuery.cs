using MediatR;
using Domain.Common;
using AutoMapper;

namespace Application.Features.Players.Queries;
public record GetPlayersQuery : IRequest<IEnumerable<PlayerResponse>>;

public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, IEnumerable<PlayerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlayersQueryHandler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(GetPlayersQueryHandler));
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlayerResponse>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = await _unitOfWork.Players.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PlayerResponse>>(players);
    }
}