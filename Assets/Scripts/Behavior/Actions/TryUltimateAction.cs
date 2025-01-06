using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryUltimate", story: "[SelfBehavior] try to use ultimate", category: "Action", id: "3474a040889af8a93162070bc5a3d3d3")]
public partial class TryUltimateAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> SelfBehavior;

    protected override Status OnStart()
    {
        if (SelfBehavior.Value.isUsingSkill)
            return Status.Failure;

        SelfBehavior.Value.PerformUltimate();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (SelfBehavior.Value.isUsingSkill)
            return Status.Running;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

