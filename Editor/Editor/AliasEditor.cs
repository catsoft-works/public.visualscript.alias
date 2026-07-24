using System;
using UnityEditor;
using UnityEngine.UIElements;
using VisualScript.Runtime;

namespace VisualScript.Editor.Alias
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Runtime.Alias.Alias))]
    public class AliasEditor : UnityEditor.Editor
    {
        private const string USS_PATH = "Packages/works.catsoft.visualscript-alias/Editor/StyleSheets/Alias.uss";

        private const string NAME_ALIAS = "VS-Alias";
        private const string CLASS_FIELD_MODE = "vs-alias-field-mode";
        private const string CLASS_FIELD_ID = "vs-alias-field-id";

        private static readonly IdStringConfig CONFIG_EDITOR = new IdStringConfig
        {
            prefix = IdStringConfig.Prefix.None,
            edition = IdStringConfig.Edition.EditField,
            marginTop = false,
            inspectorMargins = false,
            requireUniqueId = false
        };

        private static readonly IdStringConfig CONFIG_RUNTIME = new IdStringConfig
        {
            prefix = IdStringConfig.Prefix.PrefixIconId,
            edition = IdStringConfig.Edition.ReadOnly,
            marginTop = false,
            inspectorMargins = false,
            requireUniqueId = false
        };

        private const string SPACE = " ";

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private EnumField m_FieldMode;
        [NonSerialized] private IdStringElement m_FieldId;

        // PAINT METHODS: -------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement content = EditorApplication.isPlayingOrWillChangePlaymode switch
            {
                true => this.CreateRuntime(),
                false => this.CreateEditor()
            };

            content.LoadStyleSheet(USS_PATH);
            return content;
        }

        private VisualElement CreateRuntime()
        {
            VisualElement root = new VisualElement { name = NAME_ALIAS };

            SerializedProperty id = this.serializedObject.FindProperty("m_Id");
            this.m_FieldId = new IdStringElement(id, CONFIG_RUNTIME);
            this.m_FieldId.AddToClassList(CLASS_FIELD_ID);

            root.Add(this.m_FieldId);
            return root;
        }

        private VisualElement CreateEditor()
        {
            VisualElement root = new VisualElement { name = NAME_ALIAS };

            SerializedProperty mode = this.serializedObject.FindProperty("m_Mode");
            SerializedProperty id = this.serializedObject.FindProperty("m_Id");

            this.m_FieldMode = new EnumField
            {
                label = string.Empty,
                bindingPath = mode.propertyPath
            };

            this.m_FieldMode.AddToClassList(CLASS_FIELD_MODE);
            this.m_FieldMode.RegisterValueChangedCallback(this.OnChangeMode);

            this.m_FieldId = new IdStringElement(SPACE, id, CONFIG_EDITOR);
            this.m_FieldId.AddToClassList(CLASS_FIELD_ID);

            TypeId currentMode = (TypeId) mode.enumValueIndex;

            this.m_FieldMode.style.flexGrow = currentMode switch
            {
                TypeId.RandomID => new StyleFloat(1f),
                TypeId.ID => new StyleFloat(0f),
                _ => throw new ArgumentOutOfRangeException()
            };

            this.m_FieldId.style.display = currentMode switch
            {
                TypeId.RandomID => DisplayStyle.None,
                TypeId.ID => DisplayStyle.Flex,
                _ => throw new ArgumentOutOfRangeException()
            };

            root.Add(this.m_FieldMode);
            root.Add(this.m_FieldId);

            return root;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChangeMode(ChangeEvent<Enum> changeEvent)
        {
            TypeId mode = (TypeId) changeEvent.newValue;

            this.m_FieldMode.style.flexGrow = mode switch
            {
                TypeId.RandomID => new StyleFloat(1f),
                TypeId.ID => new StyleFloat(0f),
                _ => throw new ArgumentOutOfRangeException()
            };

            this.m_FieldId.style.display = mode switch
            {
                TypeId.RandomID => DisplayStyle.None,
                TypeId.ID => DisplayStyle.Flex,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}