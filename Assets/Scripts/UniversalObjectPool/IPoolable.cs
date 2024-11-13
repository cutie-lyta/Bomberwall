
using System.Reflection;
using UnityEngine;

/// <summary>
/// The base interface for every object that can be in the object pool
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// Reimplementation of Unity's default gameObject field.
    /// Will automatically get filled by MonoBehaviour inheritance
    /// </summary>
    public GameObject gameObject {get;}

    /// <summary>
    /// Register this object type into the object pool, to be sure the object pool can instantiate that kind of object.
    /// Should get executed only if the current type hasn't been registered.
    /// You can use this snippet to make sure the type isn't already registered:
    /// <code> if (ObjectPoolManager.Instance.IsRegistered(GetType())) return; </code>
    /// </summary>
    public void RegisterType();
    
    /// <summary>
    /// Function that get called when the object get pooled
    /// It's basically the Object Pool equivalent of an Awake/Start/OnEnable
    /// </summary>
    public void OnPooled();
    
    /// <summary>
    /// Function that get called when the object get unpooled
    /// It's basically the Object Pool equivalent of an OnDestroy/OnDisable
    /// </summary>
    public void OnUnPooled();
}
