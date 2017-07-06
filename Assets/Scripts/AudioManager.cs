using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    static AudioManager instance;
    public static bool isAudioMuted = false;
    private GameObject audioButton;

	void Awake () {
        //Singleton D.P.
        if(instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        AttachButton();
        LoadSoundPrefs();
	}

    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void AttachButton()
    {
        //loading the button
        foreach (GameObject audioButt in FindObjectsOfType<GameObject>())
        {
            if (audioButt.name == "MusicButton") audioButton = audioButt;
        }
    }

    // Update is called once per frame
    void Update () {

	}

    public void LoadSoundPrefs()
    {
        String soundState = PlayerPrefs.GetString("sound", "false");
        if (soundState == "true") {
			GetComponent<AudioSource>().mute = true;
            audioButton.GetComponentInChildren<Text>().text = "/music stops";
        }
        else if (soundState == "false") {
            GetComponent<AudioSource>().mute = false;
            audioButton.GetComponentInChildren<Text>().text = "/music plays";
        }
        Debug.Log("isMuted: " + GetComponent<AudioSource>().mute);
        isAudioMuted = GetComponent<AudioSource>().mute;
    }
}
