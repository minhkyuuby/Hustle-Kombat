using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GroupCameraHandler : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void SetRadiusThenRestore(int id, float radius, float time)
    {
        float sRadius = targetGroup.Targets[id].Radius;
        targetGroup.Targets[id].Radius = radius;
        DOVirtual.DelayedCall(time, () =>
        {
            targetGroup.Targets[id].Radius = sRadius;
        });
    }
}
