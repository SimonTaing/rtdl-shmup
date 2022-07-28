using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private CircleCollider2D circleCollider2D;

    public float dmgValue;

    public float HSpeed;
    public float VSpeed;
    [SerializeField] private float lifespanMax;
    [SerializeField] private float lifespan;

    void Start()
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        lifespan = lifespanMax;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(HSpeed/20, VSpeed/20, 0);
        lifespan -= Time.deltaTime;

        if(lifespan < 0)
        {
            //DestroyGameObject();
        }
    }

    void OnBecameInvisible()
    {
        DestroyGameObject();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit");
        }
    }

    public void DestroyGameObject() 
    {
        Destroy(gameObject);
    }
}
