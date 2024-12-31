using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternet : MonoBehaviour
{
    public static CheckInternet Instance { get; private set; }
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject Go;
    private void Start()
    {
        BG.SetActive(false);
        Go.SetActive(false);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CheckInternetInformation()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            BG.SetActive(true);
            Go.SetActive(true);
            return false;
        } else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //internet wwifi
            return true ;
        } else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            //mobile data
            return true;
        }
        return false;
    }
    public void DisableTab()
    {
        BG.SetActive(false);
        Go.SetActive(false);
    }
}
