using System.IO;
using UnityEditor;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem
{
    public abstract class SetNameSO : ScriptableObject
    {
        public virtual void SetNameChanged(string newName)
        {
            //string path = AssetDatabase.GetAssetPath(this); // 현재 SO 파일 경로

            //if (File.Exists(path))
            //{
            //    AssetDatabase.RenameAsset(path, newName);
            //    AssetDatabase.SaveAssets();
            //    AssetDatabase.Refresh();
            //}
            //else
            //{
            //    Debug.LogError("파일을 찾을 수 없습니다: " + path);
            //}
        }
    }
}