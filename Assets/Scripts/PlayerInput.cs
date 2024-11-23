using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]//cho phép hiển thị trong editer dù là private
    private Camera mainCamera;

    public UnityEvent OnShoot = new UnityEvent();//kích hoạt khi người chơi bắn
    public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();//sự kiện kích hoạt khi người chơi di chuyển thân xe tăng
    public UnityEvent<Vector2> OnMoveTurret = new UnityEvent<Vector2>();// sự kiện di chuyển pháo theo chuột

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;//nếu mainCamera ko đc gán, tự động lấy camera chính theo Camera.main
    }

    // Update is called once per frame
    void Update()
    {
        GetBodyMovement();//xử lí phím di chuyển thân
        GetTurretMovement();//xử lí phím di chuyển pháo
        GetShootingInput();//xử lí phím bắn
    }

    private void GetShootingInput()
    {
        if (Input.GetMouseButtonDown(0))//kiểm tra nút chuột trái bắn
        {
            OnShoot?.Invoke();//kích hoạt sự kiện bắn
        }
    }

    private void GetTurretMovement()
    {
        OnMoveTurret?.Invoke(GetMousePositon());//lấy vị trí trỏ chuột
    }

    private Vector2 GetMousePositon()
    {
        Vector3 mousePosition = Input.mousePosition;//lấy vị trí trỏ chuột trong tọa độ màn hình
        mousePosition.z = mainCamera.nearClipPlane;//xác định độ sâu gần nhất của camera, dùng để xác định vị trí chuột trong không gian thế giới
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);//chuyển vị trí chuột từ tọa độ màn hình sang tọa độ thế giới
        return mouseWorldPosition;
    }

    private void GetBodyMovement()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//lấy giá trị đầu vào ngang(A,D), lấy giá trị đầu vào dọc (W,S)
        OnMoveBody?.Invoke(movementVector.normalized);//chuẩn hóa vector để đảm bảo di chuyển với tốc độ đồng đều dù đi chéo
        //kích họa sự kiện onMoveBody truyền vector di chuyển để xư lí ở các thành phần khác
    }
}
