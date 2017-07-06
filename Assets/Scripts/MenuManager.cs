using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject highscore;
    public Button audioButton;
    static MenuManager instance;

    private void Awake()
    {
        //Handling Singleton D.P.
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Load highscore
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void MoreButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Cesarsk+Dev+Team");
    }

    private void LoadHighScore()
    {
        highscore.GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
    }

    public void Github()
    {
        Application.OpenURL("https://github.com/Cesarsk/Unity-aaEstethic");
        Debug.Log("Opened external website");
    }

    public void ToggleAudio()
    {
        if (!AudioManager.isAudioMuted)
        {
            AudioManager.GetInstance().GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetString("sound", "true");
            audioButton.GetComponentInChildren<Text>().text = "/music stops";
        }
        else
        {
            AudioManager.GetInstance().GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetString("sound", "false");
            audioButton.GetComponentInChildren<Text>().text = "/music plays";
        }
        Debug.Log("isMuted: " + AudioManager.GetInstance().GetComponent<AudioSource>().mute);
        AudioManager.isAudioMuted = !AudioManager.isAudioMuted;
    }
}
