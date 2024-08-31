using UnityEngine;
using System.Collections;
using Zombie3D;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Advertisements;

class ArenaMenuUIPosition
{   
}

public class Avatar3DFrame : UI3DFrame
{

	protected Vector3 scale;
	protected float lastMotionTime;
	public Avatar3DFrame(Rect rect, Vector3 pos, Vector3 scale)
		: base(rect, pos)
	{
		this.scale = scale;
		ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
		
	}
	
	public void ChangeAvatar(AvatarType aType)
	{
		ClearModels();
		GameObject avatarObj = AvatarFactory.GetInstance().CreateAvatar(aType);
		//		Debug.Log ("--------------Avatar---------------- " + aType);
		
		avatarObj.transform.rotation = Quaternion.Euler(0f, 200f, 0f);
		
		ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();
		
		Weapon w = GameApp.GetInstance().GetGameState().GetBattleWeapons()[0];
		string firstWeaponName = w.Name;
		string wNameEnd = Weapon.GetWeaponNameEnd(w.GetWeaponType());
		//		Debug.Log ("--------------Weapon 3---------------- " + w);
		
		GameObject weapon = WeaponFactory.GetInstance().CreateWeaponModel(firstWeaponName, avatarObj.transform.position, avatarObj.transform.rotation);
		Transform weaponBoneTrans = avatarObj.transform.Find(BoneName.WEAPON_PATH);
		weapon.transform.parent = weaponBoneTrans;
		avatarObj.transform.localScale = scale/1.1f;
		avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_IDLE + wNameEnd].wrapMode = WrapMode.Loop;
		avatarObj.GetComponent<UnityEngine.Animation>().Play(AnimationName.PLAYER_IDLE + wNameEnd);
		
		SetModel(avatarObj);
		lastMotionTime = Time.time;
	}
	
	public void UpdateAnimation()
	{
		GameObject avatarObj = GetModel();
		
		Weapon w = GameApp.GetInstance().GetGameState().GetBattleWeapons()[0];
		string wNameEnd = Weapon.GetWeaponNameEnd(w.GetWeaponType());
		if (avatarObj != null)
		{
			
			if (w.GetWeaponType() == WeaponType.RocketLauncher || w.GetWeaponType() == WeaponType.Sniper)
			{
				if (Time.time - lastMotionTime > 7.0f)
				{
					string aniName = "";
					if (avatarObj.GetComponent<UnityEngine.Animation>().IsPlaying(AnimationName.PLAYER_RUN + wNameEnd))
					{
						aniName = AnimationName.PLAYER_IDLE + wNameEnd;
					}
					else
					{
						aniName = AnimationName.PLAYER_RUN + wNameEnd;
					}
					
					avatarObj.GetComponent<UnityEngine.Animation>()[aniName].wrapMode = WrapMode.Loop;
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(aniName);
					lastMotionTime = Time.time;
				}
			}
			else if (w.GetWeaponType() == WeaponType.Saw)
			{
				if (Time.time - lastMotionTime > 7.0f)
				{
					avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_SHOT + "_Saw2"].wrapMode = WrapMode.ClampForever;
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_SHOT + "_Saw");
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFadeQueued(AnimationName.PLAYER_SHOT + "_Saw2");
					lastMotionTime = Time.time;
				}
				
				if (avatarObj.GetComponent<UnityEngine.Animation>().IsPlaying(AnimationName.PLAYER_SHOT + "_Saw2") && (Time.time - lastMotionTime > avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_SHOT + "_Saw2"].clip.length * 2))
				{
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_IDLE + wNameEnd);
				}
			}
			else
			{
				if (Time.time - lastMotionTime > 7.0f)
				{
					avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].wrapMode = WrapMode.ClampForever;
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_STANDBY);
					lastMotionTime = Time.time;
				}
				
				if (avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].time > avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].clip.length)
				{
					avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_IDLE + wNameEnd);
				}
			}
		}
	}
}

public class ArenaMenuPanel : UIPanel, UIHandler
{

    protected UIImage background;
    protected UITextImage avatarPanel;
    protected UITextImage daysPanel;
    protected CashPanel cashPanel;

    protected UITextButton upgradeButton;
    protected UITextButton equipmentButton;

    protected UITextButton battleButton;
    protected UITextButton avatarButton;

    protected UIClickButton returnButton;
    protected UIClickButton optionsButton;

    protected UIClickButton leaderButton;
    protected UIClickButton achieveButton;

//    protected Avatar3DFrame avatar3DFrame;

    private ArenaMenuUIPosition uiPos;
    protected ArenaMenuUI ui;
    protected ReviewDialog reviewDialog;
    protected Timer fadeTimer = new Timer();
    public bool BattlePressed { get; set; }

    protected float startTime;

    public ArenaMenuPanel()
    {
        
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }

    public void Update()
    {
        /*
        if (Camera.mainCamera != null)
        {
            if (!Camera.mainCamera.audio.isPlaying && !BattlePressed)
            {
                Camera.mainCamera.audio.Play();
            }
        }
        */

      
    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
       
    }
}

public class MenuName
{
    public const int ARENA = 0;
    public const int UPGRADE = 1;
    public const int EQUIPMENT = 2;
    public const int AVATAR = 3;
    public const int SHOP = 4;
    public const int MENU_COUNT = 5;
}

