using AutoMapper;
using Domain.Common;
using MediatR;
using Shared;

namespace Application.Features.Managers.Queries;

public record GetManagersQuery : IRequest<IEnumerable<ManagerResponse>>;

public class GetManagersQueryHandler : IRequestHandler<GetManagersQuery, IEnumerable<ManagerResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetManagersQueryHandler(IUnitOfWorkFactory uowFactory, IMapper mapper)
    {
        _unitOfWork = uowFactory.CreateUnitOfWork(nameof(GetManagersQueryHandler));
        _mapper = mapper;
    }

    public async Task<IEnumerable<ManagerResponse>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
    {
        var managers = await _unitOfWork.Managers.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ManagerResponse>>(managers);
    }
}
