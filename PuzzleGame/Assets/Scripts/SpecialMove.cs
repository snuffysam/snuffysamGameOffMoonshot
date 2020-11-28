using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    public string displayName;
    public Sprite iconReady, iconUsed;
    private bool canPress;
    private float pressTimer = 0f;
    private bool canBeUsed = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetAxis("Fire2") == 0){
            pressTimer += Time.deltaTime;
            if (pressTimer > 0.1f){
                canPress = true;
            }
        }
        if (Input.GetAxis("Fire2") > 0 && canPress && canBeUsed){
            canPress = false;
            pressTimer = 0f;
            UseSpecial();
            canBeUsed = false;
        }
    }

    protected virtual void UseSpecial(){
        FindObjectOfType<ScoreTimerScript>().SetPortrait(1);
    }

    public bool GetCanBeUsed(){
        return canBeUsed;
    }
}
