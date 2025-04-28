using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        Debug.Log($"Initial Gun Rotation:  {initialRotation.eulerAngles}");
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        Debug.Log($"Target Gun Rotation:  {targetRotation.eulerAngles}");

        //rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
