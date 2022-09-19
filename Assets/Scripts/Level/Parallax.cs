using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float repeatInterval;
    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private Transform previousObj;

    private float prevFrameCameraPos = 0.0f;
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        prevFrameCameraPos = mainCamera.position.x;
    }

    private void FixedUpdate()
    {
        this.transform.Translate((mainCamera.position.x - prevFrameCameraPos) * moveSpeed * Time.fixedDeltaTime * Vector3.left);

        if (this.transform.localPosition.x <= -repeatInterval)
            ResetPos();

        prevFrameCameraPos = mainCamera.position.x;
    }

    private void ResetPos()
    {
        this.transform.localPosition = new Vector3(previousObj.localPosition.x + repeatInterval, this.transform.localPosition.y, this.transform.localPosition.z);
    }
}
