using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "QuickStep", story: "[SelfBehavior] QuickStep to [rightDirection]", category: "Action", id: "5d152cb686ef279fa15cd37411373930")]
public partial class QuickStepAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> SelfBehavior;
    [SerializeReference] public BlackboardVariable<bool> RightDirection;
    protected override Status OnStart()
    {
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

