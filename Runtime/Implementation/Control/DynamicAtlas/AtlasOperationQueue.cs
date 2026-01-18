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
using System.Diagnostics;
using XDay.GUIAPI;

namespace XDay.GUIAPI
{
    internal class AtlasOperationQueue
    {
        public void Add(UIImage image)
        {
            m_Queue.Add(image);
        }

        public void Remove(UIImage image)
        {
            for (var i = 0; i < m_Queue.Count; i++)
            {
                if (m_Queue[i] == image)
                {
                    m_Queue[i] = m_Queue[^1];
                    m_Queue.RemoveAt(m_Queue.Count - 1);
                }
            }
        }

        public void Update()
        {
            var n = 0;
            m_Stopwatch.Restart();
            for (var i = m_Queue.Count - 1; i >= 0; i--)
            {
                var image = m_Queue[i];
                if (image != null)
                {
                    image.Process();
                    ++n;
                }
                m_Queue.RemoveAt(i);
                var timeCost = m_Stopwatch.ElapsedMilliseconds;
                if (timeCost > m_MaxCostMs)
                {
                    //UnityEngine.Debug.LogError($"处理了{n}个,耗时{timeCost}毫秒,在第{UnityEngine.Time.frameCount}帧");
                    break;
                }
            }
        }

        private List<UIImage> m_Queue = new();
        private Stopwatch m_Stopwatch = new();
        private const int m_MaxCostMs = 10;
    }
}
