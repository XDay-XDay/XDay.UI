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

using System.Collections.Generic;

namespace XDay.GUIAPI
{
    internal class DynamicAtlasSet
    {
        public string Name => m_Name;

        public DynamicAtlasSet(string name, DynamicAtlasGroup size)
        {
            m_Name = name;
            GetDynamicAtlas(size, name);
        }

        public void OnDestroy()
        {
            foreach (var kv in m_DynamicAtlasMap)
            {
                kv.Value.OnDestroy();
            }
            m_DynamicAtlasMap.Clear();
        }

        public void Clear()
        {
            foreach (var kv in m_DynamicAtlasMap)
            {
                kv.Value.Clear();
            }
        }

        public DynamicAtlas GetDynamicAtlas(DynamicAtlasGroup group, string name)
        {
            DynamicAtlas atlas;
            if (m_DynamicAtlasMap.ContainsKey(group))
            {
                atlas = m_DynamicAtlasMap[group];
            }
            else
            {
                atlas = new DynamicAtlas(group, alignment: AtlasHelper.GetAlignment(), name);
                m_DynamicAtlasMap[group] = atlas;
            }
            return atlas;
        }

        internal Dictionary<DynamicAtlasGroup, DynamicAtlas> Atlases => m_DynamicAtlasMap;

        private readonly string m_Name;
        private readonly Dictionary<DynamicAtlasGroup, DynamicAtlas> m_DynamicAtlasMap = new();
    }
}
