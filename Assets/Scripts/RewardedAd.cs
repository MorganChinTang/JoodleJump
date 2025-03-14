using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button showAdButton = null;
    [SerializeField] private string androidAdId = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showAdButton.interactable = false;

    }

    public void LoadAd()
    {
        Debug.Log($"Loading Ad: {androidAdId}");
        Advertisement.Load(androidAdId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if(adUnitId.Equals(androidAdId))
        {
            showAdButton.interactable = true;
            showAdButton.onClick.RemoveAllListeners();
            showAdButton.onClick.AddListener(ShowAd);
        }
    }

    public void ShowAd()
    {
        showAdButton.interactable = false;
        Advertisement.Show(androidAdId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(androidAdId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Complete");
            // if you are giving a reward, reviving, restarting, doubling points...
            // it is done here
            Advertisement.Load(androidAdId, this);
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Unity Ad {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Unity Ad {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        // can pause game, disable AI, auto savem undo in the show complete
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        // if ad is clicked, bonus reward
    }
}
