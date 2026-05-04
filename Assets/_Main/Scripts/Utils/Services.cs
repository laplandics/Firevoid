using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public interface IProjectBeginLoadService { public void OnProjectBeginLoad(); }
public interface IProjectEndLoadService { public void OnProjectEndLoad(); }
public interface IProjectBeginBootService { public IEnumerator OnProjectBeginBoot(); }
public interface IProjectEndBootService { public IEnumerator OnProjectEndBoot(); }
public interface ISceneBeginLoadService { public IEnumerator OnSceneBeginLoad(); }
public interface ISceneEndLoadService { public IEnumerator OnSceneEndLoad(); }
public interface ISceneBeginBootService { public IEnumerator OnSceneBeginBoot(); }
public interface ISceneEndBootService { public IEnumerator OnSceneEndBoot(); }
public interface ISceneBeginUnloadService { public IEnumerator OnSceneBeginUnload(); }
public interface ISceneEndUnloadService { public IEnumerator OnSceneEndUnload(); }

namespace Utils
{
    public class Services
    {
        private readonly HashSet<IProjectBeginLoadService>  _projectBeginLoadServices = new();
        private readonly HashSet<IProjectEndLoadService> _projectEndLoadServices = new();
        private readonly HashSet<IProjectBeginBootService> _projectBeginBootServices = new();
        private readonly HashSet<IProjectEndBootService> _projectEndBootServices = new();
        private readonly HashSet<ISceneBeginLoadService> _sceneBeginLoadServices = new();
        private readonly HashSet<ISceneEndLoadService> _sceneEndLoadServices = new();
        private readonly HashSet<ISceneBeginBootService> _sceneBeginBootServices = new();
        private readonly HashSet<ISceneEndBootService> _sceneEndBootServices = new();
        private readonly HashSet<ISceneBeginUnloadService> _sceneBeginUnloadServices = new();
        private readonly HashSet<ISceneEndUnloadService> _sceneEndUnloadServices = new();
        
        public Services() => CacheProjectServices();

        private void CacheProjectServices()
        {
            var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<AutoServiceAttribute>() is not null 
                    && t is { IsAbstract: false, IsInterface: false })
                .ToArray();

            foreach (var serviceType in serviceTypes)
            {
                var attr = serviceType.GetCustomAttribute<AutoServiceAttribute>();
                var instance = Activator.CreateInstance(serviceType);
                
                if (attr.Interfaces.Contains(typeof(IProjectBeginLoadService)))
                { _projectBeginLoadServices.Add((IProjectBeginLoadService)instance); }
                
                if (attr.Interfaces.Contains(typeof(IProjectEndLoadService)))
                { _projectEndLoadServices.Add((IProjectEndLoadService)instance); }

                if (attr.Interfaces.Contains(typeof(IProjectBeginBootService)))
                { _projectBeginBootServices.Add((IProjectBeginBootService)instance); }
                
                if (attr.Interfaces.Contains(typeof(IProjectEndBootService)))
                { _projectEndBootServices.Add((IProjectEndBootService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneBeginLoadService)))
                { _sceneBeginLoadServices.Add((ISceneBeginLoadService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneEndLoadService)))
                { _sceneEndLoadServices.Add((ISceneEndLoadService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneBeginBootService)))
                { _sceneBeginBootServices.Add((ISceneBeginBootService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneEndBootService)))
                { _sceneEndBootServices.Add((ISceneEndBootService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneBeginUnloadService)))
                { _sceneBeginUnloadServices.Add((ISceneBeginUnloadService)instance); }
                
                if (attr.Interfaces.Contains(typeof(ISceneEndUnloadService)))
                { _sceneEndUnloadServices.Add((ISceneEndUnloadService)instance); }
            }
        }

        public void OnProjectBeginLoad()
        {
            foreach (var service in _projectBeginLoadServices)
            { service.OnProjectBeginLoad(); }
        }

        public void OnProjectEndLoad()
        {
            foreach (var service in _projectEndLoadServices)
            { service.OnProjectEndLoad(); }
        }

        public IEnumerator OnProjectBeginBoot()
        {
            foreach (var service in _projectBeginBootServices)
            { yield return service.OnProjectBeginBoot(); yield return null; }
        }

        public IEnumerator OnProjectEndBoot()
        {
            foreach (var service in _projectEndBootServices)
            { yield return service.OnProjectEndBoot(); yield return null; }
        }

        public IEnumerator OnSceneBeginLoad()
        {
            foreach (var service in _sceneBeginLoadServices)
            { yield return service.OnSceneBeginLoad(); yield return null; }
        }

        public IEnumerator OnSceneEndLoad()
        {
            foreach (var service in _sceneEndLoadServices)
            { yield return service.OnSceneEndLoad(); yield return null; }
        }

        public IEnumerator OnSceneBeginBoot()
        {
            foreach (var service in _sceneBeginBootServices)
            { yield return service.OnSceneBeginBoot(); yield return null; }
        }

        public IEnumerator OnSceneEndBoot()
        {
            foreach (var service in _sceneEndBootServices)
            { yield return service.OnSceneEndBoot(); yield return null; }
        }

        public IEnumerator OnSceneBeginUnload()
        {
            foreach (var service in _sceneBeginUnloadServices)
            { yield return service.OnSceneBeginUnload(); yield return null; }
        }

        public IEnumerator OnSceneEndUnload()
        {
            foreach (var service in _sceneEndUnloadServices)
            { yield return service.OnSceneEndUnload(); yield return null; }
        }
    }
}