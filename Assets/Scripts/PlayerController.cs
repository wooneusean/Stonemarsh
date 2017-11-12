using System.Collections;
using System.Collections.Generic;
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
    public int maxHealth = 100;
    [Header("Weapon Settings")]
    public bool hasWeapon;
    public Transform weaponChild;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    public DialogueManager DM;
    [Header("Interaction Settings")]
    public GameObject iText;
    public bool inRange = false;
    public Transform interactedEntity;
    public Text healthText;
    public GameObject LoadingScreen;
    // Use this for initialization
    public PlayerStatistics localPlayerData = new PlayerStatistics();
    void Start () {
        iText = GameObject.Find("Canvas/iText");
        LoadingScreen = GameObject.Find("Canvas/LoadingScreen");
        healthText = GameObject.Find("Canvas/HealthText").GetComponent<Text>();
        lastTapTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<Rigidbody>();
        delay = attackDelay;
        localPlayerData.maxHealth = maxHealth;
        localPlayerData.currentHealth = maxHealth;
        GlobalControl.Instance.savedPlayerData.maxHealth = localPlayerData.maxHealth;
        if (GlobalControl.Instance.savedPlayerData.currentHealth == 0)
        {
            GlobalControl.Instance.savedPlayerData.currentHealth = localPlayerData.maxHealth;
        }
        LoadPlayer();
        iText.SetActive(false);
        LoadingScreen.SetActive(false);
    }
    public void LoadPlayer()
    {
        localPlayerData = GlobalControl.Instance.savedPlayerData;
    }
    public void SavePlayer()
    {
        GlobalControl.Instance.savedPlayerData = localPlayerData;
    }
    // Update is called once per frame
    void Update () {
        healthText.text = "Health: " + localPlayerData.currentHealth.ToString();
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
            localPlayerData.DM.DisplayNextSentence();
        }
        //Moving
        if (playerMovement)
        {
            Move();
        }

    }
    void DropWeapon()
    {
        localPlayerData.hasWeapon = false;
        Destroy(localPlayerData.weaponChild.gameObject);
        localPlayerData.weaponChild = null;
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
        Debug.DrawRay(rayGround, down * groundRange, Color.red);
        if (Physics.Raycast(transform.position, down, out groundHit, groundRange))
        {
            //do something if hit object ie
            if (!groundHit.collider.CompareTag("Interaction"))
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
        transform.Translate(movement.normalized * Time.deltaTime * moveSpeed * runSpeed , Space.World);
    }
}
