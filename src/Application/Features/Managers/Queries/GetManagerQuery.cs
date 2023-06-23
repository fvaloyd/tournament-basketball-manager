using MediatR;
using Domain.Common;
using FluentValidation;
using Domain.Managers.Exceptions;
using AutoMapper;
using Shared;

namespace Application.Features.Managers.Queries;
public record GetManagerQuery : IRequest<ManagerResponse>
{
    public Guid ManagerId { get; init; }
}

public class GetManagerQueryHandler : IRequestHandler<GetManagerQuery, ManagerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public GetManagerQueryHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerManager logger, IMapper mapper)
    {
        _unitOfWork = unitOfWorkFactory.CreateUnitOfWork(nameof(GetManagerQueryHandler));
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ManagerResponse> Handle(GetManagerQuery request, CancellationToken cancellationToken)
    {
        var manager = await _unitOfWork.Managers.GetByIdAsync(request.ManagerId, cancellationToken);
        if (manager is null)
        {
            _logger.LogWarn($"Handler::{nameof(GetManagerQueryHandler)} - Manager with id::{request.ManagerId} was not found.");
            throw new ManagerNotFoundException(request.ManagerId);
        }
        return _mapper.Map<ManagerResponse>(manager);
    }
}

public class GetManagerQueryValidator : AbstractValidator<GetManagerQuery>
{
    public GetManagerQueryValidator()
    {
        RuleFor(c => c.ManagerId).NotNull().NotEmpty().WithMessage("ManagerId is required.");
    }
}