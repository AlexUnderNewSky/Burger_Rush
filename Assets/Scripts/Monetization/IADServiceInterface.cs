using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdStatus
{
	UNLOADED,
	LOADING,
	LOADED,
	ERROR
}

public interface IADServiceInterface
{
	#region BANNER
	public AdStatus BannerStatus
	{
		get;
	}

	public void LoadAndShowBanner();
	private void CreateBannerView()
	{

	}
	public void DestroyBanner();
	#endregion

	#region INTERSTITIAL
	public AdStatus InterstitialStatus
	{
		get;
	}
	void LoadInterstitialAd();

	void ShowInterstitialAd();

	void TryDestroyInterstitial();
	#endregion

	#region REWARDED
	public AdStatus RewardStatus
	{
		get;
	}

	/// <summary>
	/// Loads the rewarded ad.
	/// </summary>
	void LoadRewardedAd();

	void ShowRewardedAd();

	void TryDestroyReward();
	#endregion
}
