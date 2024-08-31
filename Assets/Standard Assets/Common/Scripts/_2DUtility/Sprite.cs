//! @file Sprite.cs


using UnityEngine;


//! @class Sprite
//! @brief ����
public class Sprite
{
	//! �ߴ�
	protected Vector2 m_Size;

	//! λ��
	protected Vector2 m_Position;

	//! ��ת
	protected float m_Rotation;

	//! ��
	protected int m_Layer;

	//! ����
	protected Material m_Material;

	//! ��ͼ����
	protected Rect m_TextureRect;

	//! ˮƽ��ת
	protected bool m_FlipX;

	//! ��ֱ��ת
	protected bool m_FlipY;

	//! ��ɫ
	protected Color m_Color;

	//! ��������
	protected Vector3 [] m_Vertices = new Vector3[4];

	//! �Ƿ���Ҫ���¶�������
	protected bool m_UpdateVertices;

	//! UV����
	protected Vector3 [] m_UV = new Vector3[4];

	//! �Ƿ���Ҫ����UV����
	protected bool m_UpdateUV;

	//! ��������
	static protected int [] m_Triangles = new int[6];


	//! ��С���
	public const int MinLayer = 0;

	//! �����
	public const int MaxLayer = 15;


	//! ��̬����
	static Sprite()
	{
		// 0 ------ 1
		// |        |
		// |        |
		// |        |
		// 3 ------ 2

		m_Triangles[0] = 0;
		m_Triangles[1] = 3;
		m_Triangles[2] = 1;

		m_Triangles[3] = 3;
		m_Triangles[4] = 2;
		m_Triangles[5] = 1;
	}

	//! ����
	public Sprite()
	{
		m_Position = Vector2.zero;
		m_Size = Vector2.zero;
		m_Rotation = 0;
		m_Layer = 0;
		m_Material = null;
		m_TextureRect = new Rect(0, 0, 0, 0);
		m_FlipX = false;
		m_FlipY = false;
		m_Color = Color.white;

		m_Vertices[0] = Vector3.zero;
		m_Vertices[1] = Vector3.zero;
		m_Vertices[2] = Vector3.zero;
		m_Vertices[3] = Vector3.zero;
		m_UpdateVertices = true;

		m_UV[0] = Vector3.zero;
		m_UV[1] = Vector3.zero;
		m_UV[2] = Vector3.zero;
		m_UV[3] = Vector3.zero;
		m_UpdateUV = true;
	}

	//! ����
	~Sprite()
	{
	}

	//! �ߴ�
	public Vector2 Size
	{
		get { return m_Size; }
		set { m_Size = value; m_UpdateVertices = true; }
	}

	//! λ��
	public Vector2 Position
	{
		get { return m_Position; }
		set { m_Position = value; m_UpdateVertices = true; }
	}

	//! ��ת
	public float Rotation
	{
		get { return m_Rotation; }
		set { m_Rotation = value; m_UpdateVertices = true; }
	}

	//! �� [0 - 15]��16��, ����˳��Ϊ��С����
	public int Layer
	{
		get { return m_Layer; }
		set
		{
			if ((value >= MinLayer) && (value <= MaxLayer))
			{
				m_Layer = value;
				m_UpdateVertices = true;
			}
		}
	}

	//! ����
	public Material Material
	{
		get { return m_Material; }
		set { m_Material = value; m_UpdateUV = true; }
	}

	//! ��ͼ����
	public Rect TextureRect
	{
		get { return m_TextureRect; }
		set { m_TextureRect = value; m_UpdateUV = true; }
	}

	//! ˮƽ��ת
	public bool FlipX
	{
		get { return m_FlipX; }
		set { m_FlipX = value; m_UpdateUV = true; }
	}

	//! ��ֱ��ת
	public bool FlipY
	{
		get { return m_FlipY; }
		set { m_FlipY = value; m_UpdateUV = true; }
	}

