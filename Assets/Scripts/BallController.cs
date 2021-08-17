using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 delta = Vector3.zero;
    Vector3 firstPos = Vector3.zero;

    public Rigidbody Rigidbody { get => rigidbody; private set => rigidbody = value; }
    public Vector3 Delta { get => delta; private set => delta = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rigidbody.velocity != Vector3.zero)
        {
            GameManager.Instance.ShowMessage(true);
        }
        else
        {
            GameManager.Instance.ShowMessage(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && rigidbody.velocity == Vector3.zero)
        {
            firstPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0) && rigidbody.velocity == Vector3.zero)
        {
            delta = Input.mousePosition - firstPos;
        }
        if (Input.GetMouseButtonUp(0) && rigidbody.velocity == Vector3.zero)
        {
            Vector3 force = new Vector3(-delta.x, 0, -delta.y);
            if (!Input.GetMouseButton(1)) // если нажать пкм - отменить удар
                MoveBall(force/10);
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began && rigidbody.velocity == Vector3.zero)
            {
               firstPos = Input.mousePosition;
            }
            if (myTouch.phase == TouchPhase.Stationary && rigidbody.velocity == Vector3.zero)
            {
              delta = Input.mousePosition - firstPos;
            }
            if (myTouch.phase == TouchPhase.Ended)
            {
                Vector3 force = new Vector3(-delta.x, 0, -delta.y);
            if (!Input.GetMouseButton(1)) // если нажать пкм - отменить удар
                MoveBall(force/10);
            }
        }
#endif
    }

    void MoveBall(Vector3 force)
    {
        rigidbody.AddForce(force, ForceMode.Impulse);
    }
}
