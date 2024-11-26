using System.Linq;
using UnityEngine;

public class AIPlacingState : AIStateBase
{
    public AIPlacingState() {}
    public override AIStateBase Init(AIStateMachine context)
    {
        Debug.Log(GetType().FullName + "::Init");
        Context = context;
        return this;
    }

    public override void OnStateEnter()
    {
        Debug.Log(GetType().FullName + "::OnStateEnter");
        Context.BombPlacer.OnBombPlaced += (i) =>
        {
            Debug.Log(GetType().FullName + "::OnStateEnter::OnBombPlaced");
            if (i > 0) Context.TransitionTo<AIChaseState>();
            else if(ObjectPoolManager.Instance.GetAllInstanceOf<BombCollector>().Count((b) => b.gameObject.activeInHierarchy) > 0) Context.TransitionTo<AIBombState>();
            else Context.TransitionTo<AIFleeState>();
        };
        
        Context.BombPlacer.PlaceBomb();
        Context.TransitionTo<AIBombState>();
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
    }

    public override void OnStateFixedUpdate()
    {
    }
}