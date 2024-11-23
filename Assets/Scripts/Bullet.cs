using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public BulletData bulletData;

    private Vector2 startPosition; //vị trí ban đầu của đạn
    private float conquaredDistance = 0;//khoảng cách đã di chuyển
    private Rigidbody2D rb2d;// thành phần rigidbody2D để xử lí vật lí cho đạn

    public UnityEvent OnHit = new UnityEvent();//sự kiện khi đạn trúng đối tượng nào đó

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>(); //lấy thành phần rigidbody2d của đạn để điều khiển vận tốc
    }

    public void Initialize(BulletData bulletData)
    {
        this.bulletData = bulletData;
        startPosition = transform.position;//vị trí đầu tiên của đạn
        rb2d.velocity = transform.up * this.bulletData.speed;// thiết lập velocity của rb2s làm cho đạn di chuyển theo hướng trục y của nó 
    }

    private void Update()
    {
        conquaredDistance = Vector2.Distance(transform.position, startPosition);//biểu thức tính khoảng cách đã di chuyển
        if (conquaredDistance >= bulletData.maxDistance)//nếu khoảng cách đã di chuyển lớn hơn hoạc bằng maxdistance
        {
            DisableObject();//hủy viên đạn
        }
    }

    private void DisableObject()
    {
        rb2d.velocity = Vector2.zero;//dừng hoạt động viên đạn
        gameObject.SetActive(false);//vô hiệu hóa đối tượng
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit?.Invoke();//phát hiện va chạm
        var damagable = collision.GetComponent<Damagable>();//kiểm tra đạn có va chạm với component kiểu damagable hay không
        if (damagable != null)//nêu đối tượng va chạm với đạn có khả năng gây sát thương
        {
            damagable.Hit(bulletData.damage);//họi hàm Hit trong damageble và truyền sát thương từ viên đạn
        }

        DisableObject();//vô hiệu hóa đạn
    }
}
