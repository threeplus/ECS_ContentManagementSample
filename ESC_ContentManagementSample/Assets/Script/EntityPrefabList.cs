using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPrefabList : ScriptableObject
{
    #if UNITY_EDITOR
    public List<UnityEditor.SceneAsset> EditorScenes;
    public List<GameObject> EditorEntityPrefab;
    #endif

    [System.Serializable]
    public class PrefabGUID{
        public string Name;
        public Unity.Entities.Hash128 GUID;
    }

    
    public List<PrefabGUID> EntityScenes = new List<PrefabGUID>();
    public List<PrefabGUID> EntityPrefabs = new List<PrefabGUID>();

    public Unity.Entities.Hash128 GetGUIDBySceneName(string name){
        foreach (var prefab in EntityScenes)
        {
            if (prefab.Name == name)
            {
                return prefab.GUID;
            }
        }
        return new Unity.Entities.Hash128();
    }

    public Unity.Entities.Hash128 GetGUIDByPrefabName(string name){
        foreach (var prefab in EntityPrefabs)
        {
            if (prefab.Name == name)
            {
                return prefab.GUID;
            }
        }
        return new Unity.Entities.Hash128();
    }

    #if UNITY_EDITOR
    public void CollectGUID(){
        EntityScenes.Clear();
        foreach (var scene in EditorScenes)
        {
            var guid = UnityEditor.AssetDatabase.GUIDFromAssetPath(UnityEditor.AssetDatabase.GetAssetPath(scene));
            EntityScenes.Add(new PrefabGUID{Name = scene.name, GUID = guid});
        }
        EntityPrefabs.Clear();
        foreach (var prefab in EditorEntityPrefab)
        {
            var guid = UnityEditor.AssetDatabase.GUIDFromAssetPath(UnityEditor.AssetDatabase.GetAssetPath(prefab));
            EntityPrefabs.Add(new PrefabGUID{Name = prefab.name, GUID = guid});
        }
    }
    #endif

    //Update it by addressable! using Resources.Load here is just for sample
    public static EntityPrefabList GetFromReources(){
        return Resources.Load<EntityPrefabList>("EntityPrefabList");
    }
}
