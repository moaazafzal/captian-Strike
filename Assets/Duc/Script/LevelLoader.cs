using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	public GameObject LoadingScene;
	public Image LoadingBar;

	public void LoadLevel ()
	{
		StartCoroutine (LevelCoroutine ());
	}
	IEnumerator LevelCoroutine ()
	{
		LoadingScene.SetActive (true);
		AsyncOperation async = Application.LoadLevelAsync (1);

		while (!async.isDone) {
			LoadingBar.fillAmount = async.progress / 0.9f;
			yield return null;
		}
	}
}