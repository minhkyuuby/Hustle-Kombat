using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryAttack", story: "[Self] Try Attack [min] to [max] attack combo", category: "Action", id: "6f90a095b3741bb68a00002375dfbbd7")]
public partial class TryAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<BaseCharacterBehavior> Self;
    [SerializeReference] public BlackboardVariable<int> Min;
    [SerializeReference] public BlackboardVariable<int> Max;

    int comboCount;
    double lastAttackTime;

    protected override Status OnStart()
    {
        comboCount = Random.Range(Min, Max + 1);
        if (Random.Range(0, 2) == 0)
            Self.Value.PerformPunchAttack();
        else
            Self.Value.PerformKickAttack();


        lastAttackTime = Time.time;
        comboCount--;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.IsKnocked)
            return Status.Failure;

        if(!Self.Value.IsAttacking && lastAttackTime + 0.4f < Time.time)
        {
            if(Random.Range(0, 2) == 0)
                Self.Value.PerformPunchAttack();
            else
                Self.Value.PerformKickAttack();

            lastAttackTime = Time.time;
            comboCount--;
        }
        if(comboCount <= 0) return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

