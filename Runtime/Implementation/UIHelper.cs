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

using System;
using System.Text;
using UnityEngine;
using XDay.UtilityAPI.Math;

namespace XDay.GUIAPI
{
    public static class UIHelper
    {
        /// <summary>
        /// 将anchor position转换成local position,local position是以父节点的pivot为中心点的相对坐标, anchor position是以相对父节点的anchor为中心点的相对坐标
        /// </summary>
        /// <param name="anchorPosition">自己的anchor position</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public static Vector2 AnchorToLocalPosition(Vector2 anchorPosition, RectTransform parent)
        {
            var pivotDelta = new Vector2(0.5f, 0.5f) - parent.pivot;
            anchorPosition.x += parent.rect.width * pivotDelta.x;
            anchorPosition.y += parent.rect.height * pivotDelta.y;
            return anchorPosition;
        }

        /// <summary>
        /// 让ui遮盖一个世界空间下的平面
        /// </summary>
        /// <param name="sceneCamera"></param>
        /// <param name="uiCamera"></param>
        /// <param name="coverUI"></param>
        public static void CoverWorldPlane(Camera sceneCamera,
            Camera uiCamera,
            RectTransform coverUI,
            Vector3 worldMin,
            Vector3 worldMax,
            float worldZ)
        {
            if (coverUI != null)
            {
                worldMin.z = worldZ;
                worldMax.z = worldZ;

                Vector2 screenPointMin;
                Vector2 screenPointMax;
                if (uiCamera != null)
                {
                    var sl = sceneCamera.WorldToViewportPoint(worldMin);
                    var st = sceneCamera.WorldToViewportPoint(worldMax);
                    screenPointMin = uiCamera.ViewportToScreenPoint(sl);
                    screenPointMax = uiCamera.ViewportToScreenPoint(st);
                }
                else
                {
                    screenPointMin = sceneCamera.WorldToViewportPoint(worldMin);
                    screenPointMax = sceneCamera.WorldToViewportPoint(worldMax);
                }
                var parentTransform = coverUI.parent.transform as RectTransform;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, screenPointMin, uiCamera, out var localMin);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, screenPointMax, uiCamera, out var localMax);
                var size = localMax - localMin;
                coverUI.anchoredPosition = localMin + size * coverUI.pivot;
                coverUI.sizeDelta = size;
            }
        }

        public static Canvas GetFirstCanvas(GameObject obj)
        {
            var trans = obj.transform;
            while (trans != null)
            {
                if (trans.TryGetComponent<Canvas>(out var canvas))
                {
                    return canvas;
                }
                trans = trans.parent;
            }
            return null;
        }

        /// <summary>
        /// ui world position是canvas空间的坐标
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Vector3 ScreenToUIWorldPosition(Vector2 screenPosition, Canvas canvas)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, screenPosition, null, out var worldPos);
            return worldPos;
        }

        public static Vector3 WorldToUIWorldPosition(Vector3 worldPos, Canvas canvas, Camera camera)
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(camera, worldPos);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, screenPos, null, out var canvasPos);
            return canvasPos;
        }

        /// <summary>
        /// 将rect transform的rect转换到屏幕空间
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Rect LocalToUIWorldRect(RectTransform transform, Camera camera)
        {
            Vector3[] worldCorners = new Vector3[4];
            transform.GetWorldCorners(worldCorners);

            FloatBounds2D bounds = new();
            for (var i = 0; i < worldCorners.Length; ++i)
            {
                var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldCorners[i]);
                bounds.AddPoint(screenPoint.x, screenPoint.y);
            }
            return new Rect(bounds.Min, bounds.Size);
        }

        public static string FormatTimeNow()
        {
            var now = DateTime.Now;
            var hourStr = now.Hour.ToString("D2");
            var minuteStr = now.Minute.ToString("D2");
            return $"{hourStr}:{minuteStr}";
        }

        public static string FormatTime(this TimeSpan time)
        {
            var sb = new StringBuilder(128);
            if (time.Days > 0)
            {
                sb.AppendFormat("", time.Days);
                sb.AppendFormat("{0:D2}:", time.Hours);
            }
            else if (time.Hours > 0)
            {
                sb.AppendFormat("{0:D2}:", time.Hours);
            }

            var seconds = Mathf.CeilToInt(time.Seconds + time.Milliseconds / 1000.0f);
            if (seconds >= 60)
            {
                seconds = 59;
            }

            sb.AppendFormat("{0:D2}:{1:D2}", time.Minutes, seconds);
            return sb.ToString();
        }
    }
}
