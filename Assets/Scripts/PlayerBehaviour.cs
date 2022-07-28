using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("HP variables")]
    [SerializeField] float hp;
    [SerializeField] float hpMax;
    [SerializeField] Text hpDisplay;

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
        //Checks if collided object is attack from enemy
        if (col.gameObject.tag == "EnemyAttack")
        {
            //Damage player with dmg value taken from collided object script
            ProjectileBehaviour projScript = col.gameObject.GetComponent<ProjectileBehaviour>();
            hp -= projScript.dmgValue;

            //Update UI
            hpDisplay.text = hp + "/" + hpMax;

            //Destroy enemy projectile
            projScript.DestroyGameObject();

            controllable = false;
        }
        //Checks if enemy hp is below 0 (defeated)
        if (hp <= 0)
        {

        }
    }
}
