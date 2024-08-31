using UnityEngine;
using System.Collections;

public class OneShotParticleScript : MonoBehaviour
{
    private ParticleSystem particleSystem;

    // Use this for initialization
    IEnumerator Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem == null)
        {
            Debug.LogError("No ParticleSystem component found on this GameObject.");
            yield break;
        }

        // Wait for half of the main duration of the particle system
        yield return new WaitForSeconds(particleSystem.main.duration / 2);

        // Stop the particle system
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // No changes needed here for the ParticleSystem
    }
}
