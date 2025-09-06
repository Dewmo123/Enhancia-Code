using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _00.Work.CDH.Code.Entities.FSM;
using _00.Work.CDH.Code.Players.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace _00.Work.CDH.Code.FSM.Editor
{
    [CustomEditor(typeof(StateSO))]
    
    public class StateDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspectorUI = default;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            inspectorUI.CloneTree(root); // Ui를 복제해서 root의 자식으로 넣어라.
            
            DropdownField dropDown = root.Q<DropdownField>("ClassDropDownField"); // Q<> : GetComponemt와 비슷함

            CreateDropDownChoices(dropDown);
            
            return root;
        }

        private void CreateDropDownChoices(DropdownField dropDown)
        {
            dropDown.choices.Clear();
            // EntityState 라는 클래스가 속해있는 어셈블리를 가져온다/.
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(EntityState));

            List<string> typeList = fsmAssembly.GetTypes().Where(type => 
                type.IsClass && type.IsAbstract == false && 
                type.IsSubclassOf(typeof(EntityState))).Select(type => type.FullName).ToList();
            // IsSubclassOf 안에 이미 IsClass가 잇어서 지워도 됨
            
            dropDown.choices.AddRange(typeList);
        }
    }
}