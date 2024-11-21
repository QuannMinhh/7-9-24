using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    public string playerHealthKey = "PlayerHealth", sceneKey = "SceneIndex", savePresentKey = "SavePresent";
    public LoadedData LoadedData { get; private set; } //lưu trữ máu và cảnh hiện tại, chỉ cho phép cập nhật giá trị trong lớp hiện tại

    public UnityEvent<bool> OnDataLoadedResult;//thông báo kết quả khi tải dữ liệu

    private void Awake()//được gọi khi SaveSystem được khởi tạo
    {
        DontDestroyOnLoad(gameObject); //đảm bảo SaveSystem không bị phá hủy khi chuyển cảnh
    }
    private void Start()//được gọi khi SaveSystem bắt đầu hoạt động
    {
        var result = LoadData();//kiểm tra và tải dữ liệu
        OnDataLoadedResult?.Invoke(result);//kết quả true/false sẽ được gủi nếu sự kiện được gán(?. là kiểm tra null)
    }
//    public void ResetData()
//{
//    PlayerPrefs.DeleteKey(playerHealthKey); //lưu trữ lượng máu
//    PlayerPrefs.DeleteKey(sceneKey); //lưu số cảnh hiện tại
//    PlayerPrefs.DeleteKey(savePresentKey); //đánh dấu dữ liệu được ghi
//    LoadedData = null; 
//    Debug.Log("Data reset successful");
//}


    public bool LoadData()
    {

        if (PlayerPrefs.GetInt(savePresentKey) == 1)//nếu dữ liệu tồn tại
        {
            LoadedData = new LoadedData();//tạo đối tượng loaddata
            LoadedData.playerHealth = PlayerPrefs.GetInt(playerHealthKey);//lấy máu
            LoadedData.sceneIndex = PlayerPrefs.GetInt(sceneKey);//laod cảnh hiện tại
            return true;//báo thành công
        }
        return false;//dữ liệu không tồn tại

    }

    public void SaveData(int sceneIndex, int playerHealth)
    {
        if (LoadedData == null)
            LoadedData = new LoadedData();//nếu loaddata chưa tồn tại thì tạo mới
        LoadedData.playerHealth = playerHealth;//cập nhật máu vào loaddata
        LoadedData.sceneIndex = sceneIndex;//cập nhật cảnh vào loaddata
        PlayerPrefs.SetInt(playerHealthKey, playerHealth);//lưu máu 
        PlayerPrefs.SetInt(sceneKey, sceneIndex);//lưu cảnh
        PlayerPrefs.SetInt(savePresentKey, 1);//đánh dấu dữ liệu được lưu
    }

}

public class LoadedData
{
    public int playerHealth = -1;
    public int sceneIndex = -1;
}
