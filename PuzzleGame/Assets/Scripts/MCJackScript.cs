using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCJackScript : MonoBehaviour
{
    public Sprite[] frames;
    public GameObject portalLeft;
    private int state = 0;
    private float stateTimer;
    private float startTimer;
    public float explosionLevel = 0.75f;
    public GameObject explosionEffectPrefab;
    private PlanetScript planet;
    private Vector2 direction;
    private Vector3 portalOffset;
    private float directionTimer;
    private float directionMaxTimer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(transform.up.x, transform.up.y);
        RandomizeDirection();
        portalOffset = portalLeft.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        stateTimer += Time.deltaTime;
        startTimer += Time.deltaTime;
        directionTimer += Time.deltaTime;

        if (planet == null){
            planet = FindObjectOfType<PlanetScript>();
        }

        if (planet == null && startTimer > 0.1f) {
            SpawnExplosion();
            ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
            sts.AddScore(15000/sts.GetScoreScale());
            Destroy(portalLeft);
            Destroy(this.gameObject);
        }
        if (planet != null){
            startTimer = 0f;
        }

        float angleBetween = Vector3.SignedAngle(transform.up,GetComponent<Rigidbody2D>().velocity, new Vector3(0f,0f,1f));
        transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween * Time.deltaTime * 3f);

        float throwSpeed = 5f;

        if (directionTimer > directionMaxTimer){
            RandomizeDirection();
            directionTimer = 0f;
        }

        if (state == 0){
            GetComponent<Rigidbody2D>().velocity = direction*4f;
            GetComponent<SpriteRenderer>().sprite = frames[0];
            Debug.Log(portalLeft.transform.localPosition);
            portalLeft.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            portalLeft.transform.localPosition = portalOffset;
            if (stateTimer > 3f){
                portalLeft.transform.SetParent(null);
                portalLeft.GetComponent<Rigidbody2D>().velocity = new Vector3(transform.up.x, transform.up.y) * -throwSpeed;
                state = 1;
                stateTimer = 0f;
            }
        } else {
            GetComponent<Rigidbody2D>().velocity = direction*2f;
            GetComponent<SpriteRenderer>().sprite = frames[1];
            Vector3 tow = (transform.position - portalLeft.transform.position).normalized;
            if (portalLeft.GetComponent<Rigidbody2D>().velocity.magnitude > throwSpeed){
                portalLeft.GetComponent<Rigidbody2D>().velocity = portalLeft.GetComponent<Rigidbody2D>().velocity.normalized*throwSpeed;
            }
            portalLeft.GetComponent<Rigidbody2D>().velocity += new Vector2(tow.x, tow.y)*Time.deltaTime*60f*0.15f;
            if (stateTimer > 0.75f && (portalLeft.transform.position - transform.position).magnitude < 0.75f){
                portalLeft.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                portalLeft.transform.SetParent(transform);
                portalLeft.transform.localPosition = portalOffset;
                state = 0;
                stateTimer = 0f;
            }
        }
    }
    
    public void SpawnExplosion(){
        List<GameObject> toPush = new List<GameObject>();
        GemScript[] gs = FindObjectsOfType<GemScript>();
        for (int i = 0; i < gs.Length; i++){
            if (gs[i].GetComponent<PlanetScript>() == null){
                toPush.Add(gs[i].gameObject);
            }
        }
        DustScript[] ds = FindObjectsOfType<DustScript>();
        for (int i = 0; i < ds.Length; i++){
            toPush.Add(ds[i].gameObject);
        }
        CannonScript[] cs = FindObjectsOfType<CannonScript>();
        for (int i = 0; i < cs.Length; i++){
            toPush.Add(cs[i].gameObject);
        }

        for (int i = 0; i < toPush.Count; i++){
            Vector3 towards = toPush[i].transform.position - transform.position;
            Vector3 scaled = towards * (1f/(towards.magnitude*towards.magnitude));
            Vector2 scaled2 = new Vector2(scaled.x, scaled.y);
            toPush[i].GetComponent<Rigidbody2D>().velocity += scaled2*explosionLevel;
        }

        for (int i = 0; i < explosionLevel*15; i++){
            GameObject go = Instantiate<GameObject>(explosionEffectPrefab);
            Vector3 randomPosition = new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),0f).normalized*Random.Range(0f, explosionLevel);
            go.transform.position = transform.position + randomPosition;
            go.GetComponent<ExplosionEffectScript>().delayTimer = Random.Range(0f, i*0.05f);
        }
    }

    public void RandomizeDirection(){
        direction = (direction + new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f))).normalized;
        direction = (direction + new Vector2(transform.position.x, transform.position.y).normalized*-1.5f).normalized;
    }
}
