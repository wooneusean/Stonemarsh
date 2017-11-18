using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public struct PlayerStatistics
{
    public int currentLevel;
    public int currentHealth;
    public int maxHealth;
    public int currentEnergy;
    public int maxEnergy;
    public int money;
    public bool hasWeapon;
    public Transform weaponChild;
    public string weaponChildPath;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public string droppedWeaponObjectPath;
}
