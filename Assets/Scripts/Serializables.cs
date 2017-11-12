using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerStatistics
{
    public int sens;
    public float lookDamping;
    public float moveSpeed;
    public bool Grounded;
    public float groundRange;
    public float jumpHeight;
    public Rigidbody player;
    public bool playerMovement;
    public float attackDelay;
    public float delay;
    public float dashSpeed;
    public float dashAngleModifier;
    public float dashDelay;
    public bool canDash;
    public float tapSpeed; //in seconds
    private float lastTapTime;
    public int timesJumped;
    public float jumpDelay;
    public float jumpTime;
    public bool isJumping = false;
    public int currentHealth;
    public int maxHealth;
    public bool hasWeapon;
    public Collider weapon;
    public Transform weaponChild;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public GameObject iText;
    public DialogueManager DM;
    public bool inRange;
    public Transform interactedEntity;
    public Text healthText;
    public static GlobalControl Instance;
}