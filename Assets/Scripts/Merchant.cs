using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Security.Cryptography;

public class Merchant : MonoBehaviour
{
    public Button enterStore;
    public Button buying;
    public Button selling;
    public Button rifle;
    public Button shotgun;
    public Button grenade;
    public Button noGold;
    public Button leaving;

    AudioSource audio;
    public AudioClip[] audioClipArray;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        enterStore.onClick.AddListener(() => PlayAudioSample(audioClipArray[0]));
        buying.onClick.AddListener(() => PlayAudioSample(audioClipArray[RandomizeAudio(1, 4)]));
        selling.onClick.AddListener(() => PlayAudioSample(audioClipArray[RandomizeAudio(4, 6)]));
        rifle.onClick.AddListener(() => PlayAudioSample(audioClipArray[RandomizeAudio(6, 8)]));
        shotgun.onClick.AddListener(() => PlayAudioSample(audioClipArray[RandomizeAudio(8, 10)]));
        grenade.onClick.AddListener(() => PlayAudioSample(audioClipArray[10]));
        noGold.onClick.AddListener(() => PlayAudioSample(audioClipArray[11]));
        leaving.onClick.AddListener(() => PlayAudioSample(audioClipArray[12]));
    }

    void PlayAudioSample(AudioClip fileName)
    {
        audio.clip = fileName;
        audio.Play();
    }

    int RandomizeAudio(int start, int end)
    {
        int randomNum = Random.Range(start, end);
        return randomNum;
    }
}
