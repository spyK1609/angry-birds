using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] birds;
    public CameraController cam;
    private int birdIndex;
    private GameObject currentBird;
    public float reloadBirdDelay = 3f;
    private ThrowBird throwBird;
    public TextMeshProUGUI numText;
    public GameObject pauseMenu;
    public GameObject UIMenu;
    public GameObject settingsMenu;
    public AudioSource audioSource;
    public Slider volumeSlider;
    private GameObject[] pigs;
    public int pigsCount;
    public GameObject winPanel;
    public GameObject failPanel;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform trCanvas = canvas.transform;
        birdIndex = 0;
        LoadBird();
        numText = GameObject.Find("Canvas/UI/Panel/Current level number").GetComponent<TextMeshProUGUI>();
        numText.text = SceneManager.sceneCount.ToString();
        pigs = GameObject.FindGameObjectsWithTag("Pig");
        pigsCount = pigs.Length;
        pauseMenu = trCanvas.Find("Pause Menu").gameObject;
        UIMenu = trCanvas.Find("UI").gameObject;
        settingsMenu = trCanvas.Find("Settings").gameObject;
        audioSource = cam.audioSource;
        volumeSlider = canvas.GetComponentInChildren<Slider>(true);
        winPanel = trCanvas.Find("Win Panel").gameObject;
        failPanel = trCanvas.Find("Fail Panel").gameObject;
        
    }

    void Update()
    {

    }

    public void ChangePigsCount()
    {
        pigsCount -= 1;
    }

    void LoadBird()
    {
        currentBird = birds[birdIndex];
        cam.bird = currentBird;
        throwBird = currentBird.GetComponent<ThrowBird>();
        currentBird.transform.position = throwBird.startPosition.position;
        throwBird.InQueue(false);
        currentBird.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        throwBird.onStopped += HandleBirdStopped;
        throwBird.slingshotBand.bird = currentBird.transform;
    }

    void HandleBirdStopped()
    {
        throwBird.onStopped -= HandleBirdStopped;
        StartCoroutine(ReloadNextBird());
    }

    IEnumerator ReloadNextBird()
    {
        yield return new WaitForSeconds(reloadBirdDelay);
        currentBird.SetActive(false);
        if (birdIndex < birds.Length - 1)
        {
            birdIndex += 1;
            LoadBird();
        }
        else if (pigsCount > 0)
        {
            print("Level failed");
            UIMenu.SetActive(false);
            failPanel.SetActive(true);
        }
        else if (pigsCount <= 0)
        {
            print("Level passed");
            UIMenu.SetActive(false);
            winPanel.SetActive(true);

        }
    }

    public void StartScene()
    {
        print("Game started");
    }

    public void OpenPauseMenu()
    {
        UIMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoBackToGame()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        UIMenu.SetActive(true);
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoBackToPauseMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void SaveAndApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
