using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RateApp : MonoBehaviour 
{

	//Rate App
	private int _isRate = 0;
	private int _demRate;
	private string rateText = "If you enjoy using Captain Strike Zombie, please take a moment to rate it. Thanks for your support!";
	private string rateUrl = "market://details?id=com.codostudio.captainstrikezombie";
	
	void RateDialogPopUp() 
	{
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create("Rate Us", rateText, rateUrl);
		rate.ActionComplete += OnRatePopUpClose;
	}
	private void OnRatePopUpClose(AndroidDialogResult result) {
		
		switch(result) {
		case AndroidDialogResult.RATED:
			Debug.Log ("RATED button pressed");
			break;
		case AndroidDialogResult.REMIND:
			Debug.Log ("REMIND button pressed");
			break;
		case AndroidDialogResult.DECLINED:
			Debug.Log ("DECLINED button pressed");
			break;
			
		}		
	}
	
	void Start () 
	{		
		_isRate = PlayerPrefs.GetInt ("_isRate");
		_demRate += 1;
		PlayerPrefs.SetInt ("_DemRate", PlayerPrefs.GetInt("_DemRate",0) + _demRate);
		print ("Dem Rate = " + _demRate);
		if (PlayerPrefs.GetInt("_DemRate") == 5 && _isRate == 0)
		{
			print ("Dem Rate");
			RateDialogPopUp();
			_isRate = 1;
			PlayerPrefs.SetInt("_isRate", _isRate);
		}
		
	}
}
