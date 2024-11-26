
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BombExplosion : MonoBehaviour, IPoolable
{
    [SerializeField] private float _timeBeforeExplosion;

    private void Awake()
    {
        ObjectPoolManager.Instance.RegisterInstance<BombExplosion>(this);

        ObjectPoolManager.Instance.Unpool(this);
    }

    public void RegisterType()
    {
    }

    public void OnPooled()
    {
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(_timeBeforeExplosion);

        List<RaycastHit> hits = Physics.RaycastAll(transform.position, Vector3.forward * 3f).ToList();
        hits.AddRange(Physics.RaycastAll(transform.position, Vector3.back * 3f)); 
        hits.AddRange(Physics.RaycastAll(transform.position, Vector3.left * 3f)); 
        hits.AddRange(Physics.RaycastAll(transform.position, Vector3.right * 3f)); 
        
        for (int i = 0; i < hits.Count; i++) { }
        
        ObjectPoolManager.Instance.Unpool(this);
        ObjectPoolManager.Instance.Pool<BombCollector>();
    }

    public void OnUnPooled()
    {
    }
}