public class LoadingPanel : UIPanel
{
    protected UIText m_LoadingText;
    protected UIText m_desc;
/*
    public LoadingPanel()
    {
        m_LoadingText = new UIText();
        m_LoadingText.Set(ConstData.FONT_NAME1, "LOADING...", ColorName.fontColor_darkorange);
        m_LoadingText.AlignStyle = UIText.enAlignStyle.center;
        m_LoadingText.Rect = AutoRect.AutoPos(new Rect(0, 70, 960, 100));

        m_desc = new UIText();
        m_desc.Set(ConstData.FONT_NAME2, "YOUR TOWN HAS BEEN INFECTED...\n\nALL YOUR FAMILY, FRIENDS, AND CO-WORKERS HAVE TURNED INTO ZOMBIES.\n\nFIGHT FOR YOUR LIFE.\n\nFIGHT FOR AS LONG AS YOU CAN...", Color.white);
        m_desc.AlignStyle = UIText.enAlignStyle.center;
        m_desc.Rect = AutoRect.AutoPos(new Rect(0, 120, 960, 640));
		string path = Application.persistentDataPath + "/Documents/";
		this.Add(m_desc);
        if (GameApp.GetInstance().GetGameState().FirstTimeGame && Application.loadedLevelName != SceneName.ARENA_MENU)
        {           
            
        }
        else
        {
            this.Add(m_LoadingText);
            int size = AvatarInfo.TIPS_INO.Length;
            int rnd = Random.RandomRange(0, size);
            m_desc.Rect = AutoRect.AutoPos(new Rect(0, 0, 960, 640));
            m_desc.SetText(AvatarInfo.TIPS_INO[rnd]);
        }
    }
*/
    public override void Show()
    {
        base.Show();
//        m_LoadingText.Visible = true;
//        m_desc.Visible = true;
    }

    public override void Hide()
    {
        base.Hide();
//        m_LoadingText.Visible = false;
//        m_desc.Visible = false;
    }


}


public class ArenaMenuUI : MonoBehaviour, UIHandler
{

    protected Rect[] buttonRect;

    protected float screenRatioX;
    protected float screenRatioY;

    ArenaMenuPanel arenaMenuPanel;
    OptionsMenuUI optionPanel;
    protected UIPanel[] panels = new UIPanel[MenuName.MENU_COUNT];

    protected AudioPlayer audioPlayer = new AudioPlayer();
    protected LoadingPanel loadingPanel;

    protected bool init = false;
    protected bool setAudioTime = false;

