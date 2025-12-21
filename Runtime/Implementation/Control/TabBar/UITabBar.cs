using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XDay.GUIAPI
{
    [Serializable]
    internal class TabItem
    {
        public string Name;
        public GameObject Active;
        public GameObject InActive;
        public List<GameObject> Clickables = new();
    }

    [AddComponentMenu("XDay/UI/XDay TabBar", 0)]
    public class UITabBar : MonoBehaviour
    {
        public event Action<int, string> EventOnActivate;

        private void Start()
        {
            Init();

            Apply();
        }

        private void Init()
        {
            var idx = 0;
            foreach (var item in m_Items)
            {
                if (item.Clickables.Count == 0)
                {
                    Debug.LogError("TabBar no click object set");
                }
                else
                {
                    foreach (var c in item.Clickables)
                    {
                        var listener = c.GetComponent<UIEventListener>();
                        if (listener == null)
                        {
                            listener = c.AddComponent<UIEventListener>();
                        }
                        var i = idx;
                        listener.AddClickEvent((pointerData) =>
                        {
                            OnClickTabButton(i);
                        });
                    }
                    ++idx;
                }
            }
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

            Debug.LogError($"Tab item not found: {name}");
        }

        private void Apply()
        {
            for (var i = 0; i < m_Items.Count; ++i)
            {
                if (i == m_ActiveIndex)
                {
                    m_Items[i].Active.SetActive(true);
                    m_Items[i].InActive.SetActive(false);
                }
                else
                {
                    m_Items[i].Active.SetActive(false);
                    m_Items[i].InActive.SetActive(true);
                }
            }

            if (m_ActiveIndex >= 0 && m_ActiveIndex < m_Items.Count)
            {
                EventOnActivate?.Invoke(m_ActiveIndex, m_Items[m_ActiveIndex].Name);
            }
        }

        private void OnClickTabButton(int i)
        {
            SetActiveIndex(i);
        }

        internal List<TabItem> Items => m_Items;
        internal int ActiveIndex => m_ActiveIndex;

        [SerializeField]
        private List<TabItem> m_Items = new();
        [HideInInspector]
        [SerializeField]
        private int m_ActiveIndex = 0;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UITabBar))]
    class UITabBarEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GetNames();

            EditorGUI.BeginChangeCheck();
            var t = target as UITabBar;
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
            var t = target as UITabBar;
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
