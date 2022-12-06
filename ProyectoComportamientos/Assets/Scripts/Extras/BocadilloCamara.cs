using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocadilloCamara : MonoBehaviour
{
    private Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        Destroy(this.transform.GetChild(0).gameObject, 5);
        Destroy(this.gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(camera);
    }
}
