using System;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public enum DisposeMethod { Scoped, Full }
    
    private static readonly Dictionary<Type, ServiceRegistration> RegistrationsMap = new();
    private static readonly HashSet<Type> Requests = new();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset() => Dispose(DisposeMethod.Full);
    
    public static ServiceRegistration Register<T>(T service) where T : class
    {
        if (RegistrationsMap.ContainsKey(typeof(T)))
        { throw new Exception($"Duplicate registration of type {typeof(T)}"); }

        var registration = new ServiceRegistration { Service = service };
        
        RegistrationsMap.Add(typeof(T), registration);
        return registration;
    }

    public static T Resolve<T>() where T : class
    {
        if (!Requests.Add(typeof(T)))
        {throw new Exception($"Type {typeof(T)} has been already requested. Circular dependency detected"); }
        
        T result = null;
        if (RegistrationsMap.TryGetValue(typeof(T), out var service))
        { result = service as T; }
        
        if (result != null) Requests.Remove(typeof(T));
        else throw new Exception($"Requested type {typeof(T)} was not registered");
        
        return result;
    }
    
    public static void Dispose(DisposeMethod method)
    {
        foreach (var registration in RegistrationsMap)
        { registration.Value.Dispose(); }

        if (method != DisposeMethod.Full) return;
        Requests.Clear();
        RegistrationsMap.Clear();
    }

    public class ServiceRegistration : IDisposable
    {
        public object Service { get; set; }

        public void Dispose()
        { if (Service is IDisposable disposable) disposable.Dispose(); }
    }
}

