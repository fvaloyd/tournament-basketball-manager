using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Infrastructure.Sql.Context;

namespace Infrastructure.Sql.Repositories;
public class SqlUnitOfWork : IUnitOfWork
{
    private readonly TournamentBasketballManagerDbContext _db;
    private readonly Lazy<IManagerRepository> _managerRepository;
    private readonly Lazy<IPlayerRepository> _playerRepository;
    private readonly Lazy<IOrganizerRepository> _organizerRepository;
    private readonly Lazy<ITeamRepository> _teamRepository;
    private readonly Lazy<ITournamentRepository> _tournamentRepository;

    public SqlUnitOfWork(TournamentBasketballManagerDbContext db)
    {
        _db = db;
        _managerRepository = new Lazy<IManagerRepository>(() => new SqlManagerRepository(_db));
        _playerRepository = new Lazy<IPlayerRepository>(() => new SqlPlayerRepository(_db));
        _organizerRepository = new Lazy<IOrganizerRepository>(() => new SqlOrganizerRepository(_db));
        _teamRepository = new Lazy<ITeamRepository>(() => new SqlTeamRepository(_db));
        _tournamentRepository = new Lazy<ITournamentRepository>(() => new SqlTournamentRepository(_db));
    }

    public IManagerRepository Managers => _managerRepository.Value;

    public IPlayerRepository Players => _playerRepository.Value;

    public IOrganizerRepository Organizers => _organizerRepository.Value;

    public ITeamRepository Teams => _teamRepository.Value;

    public ITournamentRepository Tournaments => _tournamentRepository.Value;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}