using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UnityEngine.UI.Text LogText;
    
    public void Log(string text){
        LogText.text = $"{LogText.text}\n\n{text}";
    }

    public string Url;

    public async Task Init(string initSet){
        if (string.IsNullOrEmpty(initSet))
            Log($"Init with Empty init set");
        else
            Log($"Init with {initSet}");

        ContentDeliveryGlobalState.LogFunc = Log;
        RuntimeContentManager.LogFunc = Log;
        RuntimeContentSystem.LoadContentCatalog(Url, Application.persistentDataPath + "/content-cache", initSet);
        while (ContentDeliveryGlobalState.CurrentContentUpdateState<=ContentDeliveryGlobalState.ContentUpdateState.ContentReady){
            await Task.Yield();
        }
        if (string.IsNullOrEmpty(initSet))
            Log($"Init with Empty init set, Done!");
        else
            Log($"Init with {initSet}, Done!");
            
    }

    //Called by button
    public async void InitAll(){
        await Init("all");
    }

    //Called by button
    public async void InitEmpty(){
        await Init("");
    }

    //Called by button
    public void Delete(){
        Directory.Delete(Application.persistentDataPath + "/content-cache", true);
        Log($"Deleted Content Cache at {Application.persistentDataPath + "/content-cache"}");
    }


    public EntityPrefabLoader CubePrefabLoader;
    //Called by button
    public async void LoadCubePrefab(){
        Log("Loading Cube Prefab...");
        var entityPrefab = await CubePrefabLoader.Load();
        World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(entityPrefab);
        Log("Instantiate Cube Prefab Done!");
    }

    public EntityPrefabLoader CapsulePrefabLoader;
    //Called by button
    public async void LoadCapsulePrefab(){
        Log("Loading Capsule Prefab...");
        var entityPrefab = await CapsulePrefabLoader.Load();
        World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(entityPrefab);
        Log("Instantiate Capsule Prefab Done!");
    }

    public EntitySceneLoader CylinderSceneLoader;
    //Called by button
    public async void LoadCylinderScene(){
        Log("Loading Cylinder Scene...");
        await CylinderSceneLoader.Load();
        Log("Load Cylinder Scene Done!");
    }

    public EntitySceneLoader CapsuleSceneLoader;
    //Called by button
    public async void LoadCapsuleScene(){
        Log("Loading Capsule Scene...");
        await CapsuleSceneLoader.Load();
        Log("Load Capsule Scene Done!");
    }
}


