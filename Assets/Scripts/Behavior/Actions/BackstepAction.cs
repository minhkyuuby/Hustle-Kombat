using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Backstep", story: "[SelfBehavior] backstep in between [min] and [max] seconds", category: "Action", id: "e682821cabc377e5710c98790a87293f")]
public partial class BackstepAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> SelfBehavior;
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;

    float backTime;
    protected override Status OnStart()
    {
        if (SelfBehavior.Value.IsKnocked)
            return Status.Failure;

        backTime = Random.Range(Min, Max);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (SelfBehavior.Value.IsKnocked || SelfBehavior.Value.IsHit || SelfBehavior.Value.IsTired)
            return Status.Failure;

        backTime -= Time.deltaTime;
        if (backTime <= 0f) return Status.Success;

        SelfBehavior.Value.SetMoveDirection(Vector3.left);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        SelfBehavior.Value.SetMoveDirection(Vector3.zero);
    }
}

