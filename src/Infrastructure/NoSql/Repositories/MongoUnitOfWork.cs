using AutoMapper;
using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Microsoft.Extensions.Options;
using Infrastructure.NoSql.Repositories;

namespace Infrastructure;
public class MongoUnitOfWork : IUnitOfWork
{
    private readonly Lazy<MongoManagerRepository> _managerRepository;
    private readonly Lazy<MongoOrganizerRepository> _organizerRepository;
    private readonly Lazy<MongoPlayerRepository> _playerRepository;
    private readonly Lazy<MongoTeamRepository> _teamRepository;
    private readonly Lazy<MongoTournamentRepository> _tournamentRepository;

    public MongoUnitOfWork(IOptions<MongoDatabaseSettings> mongoDbSettingsOptions, IMapper mapper)
    {
        _managerRepository = new(() => new MongoManagerRepository(mongoDbSettingsOptions, mapper));
        _organizerRepository = new(() => new MongoOrganizerRepository(mongoDbSettingsOptions, mapper));
        _playerRepository = new(() => new MongoPlayerRepository(mongoDbSettingsOptions, mapper));
        _teamRepository = new(() => new MongoTeamRepository(mongoDbSettingsOptions, mapper));
        _tournamentRepository = new(() => new MongoTournamentRepository(mongoDbSettingsOptions, mapper));
    }

    public IManagerRepository Managers => _managerRepository.Value;

    public IPlayerRepository Players => _playerRepository.Value;

    public IOrganizerRepository Organizers => _organizerRepository.Value;

    public ITeamRepository Teams => _teamRepository.Value;

    public ITournamentRepository Tournaments => _tournamentRepository.Value;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(1);
}
