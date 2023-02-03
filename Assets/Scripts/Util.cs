using UnityEngine;

internal class Util
{
    public static Quaternion RoundRotation(Quaternion rotation)
    {
        Vector3 eulerRotation = rotation.eulerAngles;
        eulerRotation = new Vector3(Mathf.Round(eulerRotation.x), Mathf.Round(eulerRotation.y), Mathf.Round(eulerRotation.z));
        return Quaternion.Euler(eulerRotation);
    }
}
