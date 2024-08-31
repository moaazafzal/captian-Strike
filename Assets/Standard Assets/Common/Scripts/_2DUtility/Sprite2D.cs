//! @file Sprite2D.cs


using UnityEngine;
using System.Collections;


//! @class Sprite2D
//! @brief 2D����
public class Sprite2D : Sprite
{
	//! λ��
	private Vector2 m_LocalPosition;

	//! ��ת
	private float m_LocalRotation;

	//! �ٶ�
	private Vector2 m_Velocity;

	//! ���ٶ�
	private float m_AngularVelocity;

	//! ������
	private Sprite2D m_Parent;

	//! ���游����λ��
	private bool m_MountPosition;

	//! ���游������ת
	private bool m_MountRotation;


	//! ��ײ����
	public enum CollideType
	{
		Null,
		Circle,
		Rectangle,
		Polygon
	};

	//! ��ײ����(��������ϵ)
	private CollideType m_LocalCollideType;

	//! ��ײ��(��������ϵ)
	private float [] m_LocalCollidePointers;

	//! ��ײ����(ȫ������ϵ)
	private CollideType m_GlobalCollideType;

	//! ��ײ��(ȫ������ϵ)
	private float [] m_GlobalCollidePointers;

	//! ������ײ��Ϣ
	private bool m_UpdateCollideInfo;


	//! ����
	public Sprite2D()
	{
		m_LocalPosition = Vector2.zero;
		m_LocalRotation = 0;
		m_Velocity = Vector2.zero;
		m_AngularVelocity = 0;
		m_Parent = null;
		m_MountPosition = false;
		m_MountRotation = false;

		m_LocalCollideType = CollideType.Null;
		m_LocalCollidePointers = null;
		m_GlobalCollideType = CollideType.Null;
		m_GlobalCollidePointers = null;
		m_UpdateCollideInfo = true;
	}

	//! ����
	~Sprite2D()
	{
	}

	//! �ߴ�
	public new Vector2 Size
	{
		get { return base.Size; }
		set { base.Size = value; Update(0); }
	}

	//! λ��
	public new Vector2 Position
	{
		get { return m_LocalPosition; }
		set { m_LocalPosition = value; Update(0); }
	}

	//! ����λ��
	public new Vector2 WorldPosition
	{
		get
		{
			if ((m_Parent != null) && m_MountPosition)
			{
				return m_Parent.WorldPosition + m_LocalPosition;
			}
			else
			{
				return m_LocalPosition;
			}
		}
	}

	//! ��ת
	public new float Rotation
	{
		get { return m_LocalRotation; }
		set { m_LocalRotation = value; Update(0); }
	}

	//! ������ת
	public new float WorldRotation
	{
		get
		{
			if ((m_Parent != null) && m_MountRotation)
			{
				return m_Parent.WorldRotation + m_LocalRotation;
			}
			else
			{
				return m_LocalRotation;
			}
		}
	}

	//! �ٶ�
	public Vector2 Velocity
	{
		get { return m_Velocity; }
		set { m_Velocity = value; }
	}

	//! ���ٶ�
	public float AngularVelocity
	{
		get { return m_AngularVelocity; }
		set { m_AngularVelocity = value; }
	}

	//! ������
	public Sprite2D Parent
	{
		get { return m_Parent; }
		set { m_Parent = value; Update(0); }
	}

	//! ���游��λ��
	public bool MountPosition
	{
		get { return m_MountPosition; }
		set { m_MountPosition = value; Update(0); }
	}

	//! ���游����ת
	public bool MountRotation
	{
		get { return m_MountRotation; }
		set { m_MountRotation = value; Update(0); }
	}

	//! ��ײ����(��������ϵ)
	public CollideType LocalCollideType
	{
		get { return m_LocalCollideType; }
		set { m_LocalCollideType = value; m_UpdateCollideInfo = true; }
	}

	//! ��ײ��(��������ϵ)
	public float [] LocalCollidePointers
	{
		get { return m_LocalCollidePointers; }
		set { m_LocalCollidePointers = value; m_UpdateCollideInfo = true; }
	}

	//! ��ײ����(ȫ������ϵ)
	public CollideType GlobalCollideType
	{
		get
		{
			if (m_UpdateCollideInfo)
			{
				CalculateCollideInfo();
			}

			return m_GlobalCollideType;
		}
		set { m_GlobalCollideType = value; }
	}

	//! ��ײ��(ȫ������ϵ)
	public float [] GlobalCollidePointers
	{
		get
		{
			if (m_UpdateCollideInfo)
			{
				CalculateCollideInfo();
			}

			return m_GlobalCollidePointers;
		}
		set { m_GlobalCollidePointers = value; }
	}

