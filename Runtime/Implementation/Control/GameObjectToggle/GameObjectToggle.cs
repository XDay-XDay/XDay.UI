using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XDay.GUIAPI
{
    [System.Serializable]
    internal class ToggleItem
    {
        public string Name;
        public List<GameObject> GameObjects = new();
    }

    [AddComponentMenu("XDay/UI/XDay GameObjectToggle", 0)]
    public class GameObjectToggle : MonoBehaviour
    {
        public string Description;
        internal List<ToggleItem> Items => m_Items;
        internal int ActiveIndex => m_ActiveIndex;

        private void Awake()
        {
            Apply();
        }

        public void SetActiveIndex(int index)
        {
            if (m_ActiveIndex != index)
            {
                m_ActiveIndex = index;
                Apply();
            }
        }

        public void SetActive(string name)
        {
            for (var i = 0; i < m_Items.Count; ++i)
            {
                if (m_Items[i].Name == name)
                {
                    SetActiveIndex(i);
                    return;
                }
            }

            Debug.LogError($"Toggle item not found: {name}");
        }

        private void Apply()
        {
            for (var i = 0; i < m_Items.Count; ++i)
            {
                if (i == m_ActiveIndex)
                {
                    foreach (var obj in m_Items[i].GameObjects)
                    {
                        obj.SetActive(true);
                    }
                }
                else
                {
                    foreach (var obj in m_Items[i].GameObjects)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }

        [SerializeField]
        private List<ToggleItem> m_Items;
        [HideInInspector]
        [SerializeField]
        private int m_ActiveIndex = 0;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameObjectToggle))]
    class GameObjectToggleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GetNames();

            EditorGUI.BeginChangeCheck();
            var t = target as GameObjectToggle;
            t.SetActiveIndex(EditorGUILayout.Popup("当前", t.ActiveIndex, m_Names));
            if ((m_Names.Length == 0 && t.ActiveIndex >= 0) || t.ActiveIndex >= m_Names.Length)
            {
                t.SetActiveIndex(-1);
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(t.gameObject);
            }

            base.OnInspectorGUI();
        }

        private void GetNames()
        {
            var t = target as GameObjectToggle;
            if (m_Names == null || m_Names.Length != t.Items.Count)
            {
                m_Names = new string[t.Items.Count];
            }

            for (var i = 0; i < m_Names.Length; i++)
            {
                m_Names[i] = t.Items[i].Name;
            }
        }

        private string[] m_Names;
    }
#endif
}
