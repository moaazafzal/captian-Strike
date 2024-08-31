using UnityEngine;
using System.Collections;

public class MYGUI : MonoBehaviour {


	delegate void OnButtonClicked();
	// Use this for initialization
	void OnGUI() {
		CreateButton("Load Adcolony video", () => 
		             { 
			Debug.Log("Button clicked...");
			AdColonyConFig.instance.PlayAdColonyVideo(); 
		});
	}


	void CreateButton(string label, OnButtonClicked onClicked) {
		if( GUI.Button(new Rect(200 , 150, 150, 50), label) ) {
			onClicked();
		}
	}

}
