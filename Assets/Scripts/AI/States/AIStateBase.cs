using UnityEngine;

public abstract class AIStateBase
{
    public AIStateMachine Context { get; set; }

    public AIStateBase() {}
    public abstract AIStateBase Init(AIStateMachine context);
    
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
}
