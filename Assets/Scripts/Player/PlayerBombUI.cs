
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombUI : MonoBehaviour
{
    [SerializeField] private Image _slot1;
    [SerializeField] private Image _slot2;
    
    private void Start()
    {
        PlayerMain.Instance.Collector.OnBomb += (i) =>
        {
            _slot1.color = new Color32(0, 0, 0, 0);
            _slot2.color = new Color32(0, 0, 0, 0);

            if (i >= 1) _slot1.color = new Color32(0, 0, 0, 255);
            if (i == 2) _slot2.color = new Color32(0, 0, 0, 255);
        };
    }
}
