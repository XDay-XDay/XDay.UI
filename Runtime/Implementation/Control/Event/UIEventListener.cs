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
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XDay.GUIAPI
{
    public enum UIEventType
    {
        Click,
        Down,
        Up,
        Enter,
        Exit,
        Drag,
        BeginDrag,
        EndDrag,
    }

    [AddComponentMenu("XDay/UI/XDay Event Listener", 0)]
    public class UIEventListener : MonoBehaviour, 
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IDragHandler,
        IBeginDragHandler,
        IEndDragHandler
    {
        public static event UnityAction<GameObject, UIEventType, int> OnUIEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            Trigger(m_OnClick, eventData, UIEventType.Click);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Trigger(m_OnDown, eventData, UIEventType.Down);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Trigger(m_OnEnter, eventData, UIEventType.Enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Trigger(m_OnExit, eventData, UIEventType.Exit);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Trigger(m_OnUp, eventData, UIEventType.Up);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Trigger(m_OnDrag, eventData, UIEventType.Drag);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Trigger(m_OnBeginDrag, eventData, UIEventType.BeginDrag);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Trigger(m_OnEndDrag, eventData, UIEventType.EndDrag);
        }

        public void AddClickEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnClick ??= new(displayKey);
            m_OnClick.AddHandler(handler);
        }

        public void RemoveClickEvent(UnityAction<PointerEventData> handler)
        {
            m_OnClick?.RemoveHandler(handler);
        }

        public void AddUpEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnUp ??= new(displayKey);
            m_OnUp.AddHandler(handler);
        }

        public void RemoveUpEvent(UnityAction<PointerEventData> handler)
        {
            m_OnUp?.RemoveHandler(handler);
        }

        public void AddDownEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnDown ??= new(displayKey);
            m_OnDown.AddHandler(handler);
        }

        public void RemoveDownEvent(UnityAction<PointerEventData> handler)
        {
            m_OnDown?.RemoveHandler(handler);
        }

        public void AddEnterEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnEnter ??= new(displayKey);
            m_OnEnter.AddHandler(handler);
        }

        public void RemoveEnterEvent(UnityAction<PointerEventData> handler)
        {
            m_OnEnter?.RemoveHandler(handler);
        }

        public void AddExitEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnExit ??= new(displayKey);
            m_OnExit.AddHandler(handler);
        }

        public void RemoveExitEvent(UnityAction<PointerEventData> handler)
        {
            m_OnExit?.RemoveHandler(handler);
        }

        public void AddDragEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnDrag ??= new(displayKey);
            m_OnDrag.AddHandler(handler);
        }

        public void RemoveDragEvent(UnityAction<PointerEventData> handler)
        {
            m_OnDrag?.RemoveHandler(handler);
        }

        public void AddBeginDragEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnBeginDrag ??= new(displayKey);
            m_OnBeginDrag.AddHandler(handler);
        }

        public void RemoveBeginDragEvent(UnityAction<PointerEventData> handler)
        {
            m_OnBeginDrag?.RemoveHandler(handler);
        }

        public void AddEndDragEvent(UnityAction<PointerEventData> handler, int displayKey = 0)
        {
            m_OnEndDrag ??= new(displayKey);
            m_OnEndDrag.AddHandler(handler);
        }

        public void RemoveEndDragEvent(UnityAction<PointerEventData> handler)
        {
            m_OnEndDrag?.RemoveHandler(handler);
        }

        private void Trigger(Event e, PointerEventData data, UIEventType type)
        {
            if (e == null)
            {
                return;
            }

            e.Trigger(data);
            OnUIEvent?.Invoke(gameObject, type, e.DisplayKey);
        }

        private Event m_OnClick;
        private Event m_OnUp;
        private Event m_OnDown;
        private Event m_OnEnter;
        private Event m_OnExit;
        private Event m_OnDrag;
        private Event m_OnBeginDrag;
        private Event m_OnEndDrag;

        private class Event
        {
            public int DisplayKey => m_DisplayKey;

            public Event(int displayKey)
            {
                m_DisplayKey = displayKey;
            }

            public void AddHandler(UnityAction<PointerEventData> handler)
            {
                m_EventHandler ??= new();
                m_EventHandler.AddListener(handler);
            }

            public void RemoveHandler(UnityAction<PointerEventData> handler)
            {
                m_EventHandler?.RemoveListener(handler);
            }

            public void Trigger(PointerEventData data)
            {
                m_EventHandler?.Invoke(data);
            }

            private UnityEvent<PointerEventData> m_EventHandler;
            private readonly int m_DisplayKey;
        }
    }
}
