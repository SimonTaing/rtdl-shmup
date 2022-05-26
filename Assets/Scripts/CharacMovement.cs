using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacMovement : MonoBehaviour
{
    [SerializeField] GameObject projectileSmall;
    [SerializeField] GameObject projectileMedium;
    [SerializeField] GameObject projectileLarge;
    [SerializeField] float speedH = 1;
    [SerializeField] float speedV = 1;
    [SerializeField] float chargeTimer;
    [SerializeField] float chargeTimerMax;
    [SerializeField] float shootTimer;
    [SerializeField] float shootTimerMax;

    // Start is called before the first frame update
    void Start()
    {
        chargeTimer = chargeTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        float translationH = Input.GetAxis("Horizontal") * speedH / 2;
        float translationV = Input.GetAxis("Vertical") * speedV / 2;

        transform.Translate(translationH, translationV, 0);

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("shoot");
            GameObject projectile;
            projectile = Instantiate(projectileSmall, transform.position, transform.rotation);
        }

        if (Input.GetKey("space"))
        {
            chargeTimer -= Time.deltaTime;
            if(chargeTimer < 0)
            {
                Debug.Log("charge ready");
            }
        }

        if(Input.GetKeyUp("space"))
        {
            if (chargeTimer > 0 && chargeTimer < (chargeTimerMax / 2))
            {
                GameObject projectile;
                projectile = Instantiate(projectileMedium, transform.position, transform.rotation);
                projectile.GetComponent<ProjectileBehaviour>().HSpeed = 1.5f;
                projectile.GetComponent<ProjectileBehaviour>().VSpeed = 0f;
            }

            if (chargeTimer < 0)
            {
                Debug.Log("shoot large");
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
            chargeTimer = chargeTimerMax;
        }
    }
}
