using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CFX_Demo_New : MonoBehaviour
{
    public Text EffectLabel;
    public Text EffectIndexLabel;

    public Renderer groundRenderer;
    public Collider groundCollider;

    //-------------------------------------------------------------

    private GameObject[] ParticleExamples;
    private int exampleIndex;
    private bool slowMo;
    private Vector3 defaultCamPosition;
    private Quaternion defaultCamRotation;

    private List<GameObject> onScreenParticles = new List<GameObject>();

    //-------------------------------------------------------------

    void Awake()
    {
        List<GameObject> particleExampleList = new List<GameObject>();
        int nbChild = this.transform.childCount;
        for (int i = 0; i < nbChild; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            particleExampleList.Add(child);
        }
        ParticleExamples = particleExampleList.ToArray();

        defaultCamPosition = Camera.main.transform.position;
        defaultCamRotation = Camera.main.transform.rotation;

        StartCoroutine("CheckForDeletedParticles");

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            prevParticle();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextParticle();
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            destroyParticles();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
            {
                GameObject particle = spawnParticle();
                particle.transform.position = hit.point + particle.transform.position;
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Camera.main.transform.Translate(Vector3.forward * (scroll < 0f ? -1f : 1f), Space.Self);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Camera.main.transform.position = defaultCamPosition;
            Camera.main.transform.rotation = defaultCamRotation;
        }
    }

    //-------------------------------------------------------------
    // MESSAGES

    void OnToggleGround()
    {
        groundRenderer.enabled = !groundRenderer.enabled;
    }

    void OnToggleCamera()
    {
        CFX_Demo_RotateCamera.rotating = !CFX_Demo_RotateCamera.rotating;
    }

    void OnToggleSlowMo()
    {
        slowMo = !slowMo;
        Time.timeScale = slowMo ? 0.33f : 1.0f;
    }

    void OnPreviousEffect()
    {
        prevParticle();
    }

    void OnNextEffect()
    {
        nextParticle();
    }

    //-------------------------------------------------------------
    // UI

    private void UpdateUI()
    {
        EffectLabel.text = ParticleExamples[exampleIndex].name;
        EffectIndexLabel.text = string.Format("{0}/{1}", (exampleIndex + 1).ToString("00"), ParticleExamples.Length.ToString("00"));
    }

    //-------------------------------------------------------------
    // SYSTEM

    private GameObject spawnParticle()
    {
        GameObject particles = Instantiate(ParticleExamples[exampleIndex]);
        particles.transform.position = new Vector3(0, particles.transform.position.y, 0);
        particles.SetActive(true);

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        if (ps != null && ps.main.loop)
        {
            ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
            ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
        }

        onScreenParticles.Add(particles);

        return particles;
    }

    IEnumerator CheckForDeletedParticles()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            for (int i = onScreenParticles.Count - 1; i >= 0; i--)
            {
                if (onScreenParticles[i] == null)
                {
                    onScreenParticles.RemoveAt(i);
                }
            }
        }
    }

    private void prevParticle()
    {
        exampleIndex--;
        if (exampleIndex < 0) exampleIndex = ParticleExamples.Length - 1;

        UpdateUI();
    }

    private void nextParticle()
    {
        exampleIndex++;
        if (exampleIndex >= ParticleExamples.Length) exampleIndex = 0;

        UpdateUI();
    }

    private void destroyParticles()
    {
        for (int i = onScreenParticles.Count - 1; i >= 0; i--)
        {
            if (onScreenParticles[i] != null)
            {
                Destroy(onScreenParticles[i]);
            }

            onScreenParticles.RemoveAt(i);
        }
    }
}
