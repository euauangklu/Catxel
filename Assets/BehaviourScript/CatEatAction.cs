using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CatEat", story: "[Agent] Go to Eat [Food]", category: "Action", id: "93ac4eff90fadd3986f3ba7dd0f18992")]
public partial class CatEatAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<float> WalkSpeed;
    
    [SerializeReference] public BlackboardVariable<GameObject> Food;
    
    [SerializeReference] public BlackboardVariable<float> EatingTimer;
    
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    private float Timer;
    
    private Animator Animator;

    private bool Eating;

    private bool DoneRandom;

    private int RandomNum;
    
    private Vector2 CurrentScale;

    protected override Status OnStart()
    {
        CurrentScale = Agent.Value.transform.localScale;
        Animator = Agent.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        CatFoodScript catFoodScript = Food.Value.GetComponent<CatFoodScript>();
        if (Agent.Value.transform.position.x < Food.Value.transform.position.x) //TurnRight
        {
            Agent.Value.transform.localScale = new Vector2(1, 1);
        }
        else if (Agent.Value.transform.position.x > Food.Value.transform.position.x) //TurnLeft
        {
            Agent.Value.transform.localScale = new Vector2(-1, 1);
        }
        if (catFoodScript.CatFoodNum > 0)
        {
            if (!DoneRandom)
            {
                RandomNum = Random.Range(0, 2);
                DoneRandom = true;
            }

            if (RandomNum == 0)
            {
                return Status.Success;
            }

            if (RandomNum == 1)
            {
                Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, Food.Value.transform.position, WalkSpeed * 0.48f * Time.deltaTime);
                Animator.SetFloat(AnimatorSpeedParam,1);
            }
        }
        if (Vector2.Distance(Agent.Value.transform.position, Food.Value.transform.position) < 0.1f)
        {
            if (!Eating && catFoodScript.CatFoodNum > 0)
            {
                Animator.SetFloat(AnimatorSpeedParam,0);
                catFoodScript.CatFoodNum -= 1;
                Eating = true;
            }

            if (Eating)
            { 
                Timer += Time.deltaTime;
                if (Timer < EatingTimer)
                {
                    Animator.SetBool("Eat",true);
                }
                else if (Timer >= EatingTimer)
                {
                    Animator.SetBool("Eat",false);
                    return Status.Success;
                }
            }
        }

        if (catFoodScript.CatFoodNum <= 0 && !Eating)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Timer = 0;
        Eating = false;
        DoneRandom = false;
        Agent.Value.transform.localScale = CurrentScale;
    }
}

