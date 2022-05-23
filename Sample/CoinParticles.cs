using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticles : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem coinParticles;
    public GameObject chestPoint;
    private ParticleSystem.Particle[] allCoins;
    public float speed;
    private bool isReady;
    public float timeInGround;

    
    void Start()
    {


        chestPoint = GameObject.Find("Chest");
        //Debug.Log("This is " + chestPoint.transform.position);
        StartCoroutine(BringMeThoseCoins());
        allCoins = new ParticleSystem.Particle[5];

    }

    
    void Update()
    {
        
        

    }

    void FixedUpdate ()
    {
        if (isReady)
        {
            for (int i = 0; i < allCoins.Length; i++)
            {
                allCoins[i].position = Vector3.Lerp(allCoins[i].position, chestPoint.transform.localPosition, speed * Time.fixedDeltaTime);
            }
            coinParticles.SetParticles(allCoins);

        }
    }

    IEnumerator BringMeThoseCoins()
    {

        yield return new WaitForSeconds(timeInGround);
        coinParticles.GetParticles(allCoins);
        isReady = true;
    }

}