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
        startPosition = transform.position;
        rb2d.velocity = transform.up * this.bulletData.speed;// thiết lập velocity của rb2s làm cho đạn di chuyển theo hướng trục y của nó 
    }

    private void Update()
    {
        conquaredDistance = Vector2.Distance(transform.position, startPosition);
        if (conquaredDistance >= bulletData.maxDistance)
        {
            DisableObject();
        }
    }

    private void DisableObject()
    {
        rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit?.Invoke();
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.Hit(bulletData.damage);
        }

        DisableObject();
    }
}
