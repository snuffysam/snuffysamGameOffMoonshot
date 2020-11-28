using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSameGem : SpecialMove
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "CLONE BLOCKS";
    }

    protected override void UseSpecial(){
        base.UseSpecial();
        GemGenerator gg = FindObjectOfType<GemGenerator>();
        if (gg == null){
            return;
        }
        gg.CloneGems();
    }
}
