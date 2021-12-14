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
    {//enum���� ������Ʈ�� �̸��� �����ϰ� ���ȭ.
        Idle, Move, Attack, Return, Damaged, Die
    }
    EnemyState m_State;//enum�� ������ ��������

    public float findDistance = 8f;//�̵����·� �ٲ� Ž���Ÿ�
    Transform playerTransform;//�÷��̾��� Ʈ�������� ���� ����

    public float attackDistance = 5f;//���ݻ��·� ��ȯ�Ǿ� ������ ������ �Ÿ�
    public float moveSpeed = 5f;//�̵��ӵ� ����
    CharacterController cc; //ĳ���� ����ѷ� ������Ʈ�� �����ϱ����� ��������

    float currentTime = 0; //�����Ӹ��� ������ų ���� �ð�
    public float attackDelayTime = 2f; //������ �ð��� �����ؾߵ� �ð�
    public int attackPower = 3;//���� ���ݷ� �ܺο��� ���� ���� ����

    Vector3 originPosition;//�ʱ���ġ�� ������ ���� ����
    public float moveDistance = 20f;//�󸶳� �����ϴٰ� ���ƿ���
    void Start()
    {
        m_State = EnemyState.Idle;//���� �ʱ�ȭ
        playerTransform = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        //ó������ ��Ÿ���� ������ ����. �ٷ� ���ݰ����ϰ� ����
        currentTime = attackDelayTime;
        originPosition = transform.position;//�ʱ� ��ġ ����
//���ӿ�����Ʈ�� ���� ����� Find()�Լ��� �̿��Ͽ� Player��ü�� ã�� tranform������Ʈ�� �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {//�̵��� �ϵ� ��⸦ �ϵ� ���� ��Ÿ���� ��� ���� �־�� �Ǵ°� �´�.

        print("m_State : " + m_State);
        currentTime += Time.deltaTime;
        //���� �����¿����� �����Ӹ��� �Լ��� ȣ���Ѵ�.
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

    // �� �������� �ð�ȭ ��Ű�� �Լ�
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
    {//Vector3Ŭ������ �����ϴ� Distance()�Լ��� �̿��� ��ǥ�� �Ÿ��� �����Ŀ� 
    //Ž���Ÿ����� �۴ٸ� ���¸� ��ȯ�ϴ� �Լ� ����.
        if(Vector3.Distance(transform.position, playerTransform.position) < findDistance)
        {
            m_State = EnemyState.Move;  
        }
    }
    void Move()
    {   //���ʹ��� ��ġ�� �������� ��ġ�� ���ؼ� �̵��Ÿ����� Ŀ������
        if (Vector3.Distance(transform.position, originPosition) > moveDistance)
        {
            m_State = EnemyState.Return;//���ϻ��� ��ȯ
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {//�ѻ����� �Ÿ��� ���ݻ�Ÿ� �ȿ� ������ ������ȯ
            m_State = EnemyState.Attack;
        }
        else//�׷��� ������
        {//(a������ǥ - b������ǥ = b�� a�� ���ϱ� ���� ����).nomalized
         //<- ������ ���Ⱚ�� �򵵷� �������ͷ� ����ȭ
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //ĳ���� ��Ʈ�ѷ��� �����Լ�(�ӵ�*�ð�)���� �̵� ����
        }
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {//���ݻ��·� ��ȯ�� ���¿��� ���ݻ�Ÿ��ȿ� ������ ������ ���;� �Ѵ�. ����ȣ���Ÿ����� ��Լ����Ͽ� ������ ��������.
         //�����Ҽ� �ִ� �������� �����ѵڷ� ���� �󸶳� �ð��� �귶���� �˻�   
            if (currentTime > attackDelayTime)
            {
                playerTransform.GetComponent<PlayerMove>().DamageAction(attackPower);
                Debug.Log("����");
                //�����ϰ� ���� �����ð� 0����
                currentTime = 0;
            }
        }
        else
        {
            m_State = EnemyState.Move;
            Debug.Log("���߰�");
        }
    }
    void Return()
    {   //���ʹ��� ��ġ�� ���������� �����Ѱ� �ƴ� �̻� ��� �̵��Ѵ�.
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
    {//�ǰ��Լ��� �ڷ�ƾ�� ȣ���մϴ�.
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()//�ڷ�ƾ ������
    {//0.5�ʵ��� ������ �ǰ� �ִϸ��̼��� ��������.
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;//������ �԰� ������ ������
    }
    public void HitEnemy(int hitPower)
    {//���� ������ ȣ��� �Լ� ����

        if(m_State == EnemyState.Die || m_State == EnemyState.Damaged || m_State == EnemyState.Return)
        {//��Ʈ���ʹ̸� �����ϴ��� �Ǿտ��� ���°� ����� ���� ��Ʈ���ʹ̰� ������� �ȵ����� ���Ѵ�.
            return;
        }
        hp -= hitPower;//hp�� ���̰���
        if (hp > 0)//���� ����ִٸ�
        {//�ǰݻ��·� �ٲٰ� �ǰ��Լ��� ȣ��
            m_State = EnemyState.Damaged;
            Damaged();
        }
        else
        {//����
            m_State = EnemyState.Die;
            Die();
        }
    }
    void Die()//ü���� ������ ������ �̸��� �ϴ� �Լ��� �ǰݿ��� �ٷ����.
    {//�������� �ڷ�ƾ�� �����ǰ� �����
        StopAllCoroutines();
        StartCoroutine(DieProcess());
        //�������μ��� �ڷ�ƾ ����
    }
    IEnumerator DieProcess()
    {//�ڷ�ƾ ���ǹ�� IEnumerator �ڷ�ƾ�̸�(�Ű�����)
        cc.enabled = false;//ĳ������Ʈ�ѷ� ���¹�, �浹ó���� �̷��� ����
        yield return new WaitForSeconds(2f);//2���Ŀ�
        print("�Ҹ�");
        Destroy(gameObject);//����

    }
}
