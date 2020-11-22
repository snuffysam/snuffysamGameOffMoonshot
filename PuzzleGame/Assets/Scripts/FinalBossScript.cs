using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossScript : MonoBehaviour
{
    public PlanetScript[] phase1Planets;
    public RabbitAntigravBehavior[] antigravRabbits;
    private int phase;
    public GameObject phase2Parent;
    public PlanetScript[] phase2Planets;
    public GameObject hapiObject;
    public GameObject dyingHapi;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && false)
        {
            foreach (PlanetScript ps in phase1Planets){
                if (ps != null && ps.gameObject != null){
                    Destroy(ps.gameObject);
                }
            }
            foreach (PlanetScript ps in phase2Planets){
                if (ps != null && ps.gameObject != null){
                    Destroy(ps.gameObject);
                }
            }
        }

        if (phase == 1){
            bool found = false;
            foreach (PlanetScript ps in phase1Planets){
                if (ps != null && ps.gameObject != null){
                    found = true;
                    break;
                }
            }
            if (!found){
                foreach (RabbitAntigravBehavior rab in antigravRabbits){
                    rab.Explode();
                    ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
                    sts.AddScore(37500/(sts.GetScoreScale()*2));
                    phase2Parent.SetActive(true);
                    FindObjectOfType<CannonScript>().ResetPlanets();
                    GemScript[] allGems = FindObjectsOfType<GemScript>();
                    for (int i = 0; i < allGems.Length; i++){
                        allGems[i].ResetPlanets();
                    }
                }
                phase = 2;
            }
        } else if (phase == 2){
            if (phase2Parent.transform.position.y > 0f){
                phase2Parent.transform.position -= Vector3.up*Time.deltaTime*3f;
            }

            bool found = false;
            foreach (PlanetScript ps in phase2Planets){
                if (ps != null && ps.gameObject != null){
                    found = true;
                    break;
                }
            }
            if (!found){
                phase = 3;
                Destroy(phase2Parent);
                hapiObject.SetActive(true);
                hapiObject.transform.localScale = new Vector3(1f,1f,1f)*0.01f;
                ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
                sts.AddScore(37500/(sts.GetScoreScale()*2));
                FindObjectOfType<CannonScript>().ResetPlanets();
                GemScript[] allGems = FindObjectsOfType<GemScript>();
                for (int i = 0; i < allGems.Length; i++){
                    allGems[i].ResetPlanets();
                }
            }
        } else if (phase == 3){
            if (hapiObject == null){
                ScoreTimerScript sts = FindObjectOfType<ScoreTimerScript>();
                sts.StopTimer();
                sts.AddScore(37500/sts.GetScoreScale());
                phase = 4;
            } else {
                dyingHapi.transform.position = hapiObject.transform.position;
                dyingHapi.transform.rotation = hapiObject.transform.rotation;
            }
        } else {
            dyingHapi.SetActive(true);
            timer += Time.deltaTime;
            if (timer > 10f){
                FindObjectOfType<ScoreTimerScript>().SetHighScore();
                FindObjectOfType<ScoreTimerScript>().NextLevel();
            }
        }
    }
}
