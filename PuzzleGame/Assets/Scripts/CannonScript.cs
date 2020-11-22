using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject[] gemPrefabs;
    public float distance;
    public Sprite[] frames;
    public GameObject soundPrefab;
    public AudioClip brakeSFX;
    private int index;
    private bool canPressFire = false;
    private GemGenerator gemGenerator;
    private PlanetScript planet;
    private float pressTimer;
    private VictoryScreenScript vss;
    private GameObject failScreen;
    private PlanetScript[] allPlanets;
    private int spriteIndex;
    private float spriteTimer;
    public static float spawnDist = 0.5f;
    private bool braking;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-distance, 0f, 0f);
        gemGenerator = FindObjectOfType<GemGenerator>();
        pressTimer = 0f;
        braking = true;
    }

    public void ResetPlanets(){
        allPlanets = null;
    }

    public PlanetScript GetPlanet(){
        return planet;
    }

    // Update is called once per frame
    void Update()
    {
        if (vss == null){
            vss = FindObjectOfType<VictoryScreenScript>();
        } else {
            if (vss.gameObject.activeInHierarchy){
                return;
            }
        }
        if (failScreen == null){
            failScreen = GameObject.FindWithTag("Finish");
        } else {
            if (failScreen.activeInHierarchy){
                return;
            }
        }

        if (gemGenerator == null){
            return;
        }

        index = gemGenerator.GetCurrentGem();
        //float turnAngle = 0f;
        //if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0){
        //    turnAngle = Vector2.SignedAngle(new Vector2(transform.position.x, transform.position.y),new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        //}
        //if (turnAngle < 0){
        //    transform.RotateAround(new Vector3(0f,0f,0f), new Vector3(0f,0f,1f), -50f*Time.deltaTime);
        //}
        //if (turnAngle > 0){
        //    transform.RotateAround(new Vector3(0f,0f,0f), new Vector3(0f,0f,1f), 50f*Time.deltaTime);
        //}

        //float turnAngle = 0f;
        //if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0){
        //    turnAngle = Vector2.SignedAngle(new Vector2(transform.up.x,transform.up.y),new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        //}
        float turnSpeed = 165f;
        if (Input.GetAxis("Horizontal") > 0){
            transform.RotateAround(transform.position, new Vector3(0f,0f,1f), -turnSpeed*Time.deltaTime);
        }
        if (Input.GetAxis("Horizontal") < 0){
            transform.RotateAround(transform.position, new Vector3(0f,0f,1f), turnSpeed*Time.deltaTime);
        }

        GetComponent<Rigidbody2D>().angularVelocity = GetComponent<Rigidbody2D>().angularVelocity*0.5f;

        float thrust = 0.005f*40f*Time.deltaTime*60f;
        thrust = 0.005f*7f*Time.deltaTime*60f;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (Input.GetAxis("Vertical") > 0){
            //GetComponent<Rigidbody2D>().velocity -= GetComponent<Rigidbody2D>().velocity*0.07f*Time.deltaTime*60f;
            Vector2 front = new Vector2(transform.up.x,transform.up.y);
            Vector2 vel = (GetComponent<Rigidbody2D>().velocity.normalized + front*Time.deltaTime*60f*0.11f).normalized * GetComponent<Rigidbody2D>().velocity.magnitude;
            GetComponent<Rigidbody2D>().velocity = vel;
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + front*thrust;

            spriteTimer += Time.deltaTime;
            if (spriteTimer > 0.2f){
                spriteTimer = 0f;
                spriteIndex++;
                if (spriteIndex > 3){
                    spriteIndex = 0;
                }
            }
            if (spriteIndex % 2 == 0){
                spr.sprite = frames[1];
            } /*else if (spriteIndex == 2){
                spr.sprite = frames[3];
            }*/ else {
                spr.sprite = frames[3];
            }
            GetComponent<AudioSource>().volume = Jukebox.sfxVolume;
            if (!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().Play();
            }
        } else {
            spr.sprite = frames[0];
            spriteIndex = 0;
            spriteTimer = 0f;
            GetComponent<AudioSource>().Stop();
        }
        float maxSpeed = 5;
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed){
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }

        if (Input.GetAxis("Vertical") < 0){
            GetComponent<Rigidbody2D>().velocity -= GetComponent<Rigidbody2D>().velocity*0.15f*Time.deltaTime*60f;
            if (braking){
                braking = false;
                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = brakeSFX;
            }
        } else {
            braking = true;
        }

        bool wrapped = false;
        if (transform.position.x > 9.5f){
            transform.position = new Vector3(transform.position.x-(9.5f*2), transform.position.y, transform.position.z);
            wrapped = true;
        }
        if (transform.position.y > 5.6f){
            transform.position = new Vector3(transform.position.x, transform.position.y-(5.6f*2), transform.position.z);
            wrapped = true;
        }
        if (transform.position.x < -9.5f){
            transform.position = new Vector3(transform.position.x+(9.5f*2), transform.position.y, transform.position.z);
            wrapped = true;
        }
        if (transform.position.y < -5.6f){
            transform.position = new Vector3(transform.position.x, transform.position.y+(5.6f*2), transform.position.z);
            wrapped = true;
        }
        if (wrapped && GetComponent<SpecialChain>() != null){
            GetComponent<SpecialChain>().EndChain();
        }

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
            float gravDir = 1f;
            if (planet.antigravity && towardsPlanet.magnitude < planet.GetComponent<Rigidbody2D>().mass){
                gravDir = -1f;
            }
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*planet.GetComponent<Rigidbody2D>().mass*Time.deltaTime*0.4f*gravDir;
        }

        //GetComponent<SpriteRenderer>().sprite = gemPrefabs[index].GetComponent<SpriteRenderer>().sprite;

        if (Input.GetAxis("Fire1") == 0){
            pressTimer += Time.deltaTime;
            if (pressTimer > 0.1f){
                canPressFire = true;
            }
        }
        if (Input.GetAxis("Fire1") > 0 && canPressFire){
            canPressFire = false;
            pressTimer = 0f;
            SpawnGem();
        }
    }

    public GameObject SpawnGem(){
        GameObject go = Instantiate<GameObject>(gemPrefabs[index]);
        Vector3 towardsPlanet = Vector2.zero;
        if (planet != null){
            towardsPlanet = planet.transform.position - transform.position;
            towardsPlanet = towardsPlanet.normalized;
        }
        go.transform.position = transform.position + towardsPlanet*CannonScript.spawnDist;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = new Vector3(1f, 1f, 1f)*0.1f;
        go.GetComponent<Rigidbody2D>().velocity = (new Vector3(0f, 0f, 0f)-transform.position).normalized;
        gemGenerator.RemoveCurrentGem();
        FindObjectOfType<ScoreTimerScript>().ResetMultiplier();
        return go;
    }
}
