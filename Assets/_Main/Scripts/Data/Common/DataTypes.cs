namespace Data
{ public abstract class DataState {} }

namespace Data
{
    public abstract class DataProxy
    {
        public DataState Origin { get; }
        protected DataProxy(DataState origin) => Origin = origin;

        protected T GetOrigin<T>() where T : DataState => (T)Origin;
    }
}