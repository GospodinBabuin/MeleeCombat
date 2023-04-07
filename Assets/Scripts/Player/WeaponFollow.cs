using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform toFollow;

    private void Update()
    {
        transform.position = toFollow.position;
        transform.rotation = toFollow.rotation;
    }
}
