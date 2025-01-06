using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] chase [Target] in between [min] and [max] seconds", category: "Action", id: "e59f988b4e1904b43080513578a51db4")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;

    BaseCharacterBehavior characterBehavior;
    float chaseTime;
    protected override Status OnStart()
    {
        if (Self.Value.IsKnocked)
            return Status.Failure;
        characterBehavior = Self.Value;
        chaseTime = Random.Range(Min, Max);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.IsKnocked)
            return Status.Failure;

        chaseTime -= Time.deltaTime;
        if (chaseTime <= 0f) return Status.Success;

        characterBehavior.SetMoveDirection(Vector3.left);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        characterBehavior.SetMoveDirection(Vector3.zero);
    }
}

