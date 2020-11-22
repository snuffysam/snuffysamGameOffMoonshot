using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectScript : MonoBehaviour
{
    public float delayTimer;
    public Sprite frame1;
    private float delayTimerCurrent = 0f;
    private float frameTimer = 0.15f;
    private float frameTimerCurrent = 0f;
    private bool frameChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = transform.localScale*(delayTimer*2f+0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        delayTimerCurrent += Time.deltaTime;
        if (delayTimerCurrent > delayTimer){
            GetComponent<SpriteRenderer>().enabled = true;
            frameTimerCurrent += Time.deltaTime;
            if (frameTimerCurrent > frameTimer){
                if (!frameChanged){
                    frameTimerCurrent = 0f;
                    frameChanged = true;
                    GetComponent<SpriteRenderer>().sprite = frame1;
                } else {
                    Destroy (this.gameObject);
                }
            }
        }
    }
}
