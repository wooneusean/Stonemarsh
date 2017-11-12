using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerStatistics
{
    public int currentHealth;
    public int maxHealth;
    public bool hasWeapon;
    public Collider weapon;
    public Transform weaponChild;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public DialogueManager DM;
}