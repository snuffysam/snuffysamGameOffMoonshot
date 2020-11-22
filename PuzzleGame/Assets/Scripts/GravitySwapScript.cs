using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwapScript : MonoBehaviour
{
    public float gravityTime, antigravityTime;
    public Sprite gravitySprite, antigravitySprite;
    private bool isAntigrav = false;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isAntigrav){
            GetComponent<SpriteRenderer>().sprite = antigravitySprite;
            GetComponent<PlanetScript>().antigravity = true;
            GetComponent<Pulser>().enabled = true;
            if (timer > antigravityTime){
                timer = 0f;
                isAntigrav = !isAntigrav;
            }
        } else {
            GetComponent<SpriteRenderer>().sprite = gravitySprite;
            GetComponent<PlanetScript>().antigravity = false;
            GetComponent<Pulser>().enabled = false;
            if (timer > gravityTime){
                timer = 0f;
                isAntigrav = !isAntigrav;
            }
        }
    }
}
