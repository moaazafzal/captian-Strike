using UnityEngine;
using System.Collections;
using Zombie3D;

public class AdColonyConFig : MonoBehaviour 
{

	public static AdColonyConFig instance;

	private string		version;
	private string		appId;
	private string		zoneId1;

	private bool		paused;

	protected GameState gameState = new GameState();

	// Use this for initialization
	void Start () 
	{
		instance = this;
		gameState = GameApp.GetInstance().GetGameState();

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Initialize(string version, string appId, string zoneId1)
	{
		this.version = version;
		this.appId = appId;
		this.zoneId1 = zoneId1;


		//Assign any AdColony Delegates before calling Configure
		AdColony.OnVideoStarted		= this.OnVideoStarted;
		AdColony.OnVideoFinished	= this.OnVideoFinished;
		AdColony.OnV4VCResult		= this.OnV4VCResult;
		AdColony.OnAdAvailabilityChange = this.OnAdAvailabilityChange;
		
		// Replace these values with data for your own app and zones
		// The values are in the Main object in the Hierarchy of the ACUBasic scene
		AdColony.Configure
			(
				this.version,	// Arbitrary app version
				this.appId,		// ADC App ID from adcolony.com
				this.zoneId1	// A zone ID from adcolony.com
				);
	}

	// You may want to pause certain elements of your game before calling ShowVideoAd.
	// It is especially important to pause any sounds before showing a video ad.
	// If your game is resource intensive then you may also need to free up some resources before playing an ad.
	public void Pause()
	{
		this.paused = true;
	}
	
	public void Resume()
	{
		this.paused = false;
	}
	
	private void OnVideoStarted()
	{
		Debug.Log("On Video Started");
		
		this.Pause();
	}
	
	private void OnVideoFinished( bool ad_was_shown )
	{
		Debug.Log("On Video Finished, and Ad was shown: " + ad_was_shown);
		gameState.DeliverIAPItem(IAPName.Cash5W);
		AndroidMessage.Create ("Reward Success.","You getting 100 cash");

		this.Resume();
	}
	
	// The V4VCResult Delegate assigned in Start, AdColony calls this after confirming V4VC transactions with your server
	// success - true: transaction completed, virtual currency awarded by your server - false: transaction failed, no virtual currency awarded
	// name - The name of your virtual currency, defined in your AdColony account
	// amount - The amount of virtual currency awarded for watching the video, defined in your AdColony account
	private void OnV4VCResult(bool success, string name, int amount)
	{
		if(success)
		{
			Debug.Log("V4VC SUCCESS: name = " + name + ", amount = " + amount);
		}
		else
		{
			Debug.LogWarning("V4VC FAILED!");
		}
	}
	
	private void OnAdAvailabilityChange( bool avail, string zone_id)
	{
		Debug.Log("Ad Availability Changed to available=" + avail + " In zone: "+ zone_id);
	}

	public void PlayAdColonyVideo()
	{
		Debug.Log("Status for zone: " + AdColony.StatusForZone( this.zoneId1 ));
		if(AdColony.IsVideoAvailable(this.zoneId1))
		{
			Debug.Log("Playing AdColony Video1");
			AdColony.ShowVideoAd(this.zoneId1);
		}
		else
		{
			Debug.Log("Video1 is not Not Available");
		}
	}
}
