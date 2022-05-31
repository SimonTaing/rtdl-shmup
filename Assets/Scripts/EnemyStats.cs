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
        if (col.gameObject.tag == "PlayerAttack")
        {
            Debug.Log("Enemy hit");
            ProjectileBehaviour projScript = col.gameObject.GetComponent<ProjectileBehaviour>();
            hp -= projScript.dmgValue;
            hpDisplay.text = hp + "/" + hpMax;
        }
    }
}
