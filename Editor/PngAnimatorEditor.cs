// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;
using UnityEngine;

namespace GifLoader.Editor
{
    [CustomEditor(typeof(PngAnimation))]
    public class PngAnimatorEditor : UnityEditor.Editor
    {
        private SerializedProperty resourceFolder;

        private void OnEnable()
        {
            resourceFolder = serializedObject.FindProperty(nameof(resourceFolder));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField(resourceFolder.stringValue);

            if (GUILayout.Button("Select Resource Folder"))
            {
                var tempPath = EditorUtility.OpenFolderPanel("Select Resource Folder", Application.dataPath, "Resources");

                if (!Directory.Exists(tempPath))
                {
                    Debug.LogError("Invalid Path Specified");
                    return;
                }

                resourceFolder.stringValue = tempPath.Replace($"{Application.dataPath}/", string.Empty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
