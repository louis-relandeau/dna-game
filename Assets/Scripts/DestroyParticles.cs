using UnityEngine;
using System.Collections;

public class DestroyAfterParticle : MonoBehaviour
{
    new private ParticleSystem particleSystem;

    void Start() {
        if (particleSystem == null) {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }
        if (particleSystem == null) {
            Debug.LogError("No ParticleSystem found in child objects.");
            return;
        }

        // Start checking in a coroutine
        StartCoroutine(CheckIfDone());
    }

    private IEnumerator CheckIfDone()
    {
        // Wait until the particle system stops playing
        while (particleSystem.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Destroy the root GameObject
        Destroy(gameObject);
    }
}
