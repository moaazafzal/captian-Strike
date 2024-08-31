//! @file UIControlImpl.cs


using UnityEngine;
using System.Collections;


//! @class UIControlVisible
//! @brief �ɼ��ؼ�
public class UIControlVisible : UIControl
{
	//! ����
	protected UISprite [] m_Sprite;

	//! ����
	public UIControlVisible()
	{
		m_Sprite = null;
	}

	//! ��������
	protected void CreateSprite(int number)
	{
		m_Sprite = new UISprite [number];
		for (int i = 0; i < number; i++)
		{
			m_Sprite[i] = new UISprite();
		}
	}

	//! ���þ�����ͼ
	protected void SetSpriteTexture(int index, Material material, Rect texture_rect, Vector2 size)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
		m_Sprite[index].Size = size;
	}

	//! ���þ�����ͼ
	protected void SetSpriteTexture(int index, Material material, Rect texture_rect)
	{
		m_Sprite[index].Material = material;
		m_Sprite[index].TextureRect = texture_rect;
        m_Sprite[index].Size = new Vector2(texture_rect.width, texture_rect.height);
	}

	//! ���þ���ߴ�
	protected void SetSpriteSize(int index, Vector2 size)
	{
		m_Sprite[index].Size = size;
	}

	//! ���þ�����ɫ
	protected void SetSpriteColor(int index, Color color)
	{
		m_Sprite[index].Color = color;
	}

	//! ���þ���λ��
	protected void SetSpritePosition(int index, Vector2 position)
	{
		m_Sprite[index].Position = position;
	}

	//! ���þ�����ת
	protected void SetSpriteRotation(int index, float rotation)
	{
		m_Sprite[index].Rotation = rotation;
	}

    protected float GetSpriteRotation(int index)
    {
        return m_Sprite[index].Rotation;
    }

	//! ���ü���
	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].SetClip(clip_rect);
			}
		}
	}

	//! ȡ������
	public new void ClearClip()
	{
		base.ClearClip();
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Sprite[i].ClearClip();
			}
		}
	}
}


//! @class UIButtonBase
//! @brief ��ť����
public class UIButtonBase : UIControlVisible
{
	//! ״̬
	public enum State
	{
		Normal,			//!< ����
		Pressed,		//!< ����
		Disabled,		//!< ����
	}

	//! ��ť״̬
	protected State m_State;

    protected UISprite m_HoverSprite = null;
    protected Vector2 m_HoverCenterDelta;

	//! ����
	public UIButtonBase()
	{
		CreateSprite(3);
		m_State = State.Normal;
	}

	//! ������ͼ
	public void SetTexture(State state, Material material, Rect texture_rect)
	{
		SetSpriteTexture((int)state, material, texture_rect);
	}

	//! ������ͼ(���ߴ�)
	public void SetTexture(State state, Material material, Rect texture_rect, Vector2 size)
	{
		SetSpriteTexture((int)state, material, texture_rect, size);
	}

	//! ������ͼ�ߴ�
	public void SetTextureSize(State state, Vector2 size)
	{
		SetSpriteSize((int)state, size);
	}

	//! ������ɫ
	public void SetColor(State state, Color color)
	{
		SetSpriteColor((int)state, color);
	}

    public void SetRotate(float rotate)
    {
        //SetSpriteRotation((int)m_State, rotate);
        SetSpriteRotation(0, rotate);
        SetSpriteRotation(1, rotate);
        SetSpriteRotation(2, rotate);
    }

    public float GetRotate()
    {
        return GetSpriteRotation((int)m_State);
    }

    //! Hover
    public void SetHoverSprite(Material material, Rect texture_rect)
    {
        SetHoverSprite(material, texture_rect, new Vector2(0, 0));
    }

    public void SetHoverSprite(Material material, Rect texture_rect, Vector2 center_deta)
    {
        m_HoverSprite = new UISprite();
        m_HoverSprite.Material = material;
        m_HoverSprite.TextureRect = texture_rect;
        m_HoverSprite.Size = new Vector2(texture_rect.width, texture_rect.height);
        m_HoverSprite.Position = new Vector2(Rect.x + Rect.width / 2, Rect.y + Rect.height / 2) + center_deta;
        m_HoverCenterDelta = center_deta;
    }

    public new void SetClip(Rect clip_rect)
    {
        base.SetClip(clip_rect);

        if (null!=m_HoverSprite)
        {
            m_HoverSprite.SetClip(clip_rect);
        }
    }

