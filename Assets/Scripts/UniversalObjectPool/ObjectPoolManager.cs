
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of this manager
    /// </summary>
    public static ObjectPoolManager Instance { get; private set; }
    
    /// <summary>
    /// A dictionnary containing the type of the IPoolable object and a max number of instance.
    /// It can accept non IPoolable class, but it will never get used,
    /// because the type check is used at instanciation and pooling
    /// TODO: Use a TYPE variable with Odin.
    /// TODO: Use normal dictionnary with Odin
    /// </summary>
    [Tooltip("The name of the type and the number max of instance of that type")]
    [SerializeField] private SerializedDictionnary<string, int> _maxValue;
    
    /// <summary>
    /// A dictionnary containing the type of the IPoolable object and a prefab/gameobject to instanciate
    /// whenever there is not enough object in the pool and in existance.
    /// It can accept non IPoolable class, but it will never get used,
    /// because the type check is used at instanciation and pooling
    /// TODO: Use a TYPE variable with Odin.
    /// TODO: Use normal dictionnary with Odin
    /// </summary>
    [Tooltip("The name of the type and the prefab of that type")]
    [SerializeField] private SerializedDictionnary<string, GameObject> _prefabPool;
    
    /// <summary>
    /// The current number of each type in the pool actually (both instanciated and dormant)
    /// </summary>
    private Dictionary<Type, int> _currentPoolCount = new ();
    
    /// <summary>
    /// A list of every dormant object in the pool
    /// </summary>
    private List<IPoolable> _pool;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _pool = new List<IPoolable>();
    }

    /// <summary>
    /// Check if the type T already has a prefab attached to it, for easy instanciation
    /// </summary>
    /// <param name="T">
    /// Type of the IPoolable object
    /// It can accept non IPoolable class, but it will never get used,
    /// because the type check is used at instanciation and pooling
    /// </param>
    /// <returns> Whether the type T has a prefab attached or not </returns>
    public bool IsRegistered(Type T)
    {
        return _prefabPool.Keys.Contains(T.Name);
    }

    /// <summary>
    /// Add the type T to the prefab pool
    /// </summary>
    /// <param name="T">
    /// Type of the IPoolable object
    /// It can accept non IPoolable class, but it will never get used,
    /// because the type check is used at instanciation and pooling
    /// </param>
    /// <param name="prefab"> The gameobject that will be used as instanciation prefab. </param>
    public void AddToPrefabPool(Type T, GameObject prefab)
    {
        prefab.transform.SetParent(transform);
        _prefabPool.Add(T.Name, prefab);
    }

    /// <summary>
    /// Get an object of type T, which is an IPoolable.
    /// </summary>
    /// <typeparam name="T">The type of the object being return,
    /// which should be a IPoolable MonoBehaviour subclass</typeparam>
    /// <returns> The object being pooled </returns>
    public T Pool<T>() where T : MonoBehaviour, IPoolable
    {
        T pooledObject = (T)(_pool.Find(poolable => poolable.GetType() == typeof(T)));
        if (pooledObject == null)
        {
            if (!_currentPoolCount.ContainsKey(typeof(T))) _currentPoolCount.Add(typeof(T), 0);
            if (_currentPoolCount[typeof(T)] >= _maxValue[typeof(T).Name] ) return null;
            
            _currentPoolCount[typeof(T)]++;
            
            var prefab = _prefabPool[typeof(T).Name];
            var instance = Instantiate(prefab);
            pooledObject = instance.GetComponent<T>();
        }
        _pool.Remove(pooledObject);
        
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.SetParent(null);
        pooledObject.OnPooled();

        return pooledObject;
    }

    /// <summary>
    /// Store an object into the pool
    /// </summary>
    /// <param name="pooledObject"> The object to store </param>
    public void Unpool(IPoolable pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        _pool.Add(pooledObject);
    }
}