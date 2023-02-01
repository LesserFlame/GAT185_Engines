using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Runtime.CompilerServices;

public class RollerGameManager : Singleton<RollerGameManager>
{
    [SerializeField] Slider healthMeter;
    [SerializeField] Slider timeMeter;
    [SerializeField] Slider timeStopMeter;
    [SerializeField] Slider timeCooldownMeter;
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject victoryUI;

    [SerializeField] AudioSource gameMusicPlayer;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip titleMusic;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerStart;

    [SerializeField] GameObject[] pickups;

    [SerializeField] private float gameTimerMax = 60;

	[Header("Events")]
	[SerializeField] EventRouter startGameEvent;
	[SerializeField] EventRouter stopGameEvent;

	private float gameTimer;
    public enum State
    { 
        TITLE,
        START_GAME,
        PLAY_GAME,
        GAME_OVER,
        GAME_WON
    }

    State state = State.TITLE;
    float stateTimer = 0;
    private void Start()
    {
    }

	private void Update()
	{
        float timeSpeed = FindObjectOfType<TimeManager>().timeSpeed;
		switch (state)
        {
            case State.TITLE:
				gameMusicPlayer.clip = titleMusic;
				if (!gameMusicPlayer.isPlaying) gameMusicPlayer.Play();
				var cam = FindObjectOfType<RollerCamera>();
                cam.GetComponent<RollerCamera>().ResetView();
				titleUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
				foreach (var pickup in pickups)
				{
					//pickup.gameObject.SetActive(true);
				}
				break;
            case State.START_GAME:
				startGameEvent.Notify();
				gameTimer = gameTimerMax;
                gameMusicPlayer.Stop();
                gameMusicPlayer.clip = gameMusic;
                gameMusicPlayer.Play();
                titleUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                var player = Instantiate(playerPrefab, playerStart.position, playerStart.rotation);
                if (player != null)
                {
                    //FindObjectOfType<CinemachineFreeLook>().LookAt = player.transform;
                    //FindObjectOfType<CinemachineFreeLook>().Follow = player.transform;
                }
                SetScore(0);
                SetHealth(100);
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                gameTimer -= Time.deltaTime * timeSpeed;
                SetTimer(gameTimer, gameTimerMax);
                if (gameTimer <= 0)
                {
                    var playerHP = FindObjectOfType<Health>();
                    playerHP.OnApplyDamage(1000);
                }
                    break;
            case State.GAME_OVER:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameOverUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
            case State.GAME_WON:
				stateTimer -= Time.deltaTime;
				if (stateTimer <= 0)
				{
					victoryUI.SetActive(false);
					state = State.TITLE;
				}
				break;
		}
	}

	private void FixedUpdate()
	{
		var player = FindObjectOfType<RollerPlayer>();
        if (player != null) 
        { 
            transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.2f);
        }
	}

	public void SetHealth(int health)
    {
        healthMeter.value = Mathf.Clamp(health, 0, 100);
    }
    public void SetTimer(float time, float max)
    {
		timeMeter.value = Mathf.Clamp(time, 0, max);
		timeMeter.maxValue = max;
	}
    public float GetTimer()
    {
        return gameTimer;
    }
    public void AddTime(float time)
    {
        gameTimer+= time;
        gameTimer = Mathf.Clamp(gameTimer, 0, gameTimerMax);
    }
    public void SetTimeStop(float time, float max)
    {  
		timeStopMeter.value = Mathf.Clamp(time, 0, max);
        timeStopMeter.maxValue = max;
	}
    public void SetTimeCooldown(float cooldown, float max)
    {
		timeCooldownMeter.value = Mathf.Clamp(cooldown, 0, max);
        timeCooldownMeter.maxValue = max;
	}
    public void SetScore(int score)
    {
        scoreUI.text = score.ToString();
    }
    public void SetGameOver()
    {
		stopGameEvent.Notify();
		gameMusicPlayer.Stop();
		gameOverUI.SetActive(true);
        state = State.GAME_OVER;
        stateTimer = 3;
    }
	public void SetGameWon()
	{
		gameMusicPlayer.Stop();
		victoryUI.SetActive(true);
		state = State.GAME_WON;
		stateTimer = 5;
	}
	public void OnStartGame()
    {
        state = State.START_GAME;
    }
}
