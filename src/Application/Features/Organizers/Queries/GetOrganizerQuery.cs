using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Organizers.Exceptions;
using AutoMapper;
using Shared;

namespace Application.Features.Organizers.Queries;
public record GetOrganizerQuery : IRequest<OrganizerResponse>
{
    public Guid OrganizerId { get; init; }
}

public class GetOrganizerQueryHandler : IRequestHandler<GetOrganizerQuery, OrganizerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public GetOrganizerQueryHandler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILoggerManager logger)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(GetOrganizerQueryHandler));
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrganizerResponse> Handle(GetOrganizerQuery request, CancellationToken cancellationToken)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer is null)
        {
            _logger.LogWarn($"Handler::{nameof(GetOrganizerQueryHandler)} - Organizer with id::{request.OrganizerId} was not found.");
            throw new OrganizerNotFoundException(request.OrganizerId);
        }
        return _mapper.Map<OrganizerResponse>(organizer);
    }
}

public class GetOrganizerQueryValidator : AbstractValidator<GetOrganizerQuery>
{
    public GetOrganizerQueryValidator()
    {
        RuleFor(c => c.OrganizerId).NotEmpty().NotNull().WithMessage("OrganizerId is required.");
    }
}