using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Torch_Arrive: MonoBehaviour
{
    public static int Point_Number = 0;
    public GameObject Torch1;
    public GameObject Torch2;
    public GameObject Torch3;
    public GameObject Torch4;
    public GameObject Torch5;
    public GameObject Torch6;
    public GameObject Torch7;
    public GameObject Wall;
    public GameObject Instance;
    public Light2D Light;
    public Light2D playerLight;
    public ParticleSystem collisionParticles;
    public GameObject DodgeButton;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Point_Number++;
            collisionParticles.Play();
            switch (Point_Number)
            {
                case 1:
                    Torch1.SetActive(false);
                    Torch2.SetActive(true);
                    break;
                case 2:
                    Torch2.SetActive(false);
                    Torch3.SetActive(true);
                    break;
                case 3:
                    Torch3.SetActive(false);
                    Torch4.SetActive(true);
                    break;
                case 4:
                    Torch4.SetActive(false);
                    Torch5.SetActive(true);
                    break;
                case 5:
                    Torch5.SetActive(false);
                    Torch6.SetActive(true);
                    
                    break;
                case 6:
                    Torch6.SetActive(false);
                    Torch7.SetActive(true);
                    Wall.SetActive(false);
                    break;
                case 7:
                    Torch7.SetActive(false);
                    Wall.SetActive (true);
                    Instance.SetActive(true);
                    DodgeButton.SetActive(true);
                    if (Light != null)
                    {
                        Light.intensity = 18f;  // 밝기 설정 (값을 원하는 만큼 조절 가능)
                        
                    }
                    if (playerLight != null)
                    {
                        playerLight.enabled = false;  // 라이트 끄기
                    }
                    break;
            }
        }
    }


}
