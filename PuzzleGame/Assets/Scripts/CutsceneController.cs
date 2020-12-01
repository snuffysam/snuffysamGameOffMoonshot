using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public Image artwork;
    public Text textBox;
    public Font fontNormal, fontEmphasized;
    public GameObject moneyPanel;
    private bool fadeDir;
    private float fade;
    private float fadeTime = 0.2f;
    private float autoTime;
    private Sprite[] art;
    private string[] text;
    private bool[] textEmphasized;
    private AudioClip[] music;
    private bool[] musicLoop;
    private GameObject setupPrefab;
    private int index;
    private bool canPressNext;
    private Jukebox jukebox;
    private bool startedPlayingSong;
    private float autoTimer;
    // Start is called before the first frame update
    void Start()
    {
        fadeDir = true;
        fade = 0f;
        index = 0;

        artwork.color = new Color(1f, 1f, 1f, 0f);
        textBox.color = new Color(1f, 1f, 1f, 0f);

        CutsceneData cd = FindObjectOfType<CutsceneData>();
        autoTime = cd.autoTime;
        art = cd.art;
        text = cd.text;
        textEmphasized = cd.textEmphasized;
        music = cd.music;
        musicLoop = cd.musicLoop;
        setupPrefab = cd.setupPrefab;
        Destroy(cd.gameObject);

        jukebox = FindObjectOfType<Jukebox>();
        startedPlayingSong = false;
        autoTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeDir){
            fade += (Time.deltaTime/fadeTime)*0.75f;
            if (fade > 1f){
                fade = 1f;
            }
            if (artwork.color.a < fade){
                artwork.color = new Color(1f, 1f, 1f, fade);
            }
            if (textBox.color.a < fade){
                textBox.color = new Color(1f, 1f, 1f, fade);
            }
            artwork.sprite = art[index];
            textBox.text = text[index];
            if (textEmphasized[index]){
                textBox.font = fontEmphasized;
                textBox.fontSize = 44;
            } else {
                textBox.font = fontNormal;
                textBox.fontSize = 28;
            }
            if (!startedPlayingSong && (music[index] != null || musicLoop[index])){
                jukebox.PlaySong(music[index]);
                startedPlayingSong = true;
            }
            jukebox.SetLoop(musicLoop[index]);
        } else {
            fade -= Time.deltaTime/fadeTime;
            startedPlayingSong = false;
            if (fade <= 0f){
                fade = 0f;
                fadeDir = true;
            }
            if (artwork.sprite != art[index]){
                artwork.color = new Color(1f, 1f, 1f, fade);
            }
            if (!textBox.text.Equals(text[index])){
                textBox.color = new Color(1f, 1f, 1f, fade);
            }
        }

        if (Input.GetAxis("Fire1") == 0){
            canPressNext = true;
        }
        if (Input.GetAxis("Fire1") > 0 && canPressNext && fadeDir){
            canPressNext = false;
            if (index < art.Length-1 && autoTime <= 0f){
                fadeDir = false;
                index++;
            } else if (index >= art.Length-1){
                EndCutscene();
            }
        }
        if (Input.GetAxis("Pause") > 0 && canPressNext){
            canPressNext = false;
            EndCutscene();
        }

        autoTimer += Time.deltaTime;
        if (autoTime > 0f && autoTimer > autoTime && index < art.Length-1){
            autoTimer = 0f;
            fadeDir = false;
            index++;
        }
    }

    void EndCutscene(){
        if (FindObjectOfType<CutsceneData>() != null){
            SceneManager.LoadScene("CutsceneScene");
        } else if (setupPrefab == null){
            SceneManager.LoadScene("MenuScene");
        } else {
            GameObject selection = Instantiate<GameObject>(setupPrefab);
            if (selection.GetComponent<LevelSetupScript>().loadSong != null){
                FindObjectOfType<Jukebox>().PlaySong(selection.GetComponent<LevelSetupScript>().loadSong);
            }
            SceneManager.LoadScene("SampleScene");
        }
    }

    public int CurrentIndex(){
        if (fadeDir){
            return index;
        }
        return index-1;
    }
}
