using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    
    public float dmgValue;

    public float HSpeed;
    public float VSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(HSpeed/20, VSpeed/20, 0);
    }

    void OnBecameInvisible()
    {
        DestroyGameObject();
    }

    public void DestroyGameObject() 
    {
        Destroy(gameObject);
    }
}
