using R3;
using UnityEngine;

namespace Data.State
{
    public class Module : DataState
    {
        public string Key { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}

namespace Data.Proxy
{
    public class Module : DataProxy
    {
        public State.Module Origin { get; }
        
        public string Key { get; }
        
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector3> Rotation { get; }
        
        public Module(State.Module origin) : base(origin)
        {
            Origin = origin;
            Key = origin.Key;
            
            Position = new ReactiveProperty<Vector3>(origin.Position);
            Position.Skip(1).Subscribe(position => origin.Position = position);
            
            Rotation = new ReactiveProperty<Vector3>(origin.Rotation);
            Rotation.Skip(1).Subscribe(rotation => origin.Rotation = rotation);
        }
    }
}