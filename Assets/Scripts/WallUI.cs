using TMPro;
using UnityEngine;

public class WallUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    
    private WallBehaviour _wall;

    private void Start()
    {
        _wall = GetComponent<WallBehaviour>();
        _wall.OnHealthChange += OnHealthChange;
        _text.text = _wall.Health.ToString();
    }

    private void OnHealthChange()
    {
        _text.text = _wall.Health.ToString();
    }
}
