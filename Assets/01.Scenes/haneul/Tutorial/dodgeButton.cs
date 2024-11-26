using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollButtonCooldown : MonoBehaviour
{
    public Button rollButton;            // 구르기 버튼
    private TextMeshProUGUI cooldownText; // 버튼 내 자식 TMP 텍스트
    public float cooldownTime = 5f;      // 쿨타임 시간
    private float currentCooldown;       // 현재 쿨타임 시간
    private bool isCooldown = false;     // 쿨타임 상태인지 여부

    public PlayerMovement playerMovement; // PlayerMovement 스크립트 참조 추가

    private void Start()
    {
        cooldownText = rollButton.GetComponentInChildren<TextMeshProUGUI>(); // 자식 오브젝트에서 TMP 텍스트 찾기
        currentCooldown = cooldownTime;  // 쿨타임 초기화
        cooldownText.text = "";          // 초기에는 텍스트 비움

        rollButton.onClick.AddListener(OnRollButtonClick); // 버튼 클릭 리스너 추가
    }

    // 버튼 클릭 시 호출되는 메서드
    public void OnRollButtonClick()
    {
        if (!isCooldown)
        {
            StartCoroutine(StartCooldown());  // 쿨타임 시작
            playerMovement.Roll();             // Roll() 메서드 호출
        }
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;  // 쿨타임 시작 상태

        // 쿨타임 동안 텍스트 업데이트
        while (currentCooldown > 0)
        {
            cooldownText.text = (currentCooldown).ToString("F1");  // 소수 첫째 자리까지 표시
            currentCooldown -= 0.1f;  // 쿨타임 감소
            yield return new WaitForSeconds(0.1f);  // 0.1초마다 업데이트
        }

        // 쿨타임이 끝난 후
        cooldownText.text = "";  // 텍스트 초기화
        currentCooldown = cooldownTime;  // 쿨타임 리셋
        isCooldown = false;  // 쿨타임 종료
    }
}
