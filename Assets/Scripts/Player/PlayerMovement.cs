
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 _movement;
    
    [SerializeField]
    private float _speed;
    
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        PlayerMain.Instance.Register(this);
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        PlayerMain.Instance.Input.OnMove += OnMove;
    }

    private void FixedUpdate()
    {
        if(_movement.magnitude < 0.1f)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        if (Mathf.Abs(_movement.x) >= Mathf.Abs(_movement.y))
        {
            _rigidbody.velocity = new Vector3(Mathf.Sign(_movement.x) * _speed, 0f, 0f);
        }
        else
        {
            _rigidbody.velocity = new Vector3(0f, 0f, Mathf.Sign(_movement.y) * _speed);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }
}
