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

using UnityEngine;

namespace XDay.GUIAPI
{
    internal class DynamicAtlasMgrBehaviour : MonoBehaviour
    {
        void Update()
        {
            DynamicAtlasMgr.S.Update();
        }

        private void OnApplicationPause(bool pause)
        {
            DynamicAtlasMgr.S.Pause = pause;
        }

        private void OnApplicationFocus(bool focus)
        {
            DynamicAtlasMgr.S.Pause = !focus;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(DynamicAtlasMgrBehaviour))]
    internal class DynamicAtlasMgrEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var kv in DynamicAtlasMgr.S.Sets)
            {
                DrawSet(kv.Key, kv.Value);
            }
        }

        private void DrawSet(DynamicAtlasController d, DynamicAtlasSet set)
        {
            UnityEditor.EditorGUILayout.Foldout(true, $"Set: {set.Name}");
            UnityEditor.EditorGUI.indentLevel++;
            UnityEditor.EditorGUILayout.LabelField($"使用者: {d.name}");
            foreach (var atlas in set.Atlases.Values)
            {
                DrawAtlas(atlas);
            }
            UnityEditor.EditorGUI.indentLevel--;
        }

        private void DrawAtlas(DynamicAtlas atlas)
        {
            foreach (var page in atlas.Pages)
            {
                DrawPage(page);
            }
        }

        private void DrawPage(DynamicAtlasPage page)
        {
            UnityEditor.EditorGUILayout.Foldout(true, $"Page: {page.Index}");
            UnityEditor.EditorGUI.indentLevel++;
            UnityEditor.EditorGUILayout.IntField("Free List", page.FreeAreasList.Count);
            UnityEditor.EditorGUILayout.ObjectField("Texture", page.Texture, typeof(Texture2D), false);
            UnityEditor.EditorGUI.indentLevel--;
        }
    }
#endif
}
