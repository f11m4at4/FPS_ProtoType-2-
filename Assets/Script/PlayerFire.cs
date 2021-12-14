using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bombFactory;//폭탄프리팹을 연결할 공간
    public GameObject firePoint;//포구객체를 연결할 공간
    public float throwPower = 15f;//방향에 곱할 힘

    public GameObject bulletEffect;//이펙트게임오브젝트를 받을 공간
    ParticleSystem ps;//파티클시스템을 받을 공간

    public int weaponPower = 5;
    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        //파티클시스템을 받을 변수에 총알이펙트의 파티클시스템을 저장
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))//우클릭을하면
        {
            GameObject bomb = Instantiate(bombFactory);
            //프리팹을 하나 생성해서 변수에 전달
            bomb.transform.position = firePoint.transform.position;
            //폭탄변수의 위치값을 포구의 위치값으로 설정
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //폭탄변수의 리지드바디를 조작할 리지드바디변수선언
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            //리지드바디의 에드포스(Vector속도, 포스모드) 기능 수행.
        }
        if (Input.GetMouseButtonDown(0))//왼클릭하면
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //카메라의 위치에서 카메라의 z축방향으로 발사되는 레이변수
            RaycastHit hitInfo = new RaycastHit();
            //레이캐스트히트를 저장할 새로운 변수 생성

            if(Physics.Raycast(ray, out hitInfo))
            //연결한 파티클시스템은 돌이튀는 효과다. 그래서 적이 맞으면 발동안되게 else if로 뺀다.
            {//피직스의 기능인 레이캐스트가 발동해서 참이라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {//RaycastHit에는 gameObject가 없다. 그런데 layer를 불러올려면 gameObject가 필요하다.
                 //tranform에는 gameObject가 있다. 그래서 이렇게 접근한다.
                 //레이어이름을 유니티에서 가져오는건 LayerMask에 NameToLayer(string)함수가 있다.
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;
                    //총알이펙트의 위치를 충돌지점의 좌표값으로 변경
                    bulletEffect.transform.forward = hitInfo.normal;
                    //피격이펙트의 z축값을 충돌지점의 법선벡터와 일치시킨다.
                    //만약 피격이펙트가 x축방향으로 나오면 right로 일치시키면된다. 
                    ps.Play();//파티클시스템 재생
                }
            }
        }
    }
}
