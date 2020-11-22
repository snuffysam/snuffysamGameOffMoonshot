using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWildCard : SpecialMove
{
    public GameObject wildCardPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "WILD CARD";
    }

    // Update is called once per frame
    protected override void UseSpecial(){
        GameObject go = Instantiate<GameObject>(wildCardPrefab);
        Vector3 towardsPlanet = Vector2.zero;
        PlanetScript planet = GetComponent<CannonScript>().GetPlanet();
        if (planet != null){
            towardsPlanet = planet.transform.position - transform.position;
            towardsPlanet = towardsPlanet.normalized;
        }
        go.transform.position = transform.position + towardsPlanet*CannonScript.spawnDist;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = new Vector3(1f, 1f, 1f)*0.1f;
        go.GetComponent<Rigidbody2D>().velocity = (new Vector3(0f, 0f, 0f)-transform.position).normalized;
        FindObjectOfType<ScoreTimerScript>().ResetMultiplier();
    }
}
