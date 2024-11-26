
using System;
using UnityEngine;

public class PlayerBombCollector : MonoBehaviour
{
    public event Action<int> OnBomb;
    public int NumOfBombs { get; private set; }

    private void Awake()
    {
        print(GetType().FullName + "::Awake");
        PlayerMain.Instance.Register(this);
    }

    private void Start()
    {
        print(GetType().FullName + "::Start");
        PlayerMain.Instance.Input.OnBomb += context =>
        {
            print(GetType().FullName + "::Start::OnBombHandler");
            if(context.started){
                if(NumOfBombs > 0){
                    var b = ObjectPoolManager.Instance.Pool<BombExplosion>();
                    b.transform.position = transform.position;
                    NumOfBombs--;
                }
                
                OnBomb?.Invoke(NumOfBombs);
            }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        print(GetType().FullName + "::OnTriggerEnter");
        if (other.CompareTag("BombContainer"))
        {
            NumOfBombs++;
            OnBomb?.Invoke(NumOfBombs);
        }
    }
    
    
}
