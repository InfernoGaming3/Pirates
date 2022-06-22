using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Enemy : MonoBehaviour
{

    public float health = 100;
    [HideInInspector]
    public FollowPath enemyPath;
    // public PlatformerCharacter2D player;
    GameObject passwordGameObject;
    public bool weakened = false;
    public float damage;
    public float xKnockback;
    public float yKnockback;
    public Transform smokeEffect;
    public bool goldenEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        passwordGameObject = GameObject.FindGameObjectWithTag("PasswordGame");
        enemyPath = GetComponent<FollowPath>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            health = 0;
            enemyPath.moveSpeed = 0f;
            weakened = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlatformerCharacter2D _player = collision.collider.GetComponent<PlatformerCharacter2D>();
        Bullet _bullet = collision.collider.GetComponent<Bullet>();

        if(_bullet != null)
        {

                print("enemy has been hit!");
                CameraShake.Shake(0.2f, 0.3f);
                health -= _bullet.GetBulletDamage();
                if (enemyPath.moveSpeed > 0) enemyPath.moveSpeed -= 0.1f; else enemyPath.moveSpeed = 0;
            if (health <= 0)
            {
                Transform smokeEffectTransform = Instantiate(smokeEffect, transform);
                //Destroy(smokeEffectTransform, 0.5f);
            }
        }

        if (_player != null && weakened)
        {
           // print("password game started");
            passwordGameObject.GetComponent<PasswordGame>().FreezePlayerEnemy(GetComponent<Enemy>(), _player);
            passwordGameObject.GetComponent<PasswordGame>().StartPasswordGame();
        } 
        else if (_player != null && !weakened)
        {
            int side;
            if (_player.transform.position.x > transform.position.x) side = 1; else side = -1;
            print("knockback happened");
            _player.DamagePlayer(damage);
            _player.Knockback(xKnockback, yKnockback, side);
        }


    }
}
