using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchController : MonoBehaviour
{
    AudioSource bgm;

    void Awake()
    {
        bgm = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (PlayerController.Instance.isDash)
        {
            bgm.pitch = 1.5f;

        }
        else
        {
            bgm.pitch = 1f;
        }
    }

}
