using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider slider;
    public Transform target; // Who is this bar floating above?
    public Vector3 offset = new Vector3(0, 2, 0); // How high above the head?

    void LateUpdate()
    {
        // 1. Follow the target
        if (target != null)
        {
            transform.position = target.position + offset;
        }

        // 2. Face the Camera
        // We make the rotation equal to the camera's rotation so it looks flat
        transform.rotation = Camera.main.transform.rotation;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.value = currentValue / maxValue;
        }
    }
}