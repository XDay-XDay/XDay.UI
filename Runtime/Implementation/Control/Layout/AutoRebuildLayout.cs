using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XDay.GUIAPI
{
    /// <summary>
    /// 挂到有content size fitter时显示warning的节点上
    /// </summary>
    internal class AutoRebuildLayout : UIBehaviour
    {
        /// <summary>
        /// 向上查找的layout group层级数
        /// </summary>
        public int NestLevel = 1;

        protected override void OnRectTransformDimensionsChange()
        {
            var layoutGroup = FindAncestorLayoutGroup();
            if (layoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);
            }
        }

        private LayoutGroup FindAncestorLayoutGroup()
        {
            LayoutGroup ret = null;
            var cur = transform;
            var level = NestLevel;
            while (level > 0)
            {
                var layoutGroup = cur.parent.GetComponentInParent<LayoutGroup>();
                if (layoutGroup == null)
                {
                    break;
                }
                ret = layoutGroup;
                --level;
                cur = layoutGroup.transform as RectTransform;
            }
            return ret;
        }
    }
}
