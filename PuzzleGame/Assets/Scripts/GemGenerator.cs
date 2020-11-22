using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] gemSprites;
    public int[] blockedGems;
    private List<int> nextGems;

    public Image[] gemDisplay;

    private LevelSetupScript levelSetupScript;
    private bool RNG_order;
    void Start()
    {
        nextGems = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelSetupScript == null){
            levelSetupScript = FindObjectOfType<LevelSetupScript>();
            if (levelSetupScript != null){
                blockedGems = levelSetupScript.blockedGems;
                RNG_order = levelSetupScript.RNG_order;
            }
        }
        while (nextGems.Count <= gemSprites.Length){
            int n = nextGems.Count;
            GenerateGems();
            if (nextGems.Count == n){
                return;
            }
        }
        for (int i = 0; i < gemDisplay.Length; i++){
            gemDisplay[i].sprite = gemSprites[nextGems[i]];
        }
    }

    public int GetCurrentGem(){
        return nextGems[0];
    }

    public void RemoveCurrentGem(){
        nextGems.RemoveAt(0);
    }

    public void GenerateGems(){
        if (RNG_order){
            GenerateGemsRNG();
        } else {
            GenerateGemsOrder();
        }
    }

    public void GenerateGemsRNG(){
        int[] indices = new int[gemSprites.Length];

        for (int i = 0; i < indices.Length; i++){
            indices[i] = i;
        }

        for (int n = indices.Length - 1; n > 0; --n)
        {
            int k = Random.Range(0,n+1);
            int temp = indices[n];
            indices[n] = indices[k];
            indices[k] = temp;
        }

        for (int i = 0; i < indices.Length; i++){
            nextGems.Add(indices[i]);
        }

        for (int i = 0; i < blockedGems.Length; i++){
            for (int k = 0; k < nextGems.Count; k++){
                if (nextGems[k] == blockedGems[i]){
                    nextGems.RemoveAt(k);
                    k--;
                }
            }
        }
    }

    public void GenerateGemsOrder(){
        for (int i = 0; i < blockedGems.Length; i++){
            nextGems.Add(blockedGems[i]);
        }
    }

    public void CloneGems(){
        for (int i = 0; i < gemDisplay.Length && i < nextGems.Count; i++){
            nextGems[i] = nextGems[0];
        }
    }
}
