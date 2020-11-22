using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchDisplay : MonoBehaviour
{
    public Sprite[] gemSprites;
    private PlanetScript planet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (planet == null){
            PlanetScript[] allPlanets = FindObjectsOfType<PlanetScript>();
            for (int i = 0; i < allPlanets.Length; i++){
                if (allPlanets[i] != null && allPlanets[i].match > -1){
                    planet = allPlanets[i];
                    break;
                }
            }
        } else {
            GetComponent<Image>().sprite = gemSprites[planet.match];
        }
    }
}
