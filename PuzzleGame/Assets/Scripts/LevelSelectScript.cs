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
    private int levelIndex;
    public static int currentMenu = 1;
    public GameObject soundPrefab;
    public AudioClip clickSFX;
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
                    for (int k = 0; k < 4; k++){
                        characterSelects[i*4+k].gameObject.SetActive(false);
                    }
                }
            } else {
                contractNames[i].text = "Locked!";
                profits[i].gameObject.SetActive(false);
                icons[i].gameObject.SetActive(false);
                buttons[i].gameObject.SetActive(false);
            }
        }
        LevelSelectScript.currentMenu = levelIndex;
    }

    public void SelectLevel(int offset){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;
        
        GameObject selection = Instantiate<GameObject>(levelSetups[levelIndex + offset]);
        if (selection.GetComponent<LevelSetupScript>().loadSong != null){
            FindObjectOfType<Jukebox>().PlaySong(selection.GetComponent<LevelSetupScript>().loadSong);
        }
        SceneManager.LoadScene("SampleScene");
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
        FindObjectOfType<DataTracker>().selectedShip = shipPrefab;
    }
}
