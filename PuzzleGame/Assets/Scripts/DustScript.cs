using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustScript : MonoBehaviour
{
    private PlanetScript planet;
    private PlanetScript[] allPlanets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + towardsPlanet.normalized*0.5f*Time.deltaTime*planet.GetComponent<Rigidbody2D>().mass*gravDir;
        }
    }
}
