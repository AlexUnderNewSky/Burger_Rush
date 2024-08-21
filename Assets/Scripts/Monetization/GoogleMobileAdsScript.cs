using GoogleMobileAds.Api;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour, IADServiceInterface
{
#if DEVELOPMENT_BUILD || UNITY_EDITOR
	private string _adUnitBannerId = "ca-app-pub-3940256099942544/6300978111"; // test ID
	private string _adUnitInterstitialId = "ca-app-pub-3940256099942544/1033173712"; // test ID
	private string _adUnitRewardedId = "ca-app-pub-3940256099942544/5224354917"; // test ID
#else
	private string _adUnitBannerId = "ca-app-pub-3332747355345621/1757270304"; // real ID
	private string _adUnitInterstitialId = "ca-app-pub-3332747355345621/7684577920"; // real ID
	private string _adUnitRewardedId = "ca-app-pub-3332747355345621/7478949392"; // real ID
#endif

	#region BANNER
	BannerView _bannerView;
	AdStatus _bannerStatus = AdStatus.UNLOADED;

	public AdStatus BannerStatus
	{
		get { return _bannerStatus;  } 
	}

	/// <summary>
	/// Creates a 320x50 banner view at bottom of the screen.
	/// </summary>
	void CreateBannerView()
	{
		Debug.Log("Creating banner view");

		// If we already have a banner, destroy the old one.
		if (_bannerView != null)
		{
			DestroyBanner();
		}

		// Create a 320x50 banner at top of the screen
		_bannerView = new BannerView(_adUnitBannerId, AdSize.Banner, AdPosition.Bottom);
	}

	/// <summary>
	/// Creates the banner view and loads a banner ad.
	/// </summary>
	public void LoadAndShowBanner()
	{
		_bannerStatus = AdStatus.UNLOADED;
		// create an instance of a banner view first.
		if (_bannerView == null)
		{
			CreateBannerView();
		}

		// create our request used to load the ad.
		var adRequest = new AdRequest();

		// send the request to load the ad.
		Debug.Log("Loading banner ad.");
		_bannerView.LoadAd(adRequest);
		_bannerStatus = AdStatus.LOADING;
		_bannerView.OnBannerAdLoaded += () =>
		{
			_bannerStatus = AdStatus.LOADED;
			Debug.Log("Banner view loaded an ad with response : "
				+ _bannerView.GetResponseInfo());

		};
		// Raised when an ad fails to load into the banner view.
		_bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
		{
			_bannerStatus = AdStatus.ERROR;
			Debug.LogError("Banner view failed to load an ad with error : "
				+ error);
		};
	}

	public void DestroyBanner()
	{
		_bannerStatus = AdStatus.UNLOADED;
		if (_bannerView != null)
		{
			Debug.Log("Destroying banner view.");
			_bannerView.Destroy();
			_bannerView = null;
		}
	}
	#endregion

	#region INTERSTITIAL
	InterstitialAd _interstitialAd;

	AdStatus _InterstitialStatus = AdStatus.UNLOADED;

	public AdStatus InterstitialStatus
	{
		get { return _InterstitialStatus; }
	}

	/// <summary>
	/// Loads the interstitial ad.
	/// </summary>
	public void LoadInterstitialAd()
    {
		TryDestroyInterstitial();

		Debug.Log("Loading the interstitial ad.");

		// create our request used to load the ad.
		var adRequest = new AdRequest();

		// send the request to load the ad.
		InterstitialAd.Load(_adUnitInterstitialId, adRequest,
			(InterstitialAd ad, LoadAdError error) =>
			{
				// if error is not null, the load request failed.
				if (error != null || ad == null)
				{
					_InterstitialStatus = AdStatus.ERROR;
					Debug.LogError("interstitial ad failed to load an ad " +
								   "with error : " + error);
					return;
				}

				Debug.Log("Interstitial ad loaded with response : "
						  + ad.GetResponseInfo());

				_InterstitialStatus = AdStatus.LOADED;
				_interstitialAd = ad;

				RegisterReloadInterstitialHandler();
			});

		_InterstitialStatus = AdStatus.LOADING;
	}

	/// <summary>
	/// Shows the interstitial ad.
	/// </summary>
	public void ShowInterstitialAd()
	{
		if (_interstitialAd != null && _interstitialAd.CanShowAd())
		{
			Debug.Log("Showing interstitial ad.");
			_interstitialAd.Show();
			_InterstitialStatus = AdStatus.UNLOADED;
		}
		else
		{
			Debug.LogError("Interstitial ad is not ready yet.");
		}
	}

	public void TryDestroyInterstitial()
	{
		_InterstitialStatus = AdStatus.UNLOADED;
		// Clean up the old ad before loading a new one.
		if (_interstitialAd != null)
		{
			_interstitialAd.Destroy();
			_interstitialAd = null;
		}
	}

	private void RegisterReloadInterstitialHandler()
	{
		// Raised when the ad closed full screen content.
		_interstitialAd.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Interstitial Ad full screen content closed.");

			// Reload the ad so that we can show another as soon as possible.
			LoadInterstitialAd();
		};
		// Raised when the ad failed to open full screen content.
		_interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Interstitial ad failed to open full screen content " +
						   "with error : " + error);

			// Reload the ad so that we can show another as soon as possible.
			LoadInterstitialAd();
		};
	}
	#endregion

	#region REWARDED
	private RewardedAd _rewardedAd;

	AdStatus _RewardStatus = AdStatus.UNLOADED;

	public AdStatus RewardStatus
	{
		get { return _RewardStatus; }
	}

	/// <summary>
	/// Loads the rewarded ad.
	/// </summary>
	public void LoadRewardedAd()
	{
		// Clean up the old ad before loading a new one.
		TryDestroyReward();

		Debug.Log("Loading the rewarded ad.");

		// create our request used to load the ad.
		var adRequest = new AdRequest();

		// send the request to load the ad.
		RewardedAd.Load(_adUnitRewardedId, adRequest,
			(RewardedAd ad, LoadAdError error) =>
			{
				// if error is not null, the load request failed.
				if (error != null || ad == null)
				{
					_RewardStatus = AdStatus.ERROR;
					Debug.LogError("Rewarded ad failed to load an ad " +
								   "with error : " + error);
					return;
				}

				Debug.Log("Rewarded ad loaded with response : "
						  + ad.GetResponseInfo());

				_rewardedAd = ad;
				_RewardStatus = AdStatus.LOADED;
				RegisterReloadRewardHandler();
			});
		_RewardStatus = AdStatus.LOADING;
	}

	public void ShowRewardedAd()
	{
		const string rewardMsg =
			"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

		if (_rewardedAd != null && _rewardedAd.CanShowAd())
		{
			_rewardedAd.Show((Reward reward) =>
			{
				_RewardStatus = AdStatus.UNLOADED;
				// TODO: Reward the user.
				Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
			});
		}
	}

	private void RegisterReloadRewardHandler()
	{
		// Raised when the ad closed full screen content.
		_rewardedAd.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Rewarded Ad full screen content closed.");

			// Reload the ad so that we can show another as soon as possible.
			LoadRewardedAd();
		};
		// Raised when the ad failed to open full screen content.
		_rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Rewarded ad failed to open full screen content " +
						   "with error : " + error);

			// Reload the ad so that we can show another as soon as possible.
			LoadRewardedAd();
		};
	}

	public void TryDestroyReward()
	{
		_RewardStatus = AdStatus.UNLOADED;
		if (_rewardedAd != null)
		{
			_rewardedAd.Destroy();
			_rewardedAd = null;
		}
	}
	#endregion

	void Start()
	{
		// Initialize the Mobile Ads SDK.
		MobileAds.Initialize((initStatus) =>
		{
			Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
			foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
			{
				string className = keyValuePair.Key;
				AdapterStatus status = keyValuePair.Value;
				switch (status.InitializationState)
				{
					case AdapterState.NotReady:
						// The adapter initialization did not complete.
						MonoBehaviour.print("Adapter: " + className + " not ready.");
						break;
					case AdapterState.Ready:
						// The adapter was successfully initialized.
						MonoBehaviour.print("Adapter: " + className + " is initialized.");
						break;
				}
			}
		});

#if DEVELOPMENT_BUILD || UNITY_EDITO
		Debug.Log("ID TO USE BANNER IS DEVELOP");
#else
		Debug.Log("ID TO USE BANNER IS RELEASE");
#endif
	}
}
