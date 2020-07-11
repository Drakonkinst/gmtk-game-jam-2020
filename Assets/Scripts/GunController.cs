using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Transform playerTransform;
    private Transform myTransform;
    private Vector3 offset = new Vector3(0.0f, 1.5f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = SceneManager.Instance.playerTransform;
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    private void SetPosition() {
        transform.position = playerTransform.position + offset;
    }
}
