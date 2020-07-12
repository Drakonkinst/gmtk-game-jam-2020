using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventFly : MonoBehaviour
{
    public float speed = 10;

    private TextMeshProUGUI textMesh;
    private RectTransform rectTrans;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTrans.Translate(Vector3.right * 5 * speed);
    }
}
