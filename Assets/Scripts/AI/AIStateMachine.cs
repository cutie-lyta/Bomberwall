using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class AIStateMachine : SerializedMonoBehaviour
{
    public AINodeChooser NodeChooser { get; private set; }
    public AIBombPlacer BombPlacer { get; private set; }
    private Dictionary<Type, AIStateBase> _states = new();

    public AIStateBase CurrentState { get; private set; }
    
    [OdinSerialize]
    private Type _defaultState;
    
    void Awake() 
    {
        print(GetType().FullName + "::Awake");
        NodeChooser = GetComponent<AINodeChooser>();
        BombPlacer = GetComponent<AIBombPlacer>();

        var listOfStates = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(AIStateBase).IsAssignableFrom(type)
        );

        foreach (var state in listOfStates)
        {
            if(state.IsAbstract) continue;
            print(state.FullName);
            
            _states.Add(state, (AIStateBase)Activator.CreateInstance(state));
            _states[state].Init(this);
        }
        
        CurrentState = _states[_defaultState];
        CurrentState?.OnStateEnter();
    }

    public void TransitionTo<T>() where T : AIStateBase
    {
        print(GetType().FullName + "::TransitionTo<" + typeof(T).FullName + ">");
        CurrentState?.OnStateExit();
        
        CurrentState = _states[typeof(T)];
        
        CurrentState?.OnStateEnter();
    }
    
    // Update is called once per frame
    void Update()
    {
        CurrentState?.OnStateUpdate();
    }

    void FixedUpdate()
    {
        CurrentState?.OnStateFixedUpdate();
    }
}
