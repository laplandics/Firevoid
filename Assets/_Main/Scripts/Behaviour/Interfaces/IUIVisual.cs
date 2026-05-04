using UnityEngine;

namespace Behaviour.Interfaces
{
    public interface IUIVisualScreen 
    {
        public GameObject UIPrefab { get; }
        public IScreen Screen { get; }
        
        public void Open();
        public void Close();
    }

    public interface IUIVisualWindow 
    {
        public GameObject UIPrefab { get; }
        public IWindow Window { get; }
        
        public void Show();
        public void Hide();
    }

    public interface IUIVisualToken 
    {
        public GameObject UIPrefab { get; }
        public IToken Token { get; }
        
        public void Add();
        public void Remove();
    }
}
