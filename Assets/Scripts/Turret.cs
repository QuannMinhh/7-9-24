using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ObjectPool))]
public class Turret : MonoBehaviour
{
    public List<Transform> turretBarrels;

    public TurretData turretData;

    private bool canShoot = true;
    private Collider2D[] tankColliders;
    private float currentDelay = 0;

    private ObjectPool bulletPool;
    [SerializeField]
    private int bulletPoolCount = 10;

    public UnityEvent OnShoot, OnCantShoot;
        public UnityEvent<float> OnReloading;

    private void Awake()
    {
        tankColliders = GetComponentsInParent<Collider2D>();//lấy tất cả thành phần của bản thân 
        bulletPool = GetComponent<ObjectPool>();//tham chiếu đến objectPool của bản thân
    }

    private void Start()
    {
        bulletPool.Initialize(turretData.bulletPrefab, bulletPoolCount);
        OnReloading?.Invoke(currentDelay);
    }

    private void Update()
    {
        if (canShoot == false)//kiểm tra trạng thái bắn
        {
            currentDelay -= Time.deltaTime;//bắt đầu thời gian nạp lại
            OnReloading?.Invoke(currentDelay/ turretData.reloadDelay);
            if (currentDelay <= 0)
            {
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            currentDelay = turretData.reloadDelay;

            foreach (var barrel in turretBarrels)//bắn số lượng đạn theo số tháp pháo của xe tăng
            {
                var hit = Physics2D.Raycast(barrel.position, barrel.up);//kiểm tra va chạm từ vị trí theo hướng nòng pháo
                if (hit.collider != null)
                    Debug.Log(hit.collider.name);//kiểm tra xem có vật thể nào đứng trước nòng pháo không
                //GameObject bullet = Instantiate(bulletPrefab);
                GameObject bullet = bulletPool.CreateObject();//sinh đạn
                bullet.transform.position = barrel.position;//đặt vị trí đạn tại đầu nòng
                bullet.transform.localRotation = barrel.rotation;//xoay đạn theo hướng nòng súng
                bullet.GetComponent<Bullet>().Initialize(turretData.bulletData);//truyền dữ liệu đạn

                foreach (var collider in tankColliders)
                {
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), collider);//loại bỏ va chạm với bản thân
                }

            }

            OnShoot?.Invoke();//sự kiện được gọi khi bắn
            OnReloading?.Invoke(currentDelay);//cập nhật trạng thái nạp đạn
        }
        else
        {
            OnCantShoot?.Invoke();
        }

    }
}
