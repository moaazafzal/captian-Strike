//! @file UIMesh.cs


using UnityEngine;
using System.Collections;


//! @class UIMesh
//! @brief UI����
public class UIMesh : MonoBehaviour
{
	//! ʹ�õĲ�
	private int m_Layer = 0;

	//! MeshFilter
	private MeshFilter m_MeshFilter = null;

	//! MeshRenderer
	private MeshRenderer m_MeshRenderer = null;

	//! ��������
	private ArrayList m_Sprites = null;

	//! ���Ʒ���
	private ArrayList m_DrawGroups = null;

	//! ��mesh����
	private int m_SubMeshCount = -1;

	//! ��ʼ��
	public void Initialize(int layer)
	{
		//
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		//
		m_Layer = layer;
		gameObject.layer = m_Layer;

		//
		m_MeshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));

		//
		m_MeshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
		m_MeshRenderer.castShadows = false;
		m_MeshRenderer.receiveShadows = false;

		//
		m_Sprites = new ArrayList();
		m_DrawGroups = new ArrayList();
	}

	public void DoLateUpdate()
	{
		//
		m_MeshFilter.mesh.Clear();

		//
		if (m_Sprites.Count <= 0)
		{
			return;
		}

		int total_sprite_count = 0;

		// ���ղ��ʺϲ�����
		m_DrawGroups.Clear();

		ArrayList group = null;
		for (int i = 0; i < m_Sprites.Count; ++i)
		{
			Sprite sprite = (Sprite)m_Sprites[i];
			if (sprite.Material == null)
			{
				continue;
			}

			total_sprite_count++;

			if (group == null)
			{
				group = new ArrayList();
				m_DrawGroups.Add(group);

				group.Add(sprite);
			}
			else
			{
				if (((Sprite)group[0]).Material != sprite.Material)
				{
					group = new ArrayList();
					m_DrawGroups.Add(group);

					group.Add(sprite);
				}
				else
				{
					group.Add(sprite);
				}
			}	
		}

		int sub_mesh_count = m_DrawGroups.Count;

		// �ϲ�������
		Vector3 [] vertices = new Vector3[total_sprite_count * 4];
		Vector2 [] uv = new Vector2[total_sprite_count * 4];
		Color [] colors = new Color[total_sprite_count * 4];
		int vertex_index = 0;

		int [][] triangles = new int[sub_mesh_count][];
		Material [] materials = new Material [sub_mesh_count];
		int sub_mesh_index = 0;

		// ����ÿ����
		for (int i = 0; i < m_DrawGroups.Count; ++i)
		{
			ArrayList sprites = (ArrayList)m_DrawGroups[i];
			Material material = ((Sprite)sprites[0]).Material;

			//
			triangles[sub_mesh_index] = new int[sprites.Count * 6];

			//
			for (int j = 0; j < sprites.Count; ++j)
			{
				Sprite sprite = (Sprite)sprites[j];

				vertices[vertex_index + 0] = sprite.Vertices[0];
				vertices[vertex_index + 1] = sprite.Vertices[1];
				vertices[vertex_index + 2] = sprite.Vertices[2];
				vertices[vertex_index + 3] = sprite.Vertices[3];

				uv[vertex_index + 0] = sprite.UV[0];
				uv[vertex_index + 1] = sprite.UV[1];
				uv[vertex_index + 2] = sprite.UV[2];
				uv[vertex_index + 3] = sprite.UV[3];

				colors[vertex_index + 0] = sprite.Color;
				colors[vertex_index + 1] = sprite.Color;
				colors[vertex_index + 2] = sprite.Color;
				colors[vertex_index + 3] = sprite.Color;

				int index = j * 6;

				triangles[sub_mesh_index][index + 0] = vertex_index + Sprite.Triangles[0];
				triangles[sub_mesh_index][index + 1] = vertex_index + Sprite.Triangles[1];
				triangles[sub_mesh_index][index + 2] = vertex_index + Sprite.Triangles[2];
				triangles[sub_mesh_index][index + 3] = vertex_index + Sprite.Triangles[3];
				triangles[sub_mesh_index][index + 4] = vertex_index + Sprite.Triangles[4];
				triangles[sub_mesh_index][index + 5] = vertex_index + Sprite.Triangles[5];

				vertex_index += 4;
			}

			//
			materials[sub_mesh_index] = material;
			sub_mesh_index++;
		}

		// NOTICE
		if (m_SubMeshCount != sub_mesh_count)
		{
			m_SubMeshCount = sub_mesh_count;

			DestroyImmediate(m_MeshRenderer);
			m_MeshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		}

		// ��������
		m_MeshFilter.mesh.subMeshCount = sub_mesh_count;
		m_MeshFilter.mesh.vertices = vertices;
		m_MeshFilter.mesh.uv = uv;
		m_MeshFilter.mesh.colors = colors;
		for (int i = 0; i < sub_mesh_count; i++)
		{
			m_MeshFilter.mesh.SetTriangles(triangles[i], i);
		}

		// ���ò���
		m_MeshRenderer.materials = materials;
	}

	//! ��Ӿ���
	public void Add(Sprite sprite)
	{
		m_Sprites.Add(sprite);
	}

	//! �Ƴ�����
	public void Remove(Sprite sprite)
	{
		m_Sprites.Remove(sprite);
	}

	//! �Ƴ�ȫ������
	public void RemoveAll()
	{
		m_Sprites.Clear();
	}
}

