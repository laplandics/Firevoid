using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    [Serializable]
    public class Project
    {
        public List<Entity> Entities { get; set; }
    }
}

namespace Data.Proxy
{
    public class Project
    {
        public State.Project Origin { get; }
        
        public ObservableList<Entity> Entities { get; }

        public Project(State.Project origin)
        {
            Origin = origin;
            
            Entities = new ObservableList<Entity>();
            Origin.Entities.ForEach(entity => Entities.Add(new Entity(entity)));
            Entities.ObserveAdd().Subscribe(e => Origin.Entities.Add(e.Value.Origin));
            Entities.ObserveRemove().Subscribe(e => Origin.Entities.Remove(e.Value.Origin));
        }
    }
}