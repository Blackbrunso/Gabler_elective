using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [Header("UI")]
    public GameObject bust;
    public GameObject pauseMenu;
    public TextMeshProUGUI score;
    public TextMeshProUGUI finalscore;

    [Header("PlayerMovement")]
    public PlayerMovement pM;

    [Header("GetCaught")]
    public GetCaught gC;

    [Header("Spawner")]
    public SpawnerManager sM;
    public float spawnrate;

    [Header("SliceObjects")]
    public ObjectSlicer Os;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();

        if (bust != null && pM != null && gC != null && sM != null && Os != null)
        {
            if (gC.busted)
            {
                sM.enabled = false;
                pM.SetSpeed(0);
                bust.SetActive(true);
            }

            if (sM.streetGroup.maxInterval > sM.streetGroup.minInterval)
            {
                sM.streetGroup.maxInterval -= spawnrate * Time.deltaTime;
            }

            score.text = "Score: " + Os.point;
            finalscore.text = "Points: " + Os.point;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (pauseMenu != null) pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
