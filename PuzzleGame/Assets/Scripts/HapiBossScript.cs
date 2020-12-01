using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapiBossScript : MonoBehaviour
{
    public Sprite[] frames;
    public GameObject bombPrefab, skullPrefab, spellRef;
    public GameObject soundPrefab;
    public AudioClip prepareSpell;
    public AudioClip castSpell;
    private float stateTimer = 0f;
    private int state = 0;
    private GameObject createdSpell;
    private bool spell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateTimer += Time.deltaTime;

        if (transform.localScale.x < 0.85f){
            transform.localScale = new Vector3(1f,1f,1f)*(transform.localScale.x+Time.deltaTime);
        }
        if (transform.localScale.x > 0.85f){
            transform.localScale = new Vector3(1f,1f,1f)*0.85f;
        }

        if (state == 0){
            GetComponent<SpriteRenderer>().sprite = frames[0];
            GetComponent<Rigidbody2D>().velocity = Vector3.right*Mathf.Cos(stateTimer*1f)*0.5f;
            Vector3 vel3 = new Vector3(GetComponent<Rigidbody2D>().velocity.normalized.x, GetComponent<Rigidbody2D>().velocity.normalized.y, 0f);
            float angleBetween = Vector3.SignedAngle(transform.up,Vector3.up+vel3*0.75f, new Vector3(0f,0f,1f));
            transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween * Time.deltaTime * 3f);
            if (stateTimer > 5f){
                state = 1;
                stateTimer = 0f;
                GameObject selectedPrefab = bombPrefab;
                spell = false;
                if (Random.Range(0f, 100f) < 25f){
                    selectedPrefab = skullPrefab;
                    spell = true;
                }
                createdSpell = Instantiate<GameObject>(selectedPrefab);
                createdSpell.transform.SetParent(this.transform);
                createdSpell.transform.position = spellRef.transform.position;
                createdSpell.transform.rotation = spellRef.transform.rotation;
                createdSpell.transform.localScale = spellRef.transform.localScale;
                createdSpell.GetComponent<PolygonCollider2D>().enabled = false;

                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = prepareSpell;
            }
        } else if (state == 1){
            GetComponent<SpriteRenderer>().sprite = frames[1];
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized;
            if (createdSpell != null){
                createdSpell.transform.position = spellRef.transform.position;
                createdSpell.transform.rotation = spellRef.transform.rotation;
                createdSpell.transform.localScale = spellRef.transform.localScale;
                createdSpell.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            Transform target = null;
            if (spell){
                CannonScript cs = FindObjectOfType<CannonScript>();
                if (cs != null){
                    target = cs.transform;
                }
            } else {
                float minDist = 999999f;
                GemScript gem = null;
                GemScript[] allGems = FindObjectsOfType<GemScript>();
                foreach (GemScript gs in allGems){
                    if (gs != null && (gs.transform.position-transform.position).magnitude < minDist){
                        minDist = (gs.transform.position-transform.position).magnitude;
                        gem = gs;
                    }
                }
                if (gem != null){
                    target = gem.transform;
                }
            }
            if (target != null){
                float angleBetween = Vector3.SignedAngle(-transform.up,target.position-transform.position, new Vector3(0f,0f,1f));
                transform.RotateAround(transform.position, new Vector3(0f,0f,1f), angleBetween * Time.deltaTime * 6f);
            }
            if (stateTimer > 1.5f){
                state = 2;
                stateTimer = 0f;
                if (createdSpell != null){
                createdSpell.GetComponent<PolygonCollider2D>().enabled = true;
                createdSpell.transform.SetParent(null);
                if (target == null){
                    createdSpell.GetComponent<Rigidbody2D>().velocity = transform.up*-6f;
                } else {
                    createdSpell.GetComponent<Rigidbody2D>().velocity = (target.position-createdSpell.transform.position)*6f;
                }
                createdSpell = null;
                }

                GameObject snd = Instantiate<GameObject>(soundPrefab);
                snd.GetComponent<SFXScript>().sfx = castSpell;
            }
        } else {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().sprite = frames[2];
            transform.position -= (transform.position*Time.deltaTime*0.75f);
            if (stateTimer > 1.5f){
                state = 0;
                stateTimer = 0f;
            }
        }
    }
}
