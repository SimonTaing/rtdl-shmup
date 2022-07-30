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
    [SerializeField] Image hpBarDisplay;
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

    // Start is called before the first frame update
    void Start()
    {
        controllable = true;
        isInvuln = false;

        hp = hpMax;
        UpdateHealthUI();

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
                SpawnProjectile(projectileSmall, 1.5f, 0f);
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
                    SpawnProjectile(projectileMedium, 1.5f, 0f);
                }

                //Full charge
                if (chargeTimer < 0)
                {
                    SpawnProjectile(projectileLarge, 1.5f, 0f);
                    SpawnProjectile(projectileMedium, 1f, 0.5f);
                    SpawnProjectile(projectileMedium, 1f, -0.5f);
                    SpawnProjectile(projectileSmall, -1.5f, 0f);
                    SpawnProjectile(projectileSmall, -1.25f, 0.5f);
                    SpawnProjectile(projectileSmall, -1.25f, -0.5f);
                }
                //Reset timer
                chargeTimer = chargeTimerMax;
            }
        }
    }

    private void SpawnProjectile(GameObject projectype, float hSpeed, float vSpeed) 
    {
        GameObject projectile;
        projectile = Instantiate(projectype, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileBehaviour>().HSpeed = hSpeed;
        projectile.GetComponent<ProjectileBehaviour>().VSpeed = vSpeed;
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

    private void UpdateHealthUI()
    {
        //Update UI
        hpDisplay.text = hp + "/" + hpMax;
        hpBarDisplay.DOFillAmount(hp / hpMax, 0.1f);

        //If hp is over 1/2.5
        if(hp > hpMax / 2.5)
        {
            //Change to blue
            Color blue = new Color(0.3716981f, 0.9606702f, 1, 1);
            hpBarDisplay.DOColor(blue, 0.1f);
        } 
        //If hp is below 1/2.5
        else if (hp < hpMax / 2.5) 
        {
            //Change to red
            Color red = new Color(1, 0.07924521f, 0.1927032f, 1);
            hpBarDisplay.DOColor(red, 0.1f);
        }
    }

    private void Hurt(float dmgValue)
    {
        //Damage player
        hp -= dmgValue;

        //Update UI
        UpdateHealthUI();

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
        UpdateHealthUI();
    }
}
