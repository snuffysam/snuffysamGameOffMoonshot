using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBunnyBossScript : MonoBehaviour
{
    public Sprite[] frames;
    private Vector2 direction;
    private float directionTimer;
    public float directionMaxTimer = 3f;
    private float spawnDustTimer;
    public float spawnDustMaxTimer = 1f;
    public float explosionLevel = 0.75f;
    public GameObject explosionEffectPrefab;
    public GameObject dustPrefab;
    public GameObject soundPrefab;
    public AudioClip shootDustSound;
    private float frameTimer;
    private bool currentFrame;
    private float startTimer;
    // Start is called before the first frame update
    void Start()
    {
        RandomizeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        directionTimer += Time.deltaTime;
        spawnDustTimer += Time.deltaTime;
        frameTimer += Time.deltaTime;
        startTimer += Time.deltaTime;

        if (frameTimer > 0.1f){
            frameTimer = 0f;
            currentFrame = !currentFrame;
            if (currentFrame){
                GetComponent<SpriteRenderer>().sprite = frames[0];
            } else {
                GetComponent<SpriteRenderer>().sprite = frames[1];
            }
        }

        GetComponent<Rigidbody2D>().velocity += direction*Time.deltaTime*60f*0.07f;

        float angleBetween = Vector3.SignedAngle(transform.up,GetComponent<Rigidbody2D>().velocity, new Vector3(0f,0f,1f));
        transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween * Time.deltaTime * 3f);

        if (directionTimer > directionMaxTimer){
            directionTimer = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            RandomizeDirection();
        }

        if (spawnDustTimer > spawnDustMaxTimer){
            spawnDustTimer = 0f;
            GameObject go = Instantiate<GameObject>(dustPrefab);
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = shootDustSound;
            if (Random.Range(1f, 100f) < 50f){
                go.transform.position = transform.position + transform.right*0.1f;
            } else {
                go.transform.position = transform.position + transform.right*-0.1f;
            }
        }

        if (GetComponent<GemScript>().GetPlanet() == null && startTimer > 0.1f) {
            SpawnExplosion();
            ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
            sts.AddScore(15000/sts.GetScoreScale());
            Destroy(this.gameObject);
        }
        if (GetComponent<GemScript>().GetPlanet() != null){
            startTimer = 0f;
        }
    }

    public void RandomizeDirection(){
        direction = new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)).normalized;
    }

    public void SpawnExplosion(){
        List<GameObject> toPush = new List<GameObject>();
        GemScript[] gs = FindObjectsOfType<GemScript>();
        for (int i = 0; i < gs.Length; i++){
            if (gs[i].GetComponent<PlanetScript>() == null && gs[i].GetComponent<DustBunnyBossScript>() == null){
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
}
