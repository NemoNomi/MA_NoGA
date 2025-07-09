using UnityEngine;

/// <summary>
/// Stops the Loop of Particles after 1 Round.
/// </summary>

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesPrewarm : MonoBehaviour
{
    void Awake()
    {
        var ps   = GetComponent<ParticleSystem>();
        ps.Play();
        var main = ps.main;
        main.loop = false;
    }
}
