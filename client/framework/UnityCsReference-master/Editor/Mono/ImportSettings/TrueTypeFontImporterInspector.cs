// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEditor.Experimental.AssetImporters;

namespace UnityEditor
{
    [CustomEditor(typeof(TrueTypeFontImporter))]
    [CanEditMultipleObjects]
    internal class TrueTypeFontImporterInspector : AssetImporterEditor
    {
        private static class Style
        {
            public static readonly GUIContent RenderingMode = EditorGUIUtility.TrTextContent("Rendering Mode");
            public static readonly GUIContent TextureCase = EditorGUIUtility.TrTextContent("Character");
            public static readonly GUIContent AscentCalculationMode = EditorGUIUtility.TrTextContent("Ascent Calculation Mode");
            public static readonly GUIContent CustomChars = EditorGUIUtility.TrTextContent("Custom Chars");
            public static readonly GUIContent BoundCalculation = EditorGUIUtility.TrTextContent("Use Legacy Bounds");
            public static readonly GUIContent ShouldRoundAdvanceValue = EditorGUIUtility.TrTextContent("Should Round Advance Value");
            public static readonly GUIContent IncludeFontData = EditorGUIUtility.TrTextContent("Include Font Data");
            public static readonly GUIContent FamilyName = EditorGUIUtility.TrTextContent("Family Name", "This is the family name of this font. It will always be included to perform the references check, along with other names in the Font Name list.");
            public static readonly GUIContent FontName = EditorGUIUtility.TrTextContent("Font Names");
            public static readonly GUIContent FontReferences = EditorGUIUtility.TrTextContent("References to other fonts in project");
            public static readonly GUIContent NoFontReference = EditorGUIUtility.TrTextContent("No references to other fonts in project.");

            public static readonly string FormatUnsupported = L10n.Tr("Format of selected font is not supported by Unity.");
            public static readonly string ReferencesInformation = L10n.Tr("These are automatically generated by the inspector if any of the font names you supplied match fonts present in your project, which will then be used as fallbacks for this font.");
        }

        SerializedProperty m_FontSize;
        SerializedProperty m_TextureCase;
        SerializedProperty m_IncludeFontData;
        SerializedProperty m_FontName;
        SerializedProperty m_FontNames;
        SerializedProperty m_CustomCharacters;
        SerializedProperty m_FontRenderingMode;
        SerializedProperty m_AscentCalculationMode;
        SerializedProperty m_UseLegacyBoundsCalculation;
        SerializedProperty m_ShouldRoundAdvanceValue;
        List<Font> m_FallbackFontReferences;

        bool? m_FormatSupported = null;
        bool m_ReferencesExpanded = false;

        public override void OnEnable()
        {
            base.OnEnable();

            m_FontSize = serializedObject.FindProperty("m_FontSize");
            m_TextureCase = serializedObject.FindProperty("m_ForceTextureCase");
            m_IncludeFontData = serializedObject.FindProperty("m_IncludeFontData");
            m_FontName = serializedObject.FindProperty("m_FontName");
            m_FontNames = serializedObject.FindProperty("m_FontNames");
            m_CustomCharacters = serializedObject.FindProperty("m_CustomCharacters");
            m_FontRenderingMode = serializedObject.FindProperty("m_FontRenderingMode");
            m_AscentCalculationMode = serializedObject.FindProperty("m_AscentCalculationMode");
            m_UseLegacyBoundsCalculation = serializedObject.FindProperty("m_UseLegacyBoundsCalculation");
            m_ShouldRoundAdvanceValue = serializedObject.FindProperty("m_ShouldRoundAdvanceValue");
            UpdateFontReferencesList();

            m_ReferencesExpanded = EditorPrefs.GetBool("TrueTypeFontImporterInspector_m_ReferencesExpanded");
        }

        void UpdateFontReferencesList()
        {
            var fonts = serializedObject.FindProperty("m_FallbackFontReferences");
            m_FallbackFontReferences = new List<Font>();
            var it = fonts.FindPropertyRelative("Array.size");
            while (it.Next(false) && it.propertyPath.StartsWith(fonts.propertyPath))
            {
                m_FallbackFontReferences.Add(it.hasMultipleDifferentValues ? null : (Font)it.objectReferenceValue);
            }
        }

