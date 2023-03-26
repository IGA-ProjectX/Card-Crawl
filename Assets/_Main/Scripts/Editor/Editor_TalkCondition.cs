using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace IGDF
{
    [CustomEditor(typeof(SO_TalkCondition))]
    public class Editor_TalkCondition : Editor
    {
        private SO_TalkCondition so_TalkCondition;
        private void OnEnable()
        {
            so_TalkCondition = target as SO_TalkCondition;
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
            string filePath = "Assets/_Main/Scripts/Enums/Enum_TalkCondition.cs";
            string code = "namespace IGDF{\n";
            code += "public enum TalkConditionType{\n";
            foreach (string talkCondition in so_TalkCondition.talkConditions)
            {
                code += talkCondition + ",\n";
            }
            code += "\n}\n}";
            File.WriteAllText(filePath, code);
            AssetDatabase.ImportAsset("Assets/_Main/Scripts/Enums/Enum_TalkCondition.cs");
        }
    } 
}
