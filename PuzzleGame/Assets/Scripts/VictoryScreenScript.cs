using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenScript : MonoBehaviour
{
    public float delayTimer;
    public ScoreTimerScript scoreTimerScript;
    public Text finalScoreText, blocksUsedText, blocksUsedMultiplier, blocksUsedScaled, timeBonusText, timeBonusMultiplier, timeBonusScaled, profitText;
    public GameObject retryButton, nextButton, quitButton;
    public GameObject soundPrefab;
    public AudioClip appearSFX, profitSFX;
    private int index = 0;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        finalScoreText.text = "SCORE: " + scoreTimerScript.GetScore();
        blocksUsedText.text = "BLOCKS USED: " + scoreTimerScript.GetBlockUsed();
        blocksUsedMultiplier.text = "x " + scoreTimerScript.GetBlockScale();
        blocksUsedScaled.text = "= " + (scoreTimerScript.GetBlockUsed() * scoreTimerScript.GetBlockScale());
        timeBonusText.text = "TIME BONUS: " + scoreTimerScript.GetTimer();
        timeBonusMultiplier.text = "x " + scoreTimerScript.GetTimeScale();
        timeBonusScaled.text = "= " + (scoreTimerScript.GetTimeScale() * scoreTimerScript.GetTimer());
        profitText.text = "PROFIT: $" + scoreTimerScript.GetProfit();

        timer += Time.deltaTime;
        if (timer > delayTimer){
            timer = 0f;
            if (index == 0){
                finalScoreText.gameObject.SetActive(true);
            } else if (index == 1){
                blocksUsedText.gameObject.SetActive(true);
            } else if (index == 2){
                blocksUsedMultiplier.gameObject.SetActive(true);
            } else if (index == 3){
                blocksUsedScaled.gameObject.SetActive(true);
            } else if (index == 4){
                timeBonusText.gameObject.SetActive(true);
            } else if (index == 5){
                timeBonusMultiplier.gameObject.SetActive(true);
            } else if (index == 6){
                timeBonusScaled.gameObject.SetActive(true);
            } else if (index == 7){
                profitText.gameObject.SetActive(true);
            } else if (index == 8){
                retryButton.SetActive(true);
                nextButton.SetActive(true);
                quitButton.SetActive(true);
            }

            if (index == 7){
                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = profitSFX;
            } else if (index < 9) {
                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = appearSFX;
            }
            index++;
        }
    }
}
