using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using TMPro;

[InitializeOnLoad]
public class EditorKinectLoader
{
    private static string _prefabPath = "Assets/Prefabs/KinectController.prefab";
    private static string _mainScene = "MainScene";
    static EditorKinectLoader()
    {
        EditorSceneManager.sceneOpened += SceneOpenedCallback;
        EditorSceneManager.sceneSaving += SceneSavingCallback;
        EditorSceneManager.sceneSaved += SceneSavedCallback;
    }

    private static void SceneSavedCallback(Scene scene)
    {
        if (scene.name == _mainScene)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
            if (prefab != null)
            {
                foreach (var obj in scene.GetRootGameObjects())
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == prefab)
                    {
                        obj.hideFlags = HideFlags.None;
                        return;
                    }
                }
            }
        }
    }

    private static void SceneSavingCallback(Scene scene, string path)
    {
        if (scene.name == _mainScene)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
            if (prefab != null)
            {
                foreach (var obj in scene.GetRootGameObjects())
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == prefab)
                    {
                        PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.UserAction);
                        obj.hideFlags = HideFlags.DontSave;
                        return;
                    }
                }
            }
        }
    }

    private static void SceneOpenedCallback(Scene scene, OpenSceneMode mode)
    {
        if (scene.name == _mainScene)
        {
            if (AssetDatabase.IsValidFolder("Assets/AzureKinectExamples"))
            {
                Debug.Log("Kinect folder found");
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
                if (prefab != null)
                {
                    var prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    Debug.Log("Kinect Prefab Loaded Successfully");
                }
                else
                {
                    Debug.LogError("Unable to instantiate the kinect prefab");
                }
            }
            else
            {
                Debug.Log("No Kinect folder found");
            }
        }
    }
}
