using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for centralizing effect handling
/// </summary>
public class EffectManager : MonoBehaviour
{
    /// <summary>
    /// The list of active particles
    /// </summary>
#pragma warning disable IDE0044 // Stops misidentifying as read-only
    private List<GameObject> particles = new List<GameObject>();
#pragma warning restore IDE0044 

    /// <summary>
    /// Adds a particle to the list of active particles
    /// </summary>
    /// <param name="newParticle">
    /// The particle to add; should have just been instantiated
    /// </param>
    public void AddParticle(GameObject newParticle)
    {
        // Add to particle list
        particles.Add(newParticle);
    }

    // Called once per frame
    void Update()
    {
        // Remove particles that are no longer active
        removeExpiredParticles();
    }

    /// <summary>
    /// Removes particles that are non-looping and have had their animation
    /// expire.
    /// </summary>
    private void removeExpiredParticles()
    {
        // Loop through particles
        for (int i = 0; i < particles.Count; i++)
        {
            // Get particle system
            ParticleSystem ps = GetParticleSystem(i);

            // Destroy if not alive (not looping & duration expired)
            if (!ps.IsAlive())
            {
                Destroy(particles[i]);
                particles.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Gets the particle system of the particle at the given index
    /// </summary>
    /// <param name="index">
    /// The index of the particle GameObject
    /// </param>
    /// <returns>
    /// The particle system of the particle GameObject
    /// </returns>
    private ParticleSystem GetParticleSystem(int index)
    {
        // Get particle system (check both object and any children)
        return particles[index].GetComponentInChildren<ParticleSystem>();
    }
}
