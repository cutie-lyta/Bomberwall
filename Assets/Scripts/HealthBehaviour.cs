
using System;
using UnityEditor;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public event Action OnHealthChange;
    [SerializeField] private int _maxHealth;
    public int Health { get; private set; }

    private void Awake()
    {
        Health = _maxHealth;
    }

    public void Exploded()
    {
        Health--;
        OnHealthChange?.Invoke();
        if (Health <= 0)
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
        }
    }
}
