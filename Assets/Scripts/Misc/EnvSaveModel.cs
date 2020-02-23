using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EnvSaveModel {
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;
    public String Module_Resource;

    public void SetData(GameObject go) {
        ItemResorce ir = go.GetComponent<ItemResorce>();
        Module_Resource = ir.GetResorceLocation();
        position = go.transform.position;
        scale = go.transform.localScale;
        rotation = go.transform.rotation;
    }
}
[Serializable]
public class DataItem {
    public string Key;

    public string Value;

    public DataItem(string key, string value) {
        Key = key;

        Value = value;
    }
}
[Serializable]
public class EnvSaveModels {
    public List<EnvSaveModel> items = new List<EnvSaveModel>();
    public List<DataItem> generated = new List<DataItem>();

    public void ReadKeyPair(Hashtable data) {
        foreach (string key in data.Keys) {
            generated.Add(new DataItem(key, data[key].ToString()));
        }
    }

    public void ReadItems(Transform data) {
        foreach (Transform item in data) {
            EnvSaveModel i = new EnvSaveModel();
            i.SetData(item.gameObject);
            items.Add(i);
        }
    }

    public void Save() {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + "/env_config.save", json);
    }

    public static EnvSaveModels Load() {
        //Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/env_config.save")) {
            string data = File.ReadAllText(Application.persistentDataPath + "/env_config.save");
            EnvSaveModels gameSaving = JsonUtility.FromJson<EnvSaveModels>(data);
            return gameSaving;
        } else {
            return null;
        }
    }
}