using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0f;
    public static float voiceVolume = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = musicVolume;
    }

    public void PlaySong(AudioClip song){
        if (song.Equals(GetComponent<AudioSource>().clip)){
            return;
        }
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = song;
        GetComponent<AudioSource>().Play();
    }
}
