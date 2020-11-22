using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyRabbitScript : MonoBehaviour
{
    public Sprite[] frames;
    private int state = 3;
    private float stateTimer;
    private float frameTimer;
    private bool currentFrame;
    private float startTimer;
    public float explosionLevel = 0.75f;
    public GameObject explosionEffectPrefab;
    private PlanetScript planet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateTimer += Time.deltaTime;
        frameTimer += Time.deltaTime;
        startTimer += Time.deltaTime;

        if (planet == null){
            planet = FindObjectOfType<PlanetScript>();
        }

        if (planet == null && startTimer > 0.1f) {
            SpawnExplosion();
            ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
            sts.AddScore(15000/sts.GetScoreScale());
            Destroy(this.gameObject);
        }
        if (planet != null){
            startTimer = 0f;
        }

        if (state == 0){
            transform.RotateAround(Vector3.zero, Vector3.forward, -137.5f/2f * Time.deltaTime);

            if (frameTimer > 0.1f){
                frameTimer = 0f;
                currentFrame = !currentFrame;
                if (currentFrame){
                    GetComponent<SpriteRenderer>().sprite = frames[0];
                } else {
                    GetComponent<SpriteRenderer>().sprite = frames[1];
                }
            }

            if (stateTimer > 2f){
                state = 1;
                stateTimer = 0f;
            }
        }
        if (state == 1){
            if (stateTimer > 0.3f){
                state = 2;
                stateTimer = 0f;
            }
        }
        if (state == 2){
            Dictionary<int, float> distances = new Dictionary<int, float>();
            Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();

            GemScript[] allGems = FindObjectsOfType<GemScript>();

            state = 3;
            stateTimer = 0f;

            if (allGems.Length <= 0){
                return;
            }

            foreach (GemScript gs in allGems){
                float dist = (transform.position - gs.transform.position).magnitude;
                if (!distances.ContainsKey(gs.shape)){
                    distances.Add(gs.shape, dist);
                    prefabs.Add(gs.shape, gs.gameObject);
                    continue;
                }
                if (distances[gs.shape] > dist){
                    distances[gs.shape] = dist;
                    prefabs[gs.shape] = gs.gameObject;
                }
            }

            int shapeMin = -1;
            float shapeMinDist = 0f;

            for (int i = -1; i <= 5; i++){
                if (distances.ContainsKey(i) && distances[i] > shapeMinDist){
                    shapeMin = i;
                    shapeMinDist = distances[i];
                }
            }

            GameObject go = Instantiate<GameObject>(prefabs[shapeMin]);
            go.transform.position = transform.position*0.83f;
            go.transform.localScale = go.transform.localScale*0.1f;
        }
        if (state == 3){
            if (stateTimer > 0.2f){
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
}
