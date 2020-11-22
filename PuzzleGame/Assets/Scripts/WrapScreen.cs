using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 9.5f){
            transform.position = new Vector3(transform.position.x-(9.5f*2), transform.position.y, transform.position.z);
        }
        if (transform.position.y > 5.6f){
            transform.position = new Vector3(transform.position.x, transform.position.y-(5.6f*2), transform.position.z);
        }
        if (transform.position.x < -9.5f){
            transform.position = new Vector3(transform.position.x+(9.5f*2), transform.position.y, transform.position.z);
        }
        if (transform.position.y < -5.6f){
            transform.position = new Vector3(transform.position.x, transform.position.y+(5.6f*2), transform.position.z);
        }
    }
}
