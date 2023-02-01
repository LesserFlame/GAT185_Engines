using Cyan;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class TimeManager : MonoBehaviour
{
    public float timeSpeed = 1;
	public float timeLimit = 5;
	public float timeLimitMax = 5;
	public float timeCooldown = 0;
	public float timeCooldownMax = 10;
	private Object[] freezeables = null;
	public AudioSource timeSound = null;
	public AudioClip timeStart = null;
	public AudioClip timeStop = null;

	public Material timeEffectMaterial = null;
	private float rippleEffectMax = 0.2f;
	private float rippleEffect = 0;
	private float saturation = 0;
	private float hue = 0;
	private float spread = 0;

	private new Camera camera = null;
	private Volume cameraEffect = null;
	private LensDistortion distortion = null;
	private float distortionDirection = 0.5f;
	//private bool distortionGrow = true;
    // Start is called before the first frame update
    void Start()
    {
		camera = Camera.main;
		timeSpeed = 1;
		//freezeables = Object.FindObjectsOfType<TimeComponent>();
		//camera = GameObject.FindObjectOfType<Camera>();
		cameraEffect = camera.GetComponent<Volume>();
		cameraEffect.profile.TryGet(out distortion);
		//var w = Screen.width / 2;
		//var h = Screen.height / 2;
		//Vector2 screenCenter = new Vector2(w, h);
		////timeEffectMaterial.SetVector("_ScreenCenter", screenCenter);
	}

    // Update is called once per frame
    void Update()
    {
		var gm = FindObjectOfType<RollerGameManager>();
		gm.SetTimeStop(timeLimit, timeLimitMax);
		gm.SetTimeCooldown(timeCooldown, timeCooldownMax);
		freezeables = Object.FindObjectsOfType<TimeComponent>(); 
		if (timeSpeed == 0)
		{
			timeLimit -= Time.deltaTime;
			timeCooldown += Time.deltaTime * 2;
		}
		if (timeSpeed == 1)
		{
			timeCooldown -= Time.deltaTime;
			timeLimit += Time.deltaTime;
		}
		timeCooldown = Mathf.Clamp(timeCooldown, 0, timeCooldownMax);
		timeLimit = Mathf.Clamp(timeLimit, 0, timeLimitMax);

		if (Input.GetKeyDown(KeyCode.T)) 
        {
			//TryGetComponent<TimeComponent>(out TimeComponent component);
			if (timeSpeed == 1 && timeCooldown <= 0)
			{
				//Debug.Log("ZA WARUDO!!!");
				timeSound.clip = timeStop;
				timeSound.Stop();
				timeSound.Play();
				timeSpeed = 0;
				//timeCooldown = timeCooldownMax;
				foreach(var freezeable in freezeables)
				{
					freezeable.GetComponent<TimeComponent>().StopTime();
				}
			}
			else if (timeSpeed == 0 && timeLimit <= timeLimitMax - 2)
			{
				//Debug.Log("Time will now resume.");
				timeSound.clip = timeStart;
				timeSound.Stop();
				timeSound.Play();
				timeSpeed = 1;
				foreach (var freezeable in freezeables)
				{
					freezeable.GetComponent<TimeComponent>().StartTime();
				}
				//timeCooldown = timeCooldownMax - timeLimit;
				//timeLimit = timeLimitMax;
			}
        }
		else if(timeLimit <= 0)
		{
			timeSpeed = 1;
			timeLimit = timeLimitMax;
			timeSound.clip = timeStart;
			foreach (var freezeable in freezeables)
			{
				freezeable.GetComponent<TimeComponent>().StartTime();
			}
			timeSound.GetComponent<AudioSource>().Play();
			timeCooldown = timeCooldownMax;
		}

		//////fancy screen stuffs
		///
		if (timeSpeed <= 0)
		{
			rippleEffect = Mathf.Lerp(rippleEffect, rippleEffectMax, 0.01f);
			hue = Mathf.Lerp(hue, 360, 0.05f);		
			saturation = Mathf.Lerp(saturation, 0.2f, 0.01f);
			spread = Mathf.Lerp(spread, 100, 0.01f);
			if (distortion.intensity.value <= -0.9f)
			{
				distortionDirection = 0.5f;
			}
			
			distortion.intensity.value += distortionDirection * Time.deltaTime;
			distortion.intensity.value = Mathf.Clamp(distortion.intensity.value, -1, -0.3f);
		}
		else
		{
			distortionDirection = -5;
			rippleEffect = Mathf.Lerp(rippleEffect, 0, 0.1f);
			hue = Mathf.Lerp(0, hue, 0.05f);
			saturation = Mathf.Lerp(1, saturation, 0.01f);
			spread = Mathf.Lerp(0, spread, 0.01f);

			distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, 0, 0.01f);
			if (distortion.intensity.value > -0.01) distortion.intensity.value = 0;
		}
		timeEffectMaterial.SetFloat("_FullScreenInensity", rippleEffect);
		timeEffectMaterial.SetFloat("_HueOffset", hue);
		timeEffectMaterial.SetFloat("_Saturation", saturation);
		timeEffectMaterial.SetFloat("_Spread", spread);

		

		var w = Screen.width / 2;
		var h = Screen.height / 2;
		Vector2 screenCenter= new Vector2(w, h);
	}
}
