﻿
using UnityEngine;
using System.Collections;



public class UI3DFrame : UIPanel, UIHandler
{
    protected GameObject m_Model;
    protected UIMove m_UIMove;
    protected Vector3 m_Pos;
    public UI3DFrame(Rect rect, Vector3 pos)
    {
        m_UIMove = new UIMove();
        m_UIMove.Rect = rect;
        m_Pos = pos;
        this.Add(m_UIMove);
        this.SetUIHandler(this);
    }

    public void SetModel(GameObject obj)
    {
        m_Model = obj;
        m_Model.transform.position = m_Pos;
    }

    public GameObject GetModel()
    {
        return m_Model;
    }
    public void ClearModels()
    {
        if (m_Model != null)
        {
            GameObject.Destroy(m_Model);
            m_Model = null;
        }

    }

    public override void Show()
    {
        base.Show();
        m_UIMove.Enable = true;
        m_Model.SetActiveRecursively(true);

    }

    public override void Hide()
    {
        base.Hide();
        m_Model.SetActiveRecursively(false);

    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        if (control == m_UIMove)
        {
            if (command == (int)(UIMove.Command.Move))
            {
                if (m_Model != null)
                {
                    m_Model.transform.Rotate(new Vector3(0, -wparam, 0), Space.Self);
                }
            }

        }
    }

}