using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public int hp = 30;
    public int maxHp = 30;
    public Slider hpBar;//슬라이더 UI를 담을 변수
 //매시간마다 아래로향하는중력힘  //y의 이동거리값으로 전달할 변수
    float gravity = -20f,         yVelocity = 0;

    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    public bool isJumping = false;
    //캐릭터컨트롤러를 담을 수 있는 변수공간만들기
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {//캐릭터컨트롤러 컴포넌트를 가져오기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {//Input Manager에 등록된 키워드에 따라 입력값을 받는다
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;//바닥에 닿았을때 중력초기화
            if (isJumping)//점프를 했던 상태라면
            {
                isJumping = false;//다시 점프하고 있지 않다고 초기화
            }
        }
        if (Input.GetButtonDown("Jump") && !(isJumping))
        {
            yVelocity = jumpPower;//수직속력값을 점프파워로 초기화
            isJumping = true;
        }
        //방향을 책임질 dir 벡터 자료를 정의한다. 위로 올라가는 y값은 0
        //w키를 누르면 v =1 d키를 누르면 h =1 이되는데 동시에 누르면
        //둘다 1일 것이다. 이것은 문제다. 벡터의 합인 우측대각선이 1.4의 크기를 가질 것이기 때문이다.
        //그래서 dir.normalized로 정규화 해준다. 동시에 눌러도 대각선 크기를 1로 되게 만들어준다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        //중력부를 추가.. 뭔가 이상하긴해도 일단 넘어가자.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //기존의 포지션을 직접조작하던 방법과 달리 캐릭터컨트롤러의 기능을 활용
        cc.Move(dir * moveSpeed * Time.deltaTime);

        hpBar.value = (float)hp / (float)maxHp;

    }
    public void DamageAction(int damage) 
    { 
        hp -= damage; 
    }
}
