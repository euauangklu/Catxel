using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AreaWalk", story: "[Agent] walk in area", category: "Action", id: "d01e312d663ee9eb535d81240cea06bc")]
public partial class AreaWalkAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<Transform> AreaTopLeft;

    [SerializeReference] public BlackboardVariable<Transform> AreaBottomRight;

    [SerializeReference] public BlackboardVariable<float> WalkSpeed;
    
    [SerializeReference] public BlackboardVariable<float> WaitTime = new BlackboardVariable<float>(1.0f);
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");
    
    private float WaitTimer;
    
    private bool Waiting;

    private Vector2 targetPosition;
    
    private Vector2 RecentPosition;

    private bool FacingRight;
    
    private Animator Animator;

    protected override Status OnStart()
    {
        PickNewTarget();
        RecentPosition = Agent.Value.transform.position;
        WaitTimer = WaitTime.Value;
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, targetPosition, WalkSpeed * 100f * Time.deltaTime);
        if (Vector2.Distance(Agent.Value.transform.position, targetPosition) < 0.1f)
        {
            Waiting = true;
        }

        if (Waiting)
        {
            Animator.SetFloat(AnimatorSpeedParam,0);
            if (WaitTimer > 0f)
            {
                WaitTimer -= Time.deltaTime;
            }
            else
            {
                Waiting = false;
                return Status.Success;
            }
        }
        else if (!Waiting)
        {
            Animator.SetFloat(AnimatorSpeedParam,1);
            if (RecentPosition.x > targetPosition.x && !FacingRight)
            {
                Flip();
            }
            if (RecentPosition.x < targetPosition.x && FacingRight)
            {
                Flip();
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
    
    private void PickNewTarget()
    {
        float x = Random.Range(AreaTopLeft.Value.position.x, AreaBottomRight.Value.position.x);
        float y = Random.Range(AreaBottomRight.Value.position.y, AreaTopLeft.Value.position.y);
        targetPosition = new Vector2(x, y);
    }

    private void Flip()
    {
        Vector2 CurrentScale = Agent.Value.transform.localScale;
        CurrentScale.x *= -1;
        Agent.Value.transform.localScale = CurrentScale;
        FacingRight = !FacingRight;
        
        Debug.Log("Flip");
    }
}

