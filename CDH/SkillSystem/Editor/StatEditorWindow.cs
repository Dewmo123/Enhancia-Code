using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using TextField = UnityEngine.UIElements.TextField;

namespace _00.Work.CDH.Code.SkillSystem.Editor
{
	/*
	public class StatEditorWindow : EditorWindow
	{
		[SerializeField] private VisualTreeAsset viewUI = default;
		[SerializeField] private VisualTreeAsset skillViewUI = default;
		[SerializeField] private VisualTreeAsset createSkillViewUI = default;
		[SerializeField] private SkillDatabase skillDatabase = default;

		private ScrollView _listScrollView;
		private VisualElement _inspector;
		private UnityEditor.Editor _cachedEditor;
		private bool _isSelect;
		private SkillViewUI _selectedItem = null;
		private SkillSO _currentSkillSo = null;
		private string _currentSkillName = "";
		private TextField _skillNameField = null;

		[UnityEditor.MenuItem("Tools/SkillEditor")]
		public static void ShowWindow()
		{
			StatEditorWindow wnd = GetWindow<StatEditorWindow>();
			wnd.titleContent = new GUIContent("SkillEditorWindow");
			wnd.minSize = new Vector2(800, 600);
		}

		public void CreateGUI()
		{
			VisualElement root = rootVisualElement;

			viewUI.CloneTree(root);

			InitializeTable(root);
			AddListener(root);
		}

		private void AddListener(VisualElement root)
		{
			Button createBtn = root.Q<Button>("CreateBtn");
			createBtn.clicked += HandleCreateSkill;
		}

		private void HandleCreateSkill()
		{
			_inspector.Clear();
			createSkillViewUI.CloneTree(_inspector);

			_skillNameField = _inspector.Q<TextField>("skillName");

			_skillNameField.RegisterCallback<KeyDownEvent>(evt =>
			{
				if (evt.keyCode == KeyCode.Return)
				{
					HandleSkillName(_skillNameField.text);
					Debug.Log("Enter");
				}
			});
		}

		private void HandleSkillName(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				EditorUtility.DisplayDialog("Error", "Please enter a vaild name", "OK");
				return;
			}
			_currentSkillName = Regex.Replace(str, @"[^a-zA-Z0-9\s]", "");
			CreateSkill(_currentSkillName);
		}

		private void CreateSkill(string skillName)
		{
			string paramName = skillName.ToUpper();
			skillName = skillName.Replace("_", "");

			SkillSO newSkill = CreateInstance<SkillSO>();
			string savePath = newSkill.AssetPath;

			newSkill.skillName = skillName;

			newSkill.CreateIfNotExists(savePath);
			AssetDatabase.CreateAsset(newSkill, $"{savePath}/{skillName}.asset");

			_currentSkillSo = newSkill;
			_currentSkillSo.CreateSkill(skillName, paramName);

			if (skillDatabase.table == null)
				skillDatabase.table = new List<SkillSO>();
			skillDatabase.table.Add(_currentSkillSo);
			skillDatabase.playerFSM.states.Add(_currentSkillSo.skillState);

			EditorUtility.SetDirty(skillDatabase);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			RefreshUI();
		}
		private void InitializeTable(VisualElement root)
		{
			_listScrollView = root.Q<ScrollView>("ListScrollView");
			_inspector = root.Q<VisualElement>("Inspector");

			RefreshUI();
		}

		private void RefreshUI()
		{
			_listScrollView.Clear();
			_inspector.Clear();
			foreach (SkillSO skill in skillDatabase.table)
			{
				TemplateContainer skillViewUI = this.skillViewUI.Instantiate();
				_listScrollView.Add(skillViewUI);

				SkillViewUI statView = new SkillViewUI(skillViewUI, skill);

				statView.OnSelect += HandleItemSelect;
				statView.OnDelete += HandleItemDelete;
			}
		}

		private void HandleItemSelect(SkillViewUI selectUI)
		{
			if (_selectedItem != null && _selectedItem != selectUI)
				_selectedItem.Visible(false);

			_currentSkillSo = selectUI.targetSkill;
			UnityEditor.Editor.CreateCachedEditor(selectUI.targetSkill, null, ref _cachedEditor);
			VisualElement skillInspector = _cachedEditor.CreateInspectorGUI();

			SerializedObject so = new SerializedObject(selectUI.targetSkill);
			skillInspector.Bind(so);

			_inspector.Clear();
			_inspector.Add(skillInspector);

			_selectedItem?.SetSelection(false);
			_selectedItem = selectUI;
			_selectedItem.SetSelection(true);
		}

		private void HandleItemDelete(SkillViewUI target)
		{
			if (EditorUtility.DisplayDialog("Delete", "Are u sure?", "Yes", "No"))
			{
				string skillAssetPath = AssetDatabase.GetAssetPath(target.targetSkill);
				string paramAssetPath = AssetDatabase.GetAssetPath(target.targetSkill.skillAnimParam);
				string stateAssetPath = AssetDatabase.GetAssetPath(target.targetSkill.skillState);
				AssetDatabase.DeleteAsset(skillAssetPath);
				AssetDatabase.DeleteAsset(paramAssetPath);
				AssetDatabase.DeleteAsset(stateAssetPath);
				skillDatabase.table.Remove(target.targetSkill);
				skillDatabase.playerFSM.states.Remove(_currentSkillSo.skillState);
				target.targetSkill.DeleteSkillScript();
				EditorUtility.SetDirty(skillDatabase);
				AssetDatabase.SaveAssets();

				RefreshUI();
			}
		}
	}
	*/
}