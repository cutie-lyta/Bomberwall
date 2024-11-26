using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class AINodeChooser : MonoBehaviour
{
    public static List<Node> Nodes = new();

    private List<Node> _activeList = new();
    private List<List<Node>> _inactiveList = new();

    private AIMovement _aiMove;

    private Node _currentCible;
    private Coroutine _currentRoutine;

    private CancellationToken _cancelToken;
    
    private void Awake()
    {
        _aiMove = GetComponent<AIMovement>();
    }

    public void CancelSequence()
    {
        _cancelToken = new CancellationToken();
        _cancelToken.ThrowIfCancellationRequested();
    }
    
    public Task CreateSequence(Node finalNode)
    {
        if(_currentRoutine != null) StopCoroutine(_currentRoutine);

        _activeList.Clear();
        _inactiveList.Clear();
            
        _activeList.Add(FindClosestNode(transform.position));
            
        StartCoroutine(_aiMove.MoveToPoint(_activeList[0]));

        _activeList[0].H = Vector3.Distance(finalNode.Position, _activeList[0].Position);
        _activeList[0].G = 0;
            
        _currentCible = finalNode;

        int i = 0;

        while (_activeList[^1] != _currentCible) {
            if (_cancelToken.IsCancellationRequested)
            {
                break;
            }
            
            AddClosestNeighbourToList();
            SelectActiveList();

            i++;
            if (i > 133)
            {
                print("Failed on : " + Nodes.IndexOf(_activeList[0]) + " to " + Nodes.IndexOf(finalNode));
                print("List is : " + String.Join(", ", _activeList.ConvertAll(n => Nodes.IndexOf(n).ToString())));
                break;
            }
        }
        
        _currentRoutine = StartCoroutine(_aiMove.ExecuteSequence(_activeList));
        return Task.CompletedTask;
    }
    
    public void SelectActiveList()
    {
        List<Node> minList = _activeList;
        foreach(List<Node> list in _inactiveList){
            if(list[^1].F < minList[^1].F){
                minList = list;
            }
        }

        if(minList != _activeList){
            _inactiveList.Add(_activeList);
            _inactiveList.Remove(minList);
            _activeList = minList;
        }
    }
    
    public void AddClosestNeighbourToList()
    {
        var node = _activeList[^1];
        
        List<Node> minNodes = new List<Node>() { node.Connected[0] };
            
        foreach(Node neighbour in node.Connected){
            if (_activeList.Contains(neighbour)) continue;
            
            neighbour.H = Vector3.Distance(_currentCible.Position, neighbour.Position);
            neighbour.G = _activeList.Count;
            
            if(minNodes[0].F > neighbour.F){
                minNodes.Clear();
                minNodes.Add(neighbour);
            } else if (Mathf.Approximately(minNodes[0].F, neighbour.F)){
                minNodes.Add(neighbour);
            }
        }

        for (int i = 1; i < minNodes.Count; i++)
        {
            var list = new List<Node>(_activeList);
            list.Add(minNodes[i]);
            _inactiveList.Add(list);
        }

        _activeList.Add(minNodes[0]);
    }
    
    public Node FindClosestNode(Vector3 position)
    {
        Node n = Nodes[0];

        foreach (Node node in Nodes)
        {
            if ((position - node.Position).magnitude < (position - n.Position).magnitude)
            {
                n = node;
            }
        }
        
        return n;
    }

    public Node FindFarthestNode(Vector3 position)
    {
        Node n = Nodes[0];

        foreach (Node node in Nodes)
        {
            if ((position - node.Position).magnitude > (position - n.Position).magnitude)
            {
                n = node;
            }
        }
        
        return n;
    }
}
