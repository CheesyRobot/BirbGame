using System;
using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    private SaveData saveData;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private Player player;

    [System.Serializable]
    public struct SaveData {
        public QuestManagerData questManagerData;
        public PlayerData playerData;
    }

    private void HandleLoadData() {
        questManager.Load(saveData.questManagerData);
        player.Load(saveData.playerData);
    }

    public void HandleSaveData() {
        saveData.questManagerData = questManager.Save();
        saveData.playerData = player.Save();
    }

    public static string SaveFileName() {
        return Application.persistentDataPath + "/save.json";
    }
    public void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }

    public void Load()
    {
        if (File.Exists(SaveFileName()))
        {
            string saveContent = File.ReadAllText(SaveFileName());
            saveData = JsonUtility.FromJson<SaveData>(saveContent);
            HandleLoadData();
        }
        else
        {
            Debug.LogWarning(SaveFileName() + "not found");
        }
    }

    public void DeleteSave() {
        if (File.Exists(SaveFileName()))
            File.Delete(SaveFileName());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.N))
            Save();
        if (Input.GetKeyDown(KeyCode.L))
            DeleteSave();
        if (Input.GetKeyDown(KeyCode.M))
            Load();
    }
}
