using UnityEngine;

public class AudioDestroyer : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<AudioSource>().clip.length);
    }
}
