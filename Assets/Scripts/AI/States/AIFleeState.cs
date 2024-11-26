using System.Linq;
using UnityEngine;

public class AIFleeState : AIStateBase
{
    private Node _farthestPlayerNode;
    private float _timer;
    
    public AIFleeState(){}
    public override AIStateBase Init(AIStateMachine context)
    {
        Debug.Log(GetType().FullName + "::Init");
        Context = context;
        return this;
    }

    public override void OnStateEnter()
    {
        _farthestPlayerNode = null;
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
        _timer += Time.deltaTime;
    }

    public override void OnStateFixedUpdate()
    {
        if (Vector3.Distance(Context.transform.position, PlayerMain.Instance.transform.position) < 2f && Context.BombPlacer.BombCount > 0)
        {
            Context.TransitionTo<AIPlacingState>();
            return;
        }
        
        if (Vector3.Distance(Context.transform.position, PlayerMain.Instance.transform.position) > 20f || _timer == 20f)
        {
            if (ObjectPoolManager.Instance.GetAllInstanceOf<BombCollector>()
                    .Count((g) => g.gameObject.activeInHierarchy) > 0)
            {
                Context.TransitionTo<AIBombState>();
            }
            return;
        }

        var node = Context.NodeChooser.FindFarthestNode(PlayerMain.Instance.transform.position);
        if (node == _farthestPlayerNode) return;
        
        _farthestPlayerNode = node;
        
        Context.NodeChooser.CancelSequence();
        Context.NodeChooser.CreateSequence(_farthestPlayerNode);
    }
}