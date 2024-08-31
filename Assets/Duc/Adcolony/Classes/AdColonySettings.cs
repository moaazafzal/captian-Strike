using UnityEngine;
using System.Collections;

public class AdColonySettings : MonoBehaviour {
	
	public string		version;			// AdColony Version
	public string		iOS_appId;			// iOS App Id
	public string		iOS_zoneId1;		// iOS Zone Id
	public string		Android_appId;		// Android App Id
	public string		Android_zoneId1;	// Android Zone Id

	private void Start()
	{
		if(GameObject.Find("Main") == null)
		{
			GameObject go = new GameObject( "Main" );
			AdColonyConFig main = go.AddComponent<AdColonyConFig>();
			main.name = "Main";
			
			#if UNITY_ANDROID
			main.Initialize(version, Android_appId, Android_zoneId1);
			#else
			main.Initialize(version, iOS_appId, iOS_zoneId1);
			#endif
		}
	}
}
