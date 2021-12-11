using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CarController : MonoBehaviour
{

    


    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    public VariableJoystick joystick1;
    public FixedJoystick joystick2;
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    public Text Laptest;
    public Text Scoretext;
    public Text gScoreText;
    public Text lScoretext;
    private int lapt;
    private int score;
    private float timer;
    private int highscore;
    public Text pbestscore;
    public Text lbestscore;
    public Text gbestscore;
    public GameObject Gameoverpnel;
    public GameObject finishGamepanel;
    public GameObject pausepanel;
    public GameObject playpanel;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    public void Start()
    {
        Time.timeScale = 0;
        score = 0;
        timer = Time.time;
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        score = PlayerPrefs.GetInt("score", score);

    }
    private void Update()
    {
        if (Time.time - timer > 1)
        {
            score++ ;
            timer = Time.time;
            
        }
        if (score > highscore)
            highscore = score;
        Scoretext.text = "" + score;
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.SetInt("score", score);
        gScoreText.text = "" + score;
        lScoretext.text = "" + score;
        pbestscore.text = "" + highscore;
        gbestscore.text = "" + highscore;
        lbestscore.text = "" + highscore;


    }
    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        
        GetInput();
        HandleMotor();
       
        HandleSteering();
        UpdateWheels();
    }


    public void GetInput()
    {
        horizontalInput = joystick1.Horizontal;
        verticalInput = joystick2.Vertical;
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    public void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
   
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    public void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    public void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot ;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finishline")
        {

            finishGamepanel.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Level Complete");
            lapt = 1;
            Laptest.text = lapt + "/1";

            finishGamepanel.SetActive(true);
            Time.timeScale = 0;
        }

        if(other.tag == "enemy")
        {
            Debug.Log("Destroy");
            Time.timeScale = 0;
            Gameoverpnel.SetActive(true);
        }
    }
    public void Pause()
    {
        pausepanel.SetActive(true);
        Time.timeScale = 0;

    }
    public void Paureplat()
    {
        pausepanel.SetActive(false);
        Time.timeScale = 1;

    }
    public void Mainmenu()
    {

        playpanel.SetActive(true);
        pausepanel.SetActive(false);
    }
    public void play()
    {
        playpanel.SetActive(false);
        Time.timeScale = 1;
        score = 0;
    }
    public void restart()
    {
        SceneManager.LoadScene(0);
    }

}
