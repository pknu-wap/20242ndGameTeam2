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
    public GameObject Icicle; //얼음 프리팹

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
            isInvincible = true;
            Phase = 2;
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
                yield return new WaitForSeconds(0.3f); // 0.3초 대기 후

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

        yield return new WaitForSeconds(1.1f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetBool("isSkill1", false); // 공격 애니메이션 종료

        isWaiting = false; // 대기 종료
        AttackNum++;
    }

    private IEnumerator Skill2()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(1.1f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetBool("isSkill2", false); // 공격 애니메이션 종료

        isWaiting = false; // 대기 종료
        StartCoroutine(IcicleRangeInstant());
    }


    private IEnumerator Resurrect()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(3.8f);

        nextDamageTime = Time.time + damageInterval;

        anim.SetTrigger("isResurrectFinish");
        isResurrect = false;
        isWaiting = false; // 대기 종료
        isInvincible = false;
    }

    private IEnumerator IcicleRangeInstant()
    {
        Debug.Log("성공");
        isWaiting = true; // 대기 시작
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnposition = Player.position;
            GameObject instantIcicle = Instantiate(Icicle, spawnposition, Quaternion.Euler(0, 0, 90));
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
            if (Enemy_MeleeAttack_Judgment.isAttackSusses)
            {
                if (GameManager.Instance != null)
                {

                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");

                }
            }
        }


    }

}
