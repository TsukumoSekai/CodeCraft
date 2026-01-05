#if ENABLE_TMP
using OfflineFantasy.GameCraft.Models;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Editors
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        private ReorderableList m_ReorderableList;

        /// <summary>
        /// 获取或创建与给定属性关联的 ReorderableList。
        /// </summary>
        private ReorderableList GetReorderableList(SerializedProperty property)
        {
            if (m_ReorderableList == null)
            {
                SerializedProperty kvListProperty = property.FindPropertyRelative("m_SerializedDataList");

                m_ReorderableList = new ReorderableList(property.serializedObject, kvListProperty, true, true, true, true);

                // 绘制列表头部
                m_ReorderableList.drawHeaderCallback = (Rect rect) =>
                {
                    float halfWidth = rect.width / 2.0f - 5.0f;

                    Rect keyHeaderRect = new Rect(rect.x, rect.y, halfWidth, EditorGUIUtility.singleLineHeight);
                    Rect valueHeaderRect = new Rect(rect.x + halfWidth + 10.0f, rect.y, halfWidth, EditorGUIUtility.singleLineHeight);

                    GUI.Label(keyHeaderRect, "Key");
                    GUI.Label(valueHeaderRect, "Value");
                };

                // 设置元素高度
                m_ReorderableList.elementHeightCallback = (int index) => EditorGUIUtility.singleLineHeight + 4.0f;

                // 绘制列表中的每个元素
                m_ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var kvProperty = kvListProperty.GetArrayElementAtIndex(index);

                    rect.y += 2; // 内边距

                    // 计算键和值字段的布局
                    float halfWidth = rect.width / 2.0f - 5.0f; // 减去间距

                    Rect keyRect = new Rect(rect.x, rect.y, halfWidth, EditorGUIUtility.singleLineHeight);
                    Rect valueRect = new Rect(rect.x + halfWidth + 10.0f, rect.y, halfWidth, EditorGUIUtility.singleLineHeight);

                    // 绘制键和值的字段
                    EditorGUI.PropertyField(keyRect, kvProperty.FindPropertyRelative("m_Key"), GUIContent.none);
                    EditorGUI.PropertyField(valueRect, kvProperty.FindPropertyRelative("m_Value"), GUIContent.none);
                };

                // 处理添加新元素
                m_ReorderableList.onAddCallback = (ReorderableList l) =>
                {
                    int index = l.serializedProperty.arraySize;

                    l.serializedProperty.InsertArrayElementAtIndex(index);

                    SerializedProperty newKeyProp = l.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Key");

                    switch (newKeyProp.propertyType)
                    {
                        case SerializedPropertyType.String:
                            newKeyProp.stringValue = "";
                            break;

                        case SerializedPropertyType.Integer:
                            newKeyProp.intValue = 0;
                            break;

                        case SerializedPropertyType.Float:
                            newKeyProp.floatValue = 0f;
                            break;

                        case SerializedPropertyType.Boolean:
                            newKeyProp.boolValue = false;
                            break;

                        case SerializedPropertyType.Enum:
                            if (newKeyProp.enumValueIndex == newKeyProp.enumNames.Length - 1)
                                newKeyProp.enumValueIndex = 0;
                            else
                                newKeyProp.enumValueIndex++;
                            break;
                        // 可以根据需要添加更多类型
                        // 对于 ObjectReference, Enum 等，默认值通常是 null 或 0，InsertArrayElementAtIndex 通常能处理
                        default:
                            // 对于 Generic (复杂对象) 或其他类型，InsertArrayElementAtIndex 会复制或创建默认实例
                            // 如果需要清空复制的内容，可能需要特殊处理，这里暂不处理
                            break;
                    }

                    property.serializedObject.ApplyModifiedProperties();
                };

                // 处理移除元素
                m_ReorderableList.onRemoveCallback = (ReorderableList l) =>
                {
                    int index = l.index;

                    if (index >= 0 && index < l.serializedProperty.arraySize)
                    {
                        l.serializedProperty.DeleteArrayElementAtIndex(index);

                        property.serializedObject.ApplyModifiedProperties();
                    }
                };
            }

            return m_ReorderableList;
        }

        /// <summary>
        /// 绘制属性 GUI。
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // 使用 properyPath 作为折叠状态的唯一标识符
            string foldoutStateKey = $"{property.propertyPath}_FoldoutState";
            bool wasExpanded = SessionState.GetBool(foldoutStateKey, false);
            bool isExpanded = EditorGUI.Foldout(foldoutRect, wasExpanded, label, true);
            SessionState.SetBool(foldoutStateKey, isExpanded);

            if (isExpanded)
            {
                ReorderableList list = GetReorderableList(property);

                if (list != null)
                {
                    // 调整列表位置
                    Rect listRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, list.GetHeight());
                    list.DoList(listRect);
                }
                else
                {
                    EditorGUI.LabelField(position, "Error: Unable to create ReorderableList.");
                }
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// 获取属性的高度。
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string foldoutStateKey = $"{property.propertyPath}_FoldoutState";
            bool isExpanded = SessionState.GetBool(foldoutStateKey, false);

            ReorderableList list = GetReorderableList(property);
            if (list != null && isExpanded)
            {
                return EditorGUIUtility.singleLineHeight + 4.0f + list.GetHeight();
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + 2.0f;
            }
        }
    }
}
#endif