using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryAttack", story: "[Self] Try Attack [min] to [max] attack combo", category: "Action", id: "6f90a095b3741bb68a00002375dfbbd7")]
public partial class TryAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> Self;
    [SerializeReference] public BlackboardVariable<int> Min;
    [SerializeReference] public BlackboardVariable<int> Max;

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

