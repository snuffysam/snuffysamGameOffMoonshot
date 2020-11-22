using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBomb : SpecialMove
{
    public GameObject bombPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "DROP BOMB";
    }

    protected override void UseSpecial(){
        GameObject go = Instantiate<GameObject>(bombPrefab);
        Vector3 towardsPlanet = Vector2.zero;
        PlanetScript planet = GetComponent<CannonScript>().GetPlanet();
        if (planet != null){
            towardsPlanet = planet.transform.position - transform.position;
            towardsPlanet = towardsPlanet.normalized;
        }
        go.transform.position = transform.position + towardsPlanet*0.75f;
        go.transform.rotation = transform.rotation;
    }
}
