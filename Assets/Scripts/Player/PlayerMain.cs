
using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(InputHandler))]
public class PlayerMain : MonoBehaviour
{
    public static PlayerMain Instance;

    public PlayerMovement Movement { get; private set; }
    public InputHandler Input { get; private set; }
    public PlayerBombCollector Collector { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Register a component into the right field of the class
    /// </summary>
    /// <param name="component"> The component you want to register </param>
    /// <typeparam name="T"> The type of the component itself </typeparam>
    public void Register<T>(T component) where T : MonoBehaviour
    {
        // Foreach field in this type (PlayerMain)
        // that are non-static (BindingFlags.Instance)
        // Public or not (BindingFlags.Public | BindingFlags.NonPublic)
        foreach(var field in GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
        {
            // If the type of the field is the same as the type of T, set the corresponding field to the component
            if (field.FieldType == typeof(T))
            {
                field.SetValue(this, component);
            }
        }
        // If no field were found, the component isn't supposed to go here and so isn't registered.
    }
}