        public override void OnDisable()
        {
            EditorPrefs.SetBool("TrueTypeFontImporterInspector_m_ReferencesExpanded", m_ReferencesExpanded);
            base.OnDisable();
        }

        protected override bool needsApplyRevert
        {
            get
            {
                if (m_FormatSupported != null && m_FormatSupported.Value == false)
                    return false;
                return base.needsApplyRevert;
            }
        }

        void UpdateFontReferences()
        {
            serializedObject.ApplyModifiedProperties();
            for (var i = 0; i < targets.Length; i++)
            {
                var importer = (TrueTypeFontImporter)targets[i];
                var currentNames = importer.fontNames;
                int size = m_FontNames.hasMultipleDifferentValues ? currentNames.Length : m_FontNames.arraySize;
                var perImporterName = new List<string>(size);
                for (int j = 0; j < size; j++)
                {
                    var prop = m_FontNames.GetArrayElementAtIndex(j);
                    if (prop != null && prop.isValid)
                        perImporterName.Add(prop.hasMultipleDifferentValues ? currentNames[j] : prop.stringValue);
                    else
                        perImporterName.Add(currentNames[j]);
                }
                var fallbacks = importer.LookupFallbackFontReferences(perImporterName.ToArray());
                importer.fontReferences = fallbacks;
            }
            serializedObject.Update();
            UpdateFontReferencesList();
        }

        private void ShowFormatUnsupportedGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.HelpBox(Style.FormatUnsupported, MessageType.Warning);
        }

        static string GetUniquePath(string basePath, string extension)
        {
            for (int i = 0; i < 10000; i++)
            {
                string path = string.Format("{0}{1}.{2}", basePath, (i == 0 ? string.Empty : i.ToString()), extension);
                if (!File.Exists(path))
                    return path;
            }
            return "";
        }

        [MenuItem("CONTEXT/TrueTypeFontImporter/Create Editable Copy", validate = true)]
        static bool CreateEditableCopy_Validate(MenuCommand command)
        {
            var importer = (TrueTypeFontImporter)command.context;
            return AssetDatabase.Contains(importer);
        }

        [MenuItem("CONTEXT/TrueTypeFontImporter/Create Editable Copy")]
        static void CreateEditableCopy(MenuCommand command)
        {
            var importer = (TrueTypeFontImporter)command.context;
            if (importer.fontTextureCase == FontTextureCase.Dynamic)
            {
                EditorUtility.DisplayDialog(
                    "Cannot generate editable font asset for dynamic fonts",
                    "Please reimport the font in a different mode.",
                    "Ok");
                return;
            }
            string basePath = Path.Combine(Path.GetDirectoryName(importer.assetPath), Path.GetFileNameWithoutExtension(importer.assetPath));
            EditorGUIUtility.PingObject(importer.GenerateEditableFont(GetUniquePath(basePath + "_copy", "fontsettings")));
        }

