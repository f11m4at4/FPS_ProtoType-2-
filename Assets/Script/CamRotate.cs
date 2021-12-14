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
           //x축 회전은 마우스y 입력값의 반대부호가, y축회전은 마우스x 입력값
           transform.eulerAngles += dir * rotspeed * Time.deltaTime;
           //코드에서 값을 변경하고 적용하고 쓰기 위해선 변수를 선언하고 써야한다.
           //지금 위에 있는 transform이 변수다. Transform 컴포넌트의 변수다.
           //gameObject도 마찬가지로 변수다. GameObject 객체의 변수다.
           //변수에 접근해서 오일러앵글즈를 가져와 더하고 저장하는 과정을 통해 
           //CamRotate 클래스가 적용된 객체의 Transform 컴포넌트에 접근해 오일러앵글즈가 변경되고
           //오일러 앵글즈값의 변경으로 계산하여 로테이션 값의 결과를 적용한다.
           Vector3 rot = transform.eulerAngles;
           //지금 클래스의 오일러 앵글 정보를 받는 Vector3 rot변수
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
