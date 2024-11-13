
using System;
using UnityEngine;

public class AIBombPlacer : MonoBehaviour
{
    public event Action<int> OnNewBomb;
    public event Action OnBombPlaced;
    private int _numOfBombs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BombContainer"))
        {
            _numOfBombs++;
            OnNewBomb?.Invoke(_numOfBombs);
        }
        else if (other.CompareTag("Respawn"))
        {
            for (int i = 0; i < _numOfBombs; i++)
            {
                var b = ObjectPoolManager.Instance.Pool<BombExplosion>();
                b.transform.position = transform.position;
            }

            _numOfBombs = 0;
            
            OnBombPlaced?.Invoke();
        }
    }
}
