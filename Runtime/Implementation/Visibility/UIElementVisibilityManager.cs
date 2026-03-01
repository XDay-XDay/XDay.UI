using System.Collections.Generic;
using UnityEngine;

namespace XDay.GUIAPI
{
    /// <summary>
    /// 检测UI元素的可见性
    /// </summary>
    public class UIElementVisibilityManager
    {
        public void Register(IUIElementVisibilityChecker checker)
        {
            m_ConditionKeyToChecker.Add(checker.ConditionKey, checker);
        }

        public void AddGameObjectCondition(GameObject go, string conditionKey)
        {
            m_GameObjectConditionKeys.TryGetValue(go, out var keys);
            if (keys == null)
            {
                keys = new List<string>();
                m_GameObjectConditionKeys[go] = keys;
            }
            if (!keys.Contains(conditionKey))
            {
                keys.Add(conditionKey);
            }
        }

        public void RemoveGameObjectCondition(GameObject go, string conditionKey)
        {
            m_GameObjectConditionKeys.TryGetValue(go, out var keys);
            keys?.Remove(conditionKey);
        }

        /// <summary>
        /// 刷新UI元素可见性
        /// </summary>
        public void Refresh()
        {
            foreach (var kv in m_GameObjectConditionKeys)
            {
                RefreshOne(kv.Key);
            }
        }

        public void RefreshOne(GameObject go)
        {
            if (m_GameObjectConditionKeys.TryGetValue(go, out var conditionKeys))
            {
                var visible = true;
                foreach (var key in conditionKeys)
                {
                    if (m_ConditionKeyToChecker.TryGetValue(key, out var checker))
                    {
                        var visibleThis = checker.IsVisible();
                        if (!visibleThis)
                        {
                            ///条件全满足才能显示
                            visible = false;
                            break;
                        }
                    }
                    else
                    {
                        Log.Instance?.Error($"Invalid condition key: {key}");
                    }
                }
                go.SetActive(visible);
            }
        }

        private readonly Dictionary<GameObject, List<string>> m_GameObjectConditionKeys = new();
        private readonly Dictionary<string, IUIElementVisibilityChecker> m_ConditionKeyToChecker = new();
    }
}
