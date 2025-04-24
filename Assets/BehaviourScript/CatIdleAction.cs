using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatIdle", story: "[Agent] Idle", category: "Action", id: "51c72216ae69c04d8d3f20f3df7e8fca")]
public partial class CatIdleAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<float> WaitTime = new BlackboardVariable<float>(1.0f);
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");
    
    private float WaitTimer;
    
    private Animator Animator;

    protected override Status OnStart()
    {
        WaitTimer = WaitTime.Value;
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Animator.SetFloat(AnimatorSpeedParam,0);
        WaitTimer -= Time.deltaTime;
        if (WaitTimer <= 0f)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

