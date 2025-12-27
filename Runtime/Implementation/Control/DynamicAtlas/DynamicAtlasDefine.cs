/*
 * Copyright (c) 2024-2025 XDay
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

using UnityEngine;

namespace XDay.GUIAPI
{
    public enum DynamicAtlasGroup
    {
        Size_256 = 256,
        Size_512 = 512,
        Size_1024 = 1024,
        Size_2048 = 2048
    }

    public delegate void OnCallBackTexRect(Texture tex, Rect rect, Vector2 alignmentMinOffset);

    internal static class AtlasHelper
    {
        public static int CeilRound(float v, int alignment)
        {
            return Mathf.CeilToInt(v / alignment) * alignment;
        }

        public static int CeilRound(int v, int alignment)
        {
            if (v % alignment == 0)
            {
                return v;
            }

            return Mathf.CeilToInt(v / alignment) * alignment;
        }

        public static int FloorRound(float v, int alignment)
        {
            return Mathf.FloorToInt(v / alignment) * alignment;
        }

        public static int FloorRound(int v, int alignment)
        {
            if (v % alignment == 0)
            {
                return v;
            }

            return Mathf.FloorToInt(v / alignment) * alignment;
        }

        public static TextureFormat GetTextureFormat()
        {
#if UNITY_EDITOR
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            if (target == UnityEditor.BuildTarget.Android ||
                target == UnityEditor.BuildTarget.iOS)
            {
                return TextureFormat.ASTC_6x6;
            }
            return TextureFormat.DXT5;
#else
            return TextureFormat.ASTC_6x6;
#endif
        }

        public static int GetAlignment()
        {
#if UNITY_EDITOR
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            if (target == UnityEditor.BuildTarget.Android ||
                target == UnityEditor.BuildTarget.iOS)
            {
                return 6;
            }
            return 4;
#else
#if UNITY_OPENHARMONY || UNITY_OPENHARMONY_API
            return 4;
#else
            return 6;
#endif
#endif
        }
    }
}