using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    public GameObject[] levelSetups;
    public Text[] contractNumbers;
    public Text[] contractNames;
    public Text[] profits;
    public Image[] icons;
    public Button[] buttons;
    public Button[] characterSelects;
    public GameObject leftArrow,rightArrow;
    public GameObject optionsMenu;
    private int levelIndex;
    public static int currentMenu = 1;
    public GameObject soundPrefab;
    public AudioClip clickSFX;
    public Sprite lockedIcon;
    public Image controlsButton;
    public Sprite[] controlsSprites;
    public AudioClip menuSong;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levelSetups.Length; i++){
            if (i < levelSetups.Length-1){
                levelSetups[i].GetComponent<LevelSetupScript>().nextLevelPrefab = levelSetups[i+1];
            } else {
                levelSetups[i].GetComponent<LevelSetupScript>().nextLevel = levelSetups[i].GetComponent<LevelSetupScript>().quitLevel;
            }
            levelSetups[i].GetComponent<LevelSetupScript>().index = i;
        }
        levelIndex = LevelSelectScript.currentMenu;
        if (levelIndex > levelSetups.Length - 2){
            levelIndex = levelSetups.Length-2;
        }
        if (levelIndex < 1){
            levelIndex = 1;
        }
            FindObjectOfType<Jukebox>().PlaySong(menuSong);
            FindObjectOfType<Jukebox>().SetLoop(true);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++){
            LevelSetupScript lss = levelSetups[levelIndex+i-1].GetComponent<LevelSetupScript>();
            contractNumbers[i].text = "CONTRACT #" + (levelIndex+i);
            if (lss.isBossBattle){
                contractNumbers[i].text += "\n[BOSS BATTLE]";
            }
            if (DataTracker.IsUnlocked(levelIndex+i-1)){
                contractNames[i].text = lss.missionName;
                profits[i].text = "PROFIT: $" + DataTracker.GetScore(levelIndex+i-1);
                profits[i].gameObject.SetActive(true);
                buttons[i].gameObject.SetActive(true);
                if (lss.cannonPrefab == null){
                    icons[i].gameObject.SetActive(false);
                    for (int k = 0; k < 4; k++){
                        characterSelects[i*4+k].gameObject.SetActive(true);
                    }
                } else {
                    icons[i].gameObject.SetActive(true);
                    icons[i].sprite = lss.cannonPrefab.GetComponent<CannonScript>().portraits[0];
                    for (int k = 0; k < 4; k++){
                        characterSelects[i*4+k].gameObject.SetActive(false);
                    }
                }
            } else {
                contractNames[i].text = "Locked!";
                profits[i].gameObject.SetActive(false);
                icons[i].gameObject.SetActive(true);
                icons[i].sprite = lockedIcon;
                buttons[i].gameObject.SetActive(false);
                for (int k = 0; k < 4; k++){
                    characterSelects[i*4+k].gameObject.SetActive(false);
                }
            }
        }
        if (levelIndex == 1){
            leftArrow.SetActive(false);
        } else {
            leftArrow.SetActive(true);
        }
        if (levelSetups.Length - 2 == 1){
            rightArrow.SetActive(false);
        } else {
            rightArrow.SetActive(true);
        }
        LevelSelectScript.currentMenu = levelIndex;

        if (DataTracker.originalControls){
            controlsButton.sprite = controlsSprites[0];
        } else {
            controlsButton.sprite = controlsSprites[1];
        }
    }

    public void SelectLevel(int offset){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;
        
        if (levelSetups[levelIndex + offset].GetComponent<LevelSetupScript>().startingCutscene == null){
            GameObject selection = Instantiate<GameObject>(levelSetups[levelIndex + offset]);
            if (selection.GetComponent<LevelSetupScript>().loadSong != null){
                FindObjectOfType<Jukebox>().PlaySong(selection.GetComponent<LevelSetupScript>().loadSong);
            }
            SceneManager.LoadScene("SampleScene");
        } else {
            GameObject selection = Instantiate<GameObject>(levelSetups[levelIndex + offset].GetComponent<LevelSetupScript>().startingCutscene);
            selection.GetComponent<CutsceneData>().setupPrefab = levelSetups[levelIndex + offset];
            SceneManager.LoadScene("CutsceneScene");
        }
    }

    public void IndexPlus(){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;

        levelIndex++;
        if (levelIndex > levelSetups.Length - 2){
            levelIndex = levelSetups.Length-2;
        }
    }

    public void IndexMinus(){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;

        levelIndex--;
        if (levelIndex < 1){
            levelIndex = 1;
        }
    }

    public void SelectShip(GameObject shipPrefab){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;

        FindObjectOfType<DataTracker>().selectedShip = shipPrefab;
    }

    public void ToggleSimpleControls(){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;
        
        DataTracker.originalControls = !DataTracker.originalControls;
    }

    public void ToggleOptionsMenu(){
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;
    }

    public void TogglePlaytestMode(){        
        FindObjectOfType<DataTracker>().ToggleTestMode();
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;
    }
}
