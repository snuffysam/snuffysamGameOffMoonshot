using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public AudioClip sfx;
    public bool isLoop;
    private float age;
    public static int maxSFXPlaying = 10;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        age = 0f;
        AudioSource aSource = GetComponent<AudioSource>();
        if (isLoop){
            aSource.loop = true;
        }
        SFXScript[] sources = FindObjectsOfType<SFXScript>();
        List<SFXScript> activeSources = new List<SFXScript>();
        for (int i = 0; i < sources.Length; i++){
            if (sources[i] != null){
                activeSources.Add(sources[i]);
            }
        }

        int n = activeSources.Count - maxSFXPlaying;
        for (int i = 0; i < n; i++){
            float longestTime = -1f;
            SFXScript oldestSource = null;
            foreach (SFXScript temp in sources){
                if (temp.GetAge() > longestTime){
                    longestTime = temp.GetAge();
                    oldestSource = temp;
                }
            }
            activeSources.Remove(oldestSource);
            Destroy(oldestSource.gameObject);
        }

        aSource.clip = sfx;
        aSource.volume = Jukebox.sfxVolume;
        aSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;

        GetComponent<AudioSource>().volume = Jukebox.sfxVolume;

        if (!GetComponent<AudioSource>().isPlaying){
            Destroy(this.gameObject);
        }
    }

    public float GetAge(){
        return age;
    }
}
