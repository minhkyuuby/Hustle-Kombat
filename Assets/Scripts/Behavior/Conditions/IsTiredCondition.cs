using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsTired", story: "[Behavior] is Tired", category: "Conditions", id: "4b167a512e7754a2f92c980d5901df0f")]
public partial class IsTiredCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> Behavior;

    public override bool IsTrue()
    {
        return Behavior.Value.IsTired;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
