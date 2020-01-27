using UnityEngine;


public class PlayerAudio : MonoBehaviour
{

    AudioSource audioData;
    // Use this for initialization
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
