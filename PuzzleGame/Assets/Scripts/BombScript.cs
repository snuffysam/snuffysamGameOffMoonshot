using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Sprite[] frames;
    public GameObject explosionEffectPrefab;
    public GameObject soundPrefab;
    public AudioClip explodeSFX;
    private float frameTimer = 0.2f;
    private float timer = 0f;
    private bool currentFrame = true;
    private PlanetScript planet;
    private PlanetScript[] allPlanets;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (frames.Length > 1){
            if (currentFrame){
                GetComponent<SpriteRenderer>().sprite = frames[0];
            } else {
                GetComponent<SpriteRenderer>().sprite = frames[1];
            }
        }

        timer += Time.deltaTime;
        if (timer > frameTimer){
            timer = 0f;
            currentFrame = !currentFrame;
        }

        if (frames.Length > 1){
        if (allPlanets == null){
            allPlanets = FindObjectsOfType<PlanetScript>();
        } else {
            PlanetScript closestPlanet = null;
            float dist = 9999999f;
            for (int i = 0; i < allPlanets.Length; i++){
                if (allPlanets[i] == null){
                    continue;
                }

                float testDist = (allPlanets[i].transform.position-transform.position).magnitude/allPlanets[i].GetComponent<Rigidbody2D>().mass;
                if (testDist < dist){
                    dist = testDist;
                    closestPlanet = allPlanets[i];
                }
            }
            planet = closestPlanet;
        }
        if (planet != null){
            Vector2 towardsPlanet = planet.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*Time.deltaTime;
        }} else {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized*6f;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.GetComponent<GemScript>() != null) { 
            Destroy(col.gameObject);
            if (frames.Length > 1){
                FindObjectOfType<ScoreTimerScript>().AddScore(1);
            }
        }

        float explosionLevel = 0.75f;

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

        SpawnExplosion(explosionLevel);
        Destroy(this.gameObject);
    }

    public void SpawnExplosion(float explosionLevel){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = explodeSFX;

        for (int i = 0; i < explosionLevel*15; i++){
            GameObject go = Instantiate<GameObject>(explosionEffectPrefab);
            Vector3 randomPosition = new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),0f).normalized*Random.Range(0f, explosionLevel);
            go.transform.position = transform.position + randomPosition;
            go.GetComponent<ExplosionEffectScript>().delayTimer = Random.Range(0f, i*0.05f);
        }
    }
}
