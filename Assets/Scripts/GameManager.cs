using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private bool gameHasEnded = false;
    private bool level_layout = false;
    private bool adShown = false;
    private bool adEnabled = true;
    static public bool isPaused = false;
    private int level = 1;
    private AudioSource[] sounds;

    //Tuning parameters
    private const float SPEED_LEVEL_ONE = 200f;
    private const float SPEED_LEVEL_TWO = 240f;
    private const float SPEED_LEVEL_THREE = 280f;
    private const float SPEED_LEVEL_FOUR = 300f;

	private const int ROTATION_INVERSE_CHANCE_ONE = 2;
    private const int ROTATION_INVERSE_CHANCE_TWO = 3;
    private const int ROTATION_INVERSE_CHANCE_THREE = 4;
    private const int ROTATION_INVERSE_CHANCE_FOUR = 5;

    private const int SPEED_INCREASE_CHANCE_ONE = 4;
    private const int SPEED_INCREASE_CHANCE_TWO = 5;
    private const int SPEED_INCREASE_CHANCE_THREE = 6;

    //Ad chance
    private const int AD_CHANCE = 12;

    public static GameManager instance;
    public Rotator rotator;
    public Spawner spawner;
    public Camera mainCamera;
    public Animator cameraAnimator;
    public Animator scoreAnimator;
    public GameObject pauseGround;
    public Text highscoreLabel;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        //retrieving sounds
        sounds = GetComponents<AudioSource>();
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        level = 1;
        Time.timeScale = 1;
        MakeLevelHarder(level);
        isPaused = false;
    }

    public void EndGame()
    {
        if (gameHasEnded) return;

        gameHasEnded = true;
        SetHighscore();
        rotator.enabled = false;
        spawner.enabled = false;

        sounds[1].Play();
        cameraAnimator.SetTrigger("EndGame");
        scoreAnimator.SetTrigger("Idle");

        if (adEnabled) ShowAd();
    }

    public void RestartLevel()
    {
        level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        adShown = false;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseGround.SetActive(true);
            Time.timeScale = 0;
            GetHighscore();
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseGround.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Ragequit()
    {
        if (gameHasEnded) return;
        gameHasEnded = true;
        SetHighscore();
        rotator.enabled = false;
        spawner.enabled = false;
        SceneManager.LoadSceneAsync("Menu");
    }

    public void NextLevel()
    {
        if (!gameHasEnded)
        {
            sounds[0].Play();
            //Not Rotated and Rotated layout
            if (!level_layout)
            {
                cameraAnimator.SetTrigger("NextLevel");
                scoreAnimator.SetTrigger("Rotate");
            }
            else if (level < 11)
            {
                Debug.Log("Current level: " + level);
                cameraAnimator.SetTrigger("NextLevel");
                scoreAnimator.SetTrigger("Idle");
            }
            else if (level >= 11)
            {
                cameraAnimator.SetTrigger("Idle");
                scoreAnimator.SetTrigger("Idle");
            }

            level_layout = !level_layout;

            GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");

            foreach (GameObject pin in pins)
            {
                pin.GetComponent<Animator>().SetTrigger("FadeOutPin");
                //destroy obj after around 1s because the animation lasts 1 sec
                Destroy(pin, 0.9f);
            }

            if (level < 11)
            {
                level++;
                MakeLevelHarder(level);
            }
        }
    }

    public void ShowAd()
    {
        if (!adShown)
        {
            int adRate = Random.Range(1, 101);
            if (adRate >= 1 && adRate <= AD_CHANCE)
            {
                if (Advertisement.IsReady())
                {
                    adShown = true;
                    Advertisement.Show();
                }
            }
        }
    }

    private Coroutine rotationCoroutine;
    private Coroutine speedCoroutine;

    IEnumerator ChangeRotationRandomly(int chance)
    {
        //chance of rotating. if chance = n, there's n% chance of changing rotation every second
        while (true)
        {
            //Debug.Log("Chance: "+chance);
            int random = (int)Random.Range(1f, 10);
            if (random <= chance) Rotator.speed *= -1;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ChangeSpeedRandomly(int chance)
    {
        //chance of rotating. if chance = n, there's n% chance of changing speed every second
        while (true)
        {
            Debug.Log("Speed: " + Rotator.speed);
            int random = (int)Random.Range(1f, 10f);
            if (random <= chance) Rotator.speed = (int)Random.Range(25f, 32f) * 10;
            yield return new WaitForSeconds(1f);
        }
    }

	/*
     * Level 0-10 (case 1): SPEED_ONE
     * Level 10-20 (case 2): SPEED_TWO
     * Level 20-30 (case 3): SPEED_THREE
     * Level 30-40 (case 4): SPEED_FOUR
     * 
     * Level 40-50 (case 5): SPEED_TWO + ROTATION_ONE
     * Level 50-60 (case 6): SPEED_TWO + ROTATION_ONE + VELOCITY_ONE
     * Level 60-70 (case 7): SPEED_THREE + ROTATION_TWO + VELOCITY_TWO
     * Level 70-80 (case 8): SPEED_FOUR + ROTATION_TWO + VELOCITY_THREE
     * Level 80-90 (case 9): SPEED_THREE + ROTATION_THREE + VELOCITY_THREE
     * Level 90-100 (case 10): SPEED_FOUR + ROTATION_THREE + VELOCITY_THREE
     * 
     * Level 100-110 (case 11 - ENDLESS): SPEED_FOUR + ROTATION_FOUR + VELOCITY_THREE
     * */

	public void MakeLevelHarder(int level)
    {
        switch (level)
        {
            case 1:
				Debug.Log("Level 1");
                Rotator.speed = SPEED_LEVEL_ONE;
                break;

            case 2:
				Debug.Log("Level 2");
                Rotator.speed = SPEED_LEVEL_TWO;
                break;

            case 3:
				Debug.Log("Level 3");
                Rotator.speed = SPEED_LEVEL_THREE;
                break;

            case 4:
				Debug.Log("Level 4");
                Rotator.speed = SPEED_LEVEL_FOUR;
                break;

            case 5:
				Debug.Log("Level 5");
                Rotator.speed = SPEED_LEVEL_TWO;
                rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_ONE));
                break;

            case 6:
				Debug.Log("Level 6");
                StopAllCoroutines();
				Rotator.speed = SPEED_LEVEL_TWO;
				rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_ONE));
                speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_ONE));
				break;

            case 7:
				Debug.Log("Level 7");
                StopAllCoroutines();
                Rotator.speed = SPEED_LEVEL_THREE;
                rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_TWO));
                speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_TWO));
                break;

            case 8:
				Debug.Log("Level 8");
                StopAllCoroutines();
                Rotator.speed = SPEED_LEVEL_FOUR;
				rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_TWO));
                speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_THREE));
                break;

            case 9:
				Debug.Log("Level 9");
                StopAllCoroutines();
                Rotator.speed = SPEED_LEVEL_THREE;
                rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_THREE));
				speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_THREE));
                break;

            case 10:
				Debug.Log("Level 10");
                StopAllCoroutines();
                Rotator.speed = SPEED_LEVEL_FOUR;
				rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_THREE));
				speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_THREE));
                break;

            case 11:
                Debug.Log("Endless Level");
                StopAllCoroutines();
				Rotator.speed = SPEED_LEVEL_FOUR;
                rotationCoroutine = StartCoroutine(ChangeRotationRandomly(ROTATION_INVERSE_CHANCE_FOUR));
				speedCoroutine = StartCoroutine(ChangeSpeedRandomly(SPEED_INCREASE_CHANCE_THREE));
                break;

            default:
				Debug.Log("Default");
				StopAllCoroutines();
                Rotator.speed = SPEED_LEVEL_ONE;
				break;
        }
    }

    private void SetHighscore()
    {
        int currentScore = Score.PinCount + 1;
        int currentHighscore = PlayerPrefs.GetInt("highscore", 0);
        if (currentHighscore < currentScore) PlayerPrefs.SetInt("highscore", currentScore);
    }


    private void GetHighscore()
    {
        int currentHighscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreLabel.text = "Highscore:" + currentHighscore;
    }

    private void ResetHighscore()
    {
        PlayerPrefs.DeleteAll();
    }

}