using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAntigravBehavior : MonoBehaviour
{
    public Sprite[] frames;
    private float frameTimer;
    private bool currentFrame;
    private Vector2 direction;
    private float directionTimer;
    private float directionMaxTimer = 2.5f;
    private float flightSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        frameTimer = 0;
        currentFrame = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)).normalized*flightSpeed;
        //direction = GetComponent<Rigidbody2D>().velocity.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer += Time.deltaTime;
        directionTimer += Time.deltaTime;

        if (directionTimer > directionMaxTimer){
            directionTimer = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)).normalized*flightSpeed;
        }
        GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized*flightSpeed;

        if (frameTimer > 0.1f){
            frameTimer = 0f;
            currentFrame = !currentFrame;
            if (currentFrame){
                GetComponent<SpriteRenderer>().sprite = frames[0];
            } else {
                GetComponent<SpriteRenderer>().sprite = frames[1];
            }
        }
        
        float angleBetween = Vector3.SignedAngle(transform.up,GetComponent<Rigidbody2D>().velocity, new Vector3(0f,0f,1f));
        transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween * Time.deltaTime * 3f);
    }

    public void Explode(){
        GetComponent<PlanetScript>().SpawnExplosion();
        Destroy(this.gameObject);
    }
}
