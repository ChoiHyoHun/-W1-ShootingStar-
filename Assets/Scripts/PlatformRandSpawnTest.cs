using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRandSpawnTest : MonoBehaviour
{
    public GameObject[] platforms;

    private void OnEnable()
    {
        for(int i =0; i<platforms.Length; i++)
        {
            if(Random.Range(0,4)==0)
            {
                platforms[i].SetActive(true);
            }
            else
            {
                platforms[i].SetActive(false);
            }
        }
    }

}
