using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatSleep", story: "[Agent] Sleep", category: "Action", id: "8cb1e684f2ad5fb9f1a2b3fc929bff6c")]
public partial class SleepAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<float> SleepTime = new BlackboardVariable<float>(1.0f);
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSleepParam = new BlackboardVariable<string>("Sleep");

    private Animator Animator;
    
    private float SleepTimer;
    
    protected override Status OnStart()
    {
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        SleepTimer = SleepTime.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (SleepTimer > 0)
        {
            SleepTimer -= Time.deltaTime;
            Animator.SetBool(AnimatorSleepParam,true);
        }
        if (SleepTimer <= 0f)
        {
            Animator.SetBool(AnimatorSleepParam,false);
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Animator.SetBool(AnimatorSleepParam,false);
    }
}

