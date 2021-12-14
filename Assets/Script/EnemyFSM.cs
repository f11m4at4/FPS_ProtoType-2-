using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    public int hp = 15;
    public int maxHp = 15;
    public Slider hpBar;
    enum EnemyState
    {//enum으로 스태이트의 이름을 구별하고 상수화.
        Idle, Move, Attack, Return, Damaged, Die
    }
    EnemyState m_State;//enum을 저장할 변수선언

    public float findDistance = 8f;//이동상태로 바뀔 탐지거리
    Transform playerTransform;//플레이어의 트랜스폼을 받을 공간

    public float attackDistance = 5f;//공격상태로 전환되어 공격이 나가는 거리
    public float moveSpeed = 5f;//이동속도 변수
    CharacterController cc; //캐릭터 컨토롤러 컴포넌트를 조작하기위한 변수공간

    float currentTime = 0; //프레임마다 누적시킬 현재 시간
    public float attackDelayTime = 2f; //누적된 시간이 도달해야될 시간
    public int attackPower = 3;//적의 공격력 외부에서 쉽게 변경 가능

    Vector3 originPosition;//초기위치를 저장할 변수 선언
    public float moveDistance = 20f;//얼마나 진행하다가 돌아올지
    void Start()
    {
        m_State = EnemyState.Idle;//대기로 초기화
        playerTransform = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        //처음부터 쿨타임일 이유는 없다. 바로 공격가능하게 세팅
        currentTime = attackDelayTime;
        originPosition = transform.position;//초기 위치 저장
//게임오브젝트가 가진 기능중 Find()함수를 이용하여 Player객체를 찾고 tranform컴포넌트를 할당
    }

    // Update is called once per frame
    void Update()
    {//이동을 하든 대기를 하든 공격 쿨타임은 계속 돌고 있어야 되는게 맞다.

        print("m_State : " + m_State);
        currentTime += Time.deltaTime;
        //현재 적상태에따라 프레임마다 함수를 호출한다.
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
        hpBar.value = (float)hp / (float)maxHp;
    }

    // 각 범위값을 시각화 시키는 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveDistance);

    }

    void Idle()
    {//Vector3클래스가 지원하는 Distance()함수를 이용해 좌표간 거리를 측정후에 
    //탐지거리보다 작다면 상태를 전환하는 함수 구현.
        if(Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;  
        }
    }
    void Move()
    {   //에너미의 위치와 스폰지역 위치를 비교해서 이동거리보다 커졌으면
        if (Vector3.Distance(transform.position, originPosition) > moveDistance)
        {
            m_State = EnemyState.Return;//리턴상태 전환
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {//둘사이의 거리가 공격사거리 안에 들어오면 상태전환
            m_State = EnemyState.Attack;
        }
        else//그렇지 않으면
        {//(a벡터좌표 - b벡터좌표 = b가 a를 향하기 위한 벡터).nomalized
         //<- 벡터의 방향값만 얻도록 단위벡터로 정규화
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //캐릭터 컨트롤러의 무브함수(속도*시간)으로 이동 구현
        }
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {//공격상태로 전환된 상태에서 공격사거리안에 있으면 공격이 나와야 한다. 공격호출사거리보다 길게설정하여 공격이 나가게함.
         //공격할수 있는 상태인지 공격한뒤로 부터 얼마나 시간이 흘렀는지 검사   
            if (currentTime > attackDelayTime)
            {
                playerTransform.GetComponent<PlayerMove>().DamageAction(attackPower);
                Debug.Log("공격");
                //공격하고 나면 누적시간 0으로
                currentTime = 0;
            }
        }
        else
        {
            m_State = EnemyState.Move;
            Debug.Log("재추격");
        }
    }
    void Return()
    {   //에너미의 위치가 스폰지역에 근접한게 아닌 이상에 계속 이동한다.
        if(Vector3.Distance(transform.position, originPosition) > 0.1f)
        {
            Vector3 dir = (originPosition - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = originPosition;
            hp = 15;
            m_State = EnemyState.Idle;
        }
    }
    void Damaged()
    {//피격함수는 코루틴을 호출합니다.
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()//코루틴 구현부
    {//0.5초동안 대기시켜 피격 애니메이션이 나오게함.
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;//데미지 입고 나서는 움직임
    }
    public void HitEnemy(int hitPower)
    {//적을 때리면 호출될 함수 정의

        if(m_State == EnemyState.Die || m_State == EnemyState.Damaged || m_State == EnemyState.Return)
        {//히트에너미를 실행하더라도 맨앞에서 상태가 어떤지에 따라 히트에너미가 실행될지 안될지를 정한다.
            return;
        }
        hp -= hitPower;//hp가 깍이고나서
        if (hp > 0)//아직 살아있다면
        {//피격상태로 바꾸고 피격함수를 호출
            m_State = EnemyState.Damaged;
            Damaged();
        }
        else
        {//죽음
            m_State = EnemyState.Die;
            Die();
        }
    }
    void Die()//체력이 닳으면 죽음에 이르게 하는 함수는 피격에서 다루었다.
    {//진행중인 코루틴이 중지되게 만들고
        StopAllCoroutines();
        StartCoroutine(DieProcess());
        //다이프로세스 코루틴 시작
    }
    IEnumerator DieProcess()
    {//코루틴 정의방법 IEnumerator 코루틴이름(매개변수)
        cc.enabled = false;//캐릭터컨트롤러 끄는법, 충돌처리나 이런거 종료
        yield return new WaitForSeconds(2f);//2초후에
        print("소멸");
        Destroy(gameObject);//삭제

    }
}
