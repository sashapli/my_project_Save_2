using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISbIKE : MonoBehaviour
{
    public GameObject BIKE1;
    public GameObject BIKE2;
    public GameObject BIKE3;
    public GameObject BIKE4;
    public GameObject BIKE5;
    public GameObject BIKE6;
    public GameObject BIKE1H;
    public GameObject BIKE2H;
    public GameObject BIKE3H;
    public GameObject BIKE4H;
    public GameObject BIKE5H;
    public GameObject BIKE6H;
    public GameObject BIKE1T;
    public GameObject BIKE2T;
    public GameObject BIKE3T;
    public GameObject BIKE4T;
    public GameObject BIKE5T;
    public GameObject BIKE6T, BIKE6R;
    public CameraFollow CameraFollow;
    public TerrainManager TerrainManager;
    public ObjectPlacer ObjectPlacer;
    public bool IStEST;
    public bool IStEST2;
    public bool test;
    private void Awake()
    {
        if (test)
        {

            BIKE2.SetActive(false);
            BIKE1.SetActive(false);
            BIKE6.SetActive(true);
            CameraFollow.target = BIKE6H.transform;
            CameraFollow.targetRb = BIKE6T.GetComponent<Rigidbody2D>();
            TerrainManager.trackTarget = BIKE6T.transform;
            ObjectPlacer.target = BIKE6T.transform;

            return;
        }

/*
        if (CoinManager.Instance.GetISBike())
           {
               BIKE2.SetActive(true);
               BIKE1.SetActive(false);
               CameraFollow.target = BIKE2H.transform;
               CameraFollow.targetRb = BIKE2T.GetComponent<Rigidbody2D>();
               TerrainManager.trackTarget = BIKE2T.transform;
               ObjectPlacer.target = BIKE2T.transform;
           }
          else if (CoinManager.Instance.GetISTacka())
           {
           BIKE2.SetActive(false);
               BIKE1.SetActive(false);
           BIKE3.SetActive(true);
           CameraFollow.target = BIKE3H.transform;
               CameraFollow.targetRb = BIKE3T.GetComponent<Rigidbody2D>();
               TerrainManager.trackTarget = BIKE3T.transform;
               ObjectPlacer.target = BIKE3T.transform;
           }
        else if (CoinManager.Instance.GetISJeep())
        {
            BIKE2.SetActive(false);
            BIKE1.SetActive(false);
            BIKE4.SetActive(true);
            CameraFollow.target = BIKE4H.transform;
            CameraFollow.targetRb = BIKE4T.GetComponent<Rigidbody2D>();
            TerrainManager.trackTarget = BIKE4T.transform;
            ObjectPlacer.target = BIKE4T.transform;
        }
        else if (CoinManager.Instance.GetISmoto())
        {
            BIKE2.SetActive(false);
            BIKE1.SetActive(false);
            BIKE5.SetActive(true);
            CameraFollow.target = BIKE5H.transform;
            CameraFollow.targetRb = BIKE5T.GetComponent<Rigidbody2D>();
            TerrainManager.trackTarget = BIKE5T.transform;
            ObjectPlacer.target = BIKE5T.transform;
        }
        else if (CoinManager.Instance.GetISmoto())
        {
            BIKE2.SetActive(false);
            BIKE1.SetActive(false);
            BIKE6.SetActive(true);
            CameraFollow.target = BIKE6H.transform;
            CameraFollow.targetRb = BIKE6T.GetComponent<Rigidbody2D>();
            TerrainManager.trackTarget = BIKE6T.transform;
            ObjectPlacer.target = BIKE6T.transform;
        }
        else
           {
               BIKE1.SetActive(true);
               BIKE2.SetActive(false);
               CameraFollow.target = BIKE1H.transform;
               CameraFollow.targetRb = BIKE6R.GetComponent<Rigidbody2D>();
               TerrainManager.trackTarget = BIKE1T.transform;
               ObjectPlacer.target = BIKE1T.transform;
           }*/
    }
}
