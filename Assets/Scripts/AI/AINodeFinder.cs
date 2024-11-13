using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AINodeFinder : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _upleftCorner;
    
    [SerializeField]
    private Vector2Int _downRightCorner;
    
    [SerializeField]
    private float _y;

    [SerializeField] private int _spacing;
    
    // Start is called before the first frame update
    unsafe void Awake()
    {
        for (int x = _upleftCorner.x; x <= _downRightCorner.x; x += _spacing)
        {
            for (int z = _upleftCorner.y; z <= _downRightCorner.y; z += _spacing)
            {
                Debug.DrawRay(new Vector3(x, _y,  z), Vector3.up*2f, new Color(x, z, (x+z)/2f));
                RaycastHit[] hits = new RaycastHit[255];
                var hitsCount = Physics.RaycastNonAlloc(new Vector3(x, _y, z), Vector3.up*6, hits, Mathf.Infinity);
                bool flag = false;

                if(hitsCount > 0)
                {
                    for (int i = 0; i < hitsCount; i++)
                    {
                        if (hits[i].transform.CompareTag("Wall") || hits[i].transform.CompareTag("BreakableWall"))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if(!flag){
                    Node node = new(new Vector3(x, _y, z), new List<Node>());
                    
                    var west = AINodeChooser.Nodes.FindIndex(n =>
                        Mathf.Approximately(node.Position.x + _spacing, n.Position.x)
                        && Mathf.Approximately(node.Position.z, n.Position.z));
                    
                    if(west != -1){
                        if(!AINodeChooser.Nodes[west].Connected.Contains(node)) AINodeChooser.Nodes[west].Connected.Add(node);
                        node.Connected.Add(AINodeChooser.Nodes[west]);
                    }
                    
                    var east = AINodeChooser.Nodes.FindIndex(n =>
                        Mathf.Approximately(node.Position.x - _spacing, n.Position.x)
                        && Mathf.Approximately(node.Position.z, n.Position.z));
                    
                    if(east != -1){
                        if(!AINodeChooser.Nodes[east].Connected.Contains(node)) AINodeChooser.Nodes[east].Connected.Add(node);
                        node.Connected.Add(AINodeChooser.Nodes[east]);
                    }
                    
                    var south = AINodeChooser.Nodes.FindIndex(n =>
                        Mathf.Approximately(node.Position.x, n.Position.x)
                        && Mathf.Approximately(node.Position.z - _spacing, n.Position.z));
                    
                    if(south != -1){
                        if(!AINodeChooser.Nodes[south].Connected.Contains(node)) AINodeChooser.Nodes[south].Connected.Add(node);
                        node.Connected.Add(AINodeChooser.Nodes[south]);
                    }
                    
                    var north = AINodeChooser.Nodes.FindIndex(n =>
                        Mathf.Approximately(node.Position.x, n.Position.x)
                        && Mathf.Approximately(node.Position.z + _spacing, n.Position.z));
                    
                    if(north != -1){
                        if(!AINodeChooser.Nodes[north].Connected.Contains(node)) AINodeChooser.Nodes[north].Connected.Add(node);
                        node.Connected.Add((AINodeChooser.Nodes[north]));
                    }
                    
                    AINodeChooser.Nodes.Add(node);
                }

            }
        }
    }
}
