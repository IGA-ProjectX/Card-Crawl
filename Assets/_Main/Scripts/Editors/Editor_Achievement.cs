using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace IGDF
{
    [CustomEditor(typeof(SO_Achievement))]
    public class Editor_Achievement : Editor
    {
        private SO_Achievement so_Achievement;
        private void OnEnable()
        {
            so_Achievement = target as SO_Achievement;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Enum"))
            {
                GenerateEnum();
            }
        }

        private void GenerateEnum()
        {
            string filePath = "Assets/_Main/Scripts/Enums/Enum_Achievement.cs";
            string code = "namespace IGDF{";
            code += "public enum AchievementType{";
            foreach (AchievementContent achievementContent in so_Achievement.achievements)
            {
                code += achievementContent.enumType + ",";
            }
            code += "}}";
            File.WriteAllText(filePath, code);
            AssetDatabase.ImportAsset("Assets/_Main/Scripts/Enums/Enum_Achievement.cs");
        }
    }
}
