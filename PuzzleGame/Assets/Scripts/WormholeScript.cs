using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeScript : MonoBehaviour
{
    public WormholeScript partner;
    private Vector3 displacement;
    private Vector3 prevPosition;
    private Dictionary<Collider2D, float> recentlyTeleported;
    // Start is called before the first frame update
    void Start()
    {
        prevPosition = transform.position;
        displacement = Vector3.zero;
        recentlyTeleported = new Dictionary<Collider2D, float>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 1f;
        if (GetComponent<SpriteRenderer>().flipX){
            direction = -1f;
        }
        transform.RotateAround(transform.position, Vector3.forward, 90f * direction * Time.deltaTime);

        displacement = transform.position - prevPosition;
        prevPosition = transform.position;

        List<Collider2D> myKeys = new List<Collider2D>();
        myKeys.AddRange(recentlyTeleported.Keys);

        for (int i = 0; i < myKeys.Count; i++){
            Collider2D col = myKeys[i];
            recentlyTeleported[col] -= Time.deltaTime;
            if (recentlyTeleported[col] <= 0f){
                recentlyTeleported.Remove(col);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Rigidbody2D>() == null){
            return;
        }
        if (recentlyTeleported.ContainsKey(col)){
            return;
        }
        if (transform.position.magnitude > 6f || partner.transform.position.magnitude > 3.5f){
            return;
        }
        if (col.GetComponent<GemScript>() != null || col.GetComponent<CannonScript>() != null || col.GetComponent<DustScript>() != null || col.GetComponent<PlanetScript>() != null){
            float halfWidth = col.GetComponent<SpriteRenderer>().bounds.size.x/2;
            float halfHeight = col.GetComponent<SpriteRenderer>().bounds.size.y/2;

            float size = halfWidth;
            if (size < halfHeight){
                size = halfHeight;
            }

            Vector2 addition = col.GetComponent<Rigidbody2D>().velocity - partner.GetDisplacement();
            addition = addition.normalized*(size+0.1f);

            Vector3 addition3 = new Vector3(addition.x, addition.y, 0f);

            col.gameObject.transform.position = partner.transform.position + addition3*0.5f;
            partner.AddCollider(col);
            AddCollider(col);
        }
    }

    public void AddCollider(Collider2D col){
        recentlyTeleported.Add(col, 0.1f);
    }

    public Vector2 GetDisplacement(){
        return new Vector2(displacement.x, displacement.y);
    }
}
