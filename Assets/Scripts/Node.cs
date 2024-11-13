using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    [field: SerializeField]
    public Vector3 Position { get; set; }
    
    [field: SerializeReference]
    public List<Node> Connected { get; set; }
    
    public float H { get; set; }
    public float G { get; set; }
    public float F => G + H;

    public Node(Vector3 vec, List<Node> n)
    {
        Position = vec;
        Connected = n;
    }
}