using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Vector2 cameraOffset;

    void LateUpdate()
    {
        transform.position = new Vector3(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y, transform.position.z) + (Vector3)cameraOffset;
    }
}
