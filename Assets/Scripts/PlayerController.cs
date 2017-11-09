using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int sens = 5;
    public float lookDamping = 5f;
    public float moveSpeed = 5f;
    public bool Grounded;
    public float groundRange = 0.1f;
    public float jumpHeight = 4f;
    public Rigidbody player;
    public GameObject iText;
    public bool playerMovement = true;
    public DialogueManager DM;
    public bool inRange = false;
    public Transform interactedEntity;
    public Collider weapon;
    public float attackDelay = 0.1f;
    public float delay;
    public Transform weaponChild;
    public Animator weaponAnim;
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<Rigidbody>();
        delay = attackDelay;
        weaponChild = transform.Find("Weapon");
        weaponAnim = weaponChild.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update () {

        delay -= 1 * Time.deltaTime;
        if (Input.GetAxisRaw("Fire1") == 1 && delay <= 0)
        {
            delay = attackDelay;
            weaponAnim.SetBool("attack", true);
        }
        else
        {
            weaponAnim.SetBool("attack", false);
        }
        if (Input.GetKeyDown(KeyCode.E) && playerMovement == true && inRange)
        {
            interactedEntity.GetComponent<DialogueTrigger>().TriggerDialogue();
            playerMovement = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerMovement == false && inRange)
        {
            DM.DisplayNextSentence();
        }
        if (playerMovement)
        {
            Move();
        }
    }
    void FixedUpdate()
    {
        CheckForGround();
        if (Input.GetAxisRaw("Jump") == 1 && Grounded && playerMovement)
        {
            Jump();
        }
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
            if (groundHit.collider.name != "Interaction")
            {

                Grounded = true;
                Debug.Log("Close to " + groundHit.collider.name);

            }
            else if (groundHit.collider == null)
            {
                Grounded = false;
            }
        }
    }
    void Jump()
    {
        player.AddForce(Vector3.up * jumpHeight, ForceMode.Acceleration);
        Grounded = false;
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
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
        transform.Translate(movement.normalized * Time.deltaTime * moveSpeed , Space.World);
    }
}
