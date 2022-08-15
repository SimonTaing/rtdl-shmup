using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("HP variables")]
    [SerializeField] float hp;
    [SerializeField] float hpMax;
    [SerializeField] Text hpDisplay;
    [SerializeField] UiHpBar uiHpBar;
    public bool isInvuln;
    [SerializeField] float invulnTimer;
    [SerializeField] float invulnTimerMax;
    [SerializeField] float knockbackMultiplier;

    [Header("Movement variables")]
    [SerializeField] public bool controllable;
    [SerializeField] float translationH;
    [SerializeField] float translationV;
    [SerializeField] float speedH = 1;
    [SerializeField] float speedV = 1;

    [Header("Attack variables")]
    [SerializeField] GameObject projectileSmall;
    [SerializeField] GameObject projectileMedium;
    [SerializeField] GameObject projectileLarge;

    [SerializeField] float chargeTimer;
    [SerializeField] float chargeTimerMax;

    // Start is called before the first frame update
    void Start()
    {
        //Make controllable and vulnerable
        controllable = true;
        isInvuln = false;

        //Set hp and update UI
        hp = hpMax;
        uiHpBar.hpBarFront.fillAmount = hp / hpMax;
        uiHpBar.hpBarMid.fillAmount = hp / hpMax;

        //Set charge attack timer to max
        chargeTimer = chargeTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvuln)
        {
            invulnTimer -= Time.deltaTime;

            if(invulnTimer <= 0)
            {
                isInvuln = false;
            }
        }

        //Check if character should recieve inputs
        if (controllable)
        {
            //Lateral movement
            translationH = Input.GetAxis("Horizontal") * speedH / 2;
            translationV = Input.GetAxis("Vertical") * speedV / 2;

            transform.Translate(translationH, translationV, 0);

            //Always shoot 1 small projectile on KeyDown 
            if (Input.GetKeyDown("space"))
            {
                SpawnProjectile(projectileSmall, 1.5f, 0f, 3);
            }

            //Decrease chrage timer when shoot button is held down
            if (Input.GetKey("space"))
            {
                chargeTimer -= Time.deltaTime;
                if(chargeTimer < 0)
                {
                    Debug.Log("charge ready");
                }
            }

            //Sort which type of attack to spawn on KeyUp
            if(Input.GetKeyUp("space"))
            {

                //Mid charge
                if (chargeTimer > 0 && chargeTimer < (chargeTimerMax / 2))
                {
                    SpawnProjectile(projectileMedium, 1.5f, 0f, 16);
                }

                //Full charge
                if (chargeTimer < 0)
                {
                    SpawnProjectile(projectileLarge, 1.5f, 0f, 32);
                    SpawnProjectile(projectileMedium, 1f, 0.5f, 32);
                    SpawnProjectile(projectileMedium, 1f, -0.5f, 32);
                    SpawnProjectile(projectileSmall, -1.5f, 0f, 16);
                    SpawnProjectile(projectileSmall, -1.25f, 0.5f, 16);
                    SpawnProjectile(projectileSmall, -1.25f, -0.5f, 16);
                }
                //Reset timer
                chargeTimer = chargeTimerMax;
            }
        }
    }

    private void SpawnProjectile(GameObject projectype, float hSpeed, float vSpeed, float dmgValue) 
    {
        GameObject projectile;
        projectile = Instantiate(projectype, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileBehaviour>().HSpeed = hSpeed;
        projectile.GetComponent<ProjectileBehaviour>().VSpeed = vSpeed;
        projectile.GetComponent<ProjectileBehaviour>().dmgValue = dmgValue;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.tag == "HealingItem")
        {
            Heal(col.gameObject.GetComponent<HealingItemStats>().healValue);
            Destroy(col.gameObject);
        } 
        else if (col.gameObject.tag == "1UP")
        {

        } 
        else if (col.gameObject.tag == "PointStar")
        {

        }


        if (isInvuln == false)
        {
            //If collided with enemy attack
            if (col.gameObject.tag == "EnemyAttack")
            {
                //Call Hurt() to damage player with dmg value taken from collided object script
                ProjectileBehaviour projScript = col.gameObject.GetComponent<ProjectileBehaviour>();
                Hurt(projScript.dmgValue);

                //Destroy enemy projectile
                Destroy(col.gameObject);
            } 
            //If collided with enemy
            else if(col.gameObject.tag == "Enemy")
            {
                Hurt(5);
            }
        }

    }

    private void Hurt(float dmgValue)
    {
        //Damage player
        hp -= dmgValue;

        //Update UI
        uiHpBar.UpdateHealthUI("Hurt", hp, hpMax);

        //If player hp is above 0 (still alive)
        if(hp > 0)
        {
            controllable = false;
            Vector3 newPos = new Vector3(
                (transform.position.x - translationH) * knockbackMultiplier, 
                (transform.position.y - translationV) * knockbackMultiplier, 
                transform.position.z);
            transform.DOMove(newPos, 0.2f).OnComplete(() =>{
                controllable = true;
                isInvuln = true;
                invulnTimer = invulnTimerMax;
            });
        }
        //Checks if player hp is below 0 (defeated)
        else if (hp <= 0)
        {

        }
    }

    private void Heal(float healValue)
    {
        //Heal player
        hp += healValue;

        //Bring hp back to hpMax in case of overflow
        if (hp > hpMax)
        {
            hp = hpMax;
        }

        //Update UI
        uiHpBar.UpdateHealthUI("Heal", hp, hpMax);
    }
}
