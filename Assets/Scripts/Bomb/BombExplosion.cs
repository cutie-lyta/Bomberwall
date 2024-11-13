
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BombExplosion : MonoBehaviour, IPoolable
{
    [SerializeField] private float _timeBeforeExplosion;

    private void Awake()
    {
        ObjectPoolManager.Instance.Unpool(this);
    }

    public void RegisterType() { }

    public void OnPooled()
    {
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(_timeBeforeExplosion);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3f, Vector3.up);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("BreakableWall"))
            {
                hits[i].transform.SendMessage("Exploded", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        ObjectPoolManager.Instance.Unpool(this);
        ObjectPoolManager.Instance.Pool<BombCollector>();

    }
    
    public void OnUnPooled() { }
}
