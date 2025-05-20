using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatJump", story: "[Agent] Jump [Start] to [Mid] to [End]", category: "Action", id: "a86af8eea5772c828b87405b432b6c53")]
public partial class CatJumpAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<Transform> Start;
    
    [SerializeReference] public BlackboardVariable<Transform> Mid;
    
    [SerializeReference] public BlackboardVariable<Transform> End;
    
    [SerializeReference] public BlackboardVariable<float> SitTime;
    
    [SerializeReference] public BlackboardVariable<float> WalkSpeed;
    
    [SerializeReference] public BlackboardVariable<string> AnimatorJumpParam = new BlackboardVariable<string>("Jump");
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    private Animator Animator;

    private bool JumpReady;
    
    private Vector2 CurrentScale;

    private float SitTimer;

    private bool DoneSit;
    
    private DragNDrop dragNDrop;
    protected override Status OnStart()
    {
        dragNDrop = Agent.Value.GetComponent<DragNDrop>();
        CurrentScale = Agent.Value.transform.localScale;
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        SitTimer = SitTime.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (dragNDrop != null && dragNDrop.isDragging)
        {
            return Status.Success;
        }
        
        // Walk To StartPoint
        if (Vector2.Distance(Agent.Value.transform.position, Start.Value.transform.position) > 0.1f && !JumpReady)
        {
            if (Agent.Value.transform.position.x < Start.Value.transform.position.x) //TurnRight
            {
                Agent.Value.transform.localScale = new Vector2(1, 1);
            }
            else if (Agent.Value.transform.position.x > Start.Value.transform.position.x) //TurnLeft
            {
                Agent.Value.transform.localScale = new Vector2(-1, 1);
            }
            Animator.SetFloat(AnimatorSpeedParam,1);
            Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, Start.Value.transform.position, WalkSpeed * 0.48f * Time.deltaTime);
        }
        // Check if already at StartPoint
        if (Vector2.Distance(Agent.Value.transform.position, Start.Value.transform.position) < 0.1f)
        {
            JumpReady = true;
            Animator.SetBool(AnimatorJumpParam,true);
            Animator.SetFloat(AnimatorSpeedParam,0);
        }
        // Start Jump to MidPoint
        if (JumpReady && !DoneSit)
        {
            Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position,Mid.Value.transform.position,WalkSpeed * 1f * Time.deltaTime);
        }
        // Play Sit Animation
        if (Vector2.Distance(Agent.Value.transform.position, Mid.Value.transform.position) < 0.1f && !DoneSit)
        {
            Animator.SetBool(AnimatorJumpParam,false);
            SitTimer -= Time.deltaTime;
            if (SitTimer <= 0f)
            {
                DoneSit = true;
            }
        }
        // Jump To EndPoint
        if (DoneSit)
        {
            if (Agent.Value.transform.position.x < End.Value.transform.position.x) //TurnRight
            {
                Agent.Value.transform.localScale = new Vector2(1, 1);
            }
            else if (Agent.Value.transform.position.x > End.Value.transform.position.x) //TurnLeft
            {
                Agent.Value.transform.localScale = new Vector2(-1, 1);
            }
            Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position,End.Value.transform.position,WalkSpeed * 1f * Time.deltaTime);
            Animator.SetBool(AnimatorJumpParam,true);
        }
        // Check if already at EndPoint
        if (Vector2.Distance(Agent.Value.transform.position, End.Value.transform.position) < 0.1f)
        {
            Animator.SetBool(AnimatorJumpParam,false);
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Agent.Value.transform.localScale = CurrentScale;
        JumpReady = false;
        DoneSit = false;
    }
}

