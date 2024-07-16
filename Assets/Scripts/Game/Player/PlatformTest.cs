using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTest : MonoBehaviour
{
    [SerializeField] int hp;

    void BouncePlayer()
    {
        Debug.Log(PlayerController.Instance.ACCStep);

        if (PlayerController.Instance.ACCStep >= hp)
        {
            if (!PlayerController.Instance.isDash)
                PlayerController.Instance.Bounce(hp);

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            BouncePlayer();
        }
    }
}
