using UnityEngine;
using System.Collections;
using Zombie3D;
using System.IO;
using UnityEngine.UI;
using ChartboostSDK;

public class MapUIPanel : UIPanel, UIHandler
{

    protected bool buttonPressed = false;
  
    protected bool init = false;
    protected bool setAudioTime = false;

    protected float startTime;
    protected Timer fadeTimer = new Timer();

    protected const int MAP_COUNT = 5;

    protected UIColoredButton[] mapButtons = new UIColoredButton[MAP_COUNT];
    protected UIClickButton shopButton = new UIClickButton();
    protected UIAnimatedImage[] zombieAnimations = new UIAnimatedImage[MAP_COUNT];

   

    public override void Hide()
    {
        base.Hide();

    }

    public override void Show()
    {
        base.Show();
        buttonPressed = false;
    }


    public void Update()
    {
        if (fadeTimer.Ready())
        {

            if (fadeTimer.Name == "0")
            {
                
            }
            else if (fadeTimer.Name == "1")
            {
            }
            else if (fadeTimer.Name == "2")
            {
            }
            else if (fadeTimer.Name == "3")
            {
            }
			else if (fadeTimer.Name == "4")
			{
			}
            else if (fadeTimer.Name == "shop")
            {
            }
            else if (fadeTimer.Name == "return")
            {
            }

            fadeTimer.Do();
        }
    }



    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (buttonPressed)
            return;

        if (control == mapButtons[0])
        {
           
        }
        else if (control == mapButtons[1])
        {
            
        }
        else if (control == mapButtons[2])
        {
           
        }
        else if (control == mapButtons[3])
        {
        }
		else if (control == mapButtons[4])
		{

		}
        else if (control == shopButton)
        {
        }

    }


}



public class MapUI : MonoBehaviour
{
    public UIManager m_UIManager = null;
    protected MapUIPanel mapPanel;
    protected OptionsMenuUI optionsUI;
    protected AudioPlayer audioPlayer = new AudioPlayer();
    protected LoadingPanel loadingPanel;
    protected float startTime;   

	public GameObject[] Map_Count;
	protected const int MAP_COUNT = 5;

	protected bool buttonPressed = false;
	
	protected bool init = false;
	protected bool setAudioTime = false;
	
	protected Timer fadeTimer = new Timer();

	public GameObject option_Menu;
	protected GameState gameState;
	public GameObject sound_On_Button, sound_Off_Button;

	protected int[] infection = new int[MAP_COUNT];
	
	public GameObject LoadingScene;
	public Image LoadingBar;
	public float waitTime = 30.0f;

	public GameObject MapBossUI, MapVSUI;
	public GameObject MapEffectBoss, MapEffectSurvial;

	public Button BossZombieButton;
	public Button BossNurseButton;
	public Button BossSwatButton;
	public Button BossHunterButton;
	public Button BossTankButton;

	public GameObject BossZombieLockImage, BossNurseLockImage,BossSwatLockImage,BossHunterLockImage,BossTankLockImage;

	private int SaveLevelNumTam;

	bool _showChartBoost = false;

