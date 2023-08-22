using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    float angle;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        float mouseX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x - 0.5f;

        angle += mouseX * 100f * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
