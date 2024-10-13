using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody 변수를 클래스 레벨에서 선언

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
            return; 
        }

        // 힘을 추가
        Vector3 force = new Vector3(0, 10, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }

}