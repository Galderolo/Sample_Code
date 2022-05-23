using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks_golem : MonoBehaviour
{

    public GameObject EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    //private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;

    private GameObject hitter;
    

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    
    
    void OnParticleCollision(GameObject other)
    {
        bool hascollide = false;
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        if(other.CompareTag("Ground") || other.CompareTag("ColliderWall"))
        {
            if(!hascollide)
            {
                for (int i = 0; i < numCollisionEvents; i++)
                {
                    var instance = Instantiate(EffectsOnCollision, collisionEvents[0].intersection + collisionEvents[0].normal * Offset, new Quaternion()) as GameObject;
                    instance.transform.parent = transform;
                    Debug.Log("Se instancia el polvo");
                }
                
                hascollide = true;
            }
            
            
        }

        
        if (DestoyMainEffect == true)
        {
            Destroy(gameObject, DestroyTimeDelay + 0.5f);
        }



    }
}
