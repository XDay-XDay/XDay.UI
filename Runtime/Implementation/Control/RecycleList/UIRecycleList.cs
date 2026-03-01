/*
 * Copyright (c) 2024-2026 XDay
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XDay.GUIAPI
{
    public enum ListDirection
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 可重复使用item的滚动列表,挂在ScrollRect上
    /// TODO:
    ///     1.优化滚动时视野检查
    /// </summary>
    [AddComponentMenu("XDay/UI/XDay Recycle List", 0)]
    [ExecuteInEditMode]
    public class UIRecycleList : MonoBehaviour
    {
        [Header("Item")]
        public GameObject ListItemPrefab;
        [Header("Padding")]
        public float LeftPadding;
        public float RightPadding;
        public float TopPadding;
        public float BottomPadding;
        [Tooltip("自动计算Padding,Vertical时自动计算Left/Right Padding, Horizontal时自动计算Top/Bottom Padding")]
        public bool AutoCalculate = true;
        [Header("Spacing")]
        public Vector2 Spacing;
        [Header("Direction")]
        public ListDirection Direction = ListDirection.Vertical;
        public int GlobalPointer => m_LayoutBuilder.GlobalPointer;

        private void Init()
        {
            var scrollRect = GetComponent<ScrollRect>();
            if (Direction == ListDirection.Vertical)
            {
                m_LayoutBuilder = new RecycleListVerticalLayoutBuilder();
                scrollRect.horizontal = false;
                scrollRect.vertical = true;
            }
            else if (Direction == ListDirection.Horizontal)
            {
                m_LayoutBuilder = new RecycleListHorizontalLayoutBuilder();
                scrollRect.horizontal = true;
                scrollRect.vertical = false;
            }
            m_LayoutBuilder.Init(scrollRect, ListItemPrefab, LeftPadding, RightPadding, TopPadding, BottomPadding, Spacing, AutoCalculate);
        }

        private void OnDestroy()
        {
            m_LayoutBuilder?.OnDestroy();
        }

        public void SetData<View, Controller>(List<object> data)
            where View : UIView
            where Controller : UIControllerBase
        {
            if (m_LayoutBuilder == null)
            {
                Init();
            }

            m_LayoutBuilder.SetData<View, Controller>(data);
        }

        public void SetScroll(Vector2 normalizedPosition)
        {
            m_LayoutBuilder?.SetScroll(normalizedPosition);
        }

        public void MoveScroll(Vector2 normalizedPosition)
        {
            m_LayoutBuilder?.MoveScroll(normalizedPosition);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isEditor && !Application.isPlaying && ListItemPrefab != null)
            {
                //假设item的anchor必须是center,pivot是0.5,0.5
                var transform = ListItemPrefab.transform as RectTransform;
                var itemWidth = transform.rect.width;
                var itemHeight = transform.rect.height;
                var contentTransform = transform.parent as RectTransform;
                var contentWidth = contentTransform.rect.width;
                var deltaX = (contentWidth - itemWidth) * 0.5f;
                transform.anchoredPosition = new Vector2(LeftPadding - deltaX, TopPadding - itemHeight * 0.5f);
            }
        }
#endif

        private RecycleListLayoutBuilder m_LayoutBuilder;
    }
}