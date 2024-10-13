using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody ������ Ŭ���� �������� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
            return; 
        }

        // ���� �߰�
        Vector3 force = new Vector3(0, 10, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }

}