using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Data.State
{
    public class Entity
    {
        public string ID { get; set; }
        public string ConfigPath { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public List<string> Modules { get; set; }
    }
}

namespace Data.Proxy
{
    public class Entity
    {
        public State.Entity Origin { get; }
        public string ID { get; }
        public string ConfigPath { get; }
        
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector3> Rotation { get; }
        public ObservableList<string> Modules { get; }

        public Entity(State.Entity origin)
        {
            Origin = origin;
            ID = origin.ID;
            ConfigPath = origin.ConfigPath;
            
            Position = new ReactiveProperty<Vector3>(origin.Position);
            Position.Skip(1).Subscribe(position => Position.Value = position);
            
            Rotation = new ReactiveProperty<Vector3>(origin.Rotation);
            Rotation.Skip(1).Subscribe(rotation => Rotation.Value = rotation);
            
            Modules = new ObservableList<string>();
            foreach (var module in origin.Modules) Modules.Add(module);
            Modules.ObserveAdd().Subscribe(e => Origin.Modules.Add(e.Value));
            Modules.ObserveRemove().Subscribe(e => Origin.Modules.Remove(e.Value));
        }
    }
}