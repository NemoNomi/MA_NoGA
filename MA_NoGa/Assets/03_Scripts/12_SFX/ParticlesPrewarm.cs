using UnityEngine;

///
/// Plays the Particle System once by disabling looping.
///

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
