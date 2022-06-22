using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public float bulletDamage = 1f;
    public float bulletLifetime = 1f;
    public bool stickToSurface;
    public bool damagePlayer;
    public bool destroyOnHit;
    public float xKnockback;
    public float yKnockback;
    [HideInInspector]
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * bulletSpeed);
    }

    public void SetBulletSpeed(float speedValue)
    {
        bulletSpeed = speedValue;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public void SetBulletDamage(float damageValue)
    {
        bulletDamage = damageValue;
    }

    public float GetBulletDamage()
    {
        return bulletDamage;
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlatformerCharacter2D _player = collision.collider.GetComponent<PlatformerCharacter2D>();

        if(_player != null)
        {
            if (!_player.notDashing)
            {
                rb.mass = 0.1f;
                if (rb.velocity.magnitude < 1) rb.velocity = new Vector2(0, 5);
                rb.velocity *= 2;
                //Destroy(this.gameObject);
                _player.ForceDash();
            }
            else if(damagePlayer)
            {
                _player.DamagePlayer(bulletDamage);
                int side = (_player.transform.position.x - transform.position.x < 0) ? 1 : -1;
                _player.Knockback(xKnockback, yKnockback, side);
                if (destroyOnHit) Destroy(this.gameObject);
            }

        }

        if(stickToSurface)
        {
            bulletSpeed = 0;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlatformerCharacter2D _player = collision.collider.GetComponent<PlatformerCharacter2D>();

        if (_player != null)
        {
            if (!_player.notDashing)
            {
                rb.mass = 0.1f;
                if (rb.velocity.magnitude < 1) rb.velocity = new Vector2(0,5);
                rb.velocity *= 2;
                //Destroy(this.gameObject);
                _player.ForceDash();
            }

        }
    }
}
