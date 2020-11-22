using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public GameObject soundPrefab;
    public AudioClip clickSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(){
        GameObject snd = Instantiate<GameObject>(soundPrefab);
        snd.GetComponent<SFXScript>().sfx = clickSFX;

        SceneManager.LoadScene("MenuScene");
    }
}
