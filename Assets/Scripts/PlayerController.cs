using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [Header("Player Settings")]
    public int sens = 5;
    public float lookDamping = 5f;
    public float moveSpeed = 5f;
    public bool Grounded;
    public float groundRange = 0.1f;
    public float jumpHeight = 4f;
    public Rigidbody player;
    public bool playerMovement = true;
    public float attackDelay = 0.1f;
    public float delay;
    public float dashSpeed = 20f;
    public float dashDelay = 1.5f;
    public bool canDash = true;
    public float tapSpeed = 0.2f; //in seconds
    private float lastTapTime = 0;
    public int timesJumped = 0;
    public float jumpTime = 0;
    public bool isJumping = false;
    public int currentHealth;
    public int maxHealth = 100;
    [Header("Weapon Settings")]
    public bool hasWeapon = false;
    public Collider weapon;
    public Transform weaponChild;
    public Animator weaponAnim;
    public GameObject droppedWeaponObject;
    [Header("Interaction Settings")]
    public GameObject iText;
    public DialogueManager DM;
    public bool inRange = false;
    public Transform interactedEntity;
    public Text healthText;
    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        lastTapTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<Rigidbody>();
        delay = attackDelay;
    }
    // Update is called once per frame
    void Update () {

        healthText.text = "Health: " + currentHealth.ToString();
        //Weapon Stuff
        delay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;
        if (weaponChild != null)
        {
            weaponAnim = weaponChild.GetComponent<Animator>();
            if (Input.GetAxisRaw("Fire1") == 1 && delay <= 0 && playerMovement)
            {
                delay = attackDelay;
                weaponAnim.SetBool("attack", true);
            }
            else
            {
                weaponAnim.SetBool("attack", false);
            }
            if (Input.GetKeyDown(KeyCode.G) && hasWeapon)
            {
                DropWeapon();
            }
        }
        //Interacting
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
    void DropWeapon()
    {
        hasWeapon = false;
        Destroy(weaponChild.gameObject);
        weaponChild = null;
        droppedWeaponObject.SetActive(true);
        droppedWeaponObject.transform.parent = null;
        droppedWeaponObject.GetComponent<Rigidbody>().AddForce(transform.forward);
        droppedWeaponObject = null;
    }
    float time1;
    float time2;
    void FixedUpdate()
    {

        CheckForGround();
        //Dashing
        if (Input.GetKeyDown(KeyCode.W))
        {
            time1 = Time.time;
            if (((Time.time - lastTapTime) < tapSpeed) && ((time1 - time2) <= tapSpeed) && (dashDelay <= 0f) && canDash)
            {
                Dash();

            }
            lastTapTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            time2 = Time.time;
        }
        //Jumping
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
        }
        if (((timesJumped < 2) && (jumpTime >= 0.5)) && playerMovement && Input.GetAxisRaw("Jump") == 1)
        {
            Jump();
            isJumping = true;
        }
    }
    void Dash()
    {
        canDash = false;
        dashDelay = 1.5f;
        player = GetComponent<Rigidbody>();
        player.AddForce(transform.forward * dashSpeed, ForceMode.VelocityChange);
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
            else
            {
                Grounded = false;
            }
        }
    }
    void Jump()
    {
        
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
