using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadioScript : MonoBehaviour
{
    private AudioSource playingTrack;
    public GameObject songName;
    public static int controlNumber = 5;
    private int previousTrack = -1;
    public static bool firstTime = true;
    private int[] trackNumbers = new int[]{
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
    };
    //scriptul pentru radio//
    void Start()
    {
        if (firstTime)
        {
            ChangeTrack();
            firstTime = false;
        }
    }

    void Update()
    {
        if (InteractionHandler.radioState)
        {
            if (playingTrack != null && !playingTrack.isPlaying)
                playingTrack.Play();
        }
        else if (playingTrack != null && playingTrack.isPlaying)
        {
            playingTrack.Stop();
        }
        if (controlNumber != previousTrack)
        {
            ChangeTrack();
        }
    }

    void ChangeTrack()
    {
        //opreste melodia anterioara si o porneste pe cea actuala//
        if (previousTrack != -1)
        {
            GameObject prevTrackObj = GameObject.Find("Tracksong." + trackNumbers[previousTrack]);
            if (prevTrackObj != null)
            {
                Debug.Log(prevTrackObj);
                AudioSource prevAudioSource = prevTrackObj.GetComponent<AudioSource>();
                if (prevAudioSource != null)
                    prevAudioSource.Stop();
            }
        }

        GameObject trackObj = GameObject.Find("Tracksong." + trackNumbers[controlNumber]);
        if (trackObj != null)
        {
            Debug.Log(trackObj);
            playingTrack = trackObj.GetComponent<AudioSource>();
            if (playingTrack != null)
                playingTrack.Play();
        }

        previousTrack = controlNumber;
    }

}
