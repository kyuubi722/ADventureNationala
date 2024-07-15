using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class loadData : MonoBehaviour
{
    public string saveFilePath;
    public GameObject fatherDecoObjs;
    void Start()
    {
        Load();
    }
    public void Save(){
        SaveLoadJson data = new SaveLoadJson(RunningGameDB.Cash, RunningGameDB.maxScore,InteractionHandler.motherString);
        string SavedData = JsonUtility.ToJson(data);
        saveFilePath = Application.persistentDataPath + "/saveData.json";
        Debug.Log("" + saveFilePath);
        System.IO.File.WriteAllText(saveFilePath, SavedData);  
    }
    public void Load(){
        saveFilePath = Application.persistentDataPath + "/saveData.json";
        string json = File.ReadAllText(saveFilePath);
        SaveLoadJson data = JsonUtility.FromJson<SaveLoadJson>(json);
        RunningGameDB.Cash = data.Cash;
        InteractionHandler.motherString = data.boughtitems;
        string[] itemsBought = data.boughtitems.Split('_').Select(str => str.Trim()).ToArray();
        Transform[] BoughtItems = fatherDecoObjs.GetComponentsInChildren<Transform>(true);
            foreach (Transform item in BoughtItems){
                GameObject itemobj = item.gameObject;
                if(itemsBought.Contains(itemobj.name)){
                    itemobj.SetActive(true);
                }
            }
    }
}
