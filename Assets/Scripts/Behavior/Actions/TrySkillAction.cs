using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TrySkill", story: "[SelfBehavior] try to use skill", category: "Action", id: "03ab493b76135642444423e6de8517c0")]
public partial class TrySkillAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> SelfBehavior;

    protected override Status OnStart()
    {
        if(SelfBehavior.Value.isUsingSkill)
            return Status.Failure;

        SelfBehavior.Value.PerformSkill();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(SelfBehavior.Value.isUsingSkill)
            return Status.Running;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

