using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevMobAds : MonoBehaviour 
{

	private GoogleMobileAdBanner banner;

	//Admob
	private string BannersUnityId = "ca-app-pub-9183866782557538/8836899200";
	private string InterstitialUnityId = "ca-app-pub-9183866782557538/1313632409";

	public GADBannerSize size = GADBannerSize.SMART_BANNER;
	public TextAnchor anchor = TextAnchor.LowerCenter;
	private static Dictionary<string, GoogleMobileAdBanner> _refisterdBanners = null;

	void Awake()
	{
		if (!gameObject.activeInHierarchy)
			return;

		if (PlayerPrefs.GetInt("RemoveAds_Captain_Strike_Zombie",0)== 0)
		{
			//Admob
			if(AndroidAdMobController.instance.IsInited) 
			{
				if(!AndroidAdMobController.instance.InterstisialUnitId.Equals(InterstitialUnityId)) 
				{
					AndroidAdMobController.instance.SetInterstisialsUnitID(InterstitialUnityId);
				} 
			} 
			else 
			{
				AndroidAdMobController.instance.Init(InterstitialUnityId);
			}

			if(AndroidAdMobController.instance.IsInited) 
			{
				if(!AndroidAdMobController.instance.BannersUunitId.Equals(BannersUnityId)) 
				{
					AndroidAdMobController.instance.SetBannersUnitID(BannersUnityId);
				} 
			} 
			else 
			{
				AndroidAdMobController.instance.Init(BannersUnityId);
			}
		}
	}

	void Start()
	{
		if (!gameObject.activeInHierarchy)
						return;
		if (PlayerPrefs.GetInt("RemoveAds_Captain_Strike_Zombie",0) == 1)

			return;
	}

	public void ShowBanner()
	{
		if (PlayerPrefs.GetInt ("RemoveAds_Captain_Strike_Zombie",0) == 1) 
		{
			print ("Remove Banner Ads");
			return;
		}
		GoogleMobileAdBanner banner;
		if(registerdBanners.ContainsKey(sceneBannerId)) 
		{
			banner = registerdBanners[sceneBannerId];
		}  else {
			banner = AndroidAdMobController.instance.CreateAdBanner(anchor, size);
			registerdBanners.Add(sceneBannerId, banner);
		}
		
		if(banner.IsLoaded && !banner.IsOnScreen) 
		{
			banner.Show();
		}
		print ("Show Banner Ads");
	}

	public void AppLovinFullScreen()
	{
		AndroidAdMobController.instance.StartInterstitialAd();	
		print ("Show Full Ads");
	}

	public void HideBanner()
	{
		if (PlayerPrefs.GetInt ("RemoveAds_Captain_Strike_Zombie",0) == 1) 
		{
			return;	
		}
		if(registerdBanners.ContainsKey(sceneBannerId)) 
		{
			GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
			if(banner.IsLoaded) {
				if(banner.IsOnScreen) {
					banner.Hide();
				}
			} else {
				banner.ShowOnLoad = false;
			}
		}
		print ("Hide banner Ads ");
	}

	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners {
		get {
			if(_refisterdBanners == null) {
				_refisterdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}
			
			return _refisterdBanners;
		}
	}
	
	public string sceneBannerId {
		get {
			return Application.loadedLevelName + "_" + this.gameObject.name;
		}
	}

}
