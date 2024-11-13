
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AINodeChooser _aiNodeChooser;
    
    public IEnumerator MoveToPoint(Node n)
    {
        if (n != null){
            Vector3 vec = n.Position - transform.position;
            vec.y = 0;
            
            var direction = vec.normalized;
            
            _rb.velocity = direction * _speed;
            while (new Vector3(n.Position.x - transform.position.x, 0, n.Position.z - transform.position.z).magnitude > 0.2f) yield return new WaitForFixedUpdate();

            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
    
    public IEnumerator ExecuteSequence(List<Node> node)
    {
        foreach (var n in node)
        {
            IEnumerator p = MoveToPoint(n);

            yield return p;
        }
    }
}
