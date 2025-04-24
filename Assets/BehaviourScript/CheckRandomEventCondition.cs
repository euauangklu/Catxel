using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckRandomEvent", story: "If [RandomEventManager] is [Bool]", category: "Conditions", id: "859175c7fada62c3d295a609fa7aa06b")]
public partial class CheckRandomEventCondition : Condition
{
    [SerializeReference] public BlackboardVariable<RandomEventManager> RandomEventManager;
    [Comparison(comparisonType: ComparisonType.Boolean)]
    [SerializeReference] public BlackboardVariable<bool> Bool;

    public override bool IsTrue()
    {
        return RandomEventManager.Value.EventRandom == Bool.Value;;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
