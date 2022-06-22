using System;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


#pragma warning disable 649
namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] public float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] public float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        public bool m_FacingRight = true;  // For determining which way the player is currently facing.
        public bool freezePlayer;
        public bool canMove = true;
        public float health;
        [HideInInspector]public float maxHealth = 100f;
        public bool canDash = false;
        public bool haultGravity = false;
        public float dashBoost = 200f;
        public bool canDrop = false;
        public float dropBoost = 200f;
        public bool canHover = false;
        public float hoverBoost = 200f;
        public bool notDashing = true;
        public bool isCrouching;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            //InvokeRepeating("Dash", 5.5f, 5.5f);
           // InvokeRepeating("Drop", 10.1f, 10.1f);
           // InvokeRepeating("Hover", 3.5f, 3.5f);
        }


        private void FixedUpdate()
        {
            if(!freezePlayer && !haultGravity)
            {
                m_Grounded = false;
                m_Rigidbody2D.gravityScale = 3;

                // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
                // This can be done using layers instead but Sample Assets will not overwrite your project settings.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                        m_Grounded = true;
                }
                m_Anim.SetBool("Ground", m_Grounded);

                // Set the vertical animation
                m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            } else
            {
                m_Rigidbody2D.gravityScale = 0;
                if(freezePlayer)m_Rigidbody2D.velocity = Vector3.zero;
            }
            Dash();
            Drop();
            Hover();

        }


        public void Move(float move, bool crouch, bool jump)
        {
            if(canMove)
            {
                // If crouching, check to see if the character can stand up
                if (!crouch && m_Anim.GetBool("Crouch"))
                {
                    // If the character has a ceiling preventing them from standing up, keep them crouching
                    if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                    {            
                        crouch = true;
                    }
                }
                isCrouching = crouch;

                // Set whether or not the character is crouching in the animator
                m_Anim.SetBool("Crouch", crouch);

                //only control the player if grounded or airControl is turned on
                if (m_Grounded || m_AirControl)
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    move = (crouch ? move * m_CrouchSpeed : move);

                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                    // If the input is moving the player right and the player is facing left...
                    if (move > 0 && !m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (move < 0 && m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                }
                // If the player should jump...
                if (jump)
                {
                    // Add a vertical force to the player.
                    m_Grounded = false;
                    m_Anim.SetBool("Ground", false);
                    m_Rigidbody2D.velocity = Vector2.zero;
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                }

            }
 
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void DamagePlayer(float damage)
        {
            health -= damage;
        }

        public void Knockback(float horKB, float verKB, int side)
        {
            StartCoroutine(StopMoving(0.5f, true));
            m_Rigidbody2D.AddForce(new Vector2(horKB * side, verKB));

        }

        IEnumerator StopMoving(float time, bool onLanding)
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            if(m_Grounded || !onLanding)
            {
                canMove = true;
            } else
            {
                StartCoroutine(StopMoving(0.1f, true));
            }

        }

        IEnumerator HaultGravity(float time)
        {
            haultGravity = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            yield return new WaitForSeconds(time);
            haultGravity = false;
        }

        void Dash()
        {
            if (canDash && Input.GetKey(KeyCode.LeftShift) && canMove && notDashing)
            {
                m_Rigidbody2D.velocity = Vector2.zero;
                if (m_FacingRight) m_Rigidbody2D.AddForce(new Vector2(dashBoost * 1, 0f)); else m_Rigidbody2D.AddForce(new Vector2(dashBoost * -1, 0f));
                StartCoroutine(HaultGravity(0.5f));
                StartCoroutine(StopDash(0.5f));
                StartCoroutine(StopMoving(0.5f, false));
                notDashing = false;
            }
        }

        public void ForceDash()
        {
                m_Rigidbody2D.velocity = Vector2.zero;
                if (m_FacingRight) m_Rigidbody2D.AddForce(new Vector2(dashBoost * 1, 0f)); else m_Rigidbody2D.AddForce(new Vector2(dashBoost * -1, 0f));
                StartCoroutine(HaultGravity(0.5f));
                //StartCoroutine(StopDash(0.5f));
                StartCoroutine(StopMoving(0.5f, false));
               // notDashing = false;
        }

        IEnumerator StopDash(float time)
        {
            yield return new WaitForSeconds(time);
            notDashing = true;
        }

        void Drop()
        {
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (canDrop && v < 0) { 

                m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                m_Rigidbody2D.AddForce(new Vector2(0f, -dropBoost));

                StartCoroutine(HaultGravity(0.8f));
                StartCoroutine(StopMoving(0.8f, false));
            }
        }

        void Hover()
        {
            bool j = CrossPlatformInputManager.GetButtonDown("Jump");
            if (canHover && j && !m_Grounded)
            {
                m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                m_Rigidbody2D.AddForce(new Vector2(0f, hoverBoost));

                StartCoroutine(HaultGravity(0.8f));
                StartCoroutine(StopMoving(0.8f, false));
            }
        }


    }
}
