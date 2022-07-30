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
    [SerializeField] bool isInvuln;

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
    [SerializeField] float shootTimer;
    [SerializeField] float shootTimerMax;

    // Start is called before the first frame update
    void Start()
    {
        controllable = true;
        isInvuln = false;

        hp = hpMax;
        hpDisplay.text = hp + "/" + hpMax;
        chargeTimer = chargeTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
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
                ShootSmall();
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
                    ShootMidCharge();
                }

                //Full charge
                if (chargeTimer < 0)
                {
                    ShootFullCharge();
                }
                //Reset timer
                chargeTimer = chargeTimerMax;
            }
        }
    }

    private void ShootSmall()
    {
        GameObject projectile;
        projectile = Instantiate(projectileSmall, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileBehaviour>().HSpeed = 1.5f;
        projectile.GetComponent<ProjectileBehaviour>().VSpeed = 0f;
    }

    private void ShootMidCharge()
    {
        GameObject projectile;
        projectile = Instantiate(projectileMedium, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileBehaviour>().HSpeed = 1.5f;
        projectile.GetComponent<ProjectileBehaviour>().VSpeed = 0f;
    }

    private void ShootFullCharge()
    {
        GameObject projectile1;
        projectile1 = Instantiate(projectileLarge, transform.position, transform.rotation);
        projectile1.GetComponent<ProjectileBehaviour>().HSpeed = 1.5f;
        projectile1.GetComponent<ProjectileBehaviour>().VSpeed = 0f;

        GameObject projectile2;
        projectile2 = Instantiate(projectileMedium, transform.position, transform.rotation);
        projectile2.GetComponent<ProjectileBehaviour>().HSpeed = 1f;
        projectile2.GetComponent<ProjectileBehaviour>().VSpeed = 0.5f;

        GameObject projectile3;
        projectile3 = Instantiate(projectileMedium, transform.position, transform.rotation);
        projectile3.GetComponent<ProjectileBehaviour>().HSpeed = 1f;
        projectile3.GetComponent<ProjectileBehaviour>().VSpeed = -0.5f;

        GameObject projectile4;
        projectile4 = Instantiate(projectileSmall, transform.position, transform.rotation);
        projectile4.GetComponent<ProjectileBehaviour>().HSpeed = -1.5f;
        projectile4.GetComponent<ProjectileBehaviour>().VSpeed = 0f;

        GameObject projectile5;
        projectile5 = Instantiate(projectileSmall, transform.position, transform.rotation);
        projectile5.GetComponent<ProjectileBehaviour>().HSpeed = -1f;
        projectile5.GetComponent<ProjectileBehaviour>().VSpeed = 0.5f;

        GameObject projectile6;
        projectile6 = Instantiate(projectileSmall, transform.position, transform.rotation);
        projectile6.GetComponent<ProjectileBehaviour>().HSpeed = -1f;
        projectile6.GetComponent<ProjectileBehaviour>().VSpeed = -0.5f;
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
                projScript.DestroyGameObject();
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
        hpDisplay.text = hp + "/" + hpMax;

        //If player hp is above 0 (still alive)
        if(hp > 0)
        {
            controllable = false;
            transform.DOMoveX(transform.position.x - 1.5f, 0.2f).OnComplete(() =>{
                controllable = true;
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
        hpDisplay.text = hp + "/" + hpMax;
    }
}