	//! ��ѡ
	public bool Pick(Vector2 position)
	{
		float sprite_rotation = Rotation;

		if (sprite_rotation == 0)
		{
			return PickNoRotation(position);
		}
		else
		{
			Vector2 sprite_position = WorldPosition;
			Vector2 half_sprite_size = Size / 2;

			float estimate_size = (half_sprite_size.x >= half_sprite_size.y ? half_sprite_size.x : half_sprite_size.y) * 1.5f;
			if ((position.x < (sprite_position.x - estimate_size))
				||(position.x > (sprite_position.x + estimate_size))
				||(position.y < (sprite_position.y - estimate_size))
				||(position.y > (sprite_position.y + estimate_size)))
			{
				return false;
			}

			// ������ת��,���ж��Ƿ�ѡ��
			float sin = Mathf.Sin(-sprite_rotation);
			float cos = Mathf.Cos(-sprite_rotation);

			Vector2 delta = position - sprite_position;
			Vector2 delta_rotation = new Vector2(delta.x * cos - delta.y * sin, delta.x * sin + delta.y * cos);
			Vector2 position_rotation = new Vector2(sprite_position.x + delta_rotation.x, sprite_position.y + delta_rotation.y);

			if ((position_rotation.x < (sprite_position.x - half_sprite_size.x))
				||(position_rotation.x > (sprite_position.x + half_sprite_size.x))
				||(position_rotation.y < (sprite_position.y - half_sprite_size.y))
				||(position_rotation.y > (sprite_position.y + half_sprite_size.y)))
			{
				return false;
			}

			return true;
		}
	}

	//! ��ѡ(������ת)
	public bool PickNoRotation(Vector2 position)
	{
		Vector2 sprite_position = WorldPosition;
		Vector2 half_sprite_size = Size / 2;

		if ((position.x < (sprite_position.x - half_sprite_size.x))
			||(position.x > (sprite_position.x + half_sprite_size.x))
			||(position.y < (sprite_position.y - half_sprite_size.y))
			||(position.y > (sprite_position.y + half_sprite_size.y)))
		{
			return false;
		}

		return true;
	}

	//! ����
	public virtual void Update(float delta_time)
	{
		m_LocalPosition += m_Velocity * delta_time;
		m_LocalRotation += m_AngularVelocity * delta_time;

		base.Position = WorldPosition;
		base.Rotation = WorldRotation;

		m_UpdateCollideInfo = true;
	}

	//! ������ײ��Ϣ
	private void CalculateCollideInfo()
	{
		//
		if (m_LocalCollideType == CollideType.Null)
		{
			m_GlobalCollideType = CollideType.Null;
			m_GlobalCollidePointers = null;
			m_UpdateCollideInfo = false;
			return;
		}

		//
		Vector2 size = Size;
		Vector2 half_size = size / 2;

		Vector3 position = WorldPosition;
		float rotation = WorldRotation;

		//
		float sin = 0;
		float cos = 1;
		if (rotation == 0)
		{
			sin = Mathf.Sin(rotation);
			cos = Mathf.Cos(rotation);
		}

		//
		switch (m_LocalCollideType)
		{
		case CollideType.Circle:
			{
				m_GlobalCollideType = CollideType.Circle;

				if (rotation == 0)
				{
					m_GlobalCollidePointers = new float [3];
					m_GlobalCollidePointers[0] = position.x + m_LocalCollidePointers[0] * half_size.x;
					m_GlobalCollidePointers[1] = position.y + m_LocalCollidePointers[1] * half_size.y;
					
				}
				else
				{
					float x = m_LocalCollidePointers[0] * cos - m_LocalCollidePointers[1] * sin;
					float y = m_LocalCollidePointers[0] * sin + m_LocalCollidePointers[1] * cos;

					m_GlobalCollidePointers = new float [3];
					m_GlobalCollidePointers[0] = position.x + x * half_size.x;
					m_GlobalCollidePointers[1] = position.y + y * half_size.y;
				}

				m_GlobalCollidePointers[2] = m_LocalCollidePointers[2] * (half_size.x <= half_size.y ? half_size.x : half_size.y);
			}
			break;

		case CollideType.Rectangle:
			{
				m_GlobalCollideType = CollideType.Rectangle;

				if (rotation == 0)
				{
					m_GlobalCollidePointers = new float [4];
					m_GlobalCollidePointers[0] = position.x + m_LocalCollidePointers[0] * half_size.x;
					m_GlobalCollidePointers[1] = position.y + m_LocalCollidePointers[1] * half_size.y;
					m_GlobalCollidePointers[2] = position.x + m_LocalCollidePointers[2] * half_size.x;
					m_GlobalCollidePointers[3] = position.y + m_LocalCollidePointers[3] * half_size.y;
				}
				else
				{
					float [] full_pointers = new float [8];
					full_pointers[0] = m_LocalCollidePointers[0];
					full_pointers[1] = m_LocalCollidePointers[1];
					full_pointers[2] = m_LocalCollidePointers[2];
					full_pointers[3] = m_LocalCollidePointers[1];
					full_pointers[4] = m_LocalCollidePointers[2];
					full_pointers[5] = m_LocalCollidePointers[3];
					full_pointers[6] = m_LocalCollidePointers[0];
					full_pointers[7] = m_LocalCollidePointers[3];

					for (int i = 0; i < 4; i++)
					{
						float x = full_pointers[i * 2] * cos - full_pointers[i * 2 + 1] * sin;
						float y = full_pointers[i * 2] * sin + full_pointers[i * 2 + 1] * cos;

						m_GlobalCollidePointers[i * 2] = position.x + x * half_size.x;
						m_GlobalCollidePointers[i * 2 + 1] = position.y + y * half_size.y;
					}
				}
			}
			break;

		case CollideType.Polygon:
			{
				m_GlobalCollideType = CollideType.Polygon;

				if (rotation == 0)
				{
					int pointer_count = m_LocalCollidePointers.Length / 2;
					m_GlobalCollidePointers = new float [pointer_count * 2];
					for (int i = 0; i < pointer_count; i++)
					{
						m_GlobalCollidePointers[i * 2] = position.x + m_LocalCollidePointers[i * 2] * half_size.x;
						m_GlobalCollidePointers[i * 2 + 1] = position.y + m_LocalCollidePointers[i * 2 + 1] * half_size.y;
					}
				}
				else
				{
					int pointer_count = m_LocalCollidePointers.Length / 2;
					m_GlobalCollidePointers = new float [pointer_count * 2];
					for (int i = 0; i < pointer_count; i++)
					{
						float x = (m_LocalCollidePointers[i * 2] * cos - m_LocalCollidePointers[i * 2 + 1] * sin);
						float y = (m_LocalCollidePointers[i * 2] * sin + m_LocalCollidePointers[i * 2 + 1] * cos);

						m_GlobalCollidePointers[i * 2] = position.x + x * half_size.x;
						m_GlobalCollidePointers[i * 2 + 1] = position.y + y * half_size.y;
					}
				}
			}
			break;
		}

		m_UpdateCollideInfo = false;
	}
}


