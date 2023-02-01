using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.IMGUI.Controls.CapsuleBoundsHandle;

[RequireComponent(typeof(Rigidbody))]
public class RollerPlayer : MonoBehaviour
{
    [SerializeField] private Transform view;
    [SerializeField] private float maxForce = 50;
    [SerializeField, Range(0, 50)] private float jumpPower = 10;

	[SerializeField] private float groundRayLength = 1;
    [SerializeField] public Vector3 gravityDirection = Vector3.down;
	[SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameObject fallEffect;

	private Vector3 force;
    private Rigidbody rb;
    private int score;

    private Quaternion gravityAlignment = Quaternion.identity;

	// Start is called before the first frame update
	void Start()
    {
        //Camera camera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();

        view = Camera.main.transform;
        Camera.main.GetComponent<RollerCamera>().SetTarget(transform);

        GetComponent<Health>().onDamage += OnDamage;
        GetComponent<Health>().onDeath += OnDeath;
        GetComponent<Health>().onHeal += OnHeal;
		RollerGameManager.Instance.SetHealth((int)GetComponent<Health>().health);

	}

	// Update is called once per frame
	void Update()
    {
        //var gm = FindObjectOfType<RollerGameManager>();
        //var timer = gm.GetTimer();
        //if (timer <= 0) { healt}
		gravityAlignment = Quaternion.FromToRotation(gravityAlignment * Vector3.up, -gravityDirection) * gravityAlignment;

		Vector3 direction = Vector3.zero;

		direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");


        //direction = Vector3.Cross(direction, gravityDirection);

        //Debug.DrawRay(transform.position, gravityDirection * 5, Color.red);
        //Debug.Log(direction.x + " " + direction.y + " " + direction.z);
        Quaternion viewSpace = Quaternion.AngleAxis(view.rotation.eulerAngles.y, Vector3.up);
        force = viewSpace * (direction * maxForce);
        force = gravityAlignment * force;
        //Physics.gravity = gravityDirection * 9.81f;
		Ray ray = new Ray(transform.position, gravityDirection);
		bool onGround = Physics.Raycast(ray, groundRayLength, groundLayer);
		Debug.DrawRay(transform.position, ray.direction * groundRayLength);

        Debug.DrawRay(transform.position, force * 5, Color.blue);

		if (onGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(-gravityDirection * jumpPower, ForceMode.Impulse);
        }
        //RollerGameManager.Instance.SetHealth(50);

        if (score == 3100)
        {
            RollerGameManager.Instance.SetGameWon();
            Destroy(gameObject);
        }
    }

	private void FixedUpdate()
	{
		rb.AddForce(force);
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);
	}

    public void AddPoints(int points)
    {
        score += points;
        RollerGameManager.Instance.SetScore(score);
    }

	public void OnHeal()
	{
		RollerGameManager.Instance.SetHealth((int)GetComponent<Health>().health);
	}
	public void OnDamage()
    {
        RollerGameManager.Instance.SetHealth((int)GetComponent<Health>().health);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
        RollerGameManager.Instance.SetGameOver();
    }

	private void OnCollisionEnter(Collision collision)
	{
        Vector3 impact = collision.relativeVelocity.normalized;
        //if (collision.relativeVelocity.magnitude > 1)
        //{
        //    foreach (var contact in collision.contacts)
        //    {
        //        gravityDirection = -contact.normal;
        //    }
        //    //gravityDirection = -impact;
        //}
        var effect = Instantiate(fallEffect, transform.position + (impact * -0.5f), Quaternion.Euler(impact * -1));
        float volume = collision.relativeVelocity.magnitude / 10;
		volume = Mathf.Clamp(volume, 0, 1);
        //Debug.Log(volume);

		effect.GetComponent<AudioSource>().volume = volume;
        //Debug.DrawRay(transform.position, impact * 5, Color.red);
	}
}