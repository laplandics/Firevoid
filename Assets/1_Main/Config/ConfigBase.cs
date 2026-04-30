using System;
using Data;

namespace Config
{
    [Serializable]
    public abstract class ConfigBase<T> where T : DataState
    { public abstract T ToState(); }
}