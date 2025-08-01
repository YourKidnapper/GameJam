using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public List<SkillData> ownedSkills = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSkill(SkillData skill)
    {
        ownedSkills.Add(skill);
    }
}
