using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage1_Boss_Addanimation : BoseEnemy
{
    public int resurrectionHealth;//부활체력(2페이지 돌입)
    public float speed;
    public Rigidbody2D player;
    public float stopDistance;
    public int damageAmount = 1;
    public float damageInterval = 1.1f; // 공격 간격
    private float nextDamageTime;
    public int AttackNum = 1;
    public float distance;
    public int Phase = 1;
    public Transform Player;
    public GameObject Hand;
    public GameObject fireball;
    public GameObject Bomb1;
    public GameObject Bomb2;

    [SerializeField] private Slider healthSlider; // 체력 슬라이더

    bool isLive = true;
    bool isWaiting = false; // 대기 중인지 확인하는 변수
    bool isResurrect = false;
    bool isDie = false;



    Animator anim; // 애니메이터 변수

    Rigidbody2D enemy;

    protected override void Awake()
    {
        base.Awake();
        resurrectionHealth = (int)(maxHealth / 2);
        enemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        damageMultiplier = 1.0f;
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isRun", false);
        anim.SetBool("isRunAttack", false);
        anim.SetBool("isSkill1", false);
        anim.SetBool("isSkill2", false);

        healthSlider.maxValue = maxHealth; // 슬라이더 최대값 설정
        healthSlider.value = currentHealth; // 초기 슬라이더 값 설정


    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDie)
        {
            isDie = true;
            anim.SetTrigger("isDie");
            StartCoroutine(BossDie());


        }
        if (currentHealth <= resurrectionHealth && Phase != 2 && !isResurrect)
        {
            Phase = 2;
            isInvincible = true;
            isResurrect = true;
            anim.SetTrigger("isResurrect");
            AttackNum = 3;

            StartCoroutine(Resurrect());
        }
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = player.position - enemy.position;
        distance = dirVec.magnitude;

        if (distance > 8.5 && !isWaiting && !isResurrect)
        {
            if (Phase == 2)
            {
                if (AttackNum++ % 2 == 0)
                {
                    anim.SetBool("isRun", true);
                    if (!isWaiting)
                    {
                        StartCoroutine(RunAttack());
                    }
                }
                else
                {
                    anim.SetBool("isSkill2", true);
                    if (!isWaiting)
                    {
                        StartCoroutine(Skill2());
                    }
                }
            }
            else
            {
                anim.SetBool("isRun", true);
                if (!isWaiting)
                {
                    StartCoroutine(RunAttack());
                }
            }
        }

        else if (distance > stopDistance && !isResurrect)
        {
            anim.SetBool("isWalk", true);
        }

        if (dirVec.x < 0)  // 플레이어가 왼쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, 0, 0);  // 적을 왼쪽으로 회전
        }
        else if (dirVec.x > 0)  // 플레이어가 오른쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, -180, 0);  // 적을 오른쪽으로 회전
        }

        if (distance > stopDistance && anim.GetBool("isWalk") && !anim.GetBool("isAttack1") && !anim.GetBool("isAttack2") && !anim.GetBool("isSkill1") && !isResurrect && !anim.GetBool("isSkill2"))
        {

            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else
        {
            anim.SetBool("isWalk", false); // 걷기 애니메이션 종료

            if (Phase == 2)
            {
                switch (AttackNum % 3)
                {

                    case 0:
                        if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
                        {
                            anim.SetBool("isSkill1", true); // 공격 애니메이션 시작
                            StartCoroutine(Skill1());
                        }
                        break;

                    case 1:
                        if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
                        {
                            anim.SetBool("isAttack1", true); // 공격 애니메이션 시작
                            StartCoroutine(Attack1());
                        }
                        break;
                    case 2:
                        if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
                        {
                            anim.SetBool("isAttack2", true); // 공격 애니메이션 시작
                            StartCoroutine(Attack2());
                        }
                        break;

                }
            }
            else
            {
                switch (AttackNum % 2)
                {

                    case 0:
                        if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
                        {
                            anim.SetBool("isAttack1", true); // 공격 애니메이션 시작
                            StartCoroutine(Attack1());
                        }
                        break;
                    case 1:
                        if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
                        {
                            anim.SetBool("isAttack2", true); // 공격 애니메이션 시작
                            StartCoroutine(Attack2());
                        }
                        break;

                }
            }

        }

        enemy.velocity = Vector2.zero;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealth();
    }


    private IEnumerator Attack1()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(0.8f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetBool("isAttack1", false); // 공격 애니메이션 종료

        isWaiting = false; // 대기 종료
        AttackNum++;
    }
    private IEnumerator Attack2()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(0.7f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetBool("isAttack2", false); // 공격 애니메이션 종료

        isWaiting = false; // 대기 종료
        AttackNum++;
    }

    private IEnumerator RunAttack()
    {
        isWaiting = true; // 대기 시작
        speed = 4f;

        while (true)
        {
            // 공격 애니메이션이 실행 중일 때
            if (distance <= stopDistance)
            {
                anim.SetBool("isRunAttack", true);
                yield return new WaitForSeconds(0.55f); // 0.3초 대기 후

                nextDamageTime = Time.time + damageInterval;

                anim.SetBool("isAttack2", false); // 공격 애니메이션 종료

                isWaiting = false; // 대기 종료
                speed = 2.4f;
                anim.SetBool("isRunAttack", false); // 공격 중 상태 해제
                anim.SetBool("isRun", false); // 달리기 애니메이션 시작

                break; // 루프 종료
            }

            yield return null; // 매 프레임 대기 (다음 프레임까지 대기)
        }
    }

    private IEnumerator Skill1()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(1.5f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetBool("isSkill1", false); // 공격 애니메이션 종료

        isWaiting = false; // 대기 종료
        AttackNum++;
    }

    private IEnumerator Skill2()
    {
        isWaiting = true; // 대기 시작

        // 스킬 실행 대기
        yield return new WaitForSeconds(0.9f);

        // 플레이어와 몬스터 위치 교체
        Vector3 playerPosition = Player.position;
        Vector3 bossPosition = transform.position;

        Player.position = bossPosition; // 플레이어를 보스 위치로 이동
        transform.position = playerPosition; // 보스를 플레이어 위치로 이동

        yield return new WaitForSeconds(0.5f); // 위치 교체 후 잠시 대기

        // 폭탄 생성
        int bombCount = 7; // 생성할 폭탄 개수
        for (int i = 1; i <= bombCount; i++)
        {
            // 플레이어와 보스 사이 거리와 방향 계산
            Vector3 currentPlayerPosition = Player.position;
            Vector3 currentBossPosition = transform.position;

            // 플레이어와 보스 사이 방향 계산
            Vector3 direction = (currentPlayerPosition - currentBossPosition).normalized;

            // 일정 비율로 나눈 위치 계산
            float ratio = (float)i / (bombCount + 1); // 1/(5+1), 2/(5+1), ..., 5/(5+1)로 비율 생성
            Vector3 spawnPosition = currentBossPosition + direction * Vector3.Distance(currentBossPosition, currentPlayerPosition) * ratio;

            // 폭탄 랜덤 선택 (Bomb1 또는 Bomb2)
            GameObject selectedBomb = Random.value > 0.5f ? Bomb1 : Bomb2;

            // 폭탄 생성
            Instantiate(selectedBomb, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.5f); // 폭탄 생성 간격 대기
        }

        // 스킬 종료 처리
        nextDamageTime = Time.time + damageInterval;
        anim.SetBool("isSkill2", false); // 스킬 애니메이션 종료
        isWaiting = false; // 대기 종료
    }




    private IEnumerator Resurrect()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(4.5f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetTrigger("isReSurrectFinish");
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
        anim.SetBool("isRun", false);
        anim.SetBool("isRunAttack", false);
        anim.SetBool("isSkill1", false);
        anim.SetBool("isSkill2", false);
        isResurrect = false;
        isWaiting = false; // 대기 종료
        isInvincible = false;

        FireballShoot();
    }

    private void FireballShoot()
    {
        int numberOfFireballs = 18; // 발사할 파이어볼 개수
        float angleStep = 360f / numberOfFireballs; // 각도 간격
        float currentAngle = 0f;

        for (int i = 0; i < numberOfFireballs; i++)
        {
            // 각도 계산
            float fireballDirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float fireballDirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);

            Vector2 fireballDirection = new Vector2(fireballDirX, fireballDirY).normalized;

            // 파이어볼 생성 (기본 방향 보정 포함)
            Quaternion fireballRotation = Quaternion.Euler(0f, 0f, currentAngle - 180f); // 기본 방향 보정을 위해 -90도 추가
            GameObject fireballInstance = Instantiate(fireball, transform.position, fireballRotation);

            // 파이어볼에 힘을 가해 날리기
            Rigidbody2D fireballRb = fireballInstance.GetComponent<Rigidbody2D>();
            if (fireballRb != null)
            {
                float fireballSpeed = 5f; // 파이어볼 속도 설정
                fireballRb.velocity = fireballDirection * fireballSpeed;
            }

            // 다음 각도로 업데이트
            currentAngle += angleStep;
        }
    }



    private IEnumerator HandInstant()
    {
        Debug.Log("성공");
        isWaiting = true; // 대기 시작
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnposition = Player.position;
            GameObject instantHand = Instantiate(Hand, spawnposition, Quaternion.Euler(0, 0, 0));
            yield return new WaitForSeconds(1f);
        }

        nextDamageTime = Time.time + damageInterval;
        isWaiting = false; // 대기 종료

    }

    private IEnumerator BossDie()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(0.8f);

        nextDamageTime = Time.time + damageInterval;
        isWaiting = false; // 대기 종료
        Destroy(gameObject);
        base.Die();
    }

    // 체력 업데이트 메서드
    public void UpdateHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 현재 체력 제한
        healthSlider.value = currentHealth; // 슬라이더 업데이트
    }


    private void TakeDamageToPlayer()
    {
        if (anim.GetBool("isSkill1"))
        {
            if (Stage0_Boss_Judgment.isSkillSusses)
            {
                if (GameManager.Instance != null)
                {

                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");

                }
            }
        }
        else
        {
            if (anim.GetBool("isAttack1") && S0_Boss_Attack1Range.isAttackSusses1)
            {
                if (GameManager.Instance != null)
                {

                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");

                }
            }
            else if (anim.GetBool("isAttack2") && S0_Boss_Attack2Range.isAttackSusses2)
            {
                if (GameManager.Instance != null)
                {

                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");

                }
            }
            else if (anim.GetBool("isRunAttack") && S0_Boss_Attack2Range.isAttackSusses2)
            {
                if (GameManager.Instance != null)
                {

                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");

                }
            }
        }


    }

}
