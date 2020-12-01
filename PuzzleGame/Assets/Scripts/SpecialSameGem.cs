using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSameGem : SpecialMove
{
    public GameObject soundPrefab;
    public AudioClip specialSFX;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "CLONE BLOCKS";
    }

    protected override void UseSpecial(){
        base.UseSpecial();

        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = specialSFX;

        GemGenerator gg = FindObjectOfType<GemGenerator>();
        if (gg == null){
            return;
        }
        gg.CloneGems();
    }
}
