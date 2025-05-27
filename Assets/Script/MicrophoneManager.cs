using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public float loudnessThreshold = 0.1f;
    private AudioClip micRecord;
    private string device;
    private bool micInitialized;
    [SerializeField] private SpriteRenderer cat;
    public bool pressing;
    private bool CatHear;
    [SerializeField] private GameObject CommentBox;
    private float timer;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            device = Microphone.devices[0];
            micRecord = Microphone.Start(device, true, 1, 44100);
            micInitialized = true;
        }
        else
        {
            Debug.LogWarning("No microphone found!");
        }
    }

    void Update()
    {
        if (pressing)
        {
            if (!micInitialized) return;
            float loudness = GetLoudnessFromMic();
            if (loudness > loudnessThreshold)
            {
                Debug.Log("Sound: " + loudness);
                CatHear = true;
                cat.color = Color.red;
            }
            else if (loudness < loudnessThreshold)
            {
                cat.color = Color.white;
            }
        }

        if (CatHear && !pressing)
        {
            timer += Time.deltaTime;
            CommentBox.SetActive(true);
            cat.color = Color.white;
            if (timer >= 3)
            {
                CommentBox.SetActive(false);
                CatHear = false;
                timer = 0;
            }
        }
    }

    float GetLoudnessFromMic()
    {
        int micPosition = Microphone.GetPosition(device) - 128;
        if (micPosition < 0) return 0;
        float[] samples = new float[128];
        micRecord.GetData(samples, micPosition);
        float levelMax = 0;
        foreach (float sample in samples)
        {
            float wavePeak = Mathf.Abs(sample);
            if (wavePeak > levelMax)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax * sensitivity;
    }

    public void IsPress()
    {
        pressing = true;
    }
    
    public void NotPress()
    {
        pressing = false;
    }
}