	//! ����
	public override void Draw()
	{
		if (!m_Enable)
		{
			m_Parent.DrawSprite(m_Sprite[(int)State.Disabled]);
		}
		else
		{
			switch (m_State)
			{
			case State.Normal:
				m_Parent.DrawSprite(m_Sprite[(int)State.Normal]);
				break;
			case State.Pressed:
				m_Parent.DrawSprite(m_Sprite[(int)State.Pressed]);
                if (null != m_HoverSprite)
                {
                    m_HoverSprite.Position = new Vector2(Rect.x + Rect.width / 2, Rect.y + Rect.height / 2) + m_HoverCenterDelta;
                    m_Parent.DrawSprite(m_HoverSprite);
                }
				break;
			}
		}
	}
}


//! @class UIClickButton
//! @brief �����ť
public class UIClickButton : UIButtonBase
{
	//! ����
	public enum Command
	{
		Click,			//!< ���
	}

	//! ��ǰ��ָid
	protected int m_FingerId;

	//! ����
	public UIClickButton()
	{
		m_FingerId = -1;
	}

	//! ����
	public void Reset()
	{
		m_State = State.Normal;
		m_FingerId = -1;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// ���þ���λ��
			Vector2 position = new Vector2(value.x + value.width / 2, value.y + value.height / 2);
			SetSpritePosition((int)State.Normal, position);
			SetSpritePosition((int)State.Pressed, position);
			SetSpritePosition((int)State.Disabled, position);
		}
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_State = State.Pressed;
				m_FingerId = touch.fingerId;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (touch.fingerId == m_FingerId)
			{
				if (touch.phase == TouchPhase.Moved)
				{
					if (PtInRect(touch.position))
					{
						m_State = State.Pressed;
					}
					else
					{
						m_State = State.Normal;
					}
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					m_State = State.Normal;
					m_FingerId = -1;

					if (PtInRect(touch.position))
					{
						m_Parent.SendEvent(this, (int)Command.Click, 0, 0);
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}


//! @class UIPushButton
//! @brief ��ѹ��ť
public class UIPushButton : UIButtonBase
{
	//! ����
	public enum Command
	{
		Down,			//!< ����
		Up,				//!< �ɿ�
	}

	//! ��ǰ��ָid
	protected int m_FingerId;

	//! ����
	public UIPushButton()
	{
		m_FingerId = -1;
	}

	//! ����
	public void Reset()
	{
		m_FingerId = -1;
	}

	//! ����״̬
	public void Set(bool down)
	{
		if (down)
		{
			m_State = State.Pressed;
		}
		else
		{
			m_State = State.Normal;
		}

		m_FingerId = -1;
	}
	
	public bool Get()
	{
		if(State.Pressed==m_State)
			return true;
		else return false;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{

			base.Rect = value;

			// ���þ���λ��
			Vector2 position = new Vector2(value.x + value.width / 2, value.y + value.height / 2);
			SetSpritePosition((int)State.Normal, position);
			SetSpritePosition((int)State.Pressed, position);
			SetSpritePosition((int)State.Disabled, position);
		}
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (touch.fingerId == m_FingerId)
			{
				if (touch.phase == TouchPhase.Ended)
				{
					m_FingerId = -1;

					if (PtInRect(touch.position))
					{
						if (m_State == State.Normal)
						{
							m_State = State.Pressed;
							m_Parent.SendEvent(this, (int)Command.Down, 0, 0);
						}
						else if (m_State == State.Pressed)
						{
							m_State = State.Normal;
							m_Parent.SendEvent(this, (int)Command.Up, 0, 0);
						}
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}


//! @class UISelectButton
//! @brief ѡ��ť
public class UISelectButton : UIButtonBase
{
	//! ����
	public enum Command
	{
		Select,			//!< ѡ��
		Unselect,		//!< ȡ��ѡ��
	}

	//! ��ǰ��ָid
	protected int m_FingerId;

	//! ����
	public UISelectButton()
	{
		m_FingerId = -1;
	}

	//! ����
	public void Reset()
	{
		m_FingerId = -1;
	}

	//! ����״̬
	public void Set(bool select)
	{
		if (select)
		{
			m_State = State.Pressed;
		}
		else
		{
			m_State = State.Normal;
		}

		m_FingerId = -1;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// ���þ���λ��
			Vector2 position = new Vector2(value.x + value.width / 2, value.y + value.height / 2);
			SetSpritePosition((int)State.Normal, position);
			SetSpritePosition((int)State.Pressed, position);
			SetSpritePosition((int)State.Disabled, position);
		}
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (touch.fingerId == m_FingerId)
			{
				if (touch.phase == TouchPhase.Ended)
				{
					m_FingerId = -1;

					if (PtInRect(touch.position))
					{
						if (m_State == State.Normal)
						{
							m_State = State.Pressed;
							m_Parent.SendEvent(this, (int)Command.Select, 0, 0);
						}
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}


//! @class UIWheelButton
//! @brief �����̰�ť
public class UIWheelButton : UIButtonBase
{
	//! ����
	public new enum Command
	{
		Down,			//!< ����
		Rotate,			//!< ��ת
		Up,				//!< �ɿ�
	}

	//! ��ǰ��ָid
	protected int m_FingerId;

	//! ���ĵ�
	protected Vector2 m_Center;

	//! ����(����)
	protected float m_Direction;

	//! ����
	public UIWheelButton()
	{
		m_FingerId = -1;
		m_Center = new Vector2(0, 0);
		m_Direction = 0;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// ���þ���λ��
			Vector2 position = new Vector2(value.x + value.width / 2, value.y + value.height / 2);
			SetSpritePosition((int)State.Normal, position);
			SetSpritePosition((int)State.Pressed, position);
			SetSpritePosition((int)State.Disabled, position);

			// �������ĵ�λ��
			m_Center.x = value.x + value.width / 2;
			m_Center.y = value.y + value.height / 2;
		}
	}

	//! ����(����)
	public float Direction
	{
		get { return m_Direction; }
		set
		{
			m_Direction = value;

			// ���þ�����ת
			SetSpriteRotation((int)State.Normal, m_Direction);
			SetSpriteRotation((int)State.Pressed, m_Direction);
			SetSpriteRotation((int)State.Disabled, m_Direction);
		}
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_State = State.Pressed;
				m_FingerId = touch.fingerId;

				m_Parent.SendEvent(this, (int)Command.Down, 0, 0);
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (touch.fingerId == m_FingerId)
			{
				if (touch.phase ==TouchPhase.Moved)
				{
					//
					float dx = touch.position.x - m_Center.x;
					float dy = touch.position.y - m_Center.y;

					if (dy >= 0)
					{
						Direction = Mathf.Atan2(dy, dx);
					}
					else
					{
						Direction = Mathf.Atan2(dy, dx) + 2 * Mathf.PI;
					}

					m_Parent.SendEvent(this, (int)Command.Rotate, m_Direction, 0);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					m_State = State.Normal;
					m_FingerId = -1;

					m_Parent.SendEvent(this, (int)Command.Up, 0, 0);
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}


//! @class UIJoystickButton
//! @brief ҡ�˰�ť
public class UIJoystickButton : UIButtonBase
{
	//! ����
	public new enum Command
	{
		Down,			//!< ����
		Move,			//!< �ƶ�
		Up,				//!< �ɿ�
	}

	//! ��ǰ��ָid
	protected int m_FingerId;

	//! ���ĵ�
	protected Vector2 m_Center;

	//! ����(����)
	protected float m_Direction;

	//! �ƶ�����
	protected float m_Distance;

	//! ��С�ƶ�����
	protected float m_MinDistance;
	//! ����ƶ�����
	protected float m_MaxDistance;

	//! ����
	public UIJoystickButton()
	{
		m_FingerId = -1;
		m_Center = new Vector2(0, 0);
		m_Direction = 0;
		m_Distance = 0;
		m_MinDistance = -1;
		m_MaxDistance = -1;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// �������ĵ�λ��
			m_Center.x = value.x + value.width / 2;
			m_Center.y = value.y + value.height / 2;

			// ���þ���λ��
			UpdatePosition();
		}
	}

	//! ����(����)
	public float Direction
	{
		get { return m_Direction; }
		set
		{
			m_Direction = value;

			// ���þ���λ��
			UpdatePosition();
		}
	}

	//! ����
	public float Distance
	{
		get { return m_Distance; }
		set
		{
			m_Distance = value;

			// ���þ���λ��
			UpdatePosition();
		}
	}

	//! ��С����
	public float MinDistance
	{
		get { return m_MinDistance; }
		set { m_MinDistance = value; }
	}

	//! ������
	public float MaxDistance
	{
		get { return m_MaxDistance; }
		set { m_MaxDistance = value; }
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_State = State.Pressed;
				m_FingerId = touch.fingerId;

				m_Parent.SendEvent(this, (int)Command.Down, 0, 0);
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (touch.fingerId == m_FingerId)
			{
				if (touch.phase == TouchPhase.Moved)
				{
					//
					float dx = touch.position.x - m_Center.x;
					float dy = touch.position.y - m_Center.y;

					m_Distance = Mathf.Sqrt(dx * dx + dy * dy);
					if ((m_MinDistance >= 0) && (m_Distance < m_MinDistance))
					{
						m_Distance = m_MinDistance;
					}
					if ((m_MaxDistance >= 0) && (m_Distance > m_MaxDistance))
					{
						m_Distance = m_MaxDistance;
					}

					if (dy >= 0)
					{
						m_Direction = Mathf.Atan2(dy, dx);
					}
					else
					{
						m_Direction = Mathf.Atan2(dy, dx) + 2 * Mathf.PI;
					}

					UpdatePosition();

					m_Parent.SendEvent(this, (int)Command.Move, m_Direction, m_Distance);
				}
				else if (touch.phase ==TouchPhase.Ended)
				{
					m_State = State.Normal;
					m_FingerId = -1;

					m_Distance = 0;
					UpdatePosition();

					m_Parent.SendEvent(this, (int)Command.Up, 0, 0);
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}

	//! ���¾���λ��
	private void UpdatePosition()
	{
		if (m_Distance == 0)
		{
			SetSpritePosition((int)State.Normal, m_Center);
			SetSpritePosition((int)State.Pressed, m_Center);
			SetSpritePosition((int)State.Disabled, m_Center);
		}
		else
		{
			Vector2 position = new Vector2(m_Center.x + m_Distance * Mathf.Cos(m_Direction), m_Center.y + m_Distance * Mathf.Sin(m_Direction));
			SetSpritePosition((int)State.Normal, position);
			SetSpritePosition((int)State.Pressed, position);
			SetSpritePosition((int)State.Disabled, position);
		}
	}
}


//! @class UIImage
//! @brief ͼƬ
public class UIImage : UIControlVisible
{

    protected int m_TouchFingerId = -1;
    //! ����
    public enum Command
    {
        Click,			//!< ���
    }

	//! ����
	public UIImage()
	{
		CreateSprite(1);
	}

	//! ������ͼ
	public void SetTexture(Material material, Rect texture_rect, Vector2 size)
	{
		SetSpriteTexture(0, material, texture_rect, size);
	}

	//! ������ͼ
	public void SetTexture(Material material, Rect texture_rect)
	{
		SetSpriteTexture(0, material, texture_rect);
	}

	//! ������ͼ�ߴ�
	public void SetTextureSize(Vector2 size)
	{
		SetSpriteSize(0, size);
	}

	//! ������ת
	public void SetRotation(float rotation)
	{
		SetSpriteRotation(0, rotation);
	}

    public float GetRotation()
    {
        return GetSpriteRotation(0);
    }

	//! ������ɫ
	public void SetColor(Color color)
	{
		SetSpriteColor(0, color);
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// ���þ���λ��
			Vector2 position = new Vector2(value.x + value.width / 2, value.y + value.height / 2);
			SetSpritePosition(0, position);
		}
	}

	//! ����
	public override void Draw()
	{
		m_Parent.DrawSprite(m_Sprite[0]);
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
        if (touch.phase == TouchPhase.Began)
        {
            m_TouchFingerId = touch.fingerId;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            
            if (touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
            {
                m_Parent.SendEvent(this, (int)Command.Click, 0, 0);
                m_TouchFingerId = -1;
                return true;
            }
            
        }
        return false;
	}
}


//! @class UIBlock
//! @brief UI��
public class UIBlock : UIControl
{
	//! ����
	public UIBlock()
	{
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (PtInRect(touch.position))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}


//! @class UIMove
//! @brief UI�ƶ�
public class UIMove : UIControl
{
	//! ����
	public new enum Command
	{
		Begin,			//!< �ƶ���ʼ
		Move,			//!< �ƶ�
		End,
        MovePos,        //!< �ƶ�����
	}

	//! ��ǰ��ָid
	protected int m_FingerId;
	//! ������
	protected Vector2 m_TouchPosition;
	//! �Ƿ��ƶ�
	protected bool m_Move;

	//! ��С�ƶ�X
	protected float m_MinX;
	//! ��С�ƶ�Y
	protected float m_MinY;

	//! ����
	public UIMove()
	{
		m_FingerId = -1;
		m_TouchPosition = new Vector2(0, 0);
		m_Move = false;
	}

	//! ��С�ƶ�X
	public float MinX
	{
		get { return m_MinX; }
		set { m_MinX = value; }
	}

	//! ��С�ƶ�Y
	public float MinY
	{
		get { return m_MinY; }
		set { m_MinY = value; }
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
        /*
        if (!PtInRect(touch.position) && touch.phase == TouchPhase.Ended)
        {
            m_Parent.SendEvent(this, (int)Command.End, 0, 0);
            return false;
        }
        */



		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				m_TouchPosition = touch.position;
				m_Move = false;
                m_Parent.SendEvent(this, (int)Command.Begin, touch.position.x, touch.position.y);
                
			}

			return false;
		}
		else
		{
			if (touch.fingerId != m_FingerId)
			{
				return false;
			}
            /*
			if (!PtInRect(touch.position))
			{
				return false;
			}
            */


			if (touch.phase == TouchPhase.Moved)
			{
               
				float dx = touch.position.x - m_TouchPosition.x;
				float dy = touch.position.y - m_TouchPosition.y;

				if (m_Move)
				{
					m_TouchPosition = touch.position;
					m_Parent.SendEvent(this, (int)Command.Move, dx, dy);
                    m_Parent.SendEvent(this, (int)Command.MovePos, touch.position.x, touch.position.y);
				}
				else
				{
					float abs_dx = (dx >= 0) ? dx : -dx;
					float abs_dy = (dy >= 0) ? dy : -dy;

					if ((abs_dx > m_MinX) || (abs_dy > m_MinY))
					{
						m_TouchPosition = touch.position;
						m_Move = true;
						m_Parent.SendEvent(this, (int)Command.Move, dx, dy);
					}
					else
					{
                      

					}
				}

                

				return true;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				bool move = m_Move;

				m_FingerId = -1;
				m_TouchPosition = new Vector2(0, 0);
				m_Move = false;

				//if (move)
				{
					m_Parent.SendEvent(this, (int)Command.End, touch.position.x, touch.position.y);
					return false;
				}
				//else
				{
				//	return false;
				}
			}

			return false;
		}
	}
}


//! @class UIZoom
//! @brief UI����
public class UIZoom : UIControl
{
	//! ����
	public new enum Command
	{
		Begin,			//!< ���ſ�ʼ
		Zoom,			//!< ����
		End,			//!< ���Ž���
	}

	//! TouchInfo
	protected struct TouchInfo
	{
		//! ��ָid
		public int FingerId;
		//! ������
		public Vector2 TouchPosition;
	}
	//! TouchInfo
	protected TouchInfo [] m_TouchInfo;
	//! ��ǰ��ָid
	protected int m_FingerIndex;

	//! ����
	protected float m_Distance;

	//! �Ƿ�����
	protected bool m_Zoom;


	//! ����
	public UIZoom()
	{
		m_TouchInfo = new TouchInfo[2];
		m_TouchInfo[0].FingerId = -1;
		m_TouchInfo[1].FingerId = -1;
		m_FingerIndex = 0;

		m_Distance = 0;

		m_Zoom = false;
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_TouchInfo[m_FingerIndex].FingerId = touch.fingerId;
				m_TouchInfo[m_FingerIndex].TouchPosition = touch.position;
				if (m_FingerIndex == 0)
				{
					m_FingerIndex = 1;
				}
				else
				{
					m_FingerIndex = 0;
				}

				if ((m_TouchInfo[0].FingerId != -1) && (m_TouchInfo[1].FingerId != -1))
				{
					Vector2 d = m_TouchInfo[0].TouchPosition - m_TouchInfo[1].TouchPosition;
					m_Distance = d.magnitude;
					m_Zoom = true;

					m_Parent.SendEvent(this, (int)Command.Begin, 0, 0);
				}
			}

			return false;
		}
		else if (touch.phase == TouchPhase.Moved)
		{
			if ((m_TouchInfo[0].FingerId == -1) || (m_TouchInfo[1].FingerId == -1))
			{
				return false;
			}

			if (!PtInRect(touch.position))
			{
				return false;
			}

			if (m_TouchInfo[0].FingerId == touch.fingerId)
			{
				m_TouchInfo[0].TouchPosition = touch.position;
			}
			else if (m_TouchInfo[1].FingerId == touch.fingerId)
			{
				m_TouchInfo[1].TouchPosition = touch.position;
			}
			else
			{
				return false;
			}

			//
			Vector2 d = m_TouchInfo[0].TouchPosition - m_TouchInfo[1].TouchPosition;
			float distance = d.magnitude;
			float delta = distance - m_Distance;
			m_Distance = distance;

			m_Parent.SendEvent(this, (int)Command.Zoom, delta, 0);

			return true;
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			bool rc = false;

			for (int i = 0; i < 2; i++)
			{
				if (m_TouchInfo[i].FingerId == touch.fingerId)
				{
					m_TouchInfo[i].FingerId = -1;
					m_FingerIndex = i;

					if (m_Zoom)
					{
						m_Zoom = false;
						m_Parent.SendEvent(this, (int)Command.End, 0, 0);
						rc = true;
					}
				}
			}

			return rc;
		}
		else
		{
			return false;
		}
	}
}


//! @class UIRotate
//! @brief UI��ת
public class UIRotate : UIControl
{
	//! ����
	public new enum Command
	{
		Begin,			//!< ��ת��ʼ
		Rotate,			//!< ��ת
		End,			//!< ��ת����
	}

	//! ��ǰ��ָid
	protected int m_FingerId;
	//! ��������
	protected float m_TouchDirection;

	//! ���ĵ�
	protected Vector2 m_Center;

	//! �Ƿ���ת
	protected bool m_Rotate;

	//! ��С��ת����
	protected float m_MinRotate;

	//! ����
	public UIRotate()
	{
		m_FingerId = -1;
		m_TouchDirection = 0;
		m_Center = new Vector2(0, 0);
		m_Rotate = false;
		m_MinRotate = 0;
	}

	//! �ؼ�����
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			// �������ĵ�λ��
			m_Center.x = value.x + value.width / 2;
			m_Center.y = value.y + value.height / 2;
		}
	}

	//! ��С��ת�Ƕ�
	public float MinRotate
	{
		get { return m_MinRotate; }
		set { m_MinRotate = value; }
	}

	//! ��������
    public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;

				//
				float dx = touch.position.x - m_Center.x;
				float dy = touch.position.y - m_Center.y;

				if (dy >= 0)
				{
					m_TouchDirection = Mathf.Atan2(dy, dx);
				}
				else
				{
					m_TouchDirection = Mathf.Atan2(dy, dx) + 2 * Mathf.PI;
				}

				m_Rotate = false;
			}

			return false;
		}
		else
		{
			if (touch.fingerId != m_FingerId)
			{
				return false;
			}

			if (!PtInRect(touch.position))
			{
				return false;
			}

			if (touch.phase == TouchPhase.Moved)
			{
				//
				float dx = touch.position.x - m_Center.x;
				float dy = touch.position.y - m_Center.y;

				float direction;
				if (dy >= 0)
				{
					direction = Mathf.Atan2(dy, dx);
				}
				else
				{
					direction = Mathf.Atan2(dy, dx) + 2 * Mathf.PI;
				}

				float delta_direction = direction - m_TouchDirection;
				if (delta_direction < 0)
				{
					delta_direction += 2 * Mathf.PI;
				}
				if (delta_direction > Mathf.PI)
				{
					delta_direction = delta_direction - 2 * Mathf.PI;
				}

				if (m_Rotate)
				{
					m_TouchDirection = direction;
					m_Parent.SendEvent(this, (int)Command.Rotate, delta_direction, 0);
				}
				else
				{
					float abs_delta_direction = (delta_direction >= 0) ? delta_direction : -delta_direction;

					if (abs_delta_direction > m_MinRotate)
					{
						m_TouchDirection = direction;
						m_Rotate = true;

						m_Parent.SendEvent(this, (int)Command.Begin, 0, 0);
						m_Parent.SendEvent(this, (int)Command.Rotate, delta_direction, 0);
					}
					else
					{
						return false;
					}
				}

				return true;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				bool rotate = m_Rotate;

				m_FingerId = -1;
				m_TouchDirection = 0;
				m_Rotate = false;

				if (rotate)
				{
					m_Parent.SendEvent(this, (int)Command.End, 0, 0);
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}
	}
}

