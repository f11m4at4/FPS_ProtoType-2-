using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float rotspeed = 200f;
    float xIn_yRot = 0, yIn_xRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*   void Update()
       {
           float mouse_X = Input.GetAxis("Mouse X");
           float mouse_Y = Input.GetAxis("Mouse Y");
           Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);
           //x�� ȸ���� ���콺y �Է°��� �ݴ��ȣ��, y��ȸ���� ���콺x �Է°�
           transform.eulerAngles += dir * rotspeed * Time.deltaTime;
           //�ڵ忡�� ���� �����ϰ� �����ϰ� ���� ���ؼ� ������ �����ϰ� ����Ѵ�.
           //���� ���� �ִ� transform�� ������. Transform ������Ʈ�� ������.
           //gameObject�� ���������� ������. GameObject ��ü�� ������.
           //������ �����ؼ� ���Ϸ��ޱ�� ������ ���ϰ� �����ϴ� ������ ���� 
           //CamRotate Ŭ������ ����� ��ü�� Transform ������Ʈ�� ������ ���Ϸ��ޱ�� ����ǰ�
           //���Ϸ� �ޱ���� �������� ����Ͽ� �����̼� ���� ����� �����Ѵ�.
           Vector3 rot = transform.eulerAngles;
           //���� Ŭ������ ���Ϸ� �ޱ� ������ �޴� Vector3 rot����
           rot.x = Mathf.Clamp(rot.x, -90f, 90f);
           transform.eulerAngles = rot;
       }*/
    private void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        yIn_xRot += mouse_Y * rotspeed * Time.deltaTime;
        xIn_yRot += mouse_X * rotspeed * Time.deltaTime;

        yIn_xRot = Mathf.Clamp(yIn_xRot, -90f, 90f);

        transform.eulerAngles = new Vector3(-yIn_xRot, xIn_yRot, 0);
    }
}
