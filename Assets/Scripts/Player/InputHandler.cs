using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Controller
{
    KEYBOARD,
    XBOX,
    PLAYSTATION,
    SWITCH,
    GENERIC
}

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public delegate void InputAction(UnityEngine.InputSystem.InputAction.CallbackContext context); 
    
    public event InputAction OnMove;
    public event InputAction OnBomb;
    
    private PlayerInput _input;
    
    public Controller CurrentController { get; private set; }

    private void Awake()
    {
        PlayerMain.Instance.Register(this);

        _input = GetComponent<PlayerInput>();
        _input.onControlsChanged += OnControlsChanged;
        _input.onActionTriggered += OnInput;
    }

    private void OnControlsChanged(PlayerInput obj)
    {
        switch (obj.currentControlScheme)
        {
            case "Generic":
                CurrentController = Controller.GENERIC; break;

            case "Keyboard":
                CurrentController = Controller.KEYBOARD; break;

            case "PlayStation":
                CurrentController = Controller.PLAYSTATION; break;

            case "Switch":
                CurrentController = Controller.SWITCH; break;

            case "XBox":
                CurrentController = Controller.XBOX; break;

            default:
                CurrentController = Controller.GENERIC; break;
        }
    }

    private void OnInput(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        switch (value.action.name)
        {
            case "Move":
                OnMove?.Invoke(value);
                break;
            case "Bomb":
                OnBomb?.Invoke(value);
                break;
        }
    }

    public void EmptyEvents()
    {
    }

    public void ChangeActionMap(string name)
    {
        _input.SwitchCurrentActionMap(name);
    }
}
