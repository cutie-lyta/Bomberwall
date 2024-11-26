
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class AIBombState : AIStateBase
{
    private List<GameObject> _bombs = new();

    private Node _currentBombNode = null;
    private Task _task;
    
    public AIBombState() {}
    
    public override AIStateBase Init(AIStateMachine context)
    {
        Debug.Log(GetType().FullName + "::Init");
        Context = context;

        _bombs = ObjectPoolManager.Instance.GetAllInstanceOf<BombCollector>()
            .ConvertAll((b)=>b.gameObject)
            .ToList();
        
        if(_bombs.Count(o => o.activeInHierarchy) < 1)
        {
            Context.TransitionTo<AIFleeState>();
        }
        
        return this;
    }

    public Node GetClosestBombNode()
    {
        Debug.Log(GetType().FullName + "::GetClosestBombNode");
        Node bombNode = null;
        var closestNode = Context.NodeChooser.FindClosestNode(Context.transform.position);
        foreach (GameObject g in _bombs)
        {
            if (g.activeInHierarchy != true) continue;
            var n = Context.NodeChooser.FindClosestNode(g.transform.position);
            
            if (bombNode == null || 
                Vector3.Distance(n.Position, closestNode.Position) < Vector3.Distance(bombNode.Position, closestNode.Position) )
            {
                bombNode = n;
            }
        }

        return bombNode;
    }

    public void GoToClosestBomb()
    {
        Debug.Log(GetType().FullName + "::GoToClosestBomb");
        Node bombNode = GetClosestBombNode();
        _currentBombNode = bombNode;
        if (bombNode == null)
        {
            return;
        }
    
        Context.NodeChooser.CancelSequence();
        Context.NodeChooser.CreateSequence(bombNode);
    }

    private void TransitionWhenBombTouched(int b)
    {
        Debug.Log(GetType().FullName + "::TransitionWhenBombTouched");
        Context.TransitionTo<AIChaseState>();
    }
    
    public override void OnStateEnter()
    {
        Debug.Log(GetType().FullName + "::OnStateEnter");
        Context.BombPlacer.OnNewBomb += TransitionWhenBombTouched;
        
        GoToClosestBomb();
    }

    public override void OnStateExit()
    {
        Debug.Log(GetType().FullName + "::OnStateExit");
        Context.BombPlacer.OnNewBomb -= TransitionWhenBombTouched;
    }

    public override void OnStateUpdate() { }

    public override void OnStateFixedUpdate()
    {
        if (Context.BombPlacer.BombCount > 4)
        {
            Context.TransitionTo<AIChaseState>();
        }
        
        Node bombNode = GetClosestBombNode();
        if (bombNode != _currentBombNode) { GoToClosestBomb(); }
    }
}
