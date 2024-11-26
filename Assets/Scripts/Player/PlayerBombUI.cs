
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombUI : MonoBehaviour
{
    [SerializeField] private List<Image> _images;
    
    private void Start()
    {
        print(GetType().FullName + "::Start");
        PlayerMain.Instance.Collector.OnBomb += (bomb) =>
        {
            print(GetType().FullName + "::Start::OnBombHandler");
            int i = 0;
            for (; i < bomb; i++)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].color = new Color(0f, 0f, 0f, 1f);
            }

            for (; i < _images.Count; i++)
            {
                _images[i].gameObject.SetActive(false);
                _images[i].color = new Color(0f, 0f, 0f, 0f);
            }
        };
    }
}
