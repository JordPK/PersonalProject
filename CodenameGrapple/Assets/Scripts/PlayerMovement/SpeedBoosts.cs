using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoosts : MonoBehaviour
{
    public Rigidbody rb;
    public float boostSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            rb.AddRelativeForce(Vector3.forward * boostSpeed * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
