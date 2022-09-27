using UnityEngine;

public class CameraAnim
{
    // Use this for initialization
    public GameObject directionObj;
    private GameObject CameraObject;
    private float startAngle = 0.0f;
    private Vector3 initPostion;
    private Vector3 LookAt = new Vector3(0,0,0);
    private float rotateRadio;
    public bool rotateOver;

    public bool isBegin;
    void Start() {

    }

    public void BeginAnim()
    {
       // CurrentTime = 0.0f;
    }

    public void init(GameObject gameObjectTemp, float startAngleTemp, Vector3 LookAtTemp)
    {
        isBegin = true;
        initPostion.x = LookAtTemp.x;
        initPostion.y = LookAtTemp.y;
        initPostion.z = LookAtTemp.z;
        rotateRadio = 10f;
      //  rotateOver = true;
        CameraObject = gameObjectTemp;
        startAngle = startAngleTemp;
        LookAt = LookAtTemp;
        CameraObject.transform.RotateAround(LookAt, new Vector3(0, 1, 0), startAngle);
        directionObj.transform.Rotate(0, startAngle, 0);
        rotateOver = false;
    }
	// Update is called once per frame
	public void Update () {
        if (startAngle>0)
        {
            startAngle -= rotateRadio;
            CameraObject.transform.RotateAround(LookAt, new Vector3(0, 1, 0), -rotateRadio);          
        }
        if (isBegin)
        {
            if (startAngle == 0.0f)
            {
                isBegin = false;
                rotateOver = true;
            }
            else
            {
                rotateOver = false;
            }
        }
       
    }

    public void Reset()
    {
        CameraObject.transform.position = initPostion;
    }
}
