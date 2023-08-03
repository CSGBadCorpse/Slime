using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;


public class AnimationCreator:MonoBehaviour
{
    

    [MenuItem("Assets/Tools/AnimationCreator")]
    public static void CreateAnimation()
    {
        DirectoryInfo clipDirectory = new DirectoryInfo("Assets/Animations/AnimationClips");
        DirectoryInfo pngDirectory = new DirectoryInfo("Assets/The Legend of Slim/01 Original/Green");
        
        
        var clips = clipDirectory.GetFiles("*.animation");
        var clipList = new List<AnimationClip>();

        foreach (var clip in clips)
        {
            AnimationClip c = AssetDatabase.LoadAssetAtPath<AnimationClip>(DataPathToAssetPath(clip.FullName));

            var curves = AnimationUtility.GetObjectReferenceCurveBindings(c);
            Debug.Log(curves);
        }

    }

    public static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets\\"));
        else
            return path.Substring(path.IndexOf("Assets/"));
    }
}
