
using System;
using UnityEngine;

public class PlayerBombCollector : MonoBehaviour
{
    public event Action<int> OnBomb;
    private int _numOfBombs;

    private void Awake()
    {
        PlayerMain.Instance.Register(this);
    }

    private void Start()
    {
        PlayerMain.Instance.Input.OnBomb += context =>
        {
            if(context.started){
                if(_numOfBombs > 0){
                    var b = ObjectPoolManager.Instance.Pool<BombExplosion>();
                    b.transform.position = transform.position;
                    _numOfBombs--;
                }
                
                OnBomb?.Invoke(_numOfBombs);
            }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BombContainer"))
        {
            _numOfBombs++;
            OnBomb?.Invoke(_numOfBombs);
        }
    }
    
    
}
