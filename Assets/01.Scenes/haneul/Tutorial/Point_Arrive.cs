using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Arrive : MonoBehaviour
{
    public static int Point_Number = 0;
    public GameObject Point1;
    public GameObject Point2;
    public GameObject Point3;
    public GameObject Point4;
    public GameObject Point5;
    public GameObject Point6;
    public GameObject Msg;
    public GameObject Wall;
    public GameObject Instance;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Point_Number++;
            switch (Point_Number)
            {
                case 1:
                    Point1.SetActive(false);
                    Point2.SetActive(true);
                    break;
                case 2:
                    Point2.SetActive(false);
                    Point3.SetActive(true);
                    break;
                case 3:
                    Point3.SetActive(false);
                    Point4.SetActive(true);
                    break;
                case 4:
                    Point4.SetActive(false);
                    Point5.SetActive(true);
                    Msg.SetActive(true);
                    break;
                case 5:
                    Point5.SetActive(false);
                    Point6.SetActive(true);
                    Wall.SetActive(false);
                    break;
                case 6:
                    Point6.SetActive(false);
                    Wall.SetActive(true);
                    Instance.SetActive(true);
                    break;
            }
        }
    }


}
