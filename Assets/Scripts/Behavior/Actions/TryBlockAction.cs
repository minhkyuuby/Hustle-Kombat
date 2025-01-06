using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryBlock", story: "[Self] try block for [duration] seconds", category: "Action", id: "c248f223ff53620902d2a9ee6a4199bc")]
public partial class TryBlockAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> Self;
    [SerializeReference] public BlackboardVariable<float> Duration;

    float guardTime;

    protected override Status OnStart()
    {
        if (Self.Value.IsKnocked)
            return Status.Failure;

        Self.Value.PerformGuard();
        guardTime = Duration;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        guardTime -= Time.deltaTime;
        if (guardTime < 0f)
        {
            return Status.Success;
        }
        if(Self.Value.IsKnocked)
            return Status.Failure;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Self.Value.GuardCancel();
    }
}

