using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AutoServiceAttribute : Attribute
{
    public Type[] Interfaces { get; }
    
    public AutoServiceAttribute(params Type[] interfaces)
    {
        Interfaces = interfaces;
    }
}
