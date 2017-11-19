using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    [Header("Player Settings")]
    public int sens = 3;
    public float lookDamping = 2f;
    public float moveSpeed = 10f;
    public bool Grounded;
    public float groundRange = 1.01f;
    public float jumpHeight = 8f;
    public Rigidbody player;
    public bool playerMovement = true;
    public float attackDelay = 0.5f;
    public float delay;
    public float dashSpeed = 16f;
    public int maxHealth = 100;
    public int maxEnergy = 20;
    public int maxExp = 100;
    [Range(0.1f,1)]
    public float dashAngleModifier = 0.5f;
    public float dashDelay = 1.5f;
    public bool canDash = true;
    public float tapSpeed = 0.2f; //in seconds
    private float lastTapTime = 0;
    public int timesJumped = 0;
    public float jumpDelay = 0.3f;
    public float jumpTime = 0;
    public bool isJumping = false;
    public float energyCooldown = 2f;
    [Header("Interaction Settings")]
    public DialogueManager DM;
    public GameObject iText;
    public bool inRange = false;
    public Transform interactedEntity;
    public Text healthText;
    public Text energyText;
    public Text moneyText;
    public Text levelText;
    public Text expText;
    public GameObject LoadingScreen;

    // Use this for initialization
    public PlayerStatistics localPlayerData = new PlayerStatistics();
    void Start () {
        InitGO();

        Cursor.lockState = CursorLockMode.Locked;

        InitGC();

        LoadPlayer();

        InitWeap();

        iText.SetActive(false);

        LoadingScreen.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
        //QuickSaving
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayer();
        }
        //Player Stat Controlling
        //Level
        if (localPlayerData.currentExp >= localPlayerData.maxExp)
        {
            int lvlDiff = localPlayerData.currentExp - localPlayerData.maxExp;
            LevelUp();
            AddExp(lvlDiff);
        }
        //Energy
        if (localPlayerData.currentEnergy >= localPlayerData.maxEnergy)
        {
            localPlayerData.currentEnergy = localPlayerData.maxEnergy;
        }
        //Health
        if(localPlayerData.currentHealth >= localPlayerData.maxHealth)
        {
            localPlayerData.currentHealth = localPlayerData.maxHealth;
        }
        healthText.text = "Health: " + localPlayerData.currentHealth.ToString();
        energyText.text = "Energy: " + localPlayerData.currentEnergy.ToString();
        moneyText.text = "$" + localPlayerData.money.ToString();
        levelText.text = "lvl. " + localPlayerData.currentLevel;
        expText.text = localPlayerData.currentExp + "/" + localPlayerData.maxExp; 
        //Weapon Stuff
        delay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;
        if (localPlayerData.weaponChild != null)
        {
            localPlayerData.weaponAnim = localPlayerData.weaponChild.GetComponent<Animator>();
            if (Input.GetAxisRaw("Fire1") == 1 && delay <= 0 && playerMovement)
            {
                delay = attackDelay;
                localPlayerData.weaponAnim.SetBool("attack", true);
            }
            else
            {
                localPlayerData.weaponAnim.SetBool("attack", false);
            }
            if (Input.GetKeyDown(KeyCode.G) && localPlayerData.hasWeapon)
            {
                DropWeapon();
            }
        }
        //TALKING
        if (Input.GetKeyDown(KeyCode.E) && playerMovement == true && inRange)
        {
            iText.SetActive(false);
            interactedEntity.GetComponent<DialogueTrigger>().TriggerDialogue();
            playerMovement = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerMovement == false && inRange)
        {
            DM.DisplayNextSentence();
        }
        //Moving
        if (playerMovement)
        {
            Move();
        }

    }
    //==========Levels===================
    void LevelUp()
    {
        AddLevel(1);
        localPlayerData.currentExp = 0;
        localPlayerData.maxExp += Mathf.RoundToInt(0.2f * localPlayerData.maxExp);
        localPlayerData.currentEnergy = localPlayerData.maxEnergy;
        if (IsDivisble(localPlayerData.currentLevel, 5))
        {
            localPlayerData.maxHealth += Mathf.RoundToInt(0.15f * localPlayerData.maxHealth);
        }
        localPlayerData.currentHealth = localPlayerData.maxHealth;
    }
    public bool IsDivisble(int x, int n)
    {
        return (x % n) == 0;
    }
    public void AddExp(int Exp)
    {
        localPlayerData.currentExp += Exp;
    }
    public void AddLevel(int Level)
    {
        localPlayerData.currentLevel += Level;
    }
    //===================================
    //
    //==========SaveLoad=================
    public void LoadPlayer()
    {
        localPlayerData = GlobalControl.Instance.savedPlayerData;
    }
    public void SavePlayer()
    {
        GlobalControl.Instance.savedPlayerData = localPlayerData;
    }
    //====================================
    void DropWeapon()
    {
        localPlayerData.hasWeapon = false;
        Destroy(localPlayerData.weaponChild.gameObject);
        localPlayerData.weaponChild = null;
        localPlayerData.droppedWeaponObject.GetComponentInChildren<WeaponInteraction>().dropTime = 1.5f;
        localPlayerData.droppedWeaponObject.SetActive(true);
        localPlayerData.droppedWeaponObject.transform.parent = null;
        localPlayerData.droppedWeaponObject.GetComponent<Rigidbody>().AddForce(transform.forward);
        localPlayerData.droppedWeaponObject = null;
    }
    float time1;
    float time2;
    void FixedUpdate()
    {
        CheckForGround();
        //Dashing
        if (playerMovement)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                time1 = Time.time;
                if (((Time.time - lastTapTime) < tapSpeed) && ((time1 - time2) <= tapSpeed) && (dashDelay <= 0f) && canDash)
                {
                    Dash(Vector3.up * dashAngleModifier);
                }
                lastTapTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                time2 = Time.time;
            }
        }
        //Jumping
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
        }
        if (((timesJumped < 2) && (jumpTime >= jumpDelay)) && playerMovement && Input.GetAxisRaw("Jump") == 1)
        {
            Jump();
            isJumping = true;
        }
    }
    void Dash(Vector3 up)
    {
        isJumping = true;
        canDash = false;
        dashDelay = 1.5f;
        player = GetComponent<Rigidbody>();
        player.AddForce((transform.forward + up) * dashSpeed, ForceMode.VelocityChange);
    }
    void CheckForGround()
    {
        Vector3 rayGround = transform.position;
        RaycastHit groundHit;
        Vector3 down = transform.TransformDirection(Vector3.down);

        if (Physics.SphereCast(transform.position, 0.1f, down, out groundHit, 1.01f))
        {
            //do something if hit object ie
            if (!groundHit.collider.CompareTag("Interaction") && !groundHit.collider.CompareTag("Player"))
            {
                Grounded = true;
                canDash = true;
                isJumping = false;
                timesJumped = 0;
                jumpTime = 0.5f;
            }
        }
    }
    void Jump()
    {
        player.velocity = Vector3.zero;
        player.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        jumpTime = 0f;
        timesJumped++;
    }
    void Move()
    {
        float runSpeed = 1;
        var z = Input.GetAxisRaw("Vertical");
        var x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runSpeed = 1.5f;
        }
        else
        {
            runSpeed = 1f;
        }
        //Vector3 movement = new Vector3(x, 0.0f, z);
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f) * sens);
        //if (movement != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), lookDamping);
        //}
        Vector3 movement = transform.forward * z + transform.right * x;
        transform.Translate(movement.normalized * Time.deltaTime * moveSpeed * runSpeed, Space.World);
    }
    void InitWeap()
    {
        if (GlobalControl.Instance.savedPlayerData.hasWeapon)
        {
            GlobalControl.Instance.savedPlayerData.weaponChild = Resources.Load<Transform>(GlobalControl.Instance.savedPlayerData.weaponChildPath);
            GlobalControl.Instance.savedPlayerData.droppedWeaponObject = Resources.Load<GameObject>(GlobalControl.Instance.savedPlayerData.droppedWeaponObjectPath);
            Debug.Log(GlobalControl.Instance.savedPlayerData.droppedWeaponObjectPath);
            localPlayerData.droppedWeaponObject = Instantiate(Resources.Load<GameObject>(GlobalControl.Instance.savedPlayerData.droppedWeaponObjectPath), transform);
            localPlayerData.droppedWeaponObject.transform.localPosition = Vector3.zero;
            localPlayerData.droppedWeaponObject.SetActive(false);
            localPlayerData.weaponChild = Instantiate(Resources.Load<Transform>(GlobalControl.Instance.savedPlayerData.weaponChildPath), transform).transform;
        }
    }
    void InitGC()
    {
        delay = attackDelay;
        localPlayerData.maxHealth = maxHealth;
        localPlayerData.maxEnergy = maxEnergy;
        localPlayerData.maxExp = maxExp;
        GlobalControl.Instance.savedPlayerData.maxHealth = localPlayerData.maxHealth;
        GlobalControl.Instance.savedPlayerData.maxEnergy = localPlayerData.maxEnergy;
        GlobalControl.Instance.savedPlayerData.maxExp = localPlayerData.maxExp;
        if (GlobalControl.Instance.savedPlayerData.currentHealth == 0)
        {
            GlobalControl.Instance.savedPlayerData.currentHealth = localPlayerData.maxHealth;
        }
        if (GlobalControl.Instance.savedPlayerData.currentEnergy == 0)
        {
            GlobalControl.Instance.savedPlayerData.currentEnergy = localPlayerData.maxEnergy;
        }
        if (GlobalControl.Instance.savedPlayerData.maxExp == 0)
        {
            GlobalControl.Instance.savedPlayerData.maxExp = localPlayerData.maxExp;
        }
    }
    void InitGO()
    {
        iText = GameObject.Find("Canvas/iText");
        LoadingScreen = GameObject.Find("Canvas/LoadingScreen");
        healthText = GameObject.Find("Canvas/HealthText").GetComponent<Text>();
        energyText = GameObject.Find("Canvas/EnergyText").GetComponent<Text>();
        moneyText = GameObject.Find("Canvas/MoneyText").GetComponent<Text>();
        levelText = GameObject.Find("Canvas/LevelText").GetComponent<Text>();
        expText = GameObject.Find("Canvas/ExpText").GetComponent<Text>();
        player = GetComponent<Rigidbody>();
        DM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }
}
