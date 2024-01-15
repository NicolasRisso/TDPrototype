using UnityEngine;

public class AuidoManager : MonoBehaviour
{
    [SerializeField] private int maxAudiosPerSecond = 5;
    [SerializeField] private int maxCloseCalls = 2;

    private float audioCount = 0;
    private float closeCount = 0;

    private void Start()
    {
        InvokeRepeating("UpdateCount", 0f, 0.2f);
    }

    private void UpdateCount()
    {
        audioCount -= (maxAudiosPerSecond / 5);
        closeCount -= maxCloseCalls;
        if (audioCount < 0) audioCount = 0;
        if (closeCount < 0) closeCount = 0;
    }

    public bool RequestPlay()
    {
        if (audioCount >= maxAudiosPerSecond) return false;
        if (closeCount >= maxCloseCalls) return false;
        else
        {
            audioCount++;
            closeCount++;
            return true;
        }
    }
}
