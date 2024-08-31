//! @file UIContainer.cs


using UnityEngine;
using System.Collections;


//! @class UIContainer
//! @brief UI�����ӿ�
public interface UIContainer
{
	//! �����ӿؼ�����
	void DrawSprite(UISprite sprite);

	//! �����ӿؼ��¼�
	void SendEvent(UIControl control, int command, float wparam, float lparam);


    void Add(UIControl control);
}

