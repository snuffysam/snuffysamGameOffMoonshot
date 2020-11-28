using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreTimerScript : MonoBehaviour
{
    public static bool isPaused;

    public Text scoreText;
    public Text timerText;
    public Text specialMoveText;
    public GameObject victoryScreen;
    public GameObject failureScreen;
    public GameObject pauseScreen;
    public GameObject textBox;
    public Text textBoxText;
    public Text missionNameText;
    public float timeToDelay = 1f;
    private int score;
    public int timerStart;
    public Image specialMoveIcon;
    public Image portraitIcon;
    private float timer;
    private int multiplier;
    private LevelSetupScript levelSetupScript;
    private int blocksUsed;
    private int scoreScale = 1000;
    private int blocksUsedScale = -10;
    private int timeScale = 100;
    private PlanetScript planet;
    private float levelWinTimer;
    private float levelLoseTimer;
    private string nextLevel;
    private SpecialMove spMove;
    private bool canPressPause;
    private bool canPressRestart;
    private string textTip;
    private string missionName;
    private int levelIndex = -1;
    bool setTimer = false;
    public GameObject soundPrefab;
    public AudioClip clickSFX;
    private bool timeDrain = false;
    private bool stopTimer = false;
    private Sprite[] portraits;
    private float setPortraitTimer;
    private int setPortraitIndex;
    public Text controlsTutorialText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        timer = timerStart;
        multiplier = 1;
        blocksUsed = 0;

        LevelSetupScript lss = FindObjectOfType<LevelSetupScript>();
        if (lss != null){
            FindObjectOfType<Jukebox>().PlaySong(lss.loadSong);
            FindObjectOfType<Jukebox>().SetLoop(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Pause") == 0){
            canPressPause = true;
        }
        if (canPressPause && Input.GetAxisRaw("Pause") > 0 && !victoryScreen.activeInHierarchy && !failureScreen.activeInHierarchy){
            canPressPause = false;
            SetPaused();
        }
        if (Input.GetAxisRaw("Restart") == 0){
            canPressRestart = true;
        }
        if (canPressRestart && Input.GetAxisRaw("Restart") > 0){
            canPressRestart = false;
            RetryLevel();
        }
        if (textTip != null && textTip.Length > 0 && !victoryScreen.activeInHierarchy && !failureScreen.activeInHierarchy && !pauseScreen.activeInHierarchy){
            textBox.SetActive(true);
        } else {
            textBox.SetActive(false);
        }
        if (isPaused){
            return;
        }


        if (portraits == null){
            portraits = FindObjectOfType<CannonScript>().portraits;
        }
        if (portraits != null){
            setPortraitTimer -= Time.deltaTime;
            if (timeDrain){
                if (failureScreen.activeInHierarchy){
                    portraitIcon.sprite = portraits[7];
                } else {
                    portraitIcon.sprite = portraits[6];
                }
            } else if (failureScreen.activeInHierarchy){
                portraitIcon.sprite = portraits[5];
            } else if (victoryScreen.activeInHierarchy){
                portraitIcon.sprite = portraits[3];
            } else if (setPortraitTimer > 0f){
                portraitIcon.sprite = portraits[setPortraitIndex];
            } else if (timer < 20f){
                portraitIcon.sprite = portraits[4];
            } else {
                portraitIcon.sprite = portraits[0];
            }
        }

        if (levelSetupScript == null){
            levelSetupScript = FindObjectOfType<LevelSetupScript>();
            if (levelSetupScript != null){
                timerStart = levelSetupScript.startTime;
                if (!setTimer){
                    timer = timerStart;
                    setTimer = true;
                }
                textTip = levelSetupScript.missionTip;
                missionName = levelSetupScript.missionName;
                levelIndex = levelSetupScript.index;
            }
        }

        if (DataTracker.originalControls){
            controlsTutorialText.text = "STEER: Left/Right\nTHRUST: Up\nBRAKE: Down\nSHOOT BLOCK: Spacebar\nSPECIAL: Left Shift\nRETRY: R\nPAUSE: Escape";
        } else {
            controlsTutorialText.text = "FLY SHIP: Left/Right/Up/Down\nSHOOT BLOCK: Spacebar\nSPECIAL: Left Shift\nRETRY: R\nPAUSE: Escape";
        }

        LevelSelectScript.currentMenu = levelIndex;
        textBoxText.text = textTip;
        missionNameText.text = "CONTRACT: " + missionName;

        if (planet == null && timer > 0f){
            PlanetScript[] allPlanets = FindObjectsOfType<PlanetScript>();
            for (int i = 0; i < allPlanets.Length; i++){
                if (allPlanets[i] != null && allPlanets[i].match >= 0){
                    planet = allPlanets[i];
                    break;
                }
            }
            levelWinTimer += Time.deltaTime;

            if (levelWinTimer > timeToDelay){
                victoryScreen.SetActive(true);
                SetHighScore();
            }
        } else {
            levelWinTimer = 0f;

            float mult = 1f;
            if (timeDrain){
                mult = 600f;
            }
            if (stopTimer){
                mult = 0f;
            }
            timer -= Time.deltaTime*mult;
            if (timer < 0){
                timer = 0;

                levelLoseTimer += Time.deltaTime;
                if (levelLoseTimer > timeToDelay){
                    failureScreen.SetActive(true);
                }
            }
        }

        int roundedTimer = (int)(Mathf.Ceil(timer));
        int minutes = roundedTimer/60;
        int seconds = roundedTimer%60;
        string minStr = "" + minutes;
        if (minutes < 10){
            minStr = "0" + minStr;
        }
        string secStr = "" + seconds;
        if (seconds < 10){
            secStr = "0" + secStr;
        }
        timerText.text = minStr + ":" + secStr;
        scoreText.text = "SCORE: " + score;

        if (spMove == null){
            specialMoveText.text = "";
            spMove = FindObjectOfType<SpecialMove>();
        } else {
            string str = "SPECIAL : " + spMove.displayName;
            if (spMove.GetCanBeUsed()){
                str += "\n[Press Left Shift]";
                specialMoveIcon.sprite = spMove.iconReady;
            } else {
                str += "\nAlready used!";
                specialMoveIcon.sprite = spMove.iconUsed;
            }
            specialMoveText.text = str;
        }
    }

    public void SetHighScore(){
        DataTracker.SetScore(levelIndex, GetProfit());
    }

    public void ResetMultiplier(){
        multiplier = 1;
        blocksUsed++;
        if (FindObjectOfType<SpecialChain>() != null){
            FindObjectOfType<SpecialChain>().EndChain();
        }
    }

    public void AddScore(int chainLength){
        if (victoryScreen.activeInHierarchy || failureScreen.activeInHierarchy){
            return;
        }

        score += chainLength*multiplier*scoreScale;
        multiplier++;
        SetPortrait(2);
    }

    public int GetScoreScale(){
        return scoreScale;
    }

    public int GetProfit(){
        return score + (blocksUsed * blocksUsedScale) + ((int)(Mathf.Ceil(timer))*timeScale);
    }

    public int GetScore(){
        return score;
    }

    public int GetBlockScale(){
        return blocksUsedScale;
    }

    public int GetBlockUsed(){
        return blocksUsed;
    }

    public int GetTimer(){
        return (int)(Mathf.Ceil(timer));
    }

    public int GetTimeScale(){
        return timeScale;
    }

    public void RetryLevel(){
        if (isPaused){
            SetPaused();
        } else {
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = clickSFX;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(){
        if (isPaused){
            SetPaused();
        } else {
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = clickSFX;
        }
        LevelSetupScript lss = FindObjectOfType<LevelSetupScript>();
        GameObject nextLevelSetup = null;
        if (lss != null){
            nextLevel = lss.nextLevel;
            nextLevelSetup = lss.nextLevelPrefab;
            Destroy(lss.gameObject);
        }
        if (nextLevelSetup != null){
            LevelSelectScript.currentMenu = nextLevelSetup.GetComponent<LevelSetupScript>().index;
            if (nextLevel.Equals(SceneManager.GetActiveScene().name)){
                Instantiate<GameObject>(nextLevelSetup);
            }
        }
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitLevel(){
        if (isPaused){
            SetPaused();
        } else {
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = clickSFX;
        }
        LevelSetupScript lss = FindObjectOfType<LevelSetupScript>();
        string quitLevel = "";
        if (lss != null){
            quitLevel = lss.quitLevel;
            Destroy(lss.gameObject);
        }
        SceneManager.LoadScene(quitLevel);
    }

    public void SetPaused(){
        isPaused = !isPaused;
        if (isPaused){
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        } else {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            GameObject snd = Instantiate<GameObject>(soundPrefab);
            snd.GetComponent<SFXScript>().sfx = clickSFX;
        }
    }

    public void KillPlayer(){
        timeDrain = true;
    }

    public void StopTimer(){
        stopTimer = true;
    }

    public void SetPortrait(int index){
        setPortraitIndex = index;
        setPortraitTimer = 5f;
    }
}
