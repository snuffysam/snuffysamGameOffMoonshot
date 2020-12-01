using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public int levelCount;
    public bool testMode;
    private static int[] levelScores;
    public GameObject selectedShip;
    public static bool originalControls = true;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        levelScores = new int[levelCount];
        testMode = !testMode;
        ToggleTestMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetScore(int index, int score){
        if (score > levelScores[index]){
            levelScores[index] = score;
        }
    }

    public static int GetScore(int index){
        return levelScores[index];
    }

    public static int GetSum(){
        int sum = 0;
        for (int i = 0; i < levelScores.Length; i++){
            sum += levelScores[i];
        }
        return sum;
    }

    public static bool IsUnlocked(int index){
        if (index == 0){
            return true;
        }
        if (index < levelScores.Length && levelScores[index-1] == 0){
            return false;
        }
        return true;
    }

    public void ToggleTestMode(){
        testMode = !testMode;
        if (testMode){
            for (int i = 0; i < levelScores.Length; i++){
                if (levelScores[i] == 0){
                    levelScores[i] = 1;
                }
            }
        } else {
            for (int i = 0; i < levelScores.Length; i++){
                if (levelScores[i] == 1){
                    levelScores[i] = 0;
                }
            }
        }
    }
}
