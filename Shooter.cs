using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
public class Shooter : MonoBehaviour
{
    GameObject[] enemyTags;
    public List<GameObject> bullets;
    //public float bulletSpeed;
    // public float bulletLifetime;
    public float bulletDamageMultiplier = 1f;

    public float distanceToShoot;
    public float fireRate;
    public Vector3 firePointOffset;
    PlatformerCharacter2D player;
    public bool canShoot = false;
    public bool freezeShooting = false;
    public bool isShooting;
    public bool firstShot = true;
    // Start is called before the first frame update
    void Start()
    {
        enemyTags = GameObject.FindGameObjectsWithTag("Enemy");
        player = GetComponent<PlatformerCharacter2D>();
       InvokeRepeating("Shoot", fireRate, fireRate);
    }

    private void Update()
    {
        /*
        isShooting = Input.GetKey(KeyCode.Z);

        if(firstShot && isShooting)
        {
            Shoot();
            firstShot = false;
            StartCoroutine(FirstShotCooldown());
        }
        */

        if (player.freezePlayer) freezeShooting = true; else freezeShooting = false;
        
    }

    IEnumerator FirstShotCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        firstShot = true;
    }

    // Update is called once per frame
    void Shoot()
    {
        if(enemyTags != null && canShoot && !freezeShooting)
        {
                    // Do stuff
                   // print("found target, i'm now shooitng!");
                    Vector3 firePoint = (player.m_FacingRight)? transform.position + firePointOffset: transform.position - firePointOffset;
                    GameObject bulletGO = Instantiate(bullets[UnityEngine.Random.Range(0,bullets.Count)], firePoint, Quaternion.identity);
                    Bullet bulletScript = bulletGO.GetComponent<Bullet>();
                    if (player.m_FacingRight) bulletScript.SetBulletSpeed(bulletScript.bulletSpeed); else bulletScript.SetBulletSpeed(-bulletScript.bulletSpeed);
                    bulletScript.SetBulletDamage(bulletScript.bulletDamage * bulletDamageMultiplier);
                    Destroy(bulletGO, bulletScript.bulletLifetime);
        }
    }
}
