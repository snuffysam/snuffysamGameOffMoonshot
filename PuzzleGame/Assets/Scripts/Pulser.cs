using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulser : MonoBehaviour
{
    public GameObject pulsePrefab;
    private float speed;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f/GetComponent<Rigidbody2D>().mass;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > speed){
            GameObject go = Instantiate<GameObject>(pulsePrefab);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.transform.localScale = transform.localScale;
            timer = 0f;
        }
    }
}
