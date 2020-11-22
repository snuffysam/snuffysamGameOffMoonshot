using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        Color c2 = new Color(c.r, c.g, c.b, c.a-Time.deltaTime);
        GetComponent<SpriteRenderer>().color = c2;
        if (c.a <= Time.deltaTime){
            Destroy(this.gameObject);
        }
        transform.localScale = new Vector3(1f, 1f, 1f) * (transform.localScale.x+Time.deltaTime);
    }
}