	public GameObject _adcolonyButton;
	public GameObject _chartboostVideoAdsButton;

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
		MapInit();
		gameState = GameApp.GetInstance().GetGameState();
		SoundMenuInit ();
	
    }

    // Use this for initialization
    void Start()
    {

        loadingPanel = new LoadingPanel();

        loadingPanel.Show();       

        startTime = Time.time; 


        Transform audioFolderTrans = transform.Find("Audio");
        audioPlayer.AddAudio(audioFolderTrans, "Button");
        audioPlayer.AddAudio(audioFolderTrans, "Battle");

        StartCoroutine("Init");
		if (GameApp.GetInstance ().GetGameState ().LevelNum < 30)
		{
			GameApp.GetInstance().GetGameState().LevelNumBoss = GameApp.GetInstance().GetGameState().LevelNum;
			print ("Level Boss ----= " + GameApp.GetInstance().GetGameState().LevelNumBoss);

		} 
		else 
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;
			print ("Level Num --- = " + GameApp.GetInstance().GetGameState().LevelNum);

		}
		print ("Level Boss = " + GameApp.GetInstance().GetGameState().LevelNumBoss);

		CheckUnlockLevelBoss ();
    }

	void CheckUnlockLevelBoss()
	{
		if(GameApp.GetInstance().GetGameState().LevelNumBoss >= 4)
		{
			BossZombieButton.interactable= true;
			BossZombieLockImage.SetActive(false);
		}
		if (GameApp.GetInstance().GetGameState().LevelNumBoss >= 8)
		{
			BossNurseButton.interactable= true;
			BossNurseLockImage.SetActive(false);
			
		}
		if (GameApp.GetInstance().GetGameState().LevelNumBoss >= 12)
		{
			BossSwatButton.interactable= true;
			BossSwatLockImage.SetActive(false);
			
		}
		if (GameApp.GetInstance().GetGameState().LevelNumBoss >= 16)
		{
			BossHunterButton.interactable= true;
			BossHunterLockImage.SetActive(false);
			
		}
		if (GameApp.GetInstance().GetGameState().LevelNumBoss >= 20)
		{
			BossTankButton.interactable= true;
			BossTankLockImage.SetActive(false);
			
		}
	}

    IEnumerator Init()
    {
        yield return 1;

        if (GameApp.GetInstance().GetGameState().FirstTimeGame)
        {
            if (Time.time - startTime < 3.0f && !GameApp.GetInstance().GetGameState().FromShopMenu)
            {
                yield return new WaitForSeconds(3.0f - (Time.time - startTime));
            }
        }
        FadeAnimationScript.GetInstance().FadeOutBlack();
        GameApp.GetInstance().GetGameState().FromShopMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mapPanel != null)
        {
            mapPanel.Update();
        }
        {
            foreach (UITouchInner touch in iPhoneInputMgr.MockTouches())
            {
                if (m_UIManager != null)
                {
                    if (m_UIManager.HandleInput(touch))
                    {
                        continue;
                    }
                }
            }
        }

		if (fadeTimer.Ready())
		{
			StartCoroutine (LevelCoroutine ());
		}

		if (Input.GetKey (KeyCode.Escape)) 		
		{
			Application.LoadLevel("StartMenuUI");
		}

		Chartboost.cacheRewardedVideo(CBLocation.Default);
		if (Chartboost.hasRewardedVideo(CBLocation.Default))
		{ 
			_chartboostVideoAdsButton.SetActive(true);
		}
		else 
		{
			_chartboostVideoAdsButton.SetActive(false);
		}

		Chartboost.cacheInterstitial(CBLocation.Default);
		if (Chartboost.hasInterstitial (CBLocation.Default) && _showChartBoost == false && PlayerPrefs.GetInt ("RemoveAds_Captain_Strike_Zombie") == 0) 
		{
			Chartboost.showInterstitial (CBLocation.Default);
			_showChartBoost = true;
			print ("Show Chartboost");
		} 

		if(AdColony.IsVideoAvailable("vzdb0332b4d691430bb4"))
		{
			Debug.Log("Playing AdColony Video1");
			_adcolonyButton.SetActive(true);
		}
		else
		{
			Debug.Log("Video1 is not Not Available");
			_adcolonyButton.SetActive(false);
		}
	}

	IEnumerator LevelCoroutine ()
	{
		if (fadeTimer.Name == "0")
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;

			if (GameApp.GetInstance().GetGameState().FirstTimeGame)
			{
				Application.LoadLevel(SceneName.SCENE_TUTORIAL);

			}
			else
			{
				Application.LoadLevel(SceneName.SCENE_ARENA);

			}
		}
		else if (fadeTimer.Name == "1")
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;

			Application.LoadLevel(SceneName.SCENE_HOSPITAL);
		}
		else if (fadeTimer.Name == "2")
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;

			Application.LoadLevel(SceneName.SCENE_PARKING);
		}
		else if (fadeTimer.Name == "3")
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;

			Application.LoadLevel(SceneName.SCENE_VILLAGE);
		}
		else if (fadeTimer.Name == "4")
		{
			GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;

			Application.LoadLevel(SceneName.SCENE_GRAVE);
		}
		else if (fadeTimer.Name == "5")
		{
			GameApp.GetInstance().GetGameState().LevelNum = 31;
			Application.LoadLevel(SceneName.SCENE_SURVIAL);
		}
		else if (fadeTimer.Name == "6")
		{
			GameApp.GetInstance().GetGameState().LevelNum = 32;

			Application.LoadLevel(SceneName.SCENE_BOSS_ZOMBIE);
		}
		else if (fadeTimer.Name == "7")
		{			
			GameApp.GetInstance().GetGameState().LevelNum = 33;

			Application.LoadLevel(SceneName.SCENE_BOSS_TANK);
		}
		else if (fadeTimer.Name == "8")
		{
			GameApp.GetInstance().GetGameState().LevelNum = 34;
			Application.LoadLevel(SceneName.SCENE_BOSS_NURSE);
		}
		else if (fadeTimer.Name == "9")
		{
			GameApp.GetInstance().GetGameState().LevelNum = 35;
			Application.LoadLevel(SceneName.SCENE_BOSS_HUNTER);
		}
		else if (fadeTimer.Name == "10")
		{
			GameApp.GetInstance().GetGameState().LevelNum = 36;
			Application.LoadLevel(SceneName.SCENE_BOSS_SWAT);
		}

		else if (fadeTimer.Name == "shop")
		{
			Application.LoadLevel(SceneName.ARENA_MENU);
		}
		else if (fadeTimer.Name == "return")
		{
			Application.LoadLevel(SceneName.START_MENU);
		}

		fadeTimer.Do();
		yield return null;

	}

	void MapInit()
	{
		startTime = Time.time;
		
		for (int i = 0; i < MAP_COUNT; i++) 
		{
			Debug.Log(i);
			
		}
		if (GameApp.GetInstance().GetGameState().LevelNum == 1)
		{
			infection[1] = -1;
			infection[2] = -1;
			infection[3] = -1;
			infection[4] = -1;
		}
		else
		{
			Algorithem<int>.RandomSort(infection);
			int infectionNum = Random.Range(0, 4);
			if (infectionNum == 3)
			{
				infection[1] = -1;
				infection[2] = -1;
				infection[3] = -1;				
				infection[4] = -1;
			}
			if (infectionNum == 2)
			{
				infection[1] = -1;
				infection[2] = -1;
				infection[3] = -1;
			}
			else if (infectionNum == 1)
			{
				infection[2] = -1;
				infection[3] = -1;
			}
		}  

//		ShowMap ();
	}
	
	void ShowMap()
	{
		GameApp.GetInstance ().ClearScene ();
		for (int i = 0; i < MAP_COUNT; i++)
		{
			Map_Count[i].SetActive(false);
		}
		for (int i = 0; i < MAP_COUNT; i++)
		{
			int index = infection[i];
			if (index != -1)
			{
				Map_Count[i].SetActive(true);
				
			}
		}
	}

	void HideEffectMap()
	{
		MapEffectBoss.SetActive (false);
		MapEffectSurvial.SetActive (false);
	}

	void ShowEffectMap()
	{
		MapEffectBoss.SetActive (true);
		MapEffectSurvial.SetActive (true);
	}
	
	public void MapTutorialButton()
	{
		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "0";
		fadeTimer.SetTimer(2f, false);
//		GameApp.GetInstance().Save();
		buttonPressed = true;
	}
	public void MapHopitalButton()
	{		

		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "1";
		fadeTimer.SetTimer(2f, false);
//		GameApp.GetInstance().Save();
		buttonPressed = true;
	}
	public void MapPakingButton()
	{

		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "2";
		fadeTimer.SetTimer(2f, false);
//		GameApp.GetInstance().Save();
		buttonPressed = true;
	}
	
	public void MapVillageButton()
	{
		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "3";
		fadeTimer.SetTimer(2f, false);
		GameApp.GetInstance().Save();
		buttonPressed = true;
	}
	
	public void MapGraveButton()
	{

		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "4";
		fadeTimer.SetTimer(2f, false);
		GameApp.GetInstance().Save();
		buttonPressed = true;
		
	}
	public void MapSurvialButton()
	{

		HideEffectMap ();
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "5";
		fadeTimer.SetTimer(2f, false);
		GameApp.GetInstance().Save();
		buttonPressed = true;
		
	}

	public void MapBossZombieButton()
	{
		GameApp.GetInstance ().GetGameState ().LevelNumBoss = GameApp.GetInstance ().GetGameState ().LevelNum;

		print ("Level Boss-----------" + GameApp.GetInstance().GetGameState().LevelNumBoss);

		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "6";
		fadeTimer.SetTimer(2f, false);
		if (GameApp.GetInstance ().GetGameState ().LevelNumBoss < 30)
		{
			GameApp.GetInstance ().Save ();
		} 
		else 
		{

		}
		buttonPressed = true;		
	}

	public void MapBossTankButton()
	{
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "7";
		fadeTimer.SetTimer(2f, false);

		if (GameApp.GetInstance ().GetGameState ().LevelNumBoss < 30)
		{
			GameApp.GetInstance ().Save ();
		} 
		else 
		{
			
		}
		buttonPressed = true;
		
	}
	public void MapBossNurseButton()
	{

		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "8";
		fadeTimer.SetTimer(2f, false);
		if (GameApp.GetInstance ().GetGameState ().LevelNumBoss < 30)
		{
			GameApp.GetInstance ().Save ();
		} 
		else 
		{
			
		}
		buttonPressed = true;
		
	}
	public void MapBossHunterButton()
	{
		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "9";
		fadeTimer.SetTimer(2f, false);
		if (GameApp.GetInstance ().GetGameState ().LevelNumBoss < 30)
		{
			GameApp.GetInstance ().Save ();
		} 
		else 
		{
			
		}
		buttonPressed = true;
		
	}

	public void MapBossSwatButton()
	{

		LoadingScene.SetActive (true);

		GameObject.Destroy(GameObject.Find("Music"));
		MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
		FadeAnimationScript.GetInstance().FadeInBlack();
		fadeTimer.Name = "10";
		fadeTimer.SetTimer(2f, false);
		if (GameApp.GetInstance ().GetGameState ().LevelNumBoss < 30)
		{
			GameApp.GetInstance ().Save ();
		} 
		else 
		{
			
		}
		buttonPressed = true;
		
	}
	void SoundMenuInit ()
	{
		if (gameState.MusicOn)
		{
			sound_On_Button.SetActive(true);
			sound_Off_Button.SetActive(false);
		}
		else 
		{
			sound_On_Button.SetActive(false);
			sound_Off_Button.SetActive(true);
		}
	}
	
	public void Sound_Btn()
	{
		if(gameState.MusicOn == false)
		{
			AudioListener.volume = 1;
			gameState.MusicOn = true;
		}
		else if (gameState.MusicOn == true)
		{
			AudioListener.volume = 0;
			gameState.MusicOn = false;
		}
		SoundMenuInit ();
		
	}

	public void SendBug() 
	{
		AndroidSocialGate.SendMail("Send Mail", "", "Hey Codo Studio, I found a bug", "codo.apple@gmail.com");		
	}

	public void ShopButton()
	{
		LoadingScene.SetActive (true);

		GameApp.GetInstance().GetGameState().LevelNum = GameApp.GetInstance().GetGameState().LevelNumBoss;
		HideEffectMap ();

		FadeAnimationScript.GetInstance().FadeInBlack(0.5f);
		Application.LoadLevel("ArenaMenuUI");
		fadeTimer.SetTimer(0.5f, false);
		buttonPressed = true;
	}

	public void ShowMapVSButton()
	{
		HideEffectMap ();		
		MapVSUI.SetActive (true);
	}
	public void BackMapVSButton()
	{
		ShowEffectMap ();
		MapVSUI.SetActive (false);
	}

	public void ShowMapBossButton()
	{
		HideEffectMap ();

		MapBossUI.SetActive (true);
	}
	public void BackMapBossButton()
	{
		ShowEffectMap ();
		MapBossUI.SetActive (false);
	}
	public void BackButton()
	{
		LoadingScene.SetActive (true);
		HideEffectMap ();

		FadeAnimationScript.GetInstance().FadeInBlack(1);
		fadeTimer.Name = "return";
		fadeTimer.SetTimer(1f, false);
		buttonPressed = true;
	}
	
	public void OptionButton()
	{
		HideEffectMap ();

		option_Menu.SetActive (true);
	}
	
	public void Back_OptionButton()
	{
		ShowEffectMap ();
		option_Menu.SetActive (false);
	}

	public void VoteButton()
	{
		Application.OpenURL("market://details?id=com.codostudio.captainstrikezombie");
	}

	void OnEnable() 
	{
		Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
		Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
	}

	public void VideoAdsChartboostShowButton()
	{
		if (Chartboost.hasRewardedVideo(CBLocation.Default))
		{ 
			Chartboost.showRewardedVideo(CBLocation.Default);
		}
		else 
		{
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}
	}

	void OnDisable()
	{
		Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
		Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
	}

	void didCompleteRewardedVideo(CBLocation location, int reward) 
	{
		AndroidMessage.Create ("Reward Success.","You getting 300 cash");
		gameState.DeliverIAPItem(IAPName.Cash5W);
		gameState.DeliverIAPItem(IAPName.Cash5W);
		gameState.DeliverIAPItem(IAPName.Cash5W);
	}

	void didCacheRewardedVideo(CBLocation location) 
	{
		Debug.Log("didCacheRewardedVideo: " + location);
	}

	public void AdcolonyShowButton()
	{
		AdColonyConFig.instance.PlayAdColonyVideo(); 
	}

    public static MapUI GetInstance()
    {
        return GameObject.Find("MapUI").GetComponent<MapUI>();
    }

    public OptionsMenuUI GetOptionsMenuUI()
    {
        return optionsUI;
    }
    public MapUIPanel GetMapUI()
    {
        return mapPanel;
    }

    public AudioPlayer GetAudioPlayer()
    {
        return audioPlayer;
    }


}
