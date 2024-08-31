//! @file UIManager.cs


using UnityEngine;
using System.Collections;


//! @class UIHandler
//! @brief UI�¼�������
public interface UIHandler
{
	void HandleEvent(UIControl control, int command, float wparam, float lparam);
}


//! @class UIManager
//! @brief UI�ؼ�������
public class UIManager : MonoBehaviour, UIContainer
{
	//! ʹ�õĲ�(�ⲿָ��)
	public int LAYER = 0;

	//! ���(�ⲿָ��)
	public int DEPTH = 0;

	//! ����(�ⲿָ��)
	public bool CLEAR = false;

	//! UIMesh
	private UIMesh m_UIMesh;

	//! SpriteCamera
	private SpriteCamera m_SpriteCamera;

	//! UI�¼�������
	private UIHandler m_UIHandler;

	//! �ؼ�����
	private ArrayList m_Controls;

	//! ����
	public UIManager()
	{
		m_UIMesh = null;
		m_SpriteCamera = null;
		m_UIHandler = null;
		m_Controls = new ArrayList();
	}

	//! �����¼�������
	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	//! ����ӿؼ�
	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}

	//! �Ƴ��ӿؼ�
	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	//! �Ƴ�ȫ���ӿؼ�
	public void RemoveAll()
	{
		m_Controls.Clear();
	}

// 	//! ���������¼�
// 	public bool HandleInput(Touch touch)
// 	{
//         UITouchInner touch_inner = new UITouchInner();
//         touch_inner.deltaPosition = touch.deltaPosition;
//         touch_inner.deltaTime = touch.deltaTime;
//         touch_inner.fingerId = touch.fingerId;
//         touch_inner.phase = touch.phase;
//         touch_inner.position = touch.position;
//         touch_inner.tapCount = touch.tapCount;
//         return HandleInputInner(touch_inner);
// 	}

    public bool HandleInput(UITouchInner touch)
    {
        for (int i = m_Controls.Count - 1; i >= 0; --i)
        {
            UIControl control = (UIControl)m_Controls[i];
            if (control.Enable)
            {
                bool handle = control.HandleInput(touch);
                if (handle)
                {
                    return true;
                }
            }
        }
        return false;
    }

	public void Awake()
	{
	}

	public void SetParameter(int layer, int depth, bool clear)
	{
		LAYER = layer;
		DEPTH = depth;
		CLEAR = clear;
	}

	public void Start()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void LateUpdate()
	{
		// �������
		m_UIMesh.RemoveAll();
		for (int i = 0; i < m_Controls.Count; ++i)
		{
			UIControl control = (UIControl)m_Controls[i];
			control.Update();
			if (control.Visible)
			{
				control.Draw();
			}
		}

		m_UIMesh.DoLateUpdate();
	}

	//! �����ӿؼ�����
	public void DrawSprite(UISprite sprite)
	{
		m_UIMesh.Add(sprite);
	}

	//! �����ӿؼ��¼�
	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
	}

	private void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject obj = new GameObject("UIMesh");
		obj.transform.parent = gameObject.transform;

		m_UIMesh = (UIMesh)obj.AddComponent(typeof(UIMesh));
		m_UIMesh.Initialize(LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject obj = new GameObject("SpriteCamera");
		obj.transform.parent = gameObject.transform;

		m_SpriteCamera = (SpriteCamera)obj.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(CLEAR);
		m_SpriteCamera.SetDepth(DEPTH);
		m_SpriteCamera.SetViewport(new Rect(0, 0, Screen.width, Screen.height));
	}
}

