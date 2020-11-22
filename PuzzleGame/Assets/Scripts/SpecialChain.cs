using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialChain : SpecialMove
{
    public GameObject chainPrefab;
    public GameObject targetPrefab;
    private GameObject spawnedChain;
    private GemScript selectedGem;
    private GameObject targetObject;
    private float minDist;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        displayName = "BLOCK LASSO";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (targetObject != null){
            GemScript[] availableGems = FindObjectsOfType<GemScript>();
            minDist = 999999f;
            Vector3 targetPosition = transform.position;
            bool foundGem = false;

            for (int i = 0; i < availableGems.Length; i++){
                if (availableGems[i] != null){
                    foundGem = true;
                    Vector3 dist = availableGems[i].transform.position - transform.position;
                    if (dist.magnitude < minDist){
                        minDist = dist.magnitude;
                        targetPosition = availableGems[i].transform.position;
                    }
                }
            }

            if (!foundGem){
                Destroy(targetObject);
            }

            targetObject.transform.position = targetPosition;

            if (Input.GetAxis("Fire2") == 0){
                SetupChain();
            }
        }

        if (selectedGem != null){
            Vector3 towardsGem = selectedGem.transform.position - transform.position;
            if (towardsGem.magnitude > minDist){
                selectedGem.transform.position = transform.position + towardsGem.normalized*minDist;
                selectedGem.GetComponent<Rigidbody2D>().velocity *= 0.5f;
            }
            towardsGem = selectedGem.transform.position - transform.position;

            float chainAngle = Vector3.SignedAngle(Vector3.up, towardsGem, Vector3.forward);
            spawnedChain.transform.position = transform.position + towardsGem*0.5f;
            spawnedChain.transform.rotation = Quaternion.Euler(0f, 0f, chainAngle);

            spawnedChain.transform.localScale = new Vector3(0.15f, towardsGem.magnitude*1.4f, 0.15f);
        } else {
            if (spawnedChain != null){
                Destroy(spawnedChain.gameObject);
            }
        }
    }

    public void EndChain(){
        selectedGem = null;
    }

    void SetupChain(){
        Destroy (targetObject);

        GemScript[] availableGems = FindObjectsOfType<GemScript>();
        minDist = 999999f;
        selectedGem = null;

        for (int i = 0; i < availableGems.Length; i++){
            if (availableGems[i] != null){
                Vector3 dist = availableGems[i].transform.position - transform.position;
                if (dist.magnitude < minDist){
                    minDist = dist.magnitude;
                    selectedGem = availableGems[i];
                }
            }
        }

        spawnedChain = Instantiate<GameObject>(chainPrefab);
    }

    protected override void UseSpecial(){
        targetObject = Instantiate<GameObject>(targetPrefab);
    }
}
