using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeDuration = 0.0f;
    [SerializeField]
    private float baseShakeDuration = 0.1f;
    [SerializeField]
    private float baseShakeMagnitude = 0.3f;
    [SerializeField]
    private float baseDampingSpeed = 1.0f;

    private float shakeMagnitude;
    private float dampingSpeed;
    private Vector3 initialPosition;
    private const float MAX_SHAKE_DURATION = 0.5f;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
        dampingSpeed = baseDampingSpeed;
        shakeMagnitude = baseShakeMagnitude;

        RoomEdge.OnRoomComplete += TriggerShake;
    }

    private void OnDisable()
    {
        RoomEdge.OnRoomComplete -= TriggerShake;
    }

    // Update is called once per frame
    void Update()
    {
        // Only run shake when time is enabled
        if(Time.timeScale != 0.0f)
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0.0f;
                transform.localPosition = initialPosition;
                shakeMagnitude = baseShakeMagnitude;
                dampingSpeed = baseDampingSpeed;
            }
        }
    }

    // Starts camera shake
    public void TriggerShake(Room.RoomType ignore) { TriggerShake(baseShakeDuration); }

    // Starts camera shake
    public void TriggerShake(float duration)
    {
        SetShakeDuration(shakeDuration + duration);
    }

    // Starts camera shake
    public void TriggerShake(float duration, float magnitude)
    {
        SetShakeDuration(shakeDuration + duration);
        shakeMagnitude = Mathf.Max(magnitude, shakeMagnitude);
    }

    // Starts camera shake
    public void TriggerShake(float duration, float magnitude, float damping)
    {
        SetShakeDuration(shakeDuration + duration);
        shakeMagnitude = Mathf.Max(magnitude, shakeMagnitude);
        dampingSpeed = damping;
    }

    private void SetShakeDuration(float duration)
    {
        shakeDuration = Mathf.Clamp(duration, 0.0f, MAX_SHAKE_DURATION);
    }
}
