using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Damagable : MonoBehaviour
{
    public int MaxHealth = 100;//máu của vật thể

    [SerializeField]
    private int health = 0;//máu hiện tại của vật thể

    public int Health//thuộc tính cảu health
    {
        get { return health; }//trả về máu hiện tại
        set
        {
            health = value;//gán giá trị cho máu
            OnHealthChange?.Invoke((float)Health / MaxHealth);//khi mau thay đổi, truyền tỉ lệ máu tối đa để cập nhật thanh máu
        }
    }


    public UnityEvent OnDead;//kích hoạt đối tượng khi chết
    public UnityEvent<float> OnHealthChange;//kích hoạt khi máu thay đổi
    public UnityEvent OnHit, OnHeal;//kích hoạt khi gây sát thương hoặc hồi máu


    private void Start()
    {
        if(health == 0)
            Health = MaxHealth;//đặt giá trị máu hiện tại bằn máu tối đa khi bắt đầu trò chơi
    }

    internal void Hit(int damagePoints)
    {
        Health -= damagePoints;//trừ máu hiện tại theo sát thương
        if (Health <= 0)//nếu máu nhỏ hơn hoặc bằng 0
        {
            if (gameObject.CompareTag("Player"))//nếu đối tượng có tag là player
            {
                OnDead?.Invoke();//kích hoạt sự kiện chết
                SceneManager.LoadScene(0);//chuyển về menu
            }
            else
            {
                OnDead?.Invoke();//đối với xe tăng khác chỉ cho biến mất
            }
        }
        else
        {
            OnHit?.Invoke();//nếu còn máu kích hoạt sự kiện nhận sát thương
        }
    }

    public void Heal(int healthBoost)
    {
        Health += healthBoost;//tăng máu hiện tại
        Health = Mathf.Clamp(Health, 0, MaxHealth);//giới hạn giá trị máu trong khoảng [0, MaxHelth], không cho phép máu hiện tại hồi nhiều hơn lượng máu tối đa
        OnHeal?.Invoke();//kích hoạt sự kiện hối máu
    }
}
