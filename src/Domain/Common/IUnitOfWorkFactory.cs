namespace Domain.Common;
public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork(string criteria);
}