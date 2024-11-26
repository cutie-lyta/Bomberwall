using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombCollector : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Vector2Int _upleftCorner;
    
    [SerializeField]
    private Vector2Int _downRightCorner;
    
    [SerializeField]
    private float _y;

    [SerializeField] private int _spacing;

    private void Awake()
    {
        ObjectPoolManager.Instance.RegisterInstance<BombCollector>(this);
    }

    private void Start()
    {
        OnPooled();
    }

    public void RegisterType()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Entity"))
        {
            ObjectPoolManager.Instance.Unpool(this);
        }
    }

    public void OnPooled()
    {
        var n = AINodeChooser.Nodes[Random.Range(0, AINodeChooser.Nodes.Count)];
        
        transform.position = n.Position;
        transform.position = new Vector3(transform.position.x, _y, transform.position.z);
    }

    public void OnUnPooled()
    {
    }
}
