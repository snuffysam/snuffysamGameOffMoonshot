using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public int match;//0=circle, 1=square, 2=triangle, 3=hexagon, 4=pentagon, 5=star
    public GameObject[] toSpawn;
    public float minSpawnRadius;
    public float maxSpawnRadius;
    public int spawnCount;
    public float explosionLevel;
    public GameObject explosionEffectPrefab;
    public GameObject soundPrefab;
    public AudioClip explodeSFX;
    public bool antigravity = false;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0,toSpawn.Length);
        for (int i = 0; i < spawnCount; i++){
            //GameObject go = Instantiate<GameObject>(toSpawn[Random.Range(0,toSpawn.Length)]);
            GameObject go = Instantiate<GameObject>(toSpawn[index]);
            index++;
            if (index >= toSpawn.Length){
                index = 0;
            }
            go.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized*Random.Range(minSpawnRadius,maxSpawnRadius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
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
    }

    public void SpawnExplosion(){
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
