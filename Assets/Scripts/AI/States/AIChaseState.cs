using System.Threading.Tasks;
using UnityEngine;

public class AIChaseState : AIStateBase
{
    private Node _closestPlayerNode;
    private Coroutine _coroutine;

    public AIChaseState() {}
    public override AIStateBase Init(AIStateMachine context)
    {
        Debug.Log(GetType().FullName + "::Init");
        Context = context;
        return this;
    }

    public override void OnStateEnter()
    {
        _closestPlayerNode = null;
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
    }

    public override void OnStateFixedUpdate()
    {
        if (PlayerMain.Instance.Collector.NumOfBombs > 0)
        {
            Context.TransitionTo<AIFleeState>();
            return;
        }
        
        if (Vector3.Distance(Context.transform.position, PlayerMain.Instance.transform.position) < 2f)
        {
            Context.TransitionTo<AIPlacingState>();
            return;
        }
        
        var node = Context.NodeChooser.FindClosestNode(PlayerMain.Instance.transform.position);
        if(node == Context.NodeChooser.FindClosestNode(Context.NodeChooser.transform.position)) Context.TransitionTo<AIFleeState>();
        
        if (node == _closestPlayerNode) return;
        
        _closestPlayerNode = node;

        Context.NodeChooser.CancelSequence();
        Context.NodeChooser.CreateSequence(_closestPlayerNode);
    }
}