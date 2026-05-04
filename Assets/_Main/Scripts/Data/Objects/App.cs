using Parameters;
using R3;

namespace Data.State
{
    public class App : DataState
    {
        public int FPS { get; set; }
        public int VSync { get; set; }
        public SceneParams FirstSceneParams { get; set; }
    }
}

namespace Data.Proxy
{
    public class App : DataProxy
    {
        public State.App State { get; }
        
        public SceneParams FirstSceneParams { get; }
        
        public ReactiveProperty<int> FPS { get; }
        public ReactiveProperty<int> VSync { get; }
        
        public App(DataState origin) : base(origin)
        {
            State = GetOrigin<State.App>();
            
            FirstSceneParams = State.FirstSceneParams;
            
            FPS = new ReactiveProperty<int>(State.FPS);
            FPS.Skip(1).Subscribe(fps => State.FPS = fps);
            
            VSync = new ReactiveProperty<int>(State.VSync);
            VSync.Skip(1).Subscribe(vsync => State.VSync = vsync);
        }
    }
}