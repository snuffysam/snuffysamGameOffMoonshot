using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalCutsceneControl : MonoBehaviour
{
    public GameObject creditsA, creditsB;
    private GameObject scoreObject;
    private int scoreSum;
    private CutsceneController cc;
    private bool alreadySpawned = false;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreSum == 0){
            scoreSum = DataTracker.GetSum();
        }

        if (!alreadySpawned){
            alreadySpawned = true;
            if (scoreSum >= 1000000){
                Instantiate<GameObject>(creditsB);
            } else {
                Instantiate<GameObject>(creditsA);
            }
        }

        if (cc == null){
            cc = FindObjectOfType<CutsceneController>();
        } else {
            if (scoreObject == null){
                scoreObject = cc.moneyPanel;
            } else {
                scoreObject.GetComponentInChildren<Text>().text = "$" + scoreSum;
            }
            if (cc.CurrentIndex() == 11){
                scoreObject.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
