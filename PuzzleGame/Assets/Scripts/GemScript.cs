using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public int shape;//0=circle, 1=square, 2=triangle, 3=T, 4=pentagon, 5=star
    public GameObject chain;
    public GameObject soundPrefab;
    public AudioClip collideSFX;
    public AudioClip connectSFX;
    private PlanetScript planet;
    private List<GameObject> chains = new List<GameObject>();
    private List<GameObject> connected = new List<GameObject>();
    private PlanetScript[] allPlanets;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ResetPlanets(){
        allPlanets = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 1f){
            float mult = 1+(0.2f*Time.deltaTime*60f);
            if (transform.localScale.x*mult > 1f){
                transform.localScale = new Vector3(1f, 1f, 1f);
            } else {
                transform.localScale *= mult;
            }
        }
        if (allPlanets == null){
            allPlanets = FindObjectsOfType<PlanetScript>();
        } else {
            PlanetScript closestPlanet = null;
            float dist = 9999999f;
            for (int i = 0; i < allPlanets.Length; i++){
                if (allPlanets[i] == null || allPlanets[i].gameObject.Equals(this.gameObject)){
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
            float gravDir = 1f;
            if (planet.antigravity && towardsPlanet.magnitude < planet.GetComponent<Rigidbody2D>().mass){
                gravDir = -1f;
            }
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*Time.deltaTime*planet.GetComponent<Rigidbody2D>().mass*gravDir;
        }

        if (GetComponent<PlanetScript>() != null){
            GetComponent<Rigidbody2D>().velocity -= GetComponent<Rigidbody2D>().velocity*0.05f*Time.deltaTime*60f;
            if (planet == null){
                GetComponent<Rigidbody2D>().velocity -= GetComponent<Rigidbody2D>().velocity*0.5f*Time.deltaTime*60f;
                GetComponent<Rigidbody2D>().velocity -= new Vector2(transform.position.x, transform.position.y)*0.05f*Time.deltaTime*60f;
            } else {
                //Vector2 towardsPlanet = planet.transform.position - transform.position;
                //GetComponent<Rigidbody2D>().velocity *= 0.95f;
                //GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*Time.deltaTime;
                GetComponent<Rigidbody2D>().angularVelocity -= GetComponent<Rigidbody2D>().angularVelocity*0.01f*Time.deltaTime*60f;
            }
        }

        for (int i = 0; i < connected.Count; i++){
            if (connected[i] == null){
                connected.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < chains.Count && i < connected.Count; i++){
            chains[i].transform.position = transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, connected[i].transform.position-transform.position);
            chains[i].transform.eulerAngles = new Vector3(0f, 0f, angle);
            chains[i].transform.localScale = transform.localScale;
        }
        if (connected.Count > 1){
            Queue<GameObject> searchQueue = new Queue<GameObject>();
            HashSet<GameObject> allObjects = new HashSet<GameObject>();
            searchQueue.Enqueue(this.gameObject);
            allObjects.Add(this.gameObject);
            while (searchQueue.Count > 0){
                GameObject go = searchQueue.Dequeue();
                if (go.GetComponent<GemScript>() == null){
                    continue;
                }
                foreach (GameObject go2 in go.GetComponent<GemScript>().connected){
                    if (!allObjects.Contains(go2)){
                        searchQueue.Enqueue(go2);
                        allObjects.Add(go2);
                    }
                }
            }
            if (allObjects.Count >= 4){
                int destroyCount = allObjects.Count;
                FindObjectOfType<ScoreTimerScript>().AddScore(destroyCount);
                foreach (GameObject go in allObjects){
                    if (go.GetComponent<PlanetScript>() != null){
                        go.GetComponent<PlanetScript>().SpawnExplosion();
                    }
                    if (go.GetComponent<GemScript>() != null){
                        go.GetComponent<GemScript>().DestroyAllChains();
                    }
                    if (go != this.gameObject){
                        Destroy(go);
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }

    public PlanetScript GetPlanet(){
        return planet;
    }

    public void DestroyAllChains(){
        foreach(GameObject go in chains){
            Destroy(go);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(GetComponent<Rigidbody2D>().velocity.magnitude > 0.2f && (transform.position-Vector3.zero).magnitude < 11f){
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = collideSFX;
        }
        if (col.gameObject.GetComponent<GemScript>() != null && GetComponent<PlanetScript>() != null){
            GetComponent<Rigidbody2D>().velocity *= 0.75f;
            col.gameObject.GetComponent<Rigidbody2D>().velocity *= 0.75f;
        }
        if (col.gameObject.GetComponent<GemScript>() != null && (col.gameObject.GetComponent<GemScript>().shape == shape || shape == -1 || col.gameObject.GetComponent<GemScript>().shape == -1)) { 
            chains.Add(Instantiate<GameObject>(chain));
            connected.Add(col.gameObject);
            if (gameObject.name.CompareTo(col.gameObject.name) < 0f && (transform.position-Vector3.zero).magnitude < 11f){
                GameObject snd2 = Instantiate<GameObject>(soundPrefab);
                snd2.GetComponent<SFXScript>().sfx = connectSFX;
            }
        } else if (col.gameObject.GetComponent<PlanetScript>() != null && col.gameObject.GetComponent<PlanetScript>().match != -1 && (col.gameObject.GetComponent<PlanetScript>().match == shape || shape == -1)){
            chains.Add(Instantiate<GameObject>(chain));
            connected.Add(col.gameObject);
            if ((transform.position-Vector3.zero).magnitude < 11f){
                GameObject snd2 = Instantiate<GameObject>(soundPrefab);
                snd2.GetComponent<SFXScript>().sfx = connectSFX;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        int index = connected.IndexOf(col.gameObject);
        if (index >= 0){
            connected.RemoveAt(index);
            Destroy(chains[index]);
            chains.RemoveAt(index);
        }
    }
}
