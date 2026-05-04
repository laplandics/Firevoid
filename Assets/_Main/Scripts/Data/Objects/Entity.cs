namespace Data.State
{
    public class Entity : DataState
    {
        public string Id { get; set; }
    }
}

namespace Data.Proxy
{
    public class Entity : DataProxy
    {
        public State.Entity State { get; }
        
        public string Id { get; }
        
        public Entity(DataState origin) : base(origin)
        {
            State = GetOrigin<State.Entity>();
            
            Id = State.Id;
        }
    }
}