using _00.Work.CDH.Code.SkillSystem.Skills;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using System;
using UnityEngine.UIElements;

namespace _00.Work.CDH.Code.SkillSystem.Editor
{
    public class SkillViewUI
    {
        public event Action<SkillViewUI> OnSelect;
        public event Action<SkillViewUI> OnDelete;

        public VisualElement sprite;
        public Label titleLable;
        public Button deleteButton;

        public SkillItemSO targetSkill;

        private VisualElement _container;

        public SkillViewUI(TemplateContainer root, SkillItemSO stat)
        {
            root.RegisterCallback<ClickEvent>(HandleItemSelect);
            targetSkill = stat;

            _container = root.Q<VisualElement>("Container");
            sprite = root.Q<VisualElement>("Image");
            titleLable = root.Q<Label>("SkillTitle");
            deleteButton = root.Q<Button>("DeleteBtn");
            deleteButton.RegisterCallback<ClickEvent>(HandleDelete);

            Visible(false);

            RefreshUI();
        }

        public void SetSelection(bool isSelected)
        {
            if (isSelected)
                _container.AddToClassList("select");
            else
                _container.RemoveFromClassList("select");
        }

        public void RefreshUI()
        {
            sprite.style.backgroundImage = new StyleBackground(targetSkill.icon);
            titleLable.text = targetSkill.skillName;
            //targetSkill.SetNameChanged(targetSkill.skillName);
            //targetSkill.skillAnimParam.SetNameChanged(targetSkill.skillName);
        }

        private void HandleDelete(ClickEvent evt)
        {
            evt.StopPropagation();
            OnDelete?.Invoke(this);
        }

        private void HandleItemSelect(ClickEvent evt)
        {
            Visible(true);
            OnSelect?.Invoke(this);
        }

        public void Visible(bool isVisible)
        {
            if (isVisible)
            {
                deleteButton.style.visibility = Visibility.Visible;
            }
            else
            {
                deleteButton.style.visibility = Visibility.Hidden;
            }
        }
    }
}