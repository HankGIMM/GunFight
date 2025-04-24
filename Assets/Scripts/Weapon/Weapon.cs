using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform defaultPosition;
    public Transform adsPosition;
    public float adsSpeed = 10f;
    protected bool isAiming = false;
    public abstract void Shoot();

    public virtual void AimDownSights()
    {
        if (adsPosition != null)
        {
           transform.position = Vector3.Lerp(transform.position, adsPosition.position, Time.deltaTime * adsSpeed);
              transform.rotation = Quaternion.Lerp(transform.rotation, adsPosition.rotation, Time.deltaTime * adsSpeed);
        }
    }

    public virtual void ReturnToDefaultPosition()
    {
        if (defaultPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPosition.position, Time.deltaTime * adsSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultPosition.rotation, Time.deltaTime * adsSpeed);
        }
    }
}