        public override void OnInspectorGUI()
        {
            if (!m_FormatSupported.HasValue)
            {
                m_FormatSupported = true;
                foreach (Object target in targets)
                {
                    var importer = target as TrueTypeFontImporter;
                    if (importer == null || !importer.IsFormatSupported())
                        m_FormatSupported = false;
                }
            }

            if (m_FormatSupported == false)
            {
                ShowFormatUnsupportedGUI();
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_FontSize);
            if (m_FontSize.intValue < 1)
                m_FontSize.intValue = 1;
            if (m_FontSize.intValue > 500)
                m_FontSize.intValue = 500;

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                using (var property = new EditorGUI.PropertyScope(horizontal.rect, Style.RenderingMode, m_FontRenderingMode))
                {
                    using (var changed = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUI.showMixedValue = m_FontRenderingMode.hasMultipleDifferentValues;
                        var newValue = (int)(FontRenderingMode)EditorGUILayout.EnumPopup(property.content, (FontRenderingMode)m_FontRenderingMode.intValue);
                        EditorGUI.showMixedValue = false;
                        if (changed.changed)
                        {
                            m_FontRenderingMode.intValue = newValue;
                        }
                    }
                }
            }
            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                using (var property = new EditorGUI.PropertyScope(horizontal.rect, Style.TextureCase, m_TextureCase))
                {
                    using (var changed = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUI.showMixedValue = m_TextureCase.hasMultipleDifferentValues;
                        var newValue = (int)(FontTextureCase)EditorGUILayout.EnumPopup(property.content, (FontTextureCase)m_TextureCase.intValue);
                        EditorGUI.showMixedValue = false;
                        if (changed.changed)
                        {
                            m_TextureCase.intValue = newValue;
                        }
                    }
                }
            }
            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                using (var property = new EditorGUI.PropertyScope(horizontal.rect, Style.AscentCalculationMode, m_AscentCalculationMode))
                {
                    using (var changed = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUI.showMixedValue = m_AscentCalculationMode.hasMultipleDifferentValues;
                        var newValue = (int)(AscentCalculationMode)EditorGUILayout.EnumPopup(property.content, (AscentCalculationMode)m_AscentCalculationMode.intValue);
                        EditorGUI.showMixedValue = false;
                        if (changed.changed)
                        {
                            m_AscentCalculationMode.intValue = newValue;
                        }
                    }
                }
            }
            EditorGUILayout.PropertyField(m_UseLegacyBoundsCalculation, Style.BoundCalculation);
            EditorGUILayout.PropertyField(m_ShouldRoundAdvanceValue, Style.ShouldRoundAdvanceValue);

            if (!m_TextureCase.hasMultipleDifferentValues)
            {
                if ((FontTextureCase)m_TextureCase.intValue != FontTextureCase.Dynamic)
                {
                    if ((FontTextureCase)m_TextureCase.intValue == FontTextureCase.CustomSet)
                    {
                        // Characters included
                        using (var horizontal = new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUI.PropertyScope(horizontal.rect, GUIContent.none, m_CustomCharacters))
                            {
                                using (var change = new EditorGUI.ChangeCheckScope())
                                {
                                    EditorGUILayout.PrefixLabel(Style.CustomChars);
                                    EditorGUI.showMixedValue = m_CustomCharacters.hasMultipleDifferentValues;
                                    string guiChars = EditorGUILayout.TextArea(m_CustomCharacters.stringValue, GUI.skin.textArea, GUILayout.MinHeight(EditorGUI.kSingleLineHeight * 2));
                                    EditorGUI.showMixedValue = false;
                                    if (change.changed)
                                        m_CustomCharacters.stringValue = new string(guiChars.Distinct().Where(c => c != '\n' && c != '\r').ToArray());
                                }
                            }
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(m_IncludeFontData, Style.IncludeFontData);

                    EditorGUI.BeginChangeCheck();

                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.PropertyField(m_FontName, Style.FamilyName);
                    }

                    using (var changed = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUILayout.PropertyField(m_FontNames, Style.FontName);
                        if (changed.changed)
                        {
                            UpdateFontReferences();
                        }
                    }

                    m_ReferencesExpanded = EditorGUILayout.Foldout(m_ReferencesExpanded, Style.FontReferences, true);
                    if (m_ReferencesExpanded)
                    {
                        EditorGUILayout.HelpBox(Style.ReferencesInformation, MessageType.Info);

                        using (new EditorGUI.DisabledScope(true))
                        {
                            if (m_FallbackFontReferences.Count > 0)
                            {
                                foreach (var font in m_FallbackFontReferences)
                                {
                                    EditorGUI.showMixedValue = serializedObject.isEditingMultipleObjects && font == null;
                                    EditorGUILayout.ObjectField(font, typeof(Font), false);
                                    EditorGUI.showMixedValue = false;
                                }
                            }
                            else
                            {
                                GUILayout.Label(Style.NoFontReference);
                            }
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
            ApplyRevertGUI();
        }
    }
}
