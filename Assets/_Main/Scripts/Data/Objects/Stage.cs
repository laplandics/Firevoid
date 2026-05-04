using System.Collections.Generic;
using Constant;
using ObservableCollections;
using R3;

namespace Data.State
{
    public class Stage : DataState
    {
        public string Key { get; set; }
        public List<StageFlags> Flags { get; set; }
    }
}

namespace Data.Proxy
{
    public class Stage : DataProxy
    {
        public State.Stage State { get; }
        
        public string Key { get; }
        
        public ObservableList<StageFlags> Flags { get; }
        
        public Stage(DataState origin) : base(origin)
        {
            State = GetOrigin<State.Stage>();
            
            Key = State.Key;
            
            Flags = new ObservableList<StageFlags>();
            State.Flags.ForEach(flag => Flags.Add(flag));
            Flags.ObserveAdd().Subscribe(e => State.Flags.Add(e.Value));
            Flags.ObserveRemove().Subscribe(e => State.Flags.Remove(e.Value));
        }
    }
}