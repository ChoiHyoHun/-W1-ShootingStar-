using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeField] float HorizontalSpeed;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rigid.AddForce(Vector2.right * horizontalInput * HorizontalSpeed, ForceMode2D.Impulse);

    }

}
