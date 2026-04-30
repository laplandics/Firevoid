namespace Data { public abstract class DataState { } }
namespace Data
{
    public abstract class DataProxy
    {
        public DataState GetState { get; }
        protected DataProxy(DataState origin) => GetState = origin;
    }
}