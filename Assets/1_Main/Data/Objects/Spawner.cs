using System.Collections.Generic;
using UnityEngine;

namespace Data.State
{
    public class Spawner : DataState
    {
        public Vector3 Position { get; set; }
        public List<Entity> Entities { get; set; }
    }
}

namespace Data.Proxy
{
    public class Spawner : DataProxy
    {
        public State.Spawner Origin { get; }
        
        public Vector3 Position { get; set; }
        public List<Entity> Entities { get; }
        
        public Spawner(State.Spawner origin) : base(origin)
        {
            Origin = origin;
            Position = origin.Position;
            
            Entities = new List<Entity>();
            origin.Entities.ForEach(entity => Entities.Add(new Entity(entity)));
        }
    }
}