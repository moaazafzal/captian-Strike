using UnityEngine;
using System.Collections;
using System.IO;
using Zombie3D;
using ChartboostSDK;

public class StartMenuUIScript : MonoBehaviour, UIHandler, UIDialogEventHandler
{

  //  public UIManager m_UIManager = null;
 //   public string m_ui_material_path;  //×îºó´ø·´Ð±¸Ü

    protected GameDialog gameDialog;

    protected Timer fadeTimer = new Timer();

	public GameObject startButton, continueButton, exitGameMenu;

	private bool startGame = false;
	private bool continueGame = false;

    void Awake()
    {
        if (GameObject.Find("ResourceConfig") == null)
        {
            GameObject resourceConfig = Object.Instantiate(Resources.Load("ResourceConfig")) as GameObject;
            resourceConfig.name = "ResourceConfig";
            DontDestroyOnLoad(resourceConfig);
        }

        if (GameObject.Find("Music") == null)
        {
            GameApp.GetInstance().Init();
            GameObject musicObj = new GameObject("Music");
            DontDestroyOnLoad(musicObj);
            musicObj.transform.position = new Vector3(0, 1, -10);
            AudioSource audioSource = musicObj.AddComponent<AudioSource>();
            audioSource.clip = GameApp.GetInstance().GetResourceConfig().menuAudio;
            musicObj.AddComponent<MenuMusicScript>();
            audioSource.loop = true;
            audioSource.bypassEffects = true;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.Play();
            
        }

    }

    // Use this for initialization
    void Start()
    {     
		gameDialog = new GameDialog(UIDialog.DialogMode.YES_OR_NO);
		//        gameDialog.SetText(ConstData.FONT_NAME2, "\n\nAre You Sure You Want To Erase Your Progress And Start A New Game?", ColorName.fontColor_darkorange);
		gameDialog.SetDialogEventHandler(this); 
		
		GameApp.GetInstance().Init();

		string path = Application.persistentDataPath + "/Documents/";
		if (!File.Exists(path + "MySavedGame.game"))
		{
			startButton.SetActive(true);
			continueButton.SetActive(false);

		}
		else
		{
			startButton.SetActive(false);
			continueButton.SetActive(true);
		}
	}

    void Update()
    { 

		if (FadeAnimationScript.GetInstance().FadeOutComplete())
		{
			
		}
		
		if (fadeTimer.Ready())
		{
			if (FadeAnimationScript.GetInstance().FadeInComplete())
			{
				if (fadeTimer.Name == "Start")
				{
					UIResourceMgr.GetInstance().UnloadAllUIMaterials();
					//GameApp.GetInstance().GetGameState().MenuMusicTime = Camera.mainCamera.audio.time;
					Application.LoadLevel(SceneName.SCENE_TUTORIAL);
					
				}
				else if (fadeTimer.Name == "Continue")
				{
					if (!GameApp.GetInstance().Load())
					{
						GameApp.GetInstance().GetGameState().InitWeapons();
					}
					UIResourceMgr.GetInstance().UnloadAllUIMaterials();
					//GameApp.GetInstance().GetGameState().MenuMusicTime = Camera.mainCamera.audio.time;
					Application.LoadLevel(SceneName.MAP);
				}
				fadeTimer.Do();
			}
			
		}

		if (Input.GetKey (KeyCode.Escape)) 		
		{
			if (GameObject.Find ("RevMob") != null) 			
			{			
				GameObject.Find ("RevMob").GetComponent<RevMobAds> ().ShowBanner ();
			}
			exitGameMenu.SetActive (true);
		}

    }

	public void StartGameButton()
	{
		startGame = true;
		FadeAnimationScript.GetInstance().FadeInBlack();
		
		fadeTimer.Name = "Start";
		fadeTimer.SetTimer(0.5f, false);

		AudioPlayer.PlayAudio(GetComponent<AudioSource>());
	}
	public void ContinueGameButton()
	{
		continueGame = true;

		AudioPlayer.PlayAudio(GetComponent<AudioSource>());
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "Continue";
		fadeTimer.SetTimer(0.5f, false);

	}


   public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
		if (startGame == true)
        {


        }
        else if (continueGame == true)
        {
          
        }
	}

	public void NoExitGame()
	{
		exitGameMenu.SetActive (false);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

    public void Yes()
    {
        GameApp.GetInstance().GetGameState().ClearState();
        GameApp.GetInstance().GetGameState().InitWeapons();

		GameApp.GetInstance ().Save ();
        FadeAnimationScript.GetInstance().FadeInBlack();

        fadeTimer.Name = "Start";
        fadeTimer.SetTimer(0.5f, false);
    }
    public void No()
    {
    }


}
