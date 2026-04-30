using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    [Serializable]
    public class Stage : DataState
    {
        public List<Spawner> Spawners { get; set; }
        public List<Entity> Entities { get; set; }
    }
}

namespace Data.Proxy
{
    public class Stage : DataProxy
    {
        public State.Stage Origin { get; }
        
        public ObservableList<Spawner> Spawners { get; }
        public ObservableList<Entity> Entities { get; }

        public Stage(State.Stage origin) : base(origin)
        {
            Origin = origin;
            
            Spawners = new ObservableList<Spawner>();
            origin.Spawners.ForEach(spawner => Spawners.Add(new Spawner(spawner)));
            Spawners.ObserveAdd().Subscribe(e => origin.Spawners.Add(e.Value.Origin));
            Spawners.ObserveRemove().Subscribe(e => origin.Spawners.Remove(e.Value.Origin));
            
            Entities = new ObservableList<Entity>();
            origin.Entities.ForEach(entity => Entities.Add(new Entity(entity)));
            Entities.ObserveAdd().Subscribe(e => origin.Entities.Add(e.Value.Origin));
            Entities.ObserveRemove().Subscribe(e => origin.Entities.Remove(e.Value.Origin));
        }
    }
}