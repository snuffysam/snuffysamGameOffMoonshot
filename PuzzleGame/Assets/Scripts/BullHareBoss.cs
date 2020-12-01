using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullHareBoss : MonoBehaviour
{
    public Sprite[] frames;
    public GameObject targetPlanet;
    private int state = 0;
    //0 = waiting around
    //1 = regular jumping
    //2 = sees a red gem
    //3 = jumping at red gem
    private float stateTimer = 0f;
    private SpriteRenderer spr;
    private GemScript redGem;
    public float explosionLevel = 0.75f;
    public GameObject explosionEffectPrefab;
    public GameObject soundPrefab;
    public AudioClip jumpSound;
    public AudioClip angrySound;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlanetScript planet = GetComponent<GemScript>().GetPlanet();
        float maxGemDist = 10f;
        if (planet != null){
            Vector2 towardsPlanet = planet.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*Time.deltaTime*GetComponent<Rigidbody2D>().mass;
        }
        if (targetPlanet == null) {
            SpawnExplosion();
            ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
            sts.AddScore(15000/sts.GetScoreScale());
            Destroy(this.gameObject);
        }

        if (state == 0){
            spr.sprite = frames[0];
            stateTimer += Time.deltaTime;
            if (stateTimer > 0.5f){
                Vector3 addition = transform.up*250f/30f + transform.right*-100f/30f;
                GetComponent<Rigidbody2D>().velocity += new Vector2(addition.x, addition.y);
                if (planet != null){
                    GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity*3f/(transform.position-planet.transform.position).magnitude;
                }
                stateTimer = 0f;
                state = 1;

                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = jumpSound;
            }
            if (planet != null){
                float angleBetween = Vector3.SignedAngle(transform.up,transform.position-planet.transform.position, new Vector3(0f,0f,1f));
                transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween*Time.deltaTime*6f);
                GetComponent<Rigidbody2D>().velocity /= (transform.position-planet.transform.position).magnitude;
            }

            GemScript[] allGems = FindObjectsOfType<GemScript>();
            float dist = maxGemDist;
            for (int i = 0; i < allGems.Length; i++){
                float gemDist = (transform.position-allGems[i].transform.position).magnitude;
                if (allGems[i] != null && allGems[i].shape == 0 && gemDist < dist){
                    dist = gemDist;
                    redGem = allGems[i];
                }
            }
            if (redGem != null){
                state = 2;
                stateTimer = 0f;

                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = angrySound;
            }
        }
        if (state == 1){
            spr.sprite = frames[3];
            stateTimer += Time.deltaTime;
            if (stateTimer > 1.2f){
                state = 0;
                stateTimer = 0f;
            }
            if (planet != null){
                float angleBetween = Vector3.SignedAngle(transform.up,transform.position-planet.transform.position, new Vector3(0f,0f,1f));
                transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween*Time.deltaTime*6f);
            }
        }
        if (state == 2){
            float changeTime = 0.5f;
            spr.sprite = frames[2];
            if (stateTimer < changeTime*2f/3f){
                spr.sprite = frames[1];
            }
            stateTimer += Time.deltaTime;
            if (redGem == null || (transform.position-redGem.transform.position).magnitude > maxGemDist){
                state = 0;
                stateTimer = 0f;
                redGem = null;
            } else {
                float angleBetween = Vector3.SignedAngle(transform.up,redGem.transform.position-transform.position, new Vector3(0f,0f,1f));
                transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween);
                if (stateTimer > changeTime){
                    state = 3;
                    stateTimer = 0f;
                }
            }
        }
        if (state == 3){
            spr.sprite = frames[4];
            if (redGem == null || (transform.position-redGem.transform.position).magnitude > maxGemDist){
                state = 0;
                stateTimer = 0f;
                redGem = null;
            } else {
                float angleBetween = Vector3.SignedAngle(transform.up,redGem.transform.position-transform.position, new Vector3(0f,0f,1f));
                transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween);
                Vector3 addition = transform.up*Time.deltaTime*20f;
                GetComponent<Rigidbody2D>().velocity += new Vector2(addition.x, addition.y);
            }
        }
    }

    public void SpawnExplosion(){
        List<GameObject> toPush = new List<GameObject>();
        GemScript[] gs = FindObjectsOfType<GemScript>();
        for (int i = 0; i < gs.Length; i++){
            if (gs[i].GetComponent<PlanetScript>() == null && gs[i].GetComponent<BullHareBoss>() == null){
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
