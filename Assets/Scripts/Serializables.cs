using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public struct PlayerStatistics
{
    public int currentHealth;
    public int maxHealth;
    public bool hasWeapon;
    public Transform weaponChild;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public DialogueManager DM;
}