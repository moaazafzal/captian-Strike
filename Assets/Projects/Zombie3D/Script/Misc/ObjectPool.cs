using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    protected List<GameObject> objects;
    protected List<Transform> transforms;
    protected List<float> createdTime;
    protected float life;
    protected bool hasAnimator = false;
    protected bool hasParticleSystem = false;
    protected GameObject folderObject;

    public void Init(string poolName, GameObject prefab, int initNum, float life)
    {
        objects = new List<GameObject>();
        transforms = new List<Transform>();
        createdTime = new List<float>();
        this.life = life;

        folderObject = new GameObject(poolName);

        for (int i = 0; i < initNum; i++)
        {
            GameObject obj = Object.Instantiate(prefab) as GameObject;
            objects.Add(obj);
            transforms.Add(obj.transform);
            createdTime.Add(0f);

            obj.SetActive(false);
            obj.transform.parent = folderObject.transform;

            if (obj.GetComponent<Animator>() != null)
            {
                hasAnimator = true;
            }
            if (obj.GetComponent<ParticleSystem>() != null)
            {
                hasParticleSystem = true;
            }
        }
    }

    public GameObject CreateObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].activeSelf)
            {
                objects[i].SetActive(true);
                transforms[i].position = position;
                objects[i].transform.rotation = rotation;

                if (hasAnimator)
                {
                    Animator animator = objects[i].GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Play("DefaultAnimation"); // Ensure the animation name is correct or use other methods to play animations.
                    }
                }

                if (hasParticleSystem)
                {
                    ParticleSystem ps = objects[i].GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }

                createdTime[i] = Time.time;
                return objects[i];
            }
        }

        GameObject obj = Object.Instantiate(objects[0]) as GameObject;
        objects.Add(obj);
        transforms.Add(obj.transform);
        createdTime.Add(0f);
        obj.name = objects[0].name;
        obj.transform.parent = folderObject.transform;

        if (obj.GetComponent<Animator>() != null)
        {
            hasAnimator = true;
        }
        if (obj.GetComponent<ParticleSystem>() != null)
        {
            hasParticleSystem = true;
        }
        obj.SetActive(true);

        return obj;
    }

    public GameObject CreateObject(Vector3 position, Vector3 lookAtRotation)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].activeSelf)
            {
                objects[i].SetActive(true);
                transforms[i].position = position;
                objects[i].transform.rotation = Quaternion.LookRotation(lookAtRotation);

                if (hasAnimator)
                {
                    Animator animator = objects[i].GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Play("DefaultAnimation"); // Ensure the animation name is correct or use other methods to play animations.
                    }
                }

                if (hasParticleSystem)
                {
                    ParticleSystem ps = objects[i].GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }

                createdTime[i] = Time.time;
                return objects[i];
            }
        }

        GameObject obj = Object.Instantiate(objects[0]) as GameObject;
        objects.Add(obj);
        transforms.Add(obj.transform);
        createdTime.Add(0f);
        obj.name = objects[0].name;
        obj.transform.parent = folderObject.transform;

        if (obj.GetComponent<Animator>() != null)
        {
            hasAnimator = true;
        }
        if (obj.GetComponent<ParticleSystem>() != null)
        {
            hasParticleSystem = true;
        }
        obj.SetActive(true);

        return obj;
    }

    public void AutoDestruct()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].activeSelf)
            {
                if (Time.time - createdTime[i] > life)
                {
                    objects[i].SetActive(false);
                }
            }
        }
    }

    public GameObject DeleteObject(GameObject obj)
    {
        obj.SetActive(false);
        return obj;
    }
}
