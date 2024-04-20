using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    Vector3 rotation =Vector3.zero;
    public float degreesPerSec = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation.x=degreesPerSec * Time.deltaTime;
        transform.Rotate(rotation, Space.World);
    }
}
