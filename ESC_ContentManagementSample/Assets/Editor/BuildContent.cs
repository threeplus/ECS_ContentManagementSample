using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities.Build;
using Unity.Entities.Content;
using Unity.Scenes.Editor;
using UnityEditor;
using UnityEngine;

public class BuildContent : MonoBehaviour
{

    [MenuItem("Tools/Build Content")]
    static void CreateContentUpdate()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var tmpBuildFolder = Path.Combine(Path.GetDirectoryName(Application.dataPath), $"Build/RemoteContentCache/{buildTarget}");
        var publishFolder = Path.Combine(Path.GetDirectoryName(Application.dataPath), $"Build/RemoteContent/{buildTarget}");
        Build(buildTarget, tmpBuildFolder, publishFolder);
    }

    private static void Build(BuildTarget buildTarget, string tmpBuildFolder, string publishFolder)
    {
        var instance = DotsGlobalSettings.Instance;
        var playerGuid = instance.GetPlayerType() == DotsGlobalSettings.PlayerType.Client ? instance.GetClientGUID() : instance.GetServerGUID();
        if (!playerGuid.IsValid)
            throw new Exception("Invalid Player GUID");

        var remoteList = EntityPrefabList.GetFromReources();
        remoteList.CollectGUID();

        var guids = new HashSet<Unity.Entities.Hash128>();
        var entityScene = remoteList.EntityScenes;
        foreach (var remoteAsset in entityScene)
        {
            guids.Add(remoteAsset.GUID);
        }
        var entityPrefabs = remoteList.EntityPrefabs;
        foreach (var remoteAsset in entityPrefabs)
        {
            guids.Add(remoteAsset.GUID);
        }

        RemoteContentCatalogBuildUtility.BuildContent(guids, playerGuid, buildTarget, tmpBuildFolder);
        RemoteContentCatalogBuildUtility.PublishContent(tmpBuildFolder, publishFolder, f => {
            return new string[] { "all" };
        });
    }

}
