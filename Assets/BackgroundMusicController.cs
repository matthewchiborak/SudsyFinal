using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour {

    public AudioSource backgroundMusicSource;
    public BackgroundMusicModel bgMusicModel;

	public void toggleAudio()
    {
        backgroundMusicSource.mute = !backgroundMusicSource.mute;
    }

    public void switchTracks(int index)
    {
        if(index < bgMusicModel.musicTracks.Length)
        {
            backgroundMusicSource.clip = bgMusicModel.musicTracks[index];
            backgroundMusicSource.Play();
        }
    }

    //Testing to make sure audio switching works
    public void testTrackSwitch()
    {
        switchTracks(1);
    }
}
