
using System;
using System.Collections.Generic;
using UnityEngine;

public class AINodeChooser : MonoBehaviour
{
    public static List<Node> Nodes = new();

    private List<Node> _activeList = new();
    private List<List<Node>> _inactiveList = new();

    private AIMovement _aiMove;

    [SerializeField] private GameObject _wallToDestroy;
    [SerializeField] private List<GameObject> _bombs;

    private Node _currentCible;

    private Coroutine _currentRoutine;

    private Node _wallNode;

    private GameObject _currentBomb;
    
    private bool _isBombSearching;
    private bool _isGoingToWall = true;
    
    private void Awake()
    {
        _aiMove = GetComponent<AIMovement>();
    }

    private void Start()
    {
        Vector3 pos = _wallToDestroy.transform.position;
        pos.z -= 2f;
            
        _wallNode = FindClosestNode(pos);
    }

    public void CreateSequence(Node finalNode)
    {
        if(_currentRoutine != null) StopCoroutine(_currentRoutine);

        _activeList.Clear();
        _inactiveList.Clear();
            
        _activeList.Add(FindClosestNode(transform.position));
            
        StartCoroutine(_aiMove.MoveToPoint(_activeList[0]));

        _activeList[0].H = Vector3.Distance(finalNode.Position, _activeList[0].Position);
        _activeList[0].G = 0;
            
        _currentCible = finalNode;
            
        while (_activeList[^1] != finalNode) {
            AddClosestNeighbourToList();
            SelectActiveList();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BombContainer"))
        {
            _isGoingToWall = true;
            Vector3 pos = _wallToDestroy.transform.position;
            pos.z -= 2f;
            
            Node wallNode = FindClosestNode(pos);

            _currentBomb = null;
            CreateSequence(wallNode);
            _currentRoutine = StartCoroutine(_aiMove.ExecuteSequence(_activeList));
        }

        if (other.CompareTag("Respawn") && _isGoingToWall)
        {
            _isGoingToWall = false;
            
            Node bombNode = null;
            foreach (GameObject g in _bombs)
            {
                if (g.activeInHierarchy != true) continue;
                var n = FindClosestNode(g.transform.position);
                
                if (bombNode == null || 
                    Vector3.Distance(n.Position, FindClosestNode(transform.position).Position) < Vector3.Distance(bombNode.Position, FindClosestNode(transform.position).Position) )
                {
                    bombNode = n;
                    _currentBomb = g;
                }
            }
            
            if (bombNode == null)
            {
                _isBombSearching = true;
                return;
            }
    
            CreateSequence(bombNode);
            _currentRoutine = StartCoroutine(_aiMove.ExecuteSequence(_activeList));
        }
    }

    private void FixedUpdate()
    {
        if (_isBombSearching)
        {
            _isBombSearching = false;
            Node bombNode = null;
            foreach (GameObject g in _bombs)
            {
                if (g.activeInHierarchy != true) continue;
                var n = FindClosestNode(g.transform.position);
                
                if (bombNode == null || 
                    Vector3.Distance(n.Position, FindClosestNode(transform.position).Position) < Vector3.Distance(bombNode.Position, FindClosestNode(transform.position).Position) )
                {
                    bombNode = n;
                    _currentBomb = g;
                }
            }
            
            if (bombNode == null)
            {
                _isBombSearching = true;
                return;
            }
    
            CreateSequence(bombNode);
            _currentRoutine = StartCoroutine(_aiMove.ExecuteSequence(_activeList));
        }

        if (_currentBomb && !_currentBomb.activeInHierarchy)
        {
            _currentBomb = null;
            _isBombSearching = true;
        }
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
    
    private Node FindClosestNode(Vector3 position)
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
}
