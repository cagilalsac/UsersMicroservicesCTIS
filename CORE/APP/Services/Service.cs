using CORE.APP.Domain;

namespace CORE.APP.Services
{
    public abstract class Service<TEntity> : ServiceBase, IDisposable where TEntity : Entity, new()
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
