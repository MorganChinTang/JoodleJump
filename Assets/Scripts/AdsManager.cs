using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string andriodGameId="";
    [SerializeField] private bool testMode = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Advertisement.Initialize(andriodGameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete");
    }

    public void OnInitializationFailed (UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Error initializing Unity Ads: {error.ToString()} - {message}");
    }

}
