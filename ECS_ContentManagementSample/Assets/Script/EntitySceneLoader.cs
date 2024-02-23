using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class EntitySceneLoader : MonoBehaviour
{   
    public string SceneName;

    public async Task Load(){
        var epl = EntityPrefabList.GetFromReources();
        var guid = epl.GetGUIDBySceneName(SceneName);

        var sceneEntity = SceneSystem.LoadSceneAsync(World.DefaultGameObjectInjectionWorld.Unmanaged, guid, new SceneSystem.LoadParameters(){
            AutoLoad = true
        });

        SceneSystem.SceneStreamingState sceneStreamingState = SceneSystem.SceneStreamingState.Unloaded;
        while (sceneStreamingState == SceneSystem.SceneStreamingState.Loading || sceneStreamingState == SceneSystem.SceneStreamingState.Unloaded){
            await Task.Yield();
            sceneStreamingState = SceneSystem.GetSceneStreamingState(World.DefaultGameObjectInjectionWorld.Unmanaged, sceneEntity);
        }
    }

}
