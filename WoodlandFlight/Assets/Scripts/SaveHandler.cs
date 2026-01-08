using System;
using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    private SaveData saveData;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private SavePositionManager savePositionManager;
    [SerializeField] private EnemyStateManager enemyStateManager;
    [SerializeField] private Player player;
    [SerializeField] private GrabControl grabControl;

    [System.Serializable]
    public struct SaveData {
        public QuestManagerData questManagerData;
        public SavePositionManagerData savePositionManagerData;
        public EnemyStateManagerData enemyStateManagerData;
        public PlayerData playerData;
        public GrabControlData grabControlData;
    }

    private void HandleLoadData() {
        questManager.Load(saveData.questManagerData);
        grabControl.Load(saveData.grabControlData);
        savePositionManager.Load(saveData.savePositionManagerData);
        enemyStateManager.Load(saveData.enemyStateManagerData);
        player.Load(saveData.playerData);
    }

    public void HandleSaveData() {
        saveData.questManagerData = questManager.Save();
        saveData.savePositionManagerData = savePositionManager.Save();
        saveData.enemyStateManagerData = enemyStateManager.Save();
        saveData.playerData = player.Save();
        saveData.grabControlData = grabControl.Save();
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
            ScreenFade.Instance.FadeOutHoldFadeIn(0f, 1f, 1f);
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

    void Start() {
        // if (File.Exists(SaveFileName()))
        //     Load();
    }
}
