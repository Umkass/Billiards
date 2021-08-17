using System.Collections.Generic;
using UnityEngine;

public class DrawBallLine : MonoBehaviour
{
    [SerializeField] List<Vector3> lineBallPoints;
    LineRenderer drawBallLine;
    BallController ballController;
    Vector3 directionDrawBallLine;
    GameObject Circle;
    int IgnoreCurrentLayer;
    float angleBetweenVectors;
    // Start is called before the first frame update
    void Start()
    {
        lineBallPoints = new List<Vector3>();
        drawBallLine = gameObject.GetComponent<LineRenderer>();
        ballController = gameObject.GetComponent<BallController>();
        Circle = GameObject.Find("Circle");
        IgnoreCurrentLayer = ~(1 << gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && ballController.Rigidbody.velocity == Vector3.zero)
        {
            lineBallPoints.Insert(0, gameObject.transform.position);
        }
        else if (Input.GetMouseButton(0) && ballController.Rigidbody.velocity == Vector3.zero)
        {
            directionDrawBallLine = new Vector3(-ballController.Delta.x, 0, -ballController.Delta.y);
            RayFromBall();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lineBallPoints.Clear();
        }
        else
        {
            Circle.transform.position = transform.position;
        }
        drawBallLine.positionCount = lineBallPoints.Count;
        drawBallLine.SetPositions(lineBallPoints.ToArray());
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began && ballController.Rigidbody.velocity == Vector3.zero)
            {
            lineBallPoints.Insert(0, gameObject.transform.position);
            }
            else if (myTouch.phase == TouchPhase.Stationary && ballController.Rigidbody.velocity == Vector3.zero)
            {
            directionDrawBallLine = new Vector3(-ballController.Delta.x, 0, -ballController.Delta.y);
            RayFromBall();
            }
            else if (myTouch.phase == TouchPhase.Ended)
            {
            lineBallPoints.Clear();
            }
           else
        {
            Circle.transform.position = transform.position;
        }
        drawBallLine.positionCount = lineBallPoints.Count;
        drawBallLine.SetPositions(lineBallPoints.ToArray());
        }
#endif
    }
    void RayFromBall()
    {
        RaycastHit hitsInfo;
        Physics.Raycast(gameObject.transform.position, directionDrawBallLine, out hitsInfo, directionDrawBallLine.magnitude, IgnoreCurrentLayer);
        Debug.DrawRay(transform.position, directionDrawBallLine, Color.red);
        if (hitsInfo.collider != null)
        {
            Vector3 CircleMoveDirection = hitsInfo.point - (directionDrawBallLine / 100).normalized / 2f;
            Circle.transform.position = CircleMoveDirection;
            if (lineBallPoints.Count < 2)
            {
                lineBallPoints.Insert(1, Circle.transform.position);
            }
            else
            {
                lineBallPoints.RemoveAt(1);
                lineBallPoints.Insert(1, Circle.transform.position);
            }
            if (hitsInfo.collider.CompareTag("Ball"))
            {
                //неудачная попытка расчетов траектории шаров после столкновения используя физические формулы задачи "абсолютно упругий удар в пространстве"
                Vector3 centerToCenter = hitsInfo.point - gameObject.transform.position;
                angleBetweenVectors = Vector3.Angle(directionDrawBallLine, centerToCenter);
                Debug.Log(angleBetweenVectors);
                float m1 = ballController.GetComponent<Rigidbody>().mass;
                float m2 = hitsInfo.collider.GetComponent<Rigidbody>().mass;
                Vector3 mainBallDirectionAftercollision = directionDrawBallLine / 100 * (Mathf.Sqrt(Mathf.Pow(m1, 2) + Mathf.Pow(m2, 2) + 2 * m1 * m2 * Mathf.Cos(angleBetweenVectors))) / (m1 + m2);
                Debug.Log(mainBallDirectionAftercollision);
                Vector3 BallDirectionAftercollision = directionDrawBallLine / 100 * 2 * m1 / (m1 + m2) * Mathf.Sin(angleBetweenVectors / 2);
                Debug.Log(BallDirectionAftercollision);
                if (BallDirectionAftercollision != Vector3.zero)
                {
                    if (lineBallPoints.Count < 3)
                    {
                        lineBallPoints.Insert(2, mainBallDirectionAftercollision);
                    }
                    else
                    {
                        lineBallPoints.RemoveAt(2);
                        lineBallPoints.Insert(2, mainBallDirectionAftercollision);
                    }
                    if (lineBallPoints.Count < 4)
                    {
                        lineBallPoints.Insert(3, lineBallPoints[1]);
                    }
                    else
                    {
                        lineBallPoints.RemoveAt(3);
                        lineBallPoints.Insert(3, lineBallPoints[1]);
                    }
                    if (lineBallPoints.Count < 5)
                    {
                        lineBallPoints.Insert(4, BallDirectionAftercollision);
                    }
                    else
                    {
                        lineBallPoints.RemoveAt(4);
                        lineBallPoints.Insert(4, BallDirectionAftercollision);
                    }
                }
            }
            if (hitsInfo.collider.CompareTag("Wall"))
            {
                Debug.Log(lineBallPoints.Count);
                if (lineBallPoints.Count > 4)
                {
                    lineBallPoints.RemoveAt(4);
                    lineBallPoints.RemoveAt(3);
                    lineBallPoints.RemoveAt(2);
                    return;
                }
                if (lineBallPoints.Count > 3)
                {
                    lineBallPoints.RemoveAt(3);
                    lineBallPoints.RemoveAt(2);
                    return;
                }
                if (lineBallPoints.Count > 2)
                {
                    lineBallPoints.RemoveAt(2);
                    return;
                }
            }
        }
    }
}
