using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour {
    public float attackSpace = 0f;

    void OnCollisionEnter(Collision info)
    {
        if(info.collider.tag == "target")
        {
            attackSpace += Time.deltaTime;
            if (Input.GetButtonDown("Attack") && attackSpace < 2f)
            {
                info.collider.enabled = false;
            }
            attackSpace = 0f;
        }
        return;
    }
}