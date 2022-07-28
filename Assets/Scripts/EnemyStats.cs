using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float hpMax;
    [SerializeField] Text hpDisplay;

    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax;
        hpDisplay.text = hp + "/" + hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Checks if collided object is attack from player
        if (col.gameObject.tag == "PlayerAttack")
        {
            //Damage enemy with dmg value taken from collided object script
            ProjectileBehaviour projScript = col.gameObject.GetComponent<ProjectileBehaviour>();
            hp -= projScript.dmgValue;

            //Update UI
            hpDisplay.text = hp + "/" + hpMax;

            //Destroy projectile
            projScript.DestroyGameObject();
        }
        //Checks if enemy hp is below 0 (defeated)
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
