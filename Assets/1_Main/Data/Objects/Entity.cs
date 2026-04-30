using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Data.State
{
    public class Entity : DataState
    {
        public string ID { get; set; }
        public string Key { get; set; }
        public int Priority { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public List<Module> Modules { get; set; }
    }
}

namespace Data.Proxy
{
    public class Entity : DataProxy
    {
        public State.Entity Origin { get; }
        
        public string ID { get; }
        public string Key { get; }
        public int Priority { get; }
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector3> Rotation { get; }
        public ObservableList<Module> Modules { get; }
        
        public Entity(State.Entity origin) : base(origin)
        {
            Origin = origin;
            ID = origin.ID;
            Key = origin.Key;
            Priority = origin.Priority;
            
            Position = new ReactiveProperty<Vector3>(origin.Position);
            Position.Skip(1).Subscribe(position => Position.Value = position);
            
            Rotation = new ReactiveProperty<Vector3>(origin.Rotation);
            Rotation.Skip(1).Subscribe(rotation => Rotation.Value = rotation);
            
            Modules = new ObservableList<Module>();
            origin.Modules.ForEach(module => Modules.Add(new Module(module)));
            Modules.ObserveAdd().Subscribe(e => origin.Modules.Add(e.Value.Origin));
            Modules.ObserveRemove().Subscribe(e => origin.Modules.Remove(e.Value.Origin));
        }
    }
}