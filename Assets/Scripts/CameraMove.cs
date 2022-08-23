using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] [Range(10, 50)] private float _cameraSpeed = 50;
    [HideInInspector] public float _sensivity = 4000;
    [SerializeField] [Range(1, 2)] private float _scrollSensivity = 1;
    [SerializeField] private Camera _camera;
    private float posX;
    private float posY;

    private int cameraRotDegreesMin = 35;
    private int cameraRotDegreesMax = 48;
    private float limitsX;
    private float limitsY;
    private float currenLimitsX;
    private float currenLimitsY;
    private float leftLimit = -143.5f;
    private float rightLimit = -53f;
    private float upLimit = -29.3f;
    private float bottomLimit = 30.63f; 

    private float time = 0;
    private const float timer = 0.2f;

    private Vector3 mousePoint;
    private float mousePosX;
    private float mousePosY;
    private float currentMousePosX;
    private float currentMousePosY;

    private float scrollWheel;
    private void Start()
    {
        CameraLimits();
    }
    void Update()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MovementKey();
            transform.position = CameraClamp();
        }

        if (Input.GetMouseButton(0))
        {
            MovementMouse();
            transform.position = CameraClamp();
        }
        else
        {
            if(time != 0)
            {
                time = 0;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            MouseZoom();
            CameraLimits();
        }
    }
    void MovementKey()
    {
        posX = Input.GetAxis("Horizontal") * _cameraSpeed * Time.deltaTime / Time.timeScale;
        posY = Input.GetAxis("Vertical") * _cameraSpeed * Time.deltaTime / Time.timeScale;

        transform.position += new Vector3(-posY, 0f, posX);
    }
    void MovementMouse()
    {
        mousePoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        currentMousePosX = mousePosX - mousePoint.y;
        currentMousePosY = mousePosY - mousePoint.x;

        time += Time.deltaTime;
        if (time + Time.deltaTime > timer)
        {
            transform.position += new Vector3
                (-currentMousePosX * _sensivity * Time.deltaTime / Time.timeScale, 0, currentMousePosY * _sensivity * Time.deltaTime / Time.timeScale);
        }

        mousePosX = mousePoint.y;
        mousePosY = mousePoint.x;
    }
    void MouseZoom()
    {
        int degrees = cameraRotDegreesMax - cameraRotDegreesMin;
        scrollWheel -= Input.GetAxis("Mouse ScrollWheel");
        if(_camera.orthographicSize < 15) transform.Rotate(new Vector3(scrollWheel * degrees * _scrollSensivity, 0, 0));
        _camera.orthographicSize += scrollWheel * 5 * _scrollSensivity;
        transform.eulerAngles = new Vector3
            (Mathf.Clamp(transform.eulerAngles.x, cameraRotDegreesMin, cameraRotDegreesMax), transform.eulerAngles.y, transform.eulerAngles.z);
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 10, 20);
        scrollWheel = 0;
    }
    void CameraLimits()
    {
        float limX;
        float limY;
        float displacement;
        limitsX = _camera.orthographicSize - 15;
        limitsY = limitsX * _camera.aspect;
        limX = LimitCount(limitsX);
        limY = limitsY;

        currenLimitsX = limX - currenLimitsX;
        currenLimitsY = limY - currenLimitsY;

        if (transform.position.z < leftLimit + currenLimitsY + 8) displacement = currenLimitsY;
        else if (transform.position.z > rightLimit - currenLimitsY - 8) displacement = -currenLimitsY;
        else displacement = 0;

        transform.position += new Vector3(-currenLimitsX, 0, displacement); 

        currenLimitsX = limX;
        currenLimitsY = limY;

        
    }
    Vector3 CameraClamp() => new Vector3
        (Mathf.Clamp(transform.position.x, upLimit, bottomLimit - currenLimitsX), transform.position.y, 
        Mathf.Clamp(transform.position.z, leftLimit + currenLimitsY, rightLimit - currenLimitsY));
    float LimitCount(float num) => num / (Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x));
}
