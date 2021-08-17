using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    List<Vector3> linePoints;
    LineRenderer drawPowerLine;
    void Start()
    {
        linePoints = new List<Vector3>();
        drawPowerLine = gameObject.GetComponent<LineRenderer>();
        drawPowerLine.startWidth = 0.05f;
        drawPowerLine.endWidth = 0.25f;
    }
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            linePoints.Insert(0, GetMousePosition());
        }
        if (Input.GetMouseButton(0))
        {
            if (linePoints.Count < 2)
                linePoints.Insert(1, GetMousePosition());
            else
            {
                linePoints.RemoveAt(1);
                linePoints.Insert(1, GetMousePosition());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            linePoints.Clear();
        }
        drawPowerLine.positionCount = linePoints.Count;
        drawPowerLine.SetPositions(linePoints.ToArray());
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began)
            {
                linePoints.Insert(0, GetMousePosition());
            }
            if (myTouch.phase == TouchPhase.Stationary)
            {
                if (linePoints.Count < 2)
                    linePoints.Insert(1, GetMousePosition());
                else
                {
                    linePoints.RemoveAt(1);
                    linePoints.Insert(1, GetMousePosition());
                }
            }
            if (myTouch.phase == TouchPhase.Ended)
            {
                linePoints.Clear();
            }
            drawPowerLine.positionCount = linePoints.Count;
            drawPowerLine.SetPositions(linePoints.ToArray());
        }
#endif
    }
    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ray.origin =  new Vector3(ray.origin.x, 20f,ray.origin.z);
        return ray.origin + ray.direction * 10f;
    }
    }
