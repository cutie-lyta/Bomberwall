
using System;
using UnityEngine;

public class AIBombPlacer : MonoBehaviour
{
    public event Action<int> OnNewBomb;
    public event Action<int> OnBombPlaced;
    public int BombCount { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        print(GetType().FullName + "::OnTriggerEnter");
        if (other.CompareTag("BombContainer"))
        {
            BombCount++;
            OnNewBomb?.Invoke(BombCount);
        }
    }

    public void PlaceBomb()
    {
        print(GetType().FullName + "::PlaceBomb");
        if (BombCount <= 0) return;
        
        var b = ObjectPoolManager.Instance.Pool<BombExplosion>();
        b.transform.position = transform.position;

        BombCount -= 1;
            
        OnBombPlaced?.Invoke(BombCount);
    }
}
