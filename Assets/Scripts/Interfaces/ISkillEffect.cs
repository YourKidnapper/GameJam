using UnityEngine;
public interface ISkillEffect
{
    void Activate(GameObject user, GameObject target = null);
}