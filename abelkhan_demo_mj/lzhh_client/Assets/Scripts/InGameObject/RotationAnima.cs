using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnima : MonoBehaviour {

    // Use this for initialization
    GameObject RotationObject;
    float startAngle = 0.0f;
    public float PassTime = 1.0f;
    float CurrentTime = 0.0f;
    void Start()
    {

    }
    public void BeginAnim()
    {
        CurrentTime = 0.0f;
    }
    public void init(GameObject gameObjectTemp, float startAngleTemp)
    {
        RotationObject = gameObjectTemp;
        startAngle = startAngleTemp;
    }
    // Update is called once per frame
    public void Update()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime > PassTime)
        {
            return;
        }
        float step = Time.deltaTime / PassTime;
        float angle = startAngle * step;
        RotationObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), angle);
    }
}
