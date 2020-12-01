using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneData : MonoBehaviour
{
    public float autoTime;
    public Sprite[] art;
    public string[] text;
    public bool[] textEmphasized;
    public AudioClip[] music;
    public bool[] musicLoop;
    public GameObject setupPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
