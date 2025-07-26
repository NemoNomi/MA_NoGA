using UnityEngine;

///
/// Plays Particle System on GameObject Activation.
/// 

public class PlayParticlesOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        if (particles == null)
            particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (particles != null)
            particles.Play(true);
    }
}
