using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Transform camera;
    #region Variables
    [Header("Player Settings")]
    public PlayerSkills skillSlot1;
    public PlayerSkills skillSlot2;
    public PlayerSkills skillSlot3;

    public int sens = 3;
    public float lookDamping = 2f;
    public bool Grounded;
    public float groundRange = 1.01f;
    public float jumpHeight = 8f;
    public Rigidbody player;
    public bool playerMovement = true;
    public bool turnWithMove = false;
    public float attackDelay = 0.5f;
    public float delay;
    public float dashSpeed = 16f;
    public int maxHealth = 100;
    public float maxEnergy = 20;
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
    public Text classText;
    public Text intText;
    public Text strText;
    public Text dexText;
    public GameObject statMenu;
    public GameObject LoadingScreen;

    #endregion
    // Use this for initialization
    public PlayerStatistics localPlayerData = new PlayerStatistics();
    void Start () {
        InitUI();

        Cursor.lockState = CursorLockMode.Locked;
        LoadPlayer();

        if (localPlayerData.playerClass == "" || localPlayerData.playerClass == "Default")
        {
            GetClassData(0);
        }
        
        InitWeap();
        
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowMenu("Stats");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            GetClassData(1);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            GetClassData(2);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            GetClassData(3);
        }
        //QuickSaving
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadPlayer();
        }
        //Skills
        PlayerSkills.SkillProperties sp;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sp = skillSlot1.skillProperties;
            if ((sp.manacost <= localPlayerData.currentEnergy) && (sp.currentCooldown <= 0))
            {
                skillSlot1.CastAbility();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sp = skillSlot2.skillProperties;
            if ((sp.manacost <= localPlayerData.currentEnergy) && (sp.currentCooldown <= 0))
            {
                skillSlot2.CastAbility();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            sp = skillSlot3.skillProperties;
            if ((sp.manacost <= localPlayerData.currentEnergy) && (sp.currentCooldown <= 0))
            {
                skillSlot3.CastAbility();
            }
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
        UpdateUI();
        //Weapon Stuff
        delay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;
        if (localPlayerData.weaponChild != null)
        {
            localPlayerData.weaponAnim = localPlayerData.weaponChild.GetComponent<Animator>();
            if (Input.GetAxisRaw("Fire1") == 1 && delay <= 0.00f && playerMovement)
            {
                delay = localPlayerData.attackSpeed;
                localPlayerData.weaponAnim.SetBool("attack", true);
                localPlayerData.weaponAnim.speed *= 1 + localPlayerData.attackSpeed;
            }
            else
            {
                localPlayerData.weaponAnim.SetBool("attack", false);
                localPlayerData.weaponAnim.speed = 1;
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
    void UpdateUI()
    {
        healthText.text = "Health: " + localPlayerData.currentHealth.ToString();
        energyText.text = "Energy: " + localPlayerData.currentEnergy.ToString();
        moneyText.text = "$" + localPlayerData.money.ToString();
        levelText.text = "lvl. " + localPlayerData.currentLevel;
        expText.text = localPlayerData.currentExp + "/" + localPlayerData.maxExp;
        classText.text = localPlayerData.playerClass + " CLASS";
        intText.text = localPlayerData.playerInt + " INT";
        dexText.text = localPlayerData.playerDex + " DEX";
        strText.text = localPlayerData.playerStr + " STR";
    }
    void GetClassData(int classID)
    {
        if (classID == 0)
        {
            localPlayerData.playerClass = "NO";
            localPlayerData.maxHealth = 100;
            localPlayerData.maxEnergy = 20;
            localPlayerData.currentHealth = 100;
            localPlayerData.currentEnergy = 20;
            localPlayerData.maxExp = 100;
            localPlayerData.moveSpeed = 10f;
            localPlayerData.playerDex = 5;
            localPlayerData.playerInt = 5;
            localPlayerData.playerStr = 5;
            localPlayerData.critChance = 0;
            localPlayerData.critMultiplier = 1;
            localPlayerData.attackSpeed = 1f;
            delay = localPlayerData.attackSpeed;
        }
        if (classID == 1)
        {
            localPlayerData.playerClass = "MAGE";
            localPlayerData.maxHealth = 80;
            localPlayerData.maxEnergy = 40;
            localPlayerData.currentHealth = 80;
            localPlayerData.currentEnergy = 40;
            localPlayerData.maxExp = 100;
            localPlayerData.moveSpeed = 10f;
            localPlayerData.playerDex = 3;
            localPlayerData.playerInt = 9;
            localPlayerData.playerStr = 3;
            localPlayerData.critChance = 0;
            localPlayerData.critMultiplier = 1;
            localPlayerData.attackSpeed = 1.5f;
            delay = localPlayerData.attackSpeed;
        }
        if (classID == 2)
        {
            localPlayerData.playerClass = "THIEF";
            localPlayerData.maxHealth = 40;
            localPlayerData.maxEnergy = 20;
            localPlayerData.currentHealth = 40;
            localPlayerData.currentEnergy = 20;
            localPlayerData.moveSpeed = 14f;
            localPlayerData.maxExp = 100;
            localPlayerData.playerDex = 9;
            localPlayerData.playerInt = 3;
            localPlayerData.playerStr = 3;
            localPlayerData.critChance = 5f;
            localPlayerData.critMultiplier = 1.5f;
            localPlayerData.attackSpeed = 0.5f;
            delay = localPlayerData.attackSpeed;
        }
        if (classID == 3)
        {
            localPlayerData.playerClass = "WARRIOR";
            localPlayerData.maxHealth = 110;
            localPlayerData.maxEnergy = 10;
            localPlayerData.currentHealth = 110;
            localPlayerData.currentEnergy = 10;
            localPlayerData.moveSpeed = 8f;
            localPlayerData.maxExp = 100;
            localPlayerData.playerDex = 3;
            localPlayerData.playerInt = 3;
            localPlayerData.playerStr = 9;
            localPlayerData.critChance = 3f;
            localPlayerData.critMultiplier = 1.3f;
            localPlayerData.attackSpeed = 1.3f;
            delay = localPlayerData.attackSpeed;
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
        AddToAttributes(1);
        localPlayerData.maxHealth += Mathf.RoundToInt(localPlayerData.playerStr / 10);
        localPlayerData.moveSpeed += (float)(localPlayerData.playerDex / 100);
        localPlayerData.currentHealth = localPlayerData.maxHealth;
    }
    void AddToAttributes(int Ammount)
    {
        localPlayerData.playerDex += Ammount;
        localPlayerData.playerInt += Ammount;
        localPlayerData.playerStr += Ammount;
        localPlayerData.unusedSkillPoints += Ammount;
    }
    void ShowMenu(string menuName)
    {
        switch (menuName)
        {
            case "Stats":
                statMenu.SetActive(!statMenu.activeSelf);
                break;
        }
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
        //Vector3 movement = new Vector3(x, 0, z);
        //if (!turnWithMove)
        //{
        //    transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f) * sens);
        //}
        //else
        //{
        //    transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal"), 0.0f) * sens * 5f);
        //}
        Vector3 movement = transform.forward * z + transform.right * x;
        if (movement != Vector3.zero)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        
        transform.position += (movement.normalized * Time.deltaTime * localPlayerData.moveSpeed * runSpeed);
        //transform.Translate(movement.normalized * Time.deltaTime * localPlayerData.moveSpeed * runSpeed, Space.World);
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
    void InitUI()
    {
        skillSlot1 = GameObject.Find("Canvas/SkillBar/1").GetComponent<PlayerSkills>();
        skillSlot2 = GameObject.Find("Canvas/SkillBar/2").GetComponent<PlayerSkills>();
        skillSlot3 = GameObject.Find("Canvas/SkillBar/3").GetComponent<PlayerSkills>();
        statMenu = GameObject.Find("Canvas/StatMenu");
        iText = GameObject.Find("Canvas/iText");
        classText = GameObject.Find("Canvas/StatMenu/ClassText").GetComponent<Text>();
        intText = GameObject.Find("Canvas/StatMenu/IntText").GetComponent<Text>();
        strText = GameObject.Find("Canvas/StatMenu/StrText").GetComponent<Text>();
        dexText = GameObject.Find("Canvas/StatMenu/DexText").GetComponent<Text>();
        LoadingScreen = GameObject.Find("Canvas/LoadingScreen");
        healthText = GameObject.Find("Canvas/HealthText").GetComponent<Text>();
        energyText = GameObject.Find("Canvas/EnergyText").GetComponent<Text>();
        moneyText = GameObject.Find("Canvas/MoneyText").GetComponent<Text>();
        levelText = GameObject.Find("Canvas/LevelText").GetComponent<Text>();
        expText = GameObject.Find("Canvas/ExpText").GetComponent<Text>();
        player = GetComponent<Rigidbody>();
        DM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();

        statMenu.SetActive(false);
        iText.SetActive(false);
        LoadingScreen.SetActive(false);
    }
}
