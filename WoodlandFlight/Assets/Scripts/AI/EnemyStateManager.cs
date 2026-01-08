using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyStateManagerData Save() {
        EnemyAI[] items = Resources.FindObjectsOfTypeAll<EnemyAI>();
        EnemyStateManagerData managerData = new();
        managerData.enemies = items.Select(g => new EnemyData(g.ID, g.isEnabled)).ToList();
        return managerData;
    }

    public void Load(EnemyStateManagerData managerData) {
        EnemyAI[] items = Resources.FindObjectsOfTypeAll<EnemyAI>();
        foreach (EnemyAI item in items) {
            foreach (EnemyData itemData in managerData.enemies) {
                if (item.ID == itemData.id) {
                    item.EnableEnemy(itemData.isEnabled);
                }
            }
        }
    }
}

[System.Serializable]
public struct EnemyStateManagerData {
    public List<EnemyData> enemies;
}

[System.Serializable]
public struct EnemyData {
    public string id;
    public bool isEnabled;

    public EnemyData(string id, bool isEnabled) {
        this.id = id;
        this.isEnabled = isEnabled;
    }

}
