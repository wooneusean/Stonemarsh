using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public struct PlayerStatistics
{
    public int currentLevel;
    public int currentExp;
    public int maxExp;
    public int currentHealth;
    public int maxHealth;
    public float currentEnergy;
    public float maxEnergy;
    public int money;
    public float moveSpeed;
    public float critChance;
    public float critMultiplier;
    public string playerClass;
    public int playerStr;
    public int playerInt;
    public int playerDex;
    public int unusedSkillPoints;

    public float attackSpeed;
    public bool hasWeapon;
    public Transform weaponChild;
    public string weaponChildPath;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public string droppedWeaponObjectPath;
}
