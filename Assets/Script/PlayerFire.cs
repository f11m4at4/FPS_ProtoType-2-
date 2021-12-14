using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bombFactory;//��ź�������� ������ ����
    public GameObject firePoint;//������ü�� ������ ����
    public float throwPower = 15f;//���⿡ ���� ��

    public GameObject bulletEffect;//����Ʈ���ӿ�����Ʈ�� ���� ����
    ParticleSystem ps;//��ƼŬ�ý����� ���� ����

    public int weaponPower = 5;
    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        //��ƼŬ�ý����� ���� ������ �Ѿ�����Ʈ�� ��ƼŬ�ý����� ����
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))//��Ŭ�����ϸ�
        {
            GameObject bomb = Instantiate(bombFactory);
            //�������� �ϳ� �����ؼ� ������ ����
            bomb.transform.position = firePoint.transform.position;
            //��ź������ ��ġ���� ������ ��ġ������ ����
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //��ź������ ������ٵ� ������ ������ٵ𺯼�����
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            //������ٵ��� ��������(Vector�ӵ�, �������) ��� ����.
        }
        if (Input.GetMouseButtonDown(0))//��Ŭ���ϸ�
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //ī�޶��� ��ġ���� ī�޶��� z��������� �߻�Ǵ� ���̺���
            RaycastHit hitInfo = new RaycastHit();
            //����ĳ��Ʈ��Ʈ�� ������ ���ο� ���� ����

            if(Physics.Raycast(ray, out hitInfo))
            //������ ��ƼŬ�ý����� ����Ƣ�� ȿ����. �׷��� ���� ������ �ߵ��ȵǰ� else if�� ����.
            {//�������� ����� ����ĳ��Ʈ�� �ߵ��ؼ� ���̶��
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {//RaycastHit���� gameObject�� ����. �׷��� layer�� �ҷ��÷��� gameObject�� �ʿ��ϴ�.
                 //tranform���� gameObject�� �ִ�. �׷��� �̷��� �����Ѵ�.
                 //���̾��̸��� ����Ƽ���� �������°� LayerMask�� NameToLayer(string)�Լ��� �ִ�.
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;
                    //�Ѿ�����Ʈ�� ��ġ�� �浹������ ��ǥ������ ����
                    bulletEffect.transform.forward = hitInfo.normal;
                    //�ǰ�����Ʈ�� z�ప�� �浹������ �������Ϳ� ��ġ��Ų��.
                    //���� �ǰ�����Ʈ�� x��������� ������ right�� ��ġ��Ű��ȴ�. 
                    ps.Play();//��ƼŬ�ý��� ���
                }
            }
        }
    }
}
