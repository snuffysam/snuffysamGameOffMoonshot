using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public GameObject cannonPrefab;
    private DataTracker dataTracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dataTracker == null){
            dataTracker = FindObjectOfType<DataTracker>();
        } else {
            if (cannonPrefab == dataTracker.selectedShip){
                GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            } else {
                GetComponent<Image>().color = new Color(0.33f, 0.33f, 0.45f, 1f);
            }
        }
    }

    public void SetShip(){
        FindObjectOfType<LevelSelectScript>().SelectShip(cannonPrefab);
    }
}
