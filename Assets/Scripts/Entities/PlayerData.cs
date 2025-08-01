using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public List<SkillData> ownedSkills = new List<SkillData>();

    public int coins = 0;

    public event Action<int> OnCoinsChanged;

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
        if (!ownedSkills.Contains(skill))
        {
            ownedSkills.Add(skill);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"💰 Гравець отримав {amount} монет. Всього монет: {coins}");
        OnCoinsChanged?.Invoke(coins);
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke(coins);
            return true;
        }
        return false;
    }
}
