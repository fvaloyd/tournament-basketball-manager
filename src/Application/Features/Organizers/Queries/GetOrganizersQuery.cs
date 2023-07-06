using AutoMapper;
using Domain.Common;
using MediatR;
using Shared;

namespace Application.Features.Organizers.Queries;
public record GetOrganizersQuery : IRequest<IEnumerable<OrganizerResponse>>;

public class GetOrganizersQueryHandler : IRequestHandler<GetOrganizersQuery, IEnumerable<OrganizerResponse>>
{
    private readonly IUnitOfWork _unifOfWork;
    private readonly IMapper _mapper;
    public GetOrganizersQueryHandler(IUnitOfWorkFactory ofwFactory, IMapper mapper)
    {
        _unifOfWork = ofwFactory.CreateUnitOfWork(nameof(GetOrganizerQueryHandler));
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrganizerResponse>> Handle(GetOrganizersQuery request, CancellationToken cancellationToken)
    {
        var organizers = await _unifOfWork.Organizers.GetAllOrganizersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OrganizerResponse>>(organizers);
    }
}
