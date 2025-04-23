using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeState", story: "Change to [ThisState]", category: "Action", id: "f18a28134cecf3d001527e517f7fa67b")]
public partial class ChangeStateAction : Action
{
    [SerializeReference] public BlackboardVariable<CatState> ThisState;
    
    [SerializeReference] public BlackboardVariable<CatState> CurrentState;
    
    protected override Status OnStart()
    {
        CurrentState.Value = ThisState.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

