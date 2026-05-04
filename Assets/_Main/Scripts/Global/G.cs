using System;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public enum DisposeMethod { Scoped, Full }
    
    private static readonly Dictionary<Type, ServiceRegistration> RegistrationsMap = new();
    
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
        T result = null;
        if (RegistrationsMap.TryGetValue(typeof(T), out var registration))
        { result = registration.Service as T; }
        
        if (result != null) return result;
        throw new Exception($"Requested type {typeof(T)} was not registered");
    }
    
    public static void Dispose(DisposeMethod method)
    {
        if (method == DisposeMethod.Full)
        { RegistrationsMap.Clear(); return;}
        
        foreach (var registration in RegistrationsMap)
        { registration.Value.Dispose(); }
    }

    public class ServiceRegistration : IDisposable
    {
        public object Service { get; set; }

        public void Dispose()
        { if (Service is IDisposable disposable) disposable.Dispose(); }
    }
}

