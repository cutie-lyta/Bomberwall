
using System.Collections.Generic;
using UnityEngine;

public class BombVFX : MonoBehaviour
{
    [SerializeField] private float _spacing;
    private bool _isFirstDisable = true;
    
    void OnDisable()
    {
        print(GetType().FullName + "::OnDisable");
        var dir = new List<Vector3>() { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                var e = ObjectPoolManager.Instance.Pool<ExplosionVFXObject>();
                e.transform.position = transform.position + (dir[i] * (_spacing * j));
            }
        }
    }
}
