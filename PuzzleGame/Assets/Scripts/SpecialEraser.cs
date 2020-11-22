using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEraser : SpecialMove
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "BLOCK ERASE";
    }

    protected override void UseSpecial(){
        GameObject gem = GetComponent<CannonScript>().SpawnGem();

        int shapeMatch = gem.GetComponent<GemScript>().shape;

        int erasedBlocks = 0;

        GemScript[] allGems = FindObjectsOfType<GemScript>();
        for (int i = 0; i < allGems.Length; i++){
            if (allGems[i].shape == shapeMatch){
                Destroy (allGems[i].gameObject);
                erasedBlocks++;
            }
        }

        FindObjectOfType<ScoreTimerScript>().AddScore(erasedBlocks);
    }
}