using System;
using Behaviour.Interfaces;
using ObservableCollections;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Space
{
    public class UI : IDisposable
    {
        private readonly UIRoot _uiRoot;
        
        private readonly ReactiveProperty<IScreen> _screen = new();
        private readonly ObservableList<IWindow> _windows = new();
        private readonly ObservableList<IToken> _tokens = new();
        
        public UI()
        {
            var rootPrefab = R.UIPrefab;
            var rootObj = Object.Instantiate(rootPrefab);
            rootObj.name = rootPrefab.name;
            Object.DontDestroyOnLoad(rootObj);
            _uiRoot = rootObj.GetComponent<UIRoot>();
        }

        public T OpenScreen<T>(IUIVisualScreen uiVisual) where T : IScreen
        {
            if(_screen.Value != null) CloseScreen(_screen.Value);
            
            var screenPrefab = uiVisual.UIPrefab;
            var screenObj = Object.Instantiate(screenPrefab, _uiRoot.screenContainer, false);
            var screen = screenObj.GetComponent<T>();
            screen.OnOpen();
            _screen.Value = screen;
            return screen;
        }

        public void CloseScreen<T>(T screen) where T : IScreen
        {
            screen.OnClose();
            Object.Destroy(screen.Screen);
            _screen.Value = null;
        }

        public T ShowWindow<T>(IUIVisualWindow uiVisual) where T : IWindow
        {
            if (_windows.Contains(uiVisual.Window)) { HideWindow(uiVisual.Window); }
            
            var windowPrefab = uiVisual.UIPrefab;
            var windowObj = Object.Instantiate(windowPrefab, _uiRoot.windowsContainer, false);
            var window = windowObj.GetComponent<T>();
            window.OnShow();
            _windows.Add(window);
            return window;
        }

        public void HideWindow<T>(T window) where T : IWindow
        {
            window.OnHide();
            Object.Destroy(window.Window);
            _windows.Remove(window);
        }

        public T AddToken<T>(IUIVisualToken uiVisual) where T : IToken
        {
            if (_tokens.Contains(uiVisual.Token)) { RemoveToken(uiVisual.Token); }
            
            var tokenPrefab = uiVisual.UIPrefab;
            var tokenObj = Object.Instantiate(tokenPrefab, _uiRoot.tokensContainer, false);
            var token = tokenObj.GetComponent<T>();
            token.OnAdd();
            _tokens.Add(token);
            return token;
        }

        public void RemoveToken<T>(T token) where T : IToken
        {
            token.OnRemove();
            Object.Destroy(token.Token);
            _tokens.Remove(token);
        }
        
        public void Dispose()
        {
            var screen = _uiRoot.canvasTransform.GetComponentInChildren<IScreen>(true);
            if (screen != null)
            {
                var screenObj = screen as MonoBehaviour;
                if (screenObj != null)
                { screen.OnClose(); Object.Destroy(screenObj.gameObject); }
            }
            
            var windows = _uiRoot.canvasTransform.GetComponentsInChildren<IWindow>(true);
            foreach (var window in windows)
            {
                window.OnHide();
                var windowObj = window as MonoBehaviour;
                if (windowObj != null) Object.Destroy(windowObj.gameObject);
            }
            
            var tokens = _uiRoot.canvasTransform.GetComponentsInChildren<IToken>(true);
            foreach (var token in tokens)
            {
                token.OnRemove();
                var tokenObj = token as MonoBehaviour;
                if (tokenObj != null) Object.Destroy(tokenObj.gameObject);
            }
        }
    }
}

public interface IScreen { public GameObject Screen { get; } public void OnOpen(); public void OnClose(); }
public interface IWindow { public GameObject Window { get; } public void OnShow(); public void OnHide(); }
public interface IToken { public GameObject Token { get; } public void OnAdd(); public void OnRemove(); }