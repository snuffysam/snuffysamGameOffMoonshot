using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBigGem : SpecialMove
{
    public GameObject explosionPrefab;
    public GameObject soundPrefab;
    public AudioClip explodeSFX;
    public AudioClip throwSFX;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "BIG BLOCK";
    }

    protected override void UseSpecial(){
        base.UseSpecial();

        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = throwSFX;

        GameObject gem = GetComponent<CannonScript>().SpawnGem();
        gem.transform.localScale = new Vector3(1f, 1f, 1f)*2.5f;
        gem.GetComponent<Rigidbody2D>().mass = 2.5f;
        gem.AddComponent<PlanetScript>();
        gem.GetComponent<PlanetScript>().explosionEffectPrefab = explosionPrefab;
        gem.GetComponent<PlanetScript>().explosionLevel = 2.5f;
        gem.GetComponent<PlanetScript>().match = gem.GetComponent<GemScript>().shape;
        gem.GetComponent<PlanetScript>().toSpawn = new GameObject[0];
        gem.GetComponent<PlanetScript>().soundPrefab = soundPrefab;
        gem.GetComponent<PlanetScript>().explodeSFX = explodeSFX;
        
        gem.AddComponent<WrapScreen>();

        GetComponent<CannonScript>().ResetPlanets();
        GemScript[] allGems = FindObjectsOfType<GemScript>();
        for (int i = 0; i < allGems.Length; i++){
            allGems[i].ResetPlanets();
        }
    }
}
