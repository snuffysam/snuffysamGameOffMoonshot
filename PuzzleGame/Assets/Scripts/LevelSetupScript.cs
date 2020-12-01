using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetupScript : MonoBehaviour
{
    public GameObject puzzlePrefab;
    public GameObject cannonPrefab;
    public int[] blockedGems;
    public int startTime;
    public string nextLevel;
    public string quitLevel;
    public GameObject nextLevelPrefab;
    public bool RNG_order;
    public string missionName;
    public string missionTip;
    public AudioClip loadSong;
    public bool isBossBattle;
    public int index;
    public GameObject startingCutscene;
    public GameObject endingCutscene;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        GameObject go = Instantiate<GameObject>(puzzlePrefab);
        go.transform.position = Vector3.zero;
        if (cannonPrefab == null){
            cannonPrefab = FindObjectOfType<DataTracker>().selectedShip;
        }
        Instantiate<GameObject>(cannonPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