	//! ��ɫ
	public Color Color
	{
		get { return m_Color; }
		set { m_Color = value; }
	}

	//! ��������
	public Vector3 [] Vertices
	{
		get
		{
			if (m_UpdateVertices)
			{
				UpdateVertices();
			}

			return m_Vertices;
		}
	}

	//! UV����
	public Vector3 [] UV
	{
		get
		{
			if (m_UpdateUV)
			{
				UpdateUV();
			}

			return m_UV;
		}
	}

	//! ��������
	static public int [] Triangles
	{
		get
		{
			return m_Triangles;
		}
	}

	//! ���¶�������
	protected virtual void UpdateVertices()
	{
		// ����ת
		if (m_Rotation == 0)
		{
			Rect rect = new Rect((int)(m_Position.x - m_Size.x / 2), (int)(m_Position.y - m_Size.y / 2), m_Size.x, m_Size.y);

			m_Vertices[0] = new Vector3(rect.xMin, rect.yMax, 0);
			m_Vertices[1] = new Vector3(rect.xMax, rect.yMax, 0);
			m_Vertices[2] = new Vector3(rect.xMax, rect.yMin, 0);
			m_Vertices[3] = new Vector3(rect.xMin, rect.yMin, 0);
		}
		// ����ת
		else
		{
			float hx = m_Size.x / 2;
			float hy = m_Size.y / 2;

			float sin = Mathf.Sin(m_Rotation);
			float cos = Mathf.Cos(m_Rotation);

			m_Vertices[0] = new Vector3(m_Position.x + (-hx * cos - hy * sin), m_Position.y + (-hx * sin + hy * cos), 0);
			m_Vertices[1] = new Vector3(m_Position.x + (hx * cos - hy * sin), m_Position.y + (hx * sin + hy * cos), 0);
			m_Vertices[2] = new Vector3(m_Position.x + (hx * cos + hy * sin), m_Position.y + (hx * sin - hy * cos), 0);
			m_Vertices[3] = new Vector3(m_Position.x + (-hx * cos + hy * sin), m_Position.y + (-hx * sin -hy * cos), 0);
		}

		m_UpdateVertices = false;
	}

	//! ����UV����
	protected virtual void UpdateUV()
	{
		float factor_width = 1.0f / m_Material.mainTexture.width;
		float factor_height = 1.0f / m_Material.mainTexture.height;

		float u_min = m_TextureRect.xMin * factor_width;
		float u_max = m_TextureRect.xMax * factor_width;
		float v_min = 1.0f - m_TextureRect.yMax * factor_height;
		float v_max = 1.0f - m_TextureRect.yMin * factor_height;

		if ((m_FlipX == false) && (m_FlipY == false))
		{
			m_UV[0] = new Vector2(u_min, v_max);
			m_UV[1] = new Vector2(u_max, v_max);
			m_UV[2] = new Vector2(u_max, v_min);
			m_UV[3] = new Vector2(u_min, v_min);
		}
		else if ((m_FlipX == true) && (m_FlipY == false))
		{
			m_UV[0] = new Vector2(u_max, v_max);
			m_UV[1] = new Vector2(u_min, v_max);
			m_UV[2] = new Vector2(u_min, v_min);
			m_UV[3] = new Vector2(u_max, v_min);
		}
		else if ((m_FlipX == false) && (m_FlipY == true))
		{
			m_UV[0] = new Vector2(u_min, v_min);
			m_UV[1] = new Vector2(u_max, v_min);
			m_UV[2] = new Vector2(u_max, v_max);
			m_UV[3] = new Vector2(u_min, v_max);
		}
		else // ((m_FlipX == true) && (m_FlipY == true))
		{
			m_UV[0] = new Vector2(u_max, v_min);
			m_UV[1] = new Vector2(u_min, v_min);
			m_UV[2] = new Vector2(u_min, v_max);
			m_UV[3] = new Vector2(u_max, v_max);
		}

		m_UpdateUV = false;
	}
}

