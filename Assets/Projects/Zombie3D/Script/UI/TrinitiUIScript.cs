using UnityEngine;
using System.Collections;
using System.IO;
using Zombie3D;


class TrinitiUIPosition
{
    public Rect Background = new Rect(0, 0, 960, 640);

}


public class TrinitiUIScript : MonoBehaviour
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;  //×îºó´ø·´Ð±¸Ü
    protected Material trinitiMaterial;


    protected UIImage background;

    private TrinitiUIPosition uiPos;
    //private StartMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected float startTime;


    protected Timer fadeTimer = new Timer();
    // Use this for initialization
    void Start()
    {


//        FlurryTAd.ShowTAd(true);

        startTime = Time.time;

        uiPos = new TrinitiUIPosition();
        //texPos = new StartMenuTexturePosition();


        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
        m_UIManager.CLEAR = true;

        trinitiMaterial = UIResourceMgr.GetInstance().GetMaterial("Triniti");
        background = new UIImage();
        background.SetTexture(trinitiMaterial, StartMenuTexturePosition.Background, AutoRect.AutoSize(StartMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);

        m_UIManager.Add(background);        

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 3.0f)
        {
            Application.LoadLevel(SceneName.START_MENU);
        }

    }

       

}
