
using System;
using UnityEngine;

public class ExplosionVFXObject : MonoBehaviour, IPoolable
{
    [SerializeField] private ParticleSystem _p;
    
    public void RegisterType() { }

    public void Awake()
    {
        print(GetType().FullName + "::Awake");
        ObjectPoolManager.Instance.RegisterInstance<ExplosionVFXObject>(this);
    }
    
    public void OnPooled()
    {
        print(GetType().FullName + "::OnPooled");
        _p.Play();
    }

    public void OnUnPooled()
    {
        print(GetType().FullName + "::OnUnPooled");
        _p.Stop();
    }

    private void OnParticleSystemStopped()
    {
        print(GetType().FullName + "::OnParticleSystemStopped");
        ObjectPoolManager.Instance.Unpool(this);
    }
}
