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
    internal class SaveTextureData
    {
        public int texIndex = -1;
        public int referenceCount = 0;
        public Rect rect;
        //为了对齐而产生的偏移
        public Vector2 alignmentMinOffset;
    }

    internal class GetTextureData
    {
        public string name;
        public OnCallBackTexRect callback;
    }

    internal class IntegerRectangle
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int Right => x + width;
        public int Top => y + height;
        public int Size => width * height;

        public IntegerRectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return string.Format("x{0}_y:{1}_width:{2}_height{3}_top:{4}_right{5}", x, y, width, height, Top, Right);
        }
    }
}