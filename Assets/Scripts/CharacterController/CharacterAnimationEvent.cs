using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    [SerializeField] BaseCharacterBehavior behavior;

    public bool isLeft = true;

    public void ExecuteSkill()
    {
        Debug.Log("Call execute skill");
        behavior.ExecuteSkill(transform.position + new Vector3(isLeft? 1.1f : -1.1f, 1.25f, 0), transform.rotation);
    }

    public void ExecuteUltimate()
    {
        behavior.ExecuteUltimate(transform.position + new Vector3(0.5f, 0, 0), transform.rotation);
    }
}
