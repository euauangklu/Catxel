using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatPlay", story: "[Agent] Play With Random Object", category: "Action", id: "b0d4af24ff8e409a358d5cc9cc4c1437")]
public partial class CatPlayAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<List<GameObject>> ObjectList;
    
    [SerializeReference] public BlackboardVariable<float> WalkSpeed;
    
    [SerializeReference] public BlackboardVariable<float> PlayTime = new BlackboardVariable<float>(1.0f);
    
    [SerializeReference] public BlackboardVariable<string> AnimatorCatPlayParam = new BlackboardVariable<string>("CatPlay");
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");
    
    [SerializeReference] public BlackboardVariable<string> AnimatorPlayParam = new BlackboardVariable<string>("PlayMagnitude");
    
    [SerializeReference] public BlackboardVariable<float> StopDistance = new BlackboardVariable<float>(1.0f);

    private Animator Animator;
    
    private float PlayTimer;

    private Vector2 CurrentScale;
    
    private DragNDrop dragNDrop;

    private GameObject randomObject;

    private int randomIndex;

    public bool Playing;
    
    protected override Status OnStart()
    {
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        if (ObjectList != null && ObjectList.Value != null && ObjectList.Value.Count > 0)
        {
            randomIndex = Random.Range(0, ObjectList.Value.Count); 
            randomObject = ObjectList.Value[randomIndex];
            
            switch (randomIndex)
            {
                case 0:
                    Animator.SetFloat(AnimatorPlayParam,0);
                    break;

                case 1:
                    Animator.SetFloat(AnimatorPlayParam,1);
                    break;
            }
        }
        dragNDrop = Agent.Value.GetComponent<DragNDrop>();
        CurrentScale = Agent.Value.transform.localScale;
        if (Agent.Value.transform.position.x < randomObject.transform.position.x) //TurnRight
        {
            Agent.Value.transform.localScale = new Vector2(1, 1);
        }
        else if (Agent.Value.transform.position.x > randomObject.transform.position.x) //TurnLeft
        {
            Agent.Value.transform.localScale = new Vector2(-1, 1);
        }
        PlayTimer = PlayTime.Value;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        
        if (dragNDrop != null && dragNDrop.isDragging)
        {
            Animator.SetBool(AnimatorCatPlayParam,false);
            return Status.Success;
        }
        Animator.SetFloat(AnimatorSpeedParam,1);
        if (Vector2.Distance(Agent.Value.transform.position, randomObject.transform.position) > StopDistance)
        {
            Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position,randomObject.transform.position,WalkSpeed * 0.48f * Time.deltaTime);
        }
        if (Vector2.Distance(Agent.Value.transform.position, randomObject.transform.position) <= StopDistance)
        {
            Agent.Value.transform.localScale = new Vector2(-1, 1);
            Animator.SetBool(AnimatorCatPlayParam,true);
            Playing = true;
            PlayTimer -= Time.deltaTime;
        }

        if (PlayTimer <= 0f)
        {
            Animator.SetBool(AnimatorCatPlayParam,false);
            Playing = false ;
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Agent.Value.transform.localScale = CurrentScale;
    }
}

