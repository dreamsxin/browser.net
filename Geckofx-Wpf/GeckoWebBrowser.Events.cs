using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gecko
{
	partial class GeckoWebBrowser
	{
        public event EventHandler<EventArgs> GeckoInitCompleted;

        #region Event Keys
        // Dom
        public static readonly RoutedEvent DomKeyDownEvent = EventManager.RegisterRoutedEvent(
        "DomKeyDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomKeyUpEvent = EventManager.RegisterRoutedEvent(
        "DomKeyUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomKeyPressEvent = EventManager.RegisterRoutedEvent(
        "DomKeyPress", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseDownEvent = EventManager.RegisterRoutedEvent(
        "DomMouseDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseUpEvent = EventManager.RegisterRoutedEvent(
        "DomMouseUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseOverEvent = EventManager.RegisterRoutedEvent(
        "DomMouseOver", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseOutEvent = EventManager.RegisterRoutedEvent(
        "DomMouseOut", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseMoveEvent = EventManager.RegisterRoutedEvent(
        "DomMouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomContextMenuEvent = EventManager.RegisterRoutedEvent(
        "DomContextMenu", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomMouseScrollEvent = EventManager.RegisterRoutedEvent(
        "DomMouseScroll", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomSubmitEvent = EventManager.RegisterRoutedEvent(
        "DomSubmit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomCompositionStartEvent = EventManager.RegisterRoutedEvent(
        "DomCompositionStart", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomCompositionEndEvent = EventManager.RegisterRoutedEvent(
        "DomCompositionEnd", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomFocusEvent = EventManager.RegisterRoutedEvent(
        "DomFocus", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomBlurEvent = EventManager.RegisterRoutedEvent(
        "DomBlur", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent LoadEvent = EventManager.RegisterRoutedEvent(
        "Load", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DOMContentLoadedEvent = EventManager.RegisterRoutedEvent(
        "DOMContentLoaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent ReadyStateChangeEvent = EventManager.RegisterRoutedEvent(
        "ReadyStateChange", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent HashChangeEvent = EventManager.RegisterRoutedEvent(
        "HashChange", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomContentChangedEvent = EventManager.RegisterRoutedEvent(
        "DomContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomClickEvent = EventManager.RegisterRoutedEvent(
        "DomClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDoubleClickEvent = EventManager.RegisterRoutedEvent(
        "DomDoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragStartEvent = EventManager.RegisterRoutedEvent(
        "DomDragStart", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragEnterEvent = EventManager.RegisterRoutedEvent(
        "DomDragEnter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragOverEvent = EventManager.RegisterRoutedEvent(
        "DomDragOver", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragLeaveEvent = EventManager.RegisterRoutedEvent(
        "DomDragLeave", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragEvent = EventManager.RegisterRoutedEvent(
        "DomDrag", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDropEvent = EventManager.RegisterRoutedEvent(
        "DomDrop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent DomDragEndEvent = EventManager.RegisterRoutedEvent(
        "DomDragEnd", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        public static readonly RoutedEvent FullscreenChangeEvent = EventManager.RegisterRoutedEvent(
        "FullscreenChange", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GeckoWebBrowser));
        #endregion

        #region nsIDOMEventListener Members

        void nsIDOMEventListener.HandleEvent(nsIDOMEvent e)
        {
            if (e == null) return;

            OnHandleDomEvent(DomEventArgs.Create(e));
        }

        protected virtual void OnHandleDomEvent(DomEventArgs e)
        {
            switch (e.Type)
            {
                case "keydown":
                    OnDomKeyDown((DomKeyEventArgs)e);
                    break;
                case "keyup":
                    OnDomKeyUp((DomKeyEventArgs)e);
                    break;
                case "keypress":
                    OnDomKeyPress((DomKeyEventArgs)e);
                    break;
                case "mousedown":
                    OnDomMouseDown((DomMouseEventArgs)e);
                    break;
                case "mouseup":
                    OnDomMouseUp((DomMouseEventArgs)e);
                    break;
                case "mousemove":
                    OnDomMouseMove((DomMouseEventArgs)e);
                    break;
                case "mouseover":
                    OnDomMouseOver((DomMouseEventArgs)e);
                    break;
                case "mouseout":
                    OnDomMouseOut((DomMouseEventArgs)e);
                    break;
                case "click":
                    OnDomClick((DomMouseEventArgs)e);
                    break;
                case "dblclick":
                    OnDomDoubleClick((DomMouseEventArgs)e);
                    break;
                case "submit":
                    OnDomSubmit(e);
                    break;
                case "compositionstart":
                    OnDomCompositionStart(e);
                    break;
                case "compositionend":
                    OnDomCompositionEnd(e);
                    break;
                case "contextmenu":
                    OnDomContextMenu((DomMouseEventArgs)e);
                    break;
                case "DOMMouseScroll":
                    OnDomMouseScroll((DomMouseEventArgs)e);
                    break;
                case "focus":
                    OnDomFocus(e);
                    break;
                case "blur":
                    OnDomBlur(e);
                    break;
                case "load":
                    OnLoad(e);
                    break;
                case "DOMContentLoaded":
                    OnDOMContentLoaded(e);
                    break;
                case "readystatechange":
                    OnReadyStateChange(e);
                    break;
                case "change":
                    OnDomContentChanged(e);
                    break;
                case "hashchange":
                    OnHashChange((DomHashChangeEventArgs)e);
                    break;
                case "dragstart":
                    OnDomDragStart((DomDragEventArgs)e);
                    break;
                case "dragenter":
                    OnDomDragEnter((DomDragEventArgs)e);
                    break;
                case "dragover":
                    OnDomDragOver((DomDragEventArgs)e);
                    break;
                case "dragleave":
                    OnDomDragLeave((DomDragEventArgs)e);
                    break;
                case "drag":
                    OnDomDrag((DomDragEventArgs)e);
                    break;
                case "drop":
                    OnDomDrop((DomDragEventArgs)e);
                    break;
                case "dragend":
                    OnDomDragEnd((DomDragEventArgs)e);
                    break;
                case "mozfullscreenchange": //TODO: change to "fullscreenchange" after prefix removed
                    OnFullscreenChange(e);
                    break;
            }
            if (e is DomMessageEventArgs)
            {
                Action<string> action;
                DomMessageEventArgs mea = (DomMessageEventArgs)e;
                if (_messageEventListeners.TryGetValue(e.Type, out action))
                {
                    action.Invoke(mea.Message);
                }
            }

            if (e != null && e.Cancelable && e.Handled)
                e.PreventDefault();
        }

        #endregion

        #region Dom EventHandlers

        #region Dom keyboard events
        #region public event GeckoDomKeyEventHandler DomKeyDown
        [Category("DOM Events")]
        public event EventHandler<DomKeyEventArgs> DomKeyDown
        {
            add { AddHandler(DomKeyDownEvent, value); }
            remove { RemoveHandler(DomKeyDownEvent, value); }
        }

        /// <summary>Raises the <see cref="DomKeyDown"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomKeyDown(DomKeyEventArgs e)
        {            
            RaiseEvent(new RoutedEventArgs(DomKeyDownEvent, e));
        }
        #endregion

        #region public event GeckoDomKeyEventHandler DomKeyUp
        [Category("DOM Events")]
        public event EventHandler<DomKeyEventArgs> DomKeyUp
        {
            add { AddHandler(DomKeyUpEvent, value); }
            remove { RemoveHandler(DomKeyUpEvent, value); }
        }

        /// <summary>Raises the <see cref="DomKeyUp"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomKeyUp(DomKeyEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomKeyUpEvent, e));
        }
        #endregion

        #region public event GeckoDomKeyEventHandler DomKeyPress
        [Category("DOM Events")]
        public event EventHandler<DomKeyEventArgs> DomKeyPress
        {
            add { AddHandler(DomKeyPressEvent, value); }
            remove { RemoveHandler(DomKeyPressEvent, value); }
        }

        /// <summary>Raises the <see cref="DomKeyPress"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomKeyPress(DomKeyEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomKeyPressEvent, e));
        }
        #endregion
        #endregion

        #region Dom mouse events
        #region public event GeckoDomMouseEventHandler DomMouseDown
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseDown
        {
            add { AddHandler(DomMouseDownEvent, value); }
            remove { RemoveHandler(DomMouseDownEvent, value); }
        }

        /// <summary>Raises the <see cref="DomMouseDown"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseDown(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseDownEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DomMouseUp
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseUp
        {
            add { AddHandler(DomMouseUpEvent, value); }
            remove { RemoveHandler(DomMouseUpEvent, value); }
        }

        /// <summary>Raises the <see cref="DomMouseUp"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseUp(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseUpEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DomMouseOver
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseOver
        {
            add { AddHandler(DomMouseOverEvent, value); }
            remove { RemoveHandler(DomMouseOverEvent, value); }
        }


        /// <summary>Raises the <see cref="DomMouseOver"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseOver(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseOverEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DomMouseOut
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseOut
        {
            add { AddHandler(DomMouseOutEvent, value); }
            remove { RemoveHandler(DomMouseOutEvent, value); }
        }

        /// <summary>Raises the <see cref="DomMouseOut"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseOut(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseOutEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DomMouseMove
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseMove
        {
            add { AddHandler(DomMouseMoveEvent, value); }
            remove { RemoveHandler(DomMouseMoveEvent, value); }
        }

        /// <summary>Raises the <see cref="DomMouseMove"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseMove(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseMoveEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DomContextMenu
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomContextMenu
        {
            add { AddHandler(DomContextMenuEvent, value); }
            remove { RemoveHandler(DomContextMenuEvent, value); }
        }

        /// <summary>Raises the <see cref="DomContextMenu"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomContextMenu(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomContextMenuEvent, e));
        }
        #endregion

        #region public event GeckoDomMouseEventHandler DOMMouseScroll
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomMouseScroll
        {
            add { AddHandler(DomMouseScrollEvent, value); }
            remove { RemoveHandler(DomMouseScrollEvent, value); }
        }

        /// <summary>Raises the <see cref="DomMouseScroll"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomMouseScroll(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomMouseScrollEvent, e));
        }
        #endregion
        #endregion

        #region public event GeckoDomEventHandler DomSubmit
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomSubmit
        {
            add { AddHandler(DomSubmitEvent, value); }
            remove { RemoveHandler(DomSubmitEvent, value); }
        }

        /// <summary>Raises the <see cref="DomSubmit"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomSubmit(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomSubmitEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomCompositionStart
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomCompositionStart
        {
            add { AddHandler(DomCompositionStartEvent, value); }
            remove { RemoveHandler(DomCompositionStartEvent, value); }
        }

        /// <summary>Raises the <see cref="DomCompositionStart"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomCompositionStart(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomCompositionStartEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomCompositionEnd
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomCompositionEnd
        {
            add { AddHandler(DomCompositionEndEvent, value); }
            remove {RemoveHandler(DomCompositionEndEvent, value); }
        }

        /// <summary>Raises the <see cref="DomCompositionEnd"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomCompositionEnd(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomCompositionEndEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomFocus
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomFocus
        {
            add { AddHandler(DomFocusEvent, value); }
            remove { RemoveHandler(DomFocusEvent, value); }
        }

        /// <summary>Raises the <see cref="DomFocus"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomFocus(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomFocusEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomBlur
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomBlur
        {
            add { AddHandler(DomBlurEvent, value); }
            remove { RemoveHandler(DomBlurEvent, value); }
        }

        /// <summary>Raises the <see cref="DomBlur"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomBlur(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomBlurEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler Load
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> Load
        {
            add { AddHandler(LoadEvent, value); }
            remove { RemoveHandler(LoadEvent, value); }
        }

        /// <summary>Raises the <see cref="LoadEvent"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnLoad(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LoadEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DOMContentLoaded
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DOMContentLoaded
        {
            add { AddHandler(DOMContentLoadedEvent, value); }
            remove { RemoveHandler(DOMContentLoadedEvent, value); }
        }

        /// <summary>Raises the <see cref="DOMContentLoadedEvent"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDOMContentLoaded(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DOMContentLoadedEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler ReadyStateChange
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> ReadyStateChange
        {
            add { AddHandler(ReadyStateChangeEvent, value); }
            remove { RemoveHandler(ReadyStateChangeEvent, value); }
        }

        /// <summary>Raises the <see cref="ReadyStateChangeEvent"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnReadyStateChange(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ReadyStateChangeEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler HashChange
        [Category("DOM Events")]
        public event EventHandler<DomHashChangeEventArgs> HashChange
        {
            add { AddHandler(HashChangeEvent, value); }
            remove { RemoveHandler(HashChangeEvent, value); }
        }

        /// <summary>Raises the <see cref="HashChangeEvent"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnHashChange(DomHashChangeEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(HashChangeEvent, e));
        }
        #endregion

        #region drag events

        // DragStart

        public event EventHandler<DomDragEventArgs> DomDragStart
        {
            add { AddHandler(DomDragStartEvent, value); }
            remove { RemoveHandler(DomDragStartEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDragStart"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDragStart(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragStartEvent, e));
        }

        // DragEnter

        public event EventHandler<DomDragEventArgs> DomDragEnter
        {
            add { AddHandler(DomDragEnterEvent, value); }
            remove { RemoveHandler(DomDragEnterEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDragEnter"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDragEnter(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragEnterEvent, e));
        }

        // DragOver

        public event EventHandler<DomDragEventArgs> DomDragOver
        {
            add { AddHandler(DomDragOverEvent, value); }
            remove { RemoveHandler(DomDragOverEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDragOver"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDragOver(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragOverEvent, e));
        }

        // DragLeave

        public event EventHandler<DomDragEventArgs> DomDragLeave
        {
            add { AddHandler(DomDragLeaveEvent, value); }
            remove { RemoveHandler(DomDragLeaveEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDragLeave"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDragLeave(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragLeaveEvent, e));
        }

        // Drag

        public event EventHandler<DomDragEventArgs> DomDrag
        {
            add { AddHandler(DomDragEvent, value); }
            remove { RemoveHandler(DomDragEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDrag"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDrag(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragEvent, e));
        }

        // Drop

        public event EventHandler<DomDragEventArgs> DomDrop
        {
            add { AddHandler(DomDropEvent, value); }
            remove { RemoveHandler(DomDropEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDrop"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDrop(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDropEvent, e));
        }

        // DragEnd

        public event EventHandler<DomDragEventArgs> DomDragEnd
        {
            add { AddHandler(DomDragEndEvent, value); }
            remove { RemoveHandler(DomDragEndEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDragEnd"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDragEnd(DomDragEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDragEndEvent, e));
        }

        #endregion

        #region public event GeckoDomEventHandler DomContentChanged
        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> DomContentChanged
        {
            add { AddHandler(DomContentChangedEvent, value); }
            remove { RemoveHandler(DomContentChangedEvent, value); }
        }

        /// <summary>Raises the <see cref="DomContentChanged"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomContentChanged(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomContentChangedEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomClick
        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomClick
        {
            add { AddHandler(DomClickEvent, value); }
            remove { RemoveHandler(DomClickEvent, value); }
        }

        /// <summary>Raises the <see cref="DomClick"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomClick(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomClickEvent, e));
        }
        #endregion

        #region public event GeckoDomEventHandler DomDoubleClick

        [Category("DOM Events")]
        public event EventHandler<DomMouseEventArgs> DomDoubleClick
        {
            add { AddHandler(DomDoubleClickEvent, value); }
            remove { RemoveHandler(DomDoubleClickEvent, value); }
        }

        /// <summary>Raises the <see cref="DomDoubleClick"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDomDoubleClick(DomMouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DomDoubleClickEvent, e));
        }

        #endregion public event GeckoDomEventHandler DomDoubleClick

        #region public event GeckoDomEventHandler FullscreenChange

        [Category("DOM Events")]
        public event EventHandler<DomEventArgs> FullscreenChange
        {
            add { AddHandler(FullscreenChangeEvent, value); }
            remove { RemoveHandler(FullscreenChangeEvent, value); }
        }

        /// <summary>Raises the <see cref="FullscreenChange"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnFullscreenChange(DomEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FullscreenChangeEvent, e));
        }

        #endregion public event GeckoDomEventHandler FullscreenChange

        #endregion
	}
}
