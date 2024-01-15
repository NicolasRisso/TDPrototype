using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration + 1);
    }
}
