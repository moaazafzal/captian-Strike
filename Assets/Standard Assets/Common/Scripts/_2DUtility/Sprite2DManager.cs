//! @file Sprite2DManager.cs


using UnityEngine;
using System.Collections;


//! @class Sprite2DManager
//! @brief 2D���������
public class Sprite2DManager : MonoBehaviour
{
	//! ʹ�õĲ�(�ⲿָ��)
	public int LAYER = 0;

	//! ��������(�ⲿָ��)
	public int MAX_SPRITE_LAYER = 16;

	//! SpriteMesh
	private SpriteMesh m_SpriteMesh = null;

	//! SpriteCamera
	private SpriteCamera m_SpriteCamera = null;

	//! ����
	public Sprite2DManager()
	{
	}

	//! ����
	~Sprite2DManager()
	{
	}

	public void Awake()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void Start()
	{
	}

	//! ���2D����
	public void Add(Sprite2D sprite)
	{
		m_SpriteMesh.Add(sprite);
	}

	//! �Ƴ�2D����
	public void Remove(Sprite2D sprite)
	{
		m_SpriteMesh.Remove(sprite);
	}

	//! �Ƴ�ȫ��2D����
	public void RemoveAll()
	{
		m_SpriteMesh.RemoveAll();
	}

	//! ȡSpriteMesh
	public SpriteMesh GetSpriteMesh()
	{
		return m_SpriteMesh;
	}

	//! ȡSpriteCamera
	public SpriteCamera GetSpriteCamera()
	{
		return m_SpriteCamera;
	}

	//! ���ÿ��ӷ�Χ
	public void SetViewport(Rect range)
	{
		m_SpriteCamera.SetViewport(range);
	}

	//! ���ÿ��ӷ�Χ
	public void SetViewport(Vector2 position, Vector2 size)
	{
		Rect range = new Rect(position.x - size.x / 2, position.y - size.y / 2, size.x, size.y);
		m_SpriteCamera.SetViewport(range);
	}

	//! ��Ļ��ת��Ϊ�����
	public Vector2 ScreenToWorld(Vector2 point)
	{
		return m_SpriteCamera.ScreenToWorld(point);
	}

	private void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject obj = new GameObject("SpriteMesh");
		obj.transform.parent = gameObject.transform;

		m_SpriteMesh = (SpriteMesh)obj.AddComponent(typeof(SpriteMesh));
		m_SpriteMesh.Initialize(LAYER, MAX_SPRITE_LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject obj = new GameObject("SpriteCamera");
		obj.transform.parent = gameObject.transform;

		m_SpriteCamera = (SpriteCamera)obj.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(true);
		m_SpriteCamera.SetDepth(0);
	}
}

