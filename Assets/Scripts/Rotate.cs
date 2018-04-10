using UnityEngine;

public class Rotate : MonoBehaviour {
    public Controller2D movement;

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.movement), 1f);
    }
}