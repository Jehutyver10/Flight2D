﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is meant to have actions which are usable by all characters, such as movement, attack, and basic rigidbody initialization.
/// </summary>
public class CharacterBehavior : MonoBehaviour {
    [SerializeField]
    private float speedBuffer = 500;

    public float xSpeed = 10; //speed moving left and right
    public float ySpeed = 10;// speed moving up and down
    public float dashModifier = 2; //speed increase by dashing
    public float smoothing = 0.5f;
    public bool facingRight, attacking = false, charging = false;
    bool dashing;
    public Rigidbody2D rb;
    public GameManager gm;
    int comboCount = 0;
    float distance = 0;
    Animator anim;
    RaycastHit2D[] hit = new RaycastHit2D[1];
    Weapon weapon;


    public SpriteRenderer sp;
    Vector2 velocity = Vector2.zero;
    // Use this for initialization

    //possibly temporary variables for setup
    float weaponBufferX;
    public Collider2D[] attackHitBoxes;
    [SerializeField]
    Color chargeColor;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();

        //attackHitBoxes = new Collider[4];
        attackHitBoxes = GetComponentsInChildren<Collider2D>();
        //foreach(Collider2D item in attackHitBoxes)
        //{ 

        //    if(item.name.Contains("Melee Attack Hitbox"))
        //    {
        //        print(item.name);
        //        item.gameObject.SetActive(false);
        //    }
        //}
    }
    public void Start() {
        
        sp = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Weapon>();
        anim = GetComponent<Animator>();
        weaponBufferX = weapon.transform.position.x - transform.position.x;
    }

    private void Reset()
    {

    }
    //private void Update()
    //{
    //    print(gm);
    //}
    public virtual void Move(Vector2 input, float dash = 1)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = input * dash * speedBuffer * Time.fixedDeltaTime;
        distance = dir.magnitude;
        int results = rb.Cast(dir.normalized, hit, dir.magnitude * 4 - 0.01f);
        if (results > 0)
        {
            //print("detected");
            distance = hit[0].fraction * dir.normalized.magnitude;
        }
        // rb.velocity = dir ;
        if (distance < 0.01f)
        {
            distance = 0;
            //rb.velocity = Vector2.zero;
        }
        rb.MovePosition(pos + dir * distance);
        //Collider2D[] cols = 
        //rb.velocity = new Vector2(moveX, moveY) * dash * speedBuffer * Time.fixedDeltaTime;
        // rb.AddForce(new Vector2(moveX, moveY));

        // If the input is moving the player right and the player is facing left...
       
        FlipCheck(input.x);
    }

    public virtual void FlipCheck(float moveX)
    {
        if (moveX > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveX < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }


    }
    public void End()

    ///this is literally the simplest way for me to think of extending animation clip length without a new animation
    {
        return;
    }
    public void StopAttacking()
    {
        attacking = false;
    }
    public void Attack() {
        ///Continuing the combo and hitbox activation/deactivation are currently handled by the player animator
        ///The actual sprite work is handled by the weapon (for now)

        if (!attacking)
        {
            attacking = true;
            weapon.anim.SetTrigger("Attack");
            anim.SetTrigger("Attack");


        }
    }

    public void Shoot()
    {
        ///Shoot() will simply shoot
        return;
    }

    public virtual IEnumerator Charge(Vector2 movementInput, bool button) 
    {
        return null;

    }
    public virtual IEnumerator Swing(Weapon weapon, Vector3 start, Vector3 end, float totalTime = .5f) {
        //update the position of weapon to the endpoint of the swing and let it rest there for the total time
        if (!attacking)
        {


            float elapsedTime = 0;
            while (elapsedTime < totalTime)
            {
                weapon.transform.localPosition = end;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            weapon.transform.localPosition = start;
            attacking = false;
            yield return null;
        }
        else {
            yield return null;

        }

    }
    public virtual void Flip()
    {
        if (sp)
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;
            sp.flipX = !sp.flipX;
            weapon.sp.flipX = !weapon.sp.flipX;
            //Debug.Log("before:" + weapon.transform.position.x.ToString());
            weaponBufferX = weaponBufferX * -1;
            // weapon.transform.localPosition = new Vector3(1, 0, 0);
            //weapon.transform.position = new Vector3(transform.position.x + weaponBufferX, weapon.transform.position.y, weapon.transform.position.z);
            weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x * -1, weapon.transform.localPosition.y, weapon.transform.localPosition.z);
            //Debug.Log("after:" + weapon.transform.position.x.ToString());

            // Multiply the player's x local scale by -1.
            // Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;
        }
    }
}
