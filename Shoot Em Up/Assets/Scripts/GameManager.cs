using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject playerUI;
    public Text playerHPText;
    public int index;
    public float initialDelay;
    public float gameOverDelay;
    public PauseGame pause;

    private PlayerController player;
    private Fading fade;
    private bool loading;

    private void Awake()
    {
        fade = GetComponent<Fading>();
        loading = false;
        pause.enabled = false;
    }

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        StartCoroutine(InitialPause());
	}
	
	// Update is called once per frame
	void Update() {
        playerHPText.text = "Health: " + player.hp;

        if (player.isPlayerDead() && !loading)
        {
            loading = true;
            StartCoroutine(LoadScene(index));
        }
	}

    IEnumerator InitialPause()
    {
        player.enabled = false;
        pause.enabled = false;
        yield return new WaitForSeconds(initialDelay);
        player.enabled = true;
        pause.enabled = true;
    }

    public void LoadVictoryScene(int sceneIndex)
    {
        player.enabled = false;
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        float fadeTime = fade.BeginSceneFade(1);
        fade.BeginAudioFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadGameOverScene(int sceneIndex)
    {
        yield return new WaitForSeconds(gameOverDelay);
        float fadeTime = fade.BeginSceneFade(1);
        fade.BeginAudioFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