//! @class Sprite2DStatic
//! @brief 2D����(��̬)
public class Sprite2DStatic : Sprite2D
{
	//! ֡
	public struct Frame
	{
		//! ����
		public Material Material;
		//! ��ͼ����
		public Rect TextureRect;
	}


	//! ͼ��֡
	public Frame ImageFrame
	{
		set
		{
			base.Material = value.Material;
			base.TextureRect = value.TextureRect;
		}
	}
}


//! @class Sprite2DAnimated
//! @brief 2D����(����)
public class Sprite2DAnimated : Sprite2DStatic
{
	//! ����
	public class Animation
	{
		//! ͼ��֡����
		public Frame [] Frames;
		//! ֡��
		public int FrameRate;
		//! �Ƿ�ѭ��
		public bool Loop;
	}


	//! ����
	private Hashtable m_Animations = new Hashtable();

	//! ��ǰ����
	private Animation m_CurrentAnimation = null;

	//! ��ǰ����ʱ��
	private float m_AnimationTime = 0;


	//! ����
	public override void Update(float delta_time)
	{
		//
		base.Update(delta_time);

		//
		UpdateAnimation(delta_time);
	}

	//! ��Ӷ���
	public void AddAnimation(string name, Animation animation)
	{
		m_Animations[name] = animation;
	}

	//! ���Ŷ���
	public void PlayAnimation(string name)
	{
		if (m_Animations.Contains(name))
		{
			m_CurrentAnimation = (Animation)m_Animations[name];
			m_AnimationTime = 0;

			UpdateAnimation(0);
		}
	}

	//! ֹͣ����
	public void StopAnimation()
	{
		m_CurrentAnimation = null;
		m_AnimationTime = 0;
	}

	//! ���¶���
	private void UpdateAnimation(float delta_time)
	{
		//
		if (m_CurrentAnimation == null)
		{
			return;
		}

		//
		m_AnimationTime += delta_time;

		int frame_number = (int)(m_AnimationTime * m_CurrentAnimation.FrameRate);
		if (frame_number >= m_CurrentAnimation.Frames.Length)
		{
			if (m_CurrentAnimation.Loop)
			{
				frame_number %= m_CurrentAnimation.Frames.Length;
			}
			else
			{
				frame_number = m_CurrentAnimation.Frames.Length - 1;
			}
		}

		base.ImageFrame = m_CurrentAnimation.Frames[frame_number];
	}
}