    protected float startTime;
    public UIPanel GetPanel(int panelID)
    {
        return panels[panelID];
    }

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
/*		
		if (GameObject.Find ("RevMob") != null) 			
		{			
			GameObject.Find ("RevMob").GetComponent<RevMobAds> ().AppLovinFullScreen ();
		}
*/		
		if (Advertisement.isSupported) 
		{
			Advertisement.allowPrecache = true;
			Advertisement.Initialize ("109410");
		} 
		else 
		{
			Debug.Log("Platform not supported");
		}
    }


    // Use this for initialization
    void Start()
    {
		InitDuc ();

        loadingPanel = new LoadingPanel();

        loadingPanel.Show();

        startTime = Time.time;
        StartCoroutine("Init");

		if (PlayerPrefs.GetInt ("RemoveAds_Captain_Strike_Zombie") == 0)
		{
			
			_removeAdsButton.SetActive (true);
		} 
		else 
		{
			_removeAdsButton.SetActive(false);
		}
    }

    IEnumerator Init()
    {
        yield return 1;

//        UIResourceMgr.GetInstance().LoadAllUIMaterials();
        //if (GameApp.GetInstance().GetGameState().FirstTimeGame)
        {
            if (Time.time - startTime < 1.5f)
            {
                yield return new WaitForSeconds(1.5f - (Time.time - startTime));
            }
        }
		GameApp.GetInstance().Load();

        GameApp.GetInstance().ClearScene();
 

        Transform audioFolderTrans = transform.Find("Audio");
        audioPlayer.AddAudio(audioFolderTrans, "Button");
        audioPlayer.AddAudio(audioFolderTrans, "Upgrade");
        audioPlayer.AddAudio(audioFolderTrans, "Battle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null && !setAudioTime)
        {
            //Camera.mainCamera.audio.time = GameApp.GetInstance().GetGameState().MenuMusicTime;
            setAudioTime = true;
        }


        ArenaMenuPanel panel = panels[MenuName.ARENA] as ArenaMenuPanel;
        if (panel != null)
        {
            panel.Update();
        }

        for (int i = 0; i < MenuName.MENU_COUNT; i++)
        {
            if (panels[i] != null)
            {
                panels[i].UpdateLogic();
            }
        }

        ShopUI shopUI = panels[MenuName.SHOP] as ShopUI;
        if (shopUI != null)
        {
            shopUI.GetPurchaseStatus();
        }


		//--------------Duc Script---------------
		if (avatarFrame != null)
		{
			avatarFrame.UpdateAnimation();
			//print("-----------Update Animation-----");
		} 

		if (Advertisement.isReady())
		{
			_unityAdsButton.SetActive(true);
		}
		else 
		{
			_unityAdsButton.SetActive(false);
		}

		if (Input.GetKey (KeyCode.Escape)) 		
		{
			Application.LoadLevel("MapUI");
		}
    }

    public static ArenaMenuUI GetInstance()
    {
        return GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>();
    }

    public AudioPlayer GetAudioPlayer()
    {
        return audioPlayer;
    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {


    }
	//--------------------------------Duc Script -------------------------------------------

	public string[] _avataInfo;
    public GameObject[] EquipAvatarShow, EquipWeaponShow;
    public Image ImageEquipAvatar, ImageEquipWeapon1, ImageEquipWeapon2, ImageEquipWeapon3;
    public GameObject _weaponPanel, _avatarPanel, _iapPanel, _iapDialog, _equipMidlePanel, _lowPanel;
	public Text _dayText, _cashText, _avatarInfoText;

	protected AvatarType _avataType;
	protected Avatar3DFrame avatarFrame;
	protected float lastMotionTime;
	protected GameState gameState = new GameState();
	protected CashPanel cashPanel;

	public GameObject[] _buyButton, _SelectButton, _buyIcon, _equipIcon;

	public GameObject[] _lockerWeaponIcon, _buyWeaponIcon, _equipWeaponIcon;
	public GameObject[] _selectWeaponButton, _upgradeButton, _buyWeaponButton, _buyAmmoButton;
	protected Weapon weapon;

	public GameObject _unityAdsButton, _removeAdsButton;


	public void ShareButton()
	{
		StartCoroutine(PostScreenshot());
	}

	private IEnumerator PostScreenshot() 
	{		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		AndroidSocialGate.StartShareIntent("Share Image", "This is to share with friend", tex);
		
		Destroy(tex);
		
	}


	public void ShowUnityAddButton()
	{
		Advertisement.Show(null, new ShowOptions 
		{
			pause = true,
			resultCallback = result => 
			{
				Debug.Log(result.ToString());
				AndroidMessage.Create ("Reward Success.","You getting 100 cash");
				gameState.DeliverIAPItem(IAPName.Cash5W);
				UpdateCashPanel();
			}
		});
	}

	public void InitDuc()
	{
		//------------------Duc Script-----------------------

		GameApp.GetInstance().Init();

		gameState = GameApp.GetInstance().GetGameState();
		gameState.Init ();
		GameApp.GetInstance().GetGameState().InitWeapons();
		weaponList = GameApp.GetInstance().GetGameState().GetBattleWeapons();		

		rectToWeaponMap = GameApp.GetInstance().GetGameState().GetRectToWeaponMap();
		
		PutBattleWeapons ();
		
		ShowWeaponInfo ();
		UpdateLockedWeaponIcon ();

		UpgradeAmmor ();
		_iapDialog.SetActive (false);

		UpdateEquipWeaponIcon ();
		UpdateEquipAvatarIcon (); // Update Euip Avatar
		AvataInfo ();
		_dayText.text = "LEVEL " + GameApp.GetInstance ().GetGameState ().LevelNum;
		
		SetCash(GameApp.GetInstance().GetGameState().GetCash()); //Show Cash
		avatarFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 10, 400, 600)), new Vector3(-1.589703f, -0.8f, 4.420711f), new Vector3(1.5f, 1.5f, 1.5f));
		SetBuyButtonText ();
		cashPanel = new CashPanel();	

		InitIAP ();
		//---------------------------------------------------
	}

	//--------------Equip Panel -------------------

	public void Back_Map_Panel_Btn()
	{
		Application.LoadLevel ("MapUI");
	}

    public void BackEquipPanelButton()
    {
        GameApp.GetInstance().Save();
        _panelSelected.SetActive(false);
        _equipMidlePanel.SetActive(true);
		_lowPanel.SetActive (true);
    }

    GameObject _panelSelected;
    int _indexEquipWeapon;
    public void IndexEquipWeaponObject(int index)
    {
        _indexEquipWeapon = index;
    }

    public void ChooseShopButton(GameObject go)
	{
		_equipMidlePanel.SetActive (false);
		_lowPanel.SetActive (false);
        _panelSelected = go;
        _panelSelected.SetActive(true);
	}

	//--------------Set Button IAP Dialog----------

	public void BackIAPButton()
	{
		avatarFrame.Show ();
//		_equipMidlePanel.SetActive (true);
//		_lowPanel.SetActive (true);
//		_avatarPanel.SetActive (false);
//		_weaponPanel.SetActive (false);
		_iapDialog.SetActive (false);
		_iapPanel.SetActive (false);
	}

	public void YesDialogButton()
	{
		avatarFrame.Hide ();
//		_avatarPanel.SetActive (false);
//		_weaponPanel.SetActive (false);
		_iapDialog.SetActive (false);
		_iapPanel.SetActive (true);
	}
	public void NoDialogButton()
	{
		_iapDialog.SetActive (false);
	}

	public void SetCash(int cash)
	{
		_cashText.text = "$" + cash.ToString ("N0");
	}

	public void SetBuyButtonText() // Kiem tra trang thai Button (Buy, Select)cua Nhan vat
	{
		for (int i=0; i<8; i++) 
		{
			if (GameApp.GetInstance ().GetGameState ().GetAvatarData ((AvatarType)(i)) == AvatarState.Avaliable) 
			{
				_SelectButton [i].SetActive (true);
				_buyIcon[i].SetActive(false);
				print(" i= " + i);
			} 
			else 
			{			
				_buyButton [i].SetActive (true);			
			}
		}
	}
	//--------------Show Info Character ----------------
	public void ShowInfoJoe()
	{   
		_avatarInfoText.text = "" + _avataInfo[0];
		ChangeAvatarModel (0);
	}
	public void ShowInfoWorker()
	{   
		_avatarInfoText.text = "" + _avataInfo[1];
		ChangeAvatarModel (1);
	}
	public void ShowInfoNerd()
	{   
		_avatarInfoText.text = "" + _avataInfo[2];
		ChangeAvatarModel (2);
	}
	public void ShowInfoDoctor()
	{   
		_avatarInfoText.text = "" + _avataInfo[3];
		ChangeAvatarModel (3);
	}
	public void ShowInfoCowboy()
	{   
		_avatarInfoText.text = "" + _avataInfo[4];
		ChangeAvatarModel (4);
	}
	public void ShowInfoSwat()
	{   
		_avatarInfoText.text = "" + _avataInfo[5];
		ChangeAvatarModel (5);
	}
	public void ShowInfoMarine()
	{   
		_avatarInfoText.text = "" + _avataInfo[6];
		ChangeAvatarModel (6);
	}
	public void ShowInfoBeaf()
	{   
		_avatarInfoText.text = "" + _avataInfo[7];
		ChangeAvatarModel (7);
	}

	//-------------------Buy Character-----------------------

	public void UpdateEquipAvatarIcon()
	{
		int _euqipIconIndex = (int)GameApp.GetInstance().GetGameState().Avatar;
		print("-----------Update Equip Avatar Icon-----------" + _euqipIconIndex);
        for (int i = 0; i < EquipAvatarShow.Length; i++)
        {
            if (i == _euqipIconIndex)
            {
                _equipIcon[_euqipIconIndex].SetActive(true);
                ImageEquipAvatar.sprite = EquipAvatarShow[_euqipIconIndex].GetComponent<Image>().sprite;
            }
            else
            {
                _equipIcon[i].SetActive(false);
            }
        }
	}

	public void UpdateCashPanel()
	{	
	
		for (int i=0; i<8; i++) 
		{
			if (GameApp.GetInstance ().GetGameState ().GetAvatarData ((AvatarType)(i)) == AvatarState.Avaliable) 
			{
				_SelectButton [i].SetActive (true);
				_buyButton [i].SetActive (false);
				_buyIcon[i].SetActive(false);
			} 
			else 
			{
				_buyButton [i].SetActive (true);
			}
		}

		SetCash(GameApp.GetInstance().GetGameState().GetCash());
		//GameApp.GetInstance().Init();

	}

	public void BuyJoe()
	{   
		_avatarInfoText.text = "" + _avataInfo[0];
		ChangeAvatarModel (0);
		GameApp.GetInstance().GetGameState().Avatar = (AvatarType)(0);
		UpdateEquipAvatarIcon ();
	}

    public void BuyAvatar(int index)
    {
        ChangeAvatarModel(index);
        _avatarInfoText.text = "" + _avataInfo[index];
        AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
        if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(index)) == AvatarState.ToBuy)
        {
            GameConfig gConf = GameApp.GetInstance().GetGameConfig();
            if (GameApp.GetInstance().GetGameState().BuyAvatar((AvatarType)(index), gConf.GetAvatarConfig(index).price))
            {
                SetBuyButtonText();
                print("Du tien");
            }
            else
            {
                _iapDialog.SetActive(true);
                //gameState.DeliverIAPItem(IAPName Cash50W);
                print("Thieu tien. Show IAP Panel");

            }
            UpdateCashPanel();
        }
        else if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(index)) == AvatarState.Avaliable)
        {
            GameApp.GetInstance().GetGameState().Avatar = (AvatarType)(index);
        }
        UpdateEquipAvatarIcon();
    }

	public void AvataInfo()
	{   
		switch((int)_avataType)
		{
		case 0:
			print("Avatar = " + _avataType);
			_avatarInfoText.text = "" + _avataInfo[0];
			return;
		case 1:
			print("Avatar = " + _avataType);
			_avatarInfoText.text = "" + _avataInfo[1];			
			return;
		case 2:
			print("Avatar = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[2];
			return;
		case 3:
			print("Avatar = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[3];
			return;
		case 4:
			print("Avatar = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[4];
			return;
		case 5:
			print("Avatra = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[5];
			return;
		case 6:
			print("Avatar = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[6];
			return;
		case 7:
			print("Avatar = " + _avataInfo);
			_avatarInfoText.text = "" + _avataInfo[7];
			return;		
		}
	}
	public void ChangeAvatarModel(int index)
	{
		avatarFrame.ChangeAvatar ((AvatarType)index);
	}

	//------------------- Buy Ammor-----------------------
	GameConfig gConf = GameApp.GetInstance().GetGameConfig();
	public GameObject[] Star;

	public Text AmmorPrice;

	public void UpgradeAmmor()
	{
		if (gameState.ArmorLevel != gConf.playerConf.maxArmorLevel)
		{			
			if (gameState.UpgradeArmor(gameState.GetArmorPrice()))
			{
				ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
			}
			else
			{
				_iapDialog.SetActive(true);
				Debug.Log("Mua that bai. Mua them Cash");
			}
		}
		else
		{
			AndroidMessage.Create ("Armor Max."," Thank You ");
			//Debug.Log("Update that bai. Mua them Cash");
		}
		UpdateInfo("", "", gConf.playerConf.maxArmorLevel, gameState.ArmorLevel);

		if (gConf.playerConf.maxArmorLevel == gameState.ArmorLevel)
		{
			AmmorPrice.enabled = false;
		}
		AmmorPrice.text = "$ " + gameState.GetArmorPrice ();
		UpdateCashPanel();
	}

	protected void UpdateStar(int level)
	{		
		for (int i = 0; i < 10; i++)
		{
			if (i < level)
			{
				Star[i].SetActive (true);
			}
			else
			{
				Star[i].SetActive (false);
			}
		}
	}
	//-------------------Buy Weapon-----------------------

	protected Weapon selectedWeapon ;
	protected int currentSelectionWeapon;
	protected List<Weapon> weaponList;
	protected int upgradeSelection = 1;

	public Text[] currentDamageValueText;
	public Text[] nextDamageValueText;

	public Text[] currentFillRateValueText;
	public Text[] nextFillRateValueText;

	public Text[] currentAccuracyValueText;
	public Text[] nextAccuracyValueText;

	public Text[] currentAmmoValueText;
	public Text[] nextAmmoValueText;

	public Text[] CashPanelWeaponText;

	public Text[] priceBulletWeaponText;

	protected int WeaponInfo;

	public void UpdateWeaponInfo()
	{
		if (selectedWeapon != null)
		{
			print ("########################## = " + selectedWeapon.Exist);

			int maxLevel = (int)selectedWeapon.WConf.damageConf.maxLevel;
			int level = selectedWeapon.DamageLevel;
			UpdateInfo (selectedWeapon.Damage, selectedWeapon.GetNextLevelDamage (), maxLevel, level, 0);
			
			int maxFillRateLevel = (int)selectedWeapon.WConf.attackRateConf.maxLevel;
			int FillRatelevel = selectedWeapon.FrequencyLevel;
			UpdateInfo (selectedWeapon.AttackFrequency, selectedWeapon.GetNextLevelFrequency (), maxFillRateLevel, FillRatelevel, 1);
			
			
			int maxAccuracyLevel = (int)selectedWeapon.WConf.accuracyConf.maxLevel;
			int Accuracylevel = selectedWeapon.AccuracyLevel;
			UpdateInfo (selectedWeapon.Accuracy, selectedWeapon.GetNextLevelAccuracy (), maxAccuracyLevel, Accuracylevel, 2);
			
			UpdateInfo ("x" + selectedWeapon.BulletCount.ToString (), "+" + selectedWeapon.WConf.bullet, (int)selectedWeapon.GetWeaponType ());
		}
		else 
		{
			GameConfig gConf = GameApp.GetInstance().GetGameConfig();
		}
		cashPanel.SetCash(gameState.GetCash());

	}


	//Ammor,...
	public void UpdateInfo(string cStr, string nStr, int maxLevel, int level)
	{		
		UpdateStar(level);		
	}


	//Ammo,....
	public void UpdateInfo(string cStr, string nStr, int weaponTypeIndex)
	{
		Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
		Rect brect = ButtonsTexturePosition.GetBulletsLogoRect(weaponTypeIndex);
		currentAmmoValueText[WeaponInfo].text = "x " + weaponList[WeaponInfo].BulletCount;
		nextAmmoValueText[WeaponInfo].text = "+" + weaponList[WeaponInfo].WConf.bullet;
	}

	//damage,.....
	public void UpdateInfo(float cValue, float nValue, int maxLevel, int level, int uType)
	{
		if (uType == 0)
		{
			currentDamageValueText[ WeaponInfo].text = "" + (Math.SignificantFigures (cValue, 4).ToString ());
				if (level == maxLevel)
				{
				//nextDamageValueText[ WeaponInfo].enabled = false;
				nextDamageValueText[ WeaponInfo].text = "Max";
				}
				else
				{
				nextDamageValueText[ WeaponInfo].enabled = true;
				nextDamageValueText[ WeaponInfo].text = "" + (Math.SignificantFigures(nValue, 4).ToString());
				}
		}
		else if (uType == 1)
		{
			currentFillRateValueText[ WeaponInfo].text = (Math.SignificantFigures (cValue, 4).ToString ()) + "s";
				if(level== maxLevel)
				{
				nextFillRateValueText[ WeaponInfo].text = "Max";
				}
				else
				{
				nextFillRateValueText[ WeaponInfo].text = (Math.SignificantFigures(nValue, 4).ToString()) + "s";
				}
		}
		else if (uType == 2)
		{
			currentAccuracyValueText[ WeaponInfo].text = (Math.SignificantFigures (cValue, 4).ToString ()) + "%";
				if(level== maxLevel)
				{
				nextAccuracyValueText[ WeaponInfo].text = "Max";
				}
				else
				{
				nextAccuracyValueText[ WeaponInfo].text = (Math.SignificantFigures(nValue, 4).ToString()) + "%";
				}
		}	
	}

	public void BuyWeapon()
	{ 
		if (currentSelectionWeapon >=0)
		{
			Debug.Log("selection weapon:" + weaponList[currentSelectionWeapon].Name);
			selectedWeapon = weaponList[currentSelectionWeapon];
			if (selectedWeapon.Exist == WeaponExistState.Owned)
			{
				
			}
			else if (selectedWeapon.Exist == WeaponExistState.Unlocked)
			{
				Debug.Log("selection weapon:" + selectedWeapon);
				WeaponBuyStatus wBuyStatus = gameState.BuyWeapon(selectedWeapon, selectedWeapon.WConf.price);
				if (wBuyStatus == WeaponBuyStatus.Succeed)
				{
					ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
					Debug.Log("Mua thanh Cong: " + selectedWeapon);
					
				}
				else if (wBuyStatus == WeaponBuyStatus.NotEnoughCash)
				{
							
					_iapDialog.SetActive(true);
					Debug.Log("Mua that bai. Mua them Cash: " + selectedWeapon);
//					gameState.DeliverIAPItem(IAPName.Cash1650W);

				}
				else if (wBuyStatus == WeaponBuyStatus.Locked)
				{
					//			iapDialog.SetText(IAPDialog.NOT_AVAILABLE);
					//			iapDialog.Show();
					Debug.Log("Weapon chua mo khoa. khong the mua: " + selectedWeapon);
				}
				
				if (selectedWeapon.Exist == WeaponExistState.Owned)
				{
					int index = gameState.GetWeaponIndex(selectedWeapon);
					Debug.Log("Selection weapon:" + selectedWeapon);					
				}
			}
		}

		UpdateCashPanel();
	}

	void PutBattleWeapons()
	{		
		for (int i = 0; i < SELECTION_NUM; i++)
		{
			int weaponID = rectToWeaponMap[i];
			if (weaponID != -1)
			{
				Debug.Log("Weapon ID :" + weaponID);
                // display weapon
                EquipWeapon(i, weaponID);
			}
		}
	}
	int InBattleWeaponCount()
	{
		int sum = 0;
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i].IsSelectedForBattle)
			{
				sum++;
			}
		}
		return sum;
	}

	void SelectWeapon(int weaponID, int selectRectIndex)
	{
		bool alreadySelected = false;
		for (int j = 0; j < SELECTION_NUM; j++)
		{
			if (rectToWeaponMap[j] != -1)
			{
				if (rectToWeaponMap[j] == weaponID)
				{
					alreadySelected = true;
				}
				
			}
		}
		
		if (!alreadySelected)
		{			
			int oldWeaponID = rectToWeaponMap[selectRectIndex];
			if (oldWeaponID != -1)
			{				
				weaponList[oldWeaponID].IsSelectedForBattle = false;
			}
			weaponList[weaponID].IsSelectedForBattle = true;    
			rectToWeaponMap[selectRectIndex] = weaponID;
			avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
		}	
	}

	protected Rect[] selectionRect = new Rect[Constant.SELECTION_NUM];

	public void Select_WIN_Btn(int index)
	{
        currentSelectionWeaponIndex = index;
        
        // select weapon
        SelectWeapon(currentSelectionWeaponIndex, _indexEquipWeapon);
        
        // display weapon
        EquipWeapon(_indexEquipWeapon, index);

        UpdateEquipWeaponIcon();
        UpdateLockedWeaponIcon();
	}

	public void Buy_More_Cash_Btn()
	{
		_iapDialog.SetActive(true);
	}

	public void Buy_Bullet_Btn(int index)
	{
		currentSelectionWeapon = index;
		if (weaponList[currentSelectionWeapon].BulletCount < 9999)
		{
			if (gameState.BuyBullets(weaponList[currentSelectionWeapon], weaponList[currentSelectionWeapon].WConf.bullet, weaponList[currentSelectionWeapon].WConf.bulletPrice))
			{
				ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
			}
			else
			{
				_iapDialog.SetActive(true);
				Debug.Log("Update that bai. Mua them Cash");
			}
		}
		else
		{
			AndroidMessage.Create ("Bullet Max."," Thank You ");
			//Debug.Log("Update that bai. Mua them Cash");
		}
		UpdateWeaponInfo ();
		UpdateCashPanel();
	}


	public void Buy_Weapon_Btn(int index)
	{
        currentSelectionWeapon = index;
		if (weaponList [currentSelectionWeapon].Exist == WeaponExistState.Unlocked)
		{
            WeaponInfo = index;
			BuyWeapon ();
		}
		else if (weaponList [currentSelectionWeapon].Exist == WeaponExistState.Owned)
		{
			if (!weaponList[currentSelectionWeapon].IsMaxLevelDamage())
			{
				WeaponInfo = index;

				if (gameState.UpgradeWeapon(weaponList[currentSelectionWeapon], weaponList[currentSelectionWeapon].Damage * weaponList[currentSelectionWeapon].WConf.damageConf.upFactor, 0f, 0f, weaponList[currentSelectionWeapon].GetDamageUpgradePrice()))
				{
					Debug.Log("upgrade");
					ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
					if (gameState.UpgradeWeapon(weaponList[currentSelectionWeapon], 0f, weaponList[currentSelectionWeapon].AttackFrequency * weaponList[currentSelectionWeapon].WConf.attackRateConf.upFactor, 0f, weaponList[currentSelectionWeapon].GetDamageUpgradePrice()))
					{
						ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
					}
					if (gameState.UpgradeWeapon(weaponList[currentSelectionWeapon], 0f, 0f, weaponList[currentSelectionWeapon].Accuracy * weaponList[currentSelectionWeapon].WConf.accuracyConf.upFactor, weaponList[currentSelectionWeapon].GetDamageUpgradePrice()))
					{
						ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
					}
				}
				else
				{
					_iapDialog.SetActive(true);
					Debug.Log("Update that bai. Mua them Cash");
				}
			}
			else
			{
				AndroidMessage.Create ("Max Level."," Thank You ");
				//Debug.Log("Update that bai. Mua them Cash");
			}
		}
		UpdateCashPanel ();
		UpdateWeaponInfo ();
		UpdateLockedWeaponIcon ();
	}

	//-------------------Check Locked / Unlock/ Buyed Weapon-----------------------
	public void UpdateLockedWeaponIcon()
	{			
		AmmorPrice.text = "$ " + gameState.GetArmorPrice ();

		weaponList = GameApp.GetInstance ().GetGameState ().GetWeapons ();
		
//		print ("-----------Weapon-------------" + weaponList);
		
		for (int i = 0; i < weaponList.Count; i++) 
		{
			if (weaponList [i].Exist == WeaponExistState.Locked) 
			{
				//print("-----------Weapon Dang Khoa-------------" + weaponList[i]);
				_lockerWeaponIcon [i].SetActive (true);
				_buyAmmoButton[i].SetActive(false);

				CashPanelWeaponText [i].text = "Locked";

			} 
			else if (weaponList [i].Exist == WeaponExistState.Unlocked) 
			{
				//print("-----------Weapon Da Mo Khoa-------------" + weaponList[i].Name);
				_lockerWeaponIcon [i].SetActive (false);
				_buyWeaponButton [i].SetActive (true);
				_buyWeaponIcon [i].SetActive (true);
				_buyAmmoButton[i].SetActive(false);

				CashPanelWeaponText [i].text = "$" + weaponList [i].WConf.price;

			} 
			else if (weaponList [i].Exist == WeaponExistState.Owned)
			{
				//print("-----------Weapon Da Mua-------------" + weaponList[i]);
				_buyWeaponButton [i].SetActive (false);
				_buyWeaponIcon [i].SetActive (false);
				_selectWeaponButton [i].SetActive (true);
				_upgradeButton [i].SetActive (true);
				_buyAmmoButton[i].SetActive(true);

				CashPanelWeaponText [i].text = "$" + weaponList [i].GetDamageUpgradePrice ();

				priceBulletWeaponText[i].text = "$" + weaponList[i].WConf.bulletPrice;

				if(weaponList[i].IsSelectedForBattle == true)
				{
					_selectWeaponButton [i].SetActive (false);
				}
			}
		}
	}
	public void ShowWeaponInfo()
	{			
		weaponList = GameApp.GetInstance().GetGameState().GetWeapons();

//		print("-----------Weapon-------------" + weaponList);

		for (int i = 0; i < weaponList.Count; i++)
		{
			currentDamageValueText[i].text = "" + weaponList[i].Damage;
			nextDamageValueText[i].text = "" + weaponList[i].GetNextLevelDamage();

			currentFillRateValueText[i].text = weaponList[i].AttackFrequency + "s";
			nextFillRateValueText[i].text = weaponList[i].GetNextLevelFrequency() + "s";

			currentAccuracyValueText[i].text = weaponList[i].Accuracy + "%";
			nextAccuracyValueText[i].text = weaponList[i].GetNextLevelAccuracy()+ "%";

			currentAmmoValueText[i].text = "x " + weaponList[i].BulletCount;
			nextAmmoValueText[i].text = "+" + weaponList[i].WConf.bullet;
		}

	}

	//-------------------Check Equip Weapon-----------------------

	protected int[] SelecWeapon;
	protected bool _SelecWeapon01, _SelecWeapon02, _SelecWeapon03;
	protected int currentSelectionWeaponIndex;
	protected int[] rectToWeaponMap;

	protected int SELECTION_NUM = Constant.SELECTION_NUM;
	int _equipWeaponIconIndex = 0;

	public void UpdateEquipWeaponIcon()
	{
		for (int j = 0; j < weaponList.Count; j++) 
		{
			if (weaponList[j].IsSelectedForBattle == true)
			{
				print ("-----------Current Equip Weapon----------- =" + weaponList[j].Name);
				_equipWeaponIcon[j].SetActive(true);
				_selectWeaponButton [j].SetActive (false);
			}
			else 
			{
                print("-----------Current No Equip Weapon----------- =" + weaponList[j].Name);
				_equipWeaponIcon[j].SetActive(false);
			}
		}

	}

    void EquipWeapon(int type, int indexWeapon)
    {
        Image img = null;
        if (type == 0) img = ImageEquipWeapon1;
        else if (type == 1) img = ImageEquipWeapon2;
        else if (type == 2) img = ImageEquipWeapon3;
        img.sprite = EquipWeaponShow[indexWeapon].GetComponent<Image>().sprite;
    }


    public void SetEquipWeaponIcon()
    {
        for (int i = 0; i < 13; i++)
        {
            if (i == _equipWeaponIconIndex)
            {
                _equipWeaponIcon[_equipWeaponIconIndex].SetActive(true);
                _selectWeaponButton[_equipWeaponIconIndex].SetActive(false);
            }
            else
            {
                _equipWeaponIcon[i].SetActive(false);
                if (weaponList[i].Exist == WeaponExistState.Owned)
                {
                    _selectWeaponButton[i].SetActive(true);
                }
            }
        }
    }

	//------------------------IAP--------------------------------



	private bool _isInited = false;

	public const string IAP_099 = "com.codostudio.captainstrikezombie.0.99";
	public const string IAP_199 = "com.codostudio.captainstrikezombie.1.99";
	public const string IAP_399 = "com.codostudio.captainstrikezombie.3.99";
	public const string IAP_999 = "com.codostudio.captainstrikezombie.9.99";
	public const string IAP_1999 = "com.codostudio.captainstrikezombie.19.99";
	public const string REMOVE_ADS = "com.codostudio.captainstrikezombie.removeads";


	private bool ListnersAdded = false;


	public void InitIAP() 
	{		
		//Filling product list
		if(ListnersAdded) {
			return;
		}
		//When you will add your own proucts you can skip this code section of you already have added
		//your products ids under the editor setings menu
		AndroidInAppPurchaseManager.instance.AddProduct(IAP_099);
		AndroidInAppPurchaseManager.instance.AddProduct(IAP_199);
		AndroidInAppPurchaseManager.instance.AddProduct(IAP_399);
		AndroidInAppPurchaseManager.instance.AddProduct(IAP_999);
		AndroidInAppPurchaseManager.instance.AddProduct(IAP_1999);
		AndroidInAppPurchaseManager.instance.AddProduct(REMOVE_ADS);
		
		
		//listening for purchase and consume events
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		
		
		//listening for store initilaizing finish
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		
		
		//you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
		AndroidInAppPurchaseManager.Instance.LoadStore();
		
		//or You can pass base64EncodedPublicKey using scirption:
		//AndroidInAppPurchaseManager.instance.loadStore(YOU_BASE_64_KEY_HERE);
		
		ListnersAdded = true;

		
	}
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void purchase(string SKU) {
		AndroidInAppPurchaseManager.Instance.Purchase (SKU);
	}
	
	public void consume(string SKU) {
		AndroidInAppPurchaseManager.Instance.Consume (SKU);
	}
	
	//--------------------------------------
	//  GET / SET
	//--------------------------------------
	
	public bool isInited {
		get {
			return _isInited;
		}
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) 
	{
		//some stuff for processing product purchse. Add coins, unlock track, etc
		
		switch(purchase.SKU) 
		{
		case IAP_099:
			consume(IAP_099);
			break;
		case IAP_199:
			consume(IAP_199);
			break;
		case IAP_399:
			consume(IAP_399);
			break;
		case IAP_999:
			consume(IAP_999);
			break;
		case IAP_1999:
			consume(IAP_1999);
			break;
		case REMOVE_ADS:
			consume(REMOVE_ADS);
			break;
		}
	}
	
	private void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) 
	{
		switch(purchase.SKU) 
		{
		case IAP_099:
			gameState.DeliverIAPItem(IAPName.Cash50W);
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Back Game");
			break;
		case IAP_199:
			gameState.DeliverIAPItem(IAPName.Cash120W);
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Back Game");
			break;
		case IAP_399:
			gameState.DeliverIAPItem(IAPName.Cash270W);
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Back Game");
			break;
		case IAP_999:
			gameState.DeliverIAPItem(IAPName.Cash750W);
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Back Game");
			break;
		case IAP_1999:
			gameState.DeliverIAPItem(IAPName.Cash1650W);
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Back Game");
			break;
		case REMOVE_ADS:
			PlayerPrefs.SetInt ("RemoveAds_Captain_Strike_Zombie", 1) ;
			AndroidMessage.Create ("Buy Product Success.","Thank you for support. Please Restart Game");
			break;
		}
		UpdateCashPanel();
	}
	
	private void OnProductPurchased(BillingResult result) {
		
		//this flag will tell you if purchase is available
		//result.isSuccess
		
		
		//infomation about purchase stored here
		//result.purchase
		
		//here is how for example you can get product SKU
		//result.purchase.SKU
		
		
		if(result.isSuccess)
		{
			OnProcessingPurchasedProduct (result.purchase);
		} 
		else 
		{
			//AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
			Debug.Log ("Product Purchase Failed: " + result.response.ToString() + " " + result.message);
		}
		
		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
	}
	
	
	private void OnProductConsumed(BillingResult result)
	{		
		if(result.isSuccess)
		{
			OnProcessingConsumeProduct (result.purchase);
		} 
		else
		{
			//AndroidMessage.Create("Product Consume Failed", result.response.ToString() + " " + result.message);
			Debug.Log ("Product Consume Failed: " + result.response.ToString() + " " + result.message);
		}
		
		Debug.Log ("Consume Responce: " + result.response.ToString() + " " + result.message);
	}
	
	
	private void OnBillingConnected(BillingResult result)
	{		
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;		
		
		if(result.isSuccess)
		{
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
			AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();			
		} 
		
//		AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}	
	
	private void OnRetrieveProductsFinised(BillingResult result)
	{
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		if(result.isSuccess) 
		{
			UpdateStoreData();
			_isInited = true;
		} 
		else
		{
//			AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
			Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
		}
	}
	private void UpdateStoreData()
	{
		
		foreach(GoogleProductTemplate p in AndroidInAppPurchaseManager.instance.Inventory.Products)
		{
			Debug.Log("Loaded product: " + p.Title);
		}	

		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(IAP_099))
		{
			consume(IAP_099);
		}
		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(IAP_199))
		{
			consume(IAP_199);
		}
		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(IAP_399))
		{
			consume(IAP_399);
		}
		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(IAP_999))
		{
			consume(IAP_999);
		}
		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(IAP_1999))
		{
			consume(IAP_1999);
		}
		if(AndroidInAppPurchaseManager.instance.Inventory.IsProductPurchased(REMOVE_ADS))
		{
			consume(REMOVE_ADS);
		}
	}

	public void BuyRemoveAds()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (REMOVE_ADS);
	}
	public void BuyIAP_099()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (IAP_099);
	}
	public void BuyIAP_199()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (IAP_199);
	}
	public void BuyIAP_399()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (IAP_399);
	}
	public void BuyIAP_999()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (IAP_999);
	}
	public void BuyIAP_1999()
	{
		AndroidInAppPurchaseManager.Instance.Purchase (IAP_1999);
	}
	//------------------------End IAP----------------------------

}
