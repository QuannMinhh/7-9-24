using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTurret : MonoBehaviour
{
    public float turretRotationSpeed = 150;

    public void Aim(Vector2 inputPointerPosition) //vị trí trỏ chuột có kiểu dữ liệu vector2
    {
        var turretDirection = (Vector3)inputPointerPosition - transform.position; //ép kiểu vector3 để trừ đi vị trí hiện tại của pháo
        //trả về vector mới đại diện cho hướng từ pháo đến mục tiêu -> giúp tháp pháo hướng về phía mục tiêu


        var desiredAngle = Mathf.Atan2(turretDirection.y, turretDirection.x) * Mathf.Rad2Deg; //Atan2 là radian nhân để chuyển sang độ
        //Math.Atan2(y, x) tính góc của vector(x, y) theo trục x
        //Các thành phần y,x là hướng của tháp pháo đến mục tiêu
        //Hàm Atan2 trả về góc -180 đến 180 cho biết góc giữa vector turrtDirection và trục x dương

        var rotationStep = turretRotationSpeed * Time.deltaTime;//số độ tháp pháo quay trong 1 khung hình, nhân tốc độ quay với thời gian giữa các khung hình

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, desiredAngle), rotationStep);
        //Quaternion.RotateTowards(start, end, maxDegreesDelta)
        //start là góc hiện tại
        //end là góc đích
        //maxDegreesDelta là số độ góc pháo có thể quay trong khung hình
    }
}
