using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public int hp = 30;
    public int maxHp = 30;
    public Slider hpBar;//�����̴� UI�� ���� ����
 //�Žð����� �Ʒ������ϴ��߷���  //y�� �̵��Ÿ������� ������ ����
    float gravity = -20f,         yVelocity = 0;

    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    public bool isJumping = false;
    //ĳ������Ʈ�ѷ��� ���� �� �ִ� �������������
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {//ĳ������Ʈ�ѷ� ������Ʈ�� ��������
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {//Input Manager�� ��ϵ� Ű���忡 ���� �Է°��� �޴´�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;//�ٴڿ� ������� �߷��ʱ�ȭ
            if (isJumping)//������ �ߴ� ���¶��
            {
                isJumping = false;//�ٽ� �����ϰ� ���� �ʴٰ� �ʱ�ȭ
            }
        }
        if (Input.GetButtonDown("Jump") && !(isJumping))
        {
            yVelocity = jumpPower;//�����ӷ°��� �����Ŀ��� �ʱ�ȭ
            isJumping = true;
        }
        //������ å���� dir ���� �ڷḦ �����Ѵ�. ���� �ö󰡴� y���� 0
        //wŰ�� ������ v =1 dŰ�� ������ h =1 �̵Ǵµ� ���ÿ� ������
        //�Ѵ� 1�� ���̴�. �̰��� ������. ������ ���� �����밢���� 1.4�� ũ�⸦ ���� ���̱� �����̴�.
        //�׷��� dir.normalized�� ����ȭ ���ش�. ���ÿ� ������ �밢�� ũ�⸦ 1�� �ǰ� ������ش�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        //�߷ºθ� �߰�.. ���� �̻��ϱ��ص� �ϴ� �Ѿ��.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //������ �������� ���������ϴ� ����� �޸� ĳ������Ʈ�ѷ��� ����� Ȱ��
        cc.Move(dir * moveSpeed * Time.deltaTime);

        hpBar.value = (float)hp / (float)maxHp;

    }
    public void DamageAction(int damage) 
    { 
        hp -= damage; 
    }
}
