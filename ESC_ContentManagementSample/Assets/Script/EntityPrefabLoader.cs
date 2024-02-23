using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class EntityPrefabLoader : MonoBehaviour
{   
    public string PrefabName;

    public async Task<Entity> Load(){
        var epl = EntityPrefabList.GetFromReources();
        var guid = epl.GetGUIDByPrefabName(PrefabName);

        var sceneEntity = SceneSystem.LoadSceneAsync(World.DefaultGameObjectInjectionWorld.Unmanaged, guid, new SceneSystem.LoadParameters(){
            AutoLoad = true
        });

        SceneSystem.SceneStreamingState sceneStreamingState = SceneSystem.SceneStreamingState.Unloaded;
        while (sceneStreamingState == SceneSystem.SceneStreamingState.Loading || sceneStreamingState == SceneSystem.SceneStreamingState.Unloaded){
            await Task.Yield();
            sceneStreamingState = SceneSystem.GetSceneStreamingState(World.DefaultGameObjectInjectionWorld.Unmanaged, sceneEntity);
        }

        var prefabRoot = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<PrefabRoot>(sceneEntity);
        return prefabRoot.Root;
    }

}
