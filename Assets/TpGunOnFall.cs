using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpGunOnFall : MonoBehaviour
{
    public Transform initialGunTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall"))
        {
            transform.position = initialGunTransform.position;
            transform.rotation = initialGunTransform.rotation;
        }
    }
}
