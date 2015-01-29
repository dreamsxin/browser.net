using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Gecko.Interop;
using Gecko.IO;
using Gecko.Events;

namespace Gecko
{
	public partial class GeckoWebBrowser
		: HwndHost,
		IGeckoWebBrowser,
		// window chrome
		nsIWebBrowserChrome,
		nsIEmbeddingSiteWindow,
		nsIInterfaceRequestor,
		// weak reference creation
		nsISupportsWeakReference,
		nsIDOMEventListener
	{
		private WebProgressListener _webProgressListener=new WebProgressListener();
		private nsIWeakReference _webProgressWeakReference;


		protected override HandleRef BuildWindowCore( HandleRef hwndParent )
		{
			Loaded += new System.Windows.RoutedEventHandler(GeckoWebBrowser_Loaded);
			HwndSourceParameters param = new HwndSourceParameters( "web browser container" );
			param.Width = 100;
			param.Height = 100;
			param.ParentWindow = hwndParent.Handle;
			param.WindowStyle = 0x10000000 | 0x40000000;
			_source = new HwndSource( param );
			return new HandleRef( this, _source.Handle );
		}

		void GeckoWebBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			Xpcom.Initialize();
			_webBrowser = Xpcom.CreateInstance2<nsIWebBrowser>(Contracts.WebBrowser);
			_webBrowserFocus = (nsIWebBrowserFocus)_webBrowser.Instance;
			_baseWindow = (nsIBaseWindow)_webBrowser.Instance;
			_webNav = (nsIWebNavigation)_webBrowser.Instance;
			_webBrowser.Instance.SetContainerWindowAttribute(this);
			_baseWindow.InitWindow(Handle, IntPtr.Zero, 0, 0, (int)ActualWidth, (int)ActualHeight);
			_baseWindow.Create();

			#region nsIWebProgressListener/nsIWebProgressListener2
			Guid nsIWebProgressListenerGUID = typeof(nsIWebProgressListener).GUID;
			Guid nsIWebProgressListener2GUID = typeof(nsIWebProgressListener2).GUID;
            _webProgressListener.OnStateChangeCallback = OnStateChange;
			_webProgressWeakReference = _webProgressListener.GetWeakReference();
			_webBrowser.Instance.AddWebBrowserListener(_webProgressWeakReference, ref nsIWebProgressListenerGUID);
			_webBrowser.Instance.AddWebBrowserListener(_webProgressWeakReference, ref nsIWebProgressListener2GUID);
			#endregion
            _baseWindow.SetVisibilityAttribute(true);
            EventArgs args = new EventArgs();
            OnInitCompleted(args);
		}

        protected virtual void OnInitCompleted(EventArgs e)
        {
            EventHandler<EventArgs> handler = GeckoInitCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Register a listener for a custom jscrip-initiated MessageEvent
        /// https://developer.mozilla.org/en/DOM/document.createEvent
        /// http://help.dottoro.com/ljknkjqd.php
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        /// <example>AddMessageEventListener("callMe", (message=>MessageBox.Show(message)));</example>
        public void AddMessageEventListener(string eventName, Action<string> action)
        {
            AddMessageEventListener(eventName, action, true);
        }

        /// <summary>
        /// Register a listener for a custom jscrip-initiated MessageEvent
        /// https://developer.mozilla.org/en/DOM/document.createEvent
        /// http://help.dottoro.com/ljknkjqd.php
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        /// <example>AddMessageEventListener("callMe", (message=>MessageBox.Show(message)));</example>
        public void AddMessageEventListener(string eventName, Action<string> action, bool useCapture)
        {
            nsIDOMEventTarget target = Xpcom.QueryInterface<nsIDOMEventTarget>(Xpcom.QueryInterface<nsIDOMWindow>(_webBrowser.Instance.GetContentDOMWindowAttribute()).GetWindowRootAttribute());
            if (target != null)
            {
                // the argc parameter is the number of optionial argumetns we are passing. 
                // (useCapture and wantsUntrusted are specified as optional so we always pass 2 when calling interface from C#)
                target.AddEventListener(new nsAString(eventName), this, /*Review*/ useCapture, true, 2);
                _messageEventListeners.Add(eventName, action);
            }
        }

		protected override void DestroyWindowCore( HandleRef hwnd )
		{
			#region nsIWebProgressListener/nsIWebProgressListener2
			_webProgressListener.IsListening = false;
			Guid nsIWebProgressListenerGUID = typeof(nsIWebProgressListener).GUID;
			Guid nsIWebProgressListener2GUID = typeof(nsIWebProgressListener2).GUID;
			_webBrowser.Instance.RemoveWebBrowserListener(_webProgressWeakReference, ref nsIWebProgressListenerGUID);
			_webBrowser.Instance.RemoveWebBrowserListener(_webProgressWeakReference, ref nsIWebProgressListener2GUID);
			_webProgressWeakReference = null;
			_webProgressListener = null;
			#endregion
			//_webNav.Stop(  );
			_webBrowser.FinalRelease();
			_webBrowser.Dispose();
			_webBrowser = null;
			_source.Dispose();
		}

       private void OnStateChange(nsIWebProgress aWebProgress, nsIRequest aRequest, uint aStateFlags, int aStatus) {
			const int NS_BINDING_ABORTED = unchecked((int)0x804B0002);
			
			#region validity checks
			// The request parametere may be null
			if (aRequest == null)
				return;

			// Ignore ViewSource requests, they don't provide the URL
			// see: http://mxr.mozilla.org/mozilla-central/source/netwerk/protocol/viewsource/nsViewSourceChannel.cpp#114
			{
				var viewSource = Xpcom.QueryInterface<nsIViewSourceChannel>( aRequest );
				if ( viewSource != null )
				{
					Marshal.ReleaseComObject( viewSource );
					return;
				}
			}
	
			#endregion validity checks

			var request=Gecko.Net.Request.CreateRequest( aRequest );
			
			#region request parameters
			Uri destUri = null;
			Uri.TryCreate( request.Name, UriKind.Absolute, out destUri );
			var domWindow = aWebProgress.GetDOMWindowAttribute().Wrap( x => new GeckoWindow( x ) );
			bool stateIsRequest = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_REQUEST) != 0);
			bool stateIsDocument = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_DOCUMENT) != 0);
			bool stateIsNetwork = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_NETWORK) != 0);
			bool stateIsWindow = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_WINDOW) != 0);
			#endregion request parameters

			#region STATE_START
			/* This flag indicates the start of a request.
			 * This flag is set when a request is initiated.
			 * The request is complete when onStateChange() is called for the same request with the STATE_STOP flag set.
			 */
			if ((aStateFlags & nsIWebProgressListenerConstants.STATE_START) != 0)
			{

				// TODO: replace to aWebProgress.GetIsTopLevelAttribute() // Gecko 24+
				if (stateIsNetwork && domWindow.IsTopWindow())
				{
					GeckoNavigatingEventArgs ea = new GeckoNavigatingEventArgs(destUri, domWindow);
					OnNavigating(ea);

					if (ea.Cancel)
					{
						aRequest.Cancel(NS_BINDING_ABORTED);
						OnProgressChanged(new GeckoProgressEventArgs(100, 100));
					}
				}
				else if (stateIsDocument)
				{
					GeckoNavigatingEventArgs ea = new GeckoNavigatingEventArgs(destUri, domWindow);
					OnFrameNavigating(ea);

					if (ea.Cancel)
					{
						// TODO: test it on Linux
						if (!Xpcom.IsLinux)
							aRequest.Cancel(NS_BINDING_ABORTED);
					}
				}
			}
			#endregion STATE_START

			#region STATE_REDIRECTING
			/* This flag indicates that a request is being redirected.
			 * The request passed to onStateChange() is the request that is being redirected.
			 * When a redirect occurs, a new request is generated automatically to process the new request.
			 * Expect a corresponding STATE_START event for the new request, and a STATE_STOP for the redirected request.
			 */
			else if ((aStateFlags & nsIWebProgressListenerConstants.STATE_REDIRECTING) != 0)
			{

				// make sure we're loading the top-level window
				GeckoRedirectingEventArgs ea = new GeckoRedirectingEventArgs(destUri, domWindow);
				OnRedirecting(ea);

				if (ea.Cancel)
				{
					aRequest.Cancel(NS_BINDING_ABORTED);
				}
			}
			#endregion STATE_REDIRECTING

			#region STATE_TRANSFERRING
			/* This flag indicates that data for a request is being transferred to an end consumer.
			 * This flag indicates that the request has been targeted, and that the user may start seeing content corresponding to the request.
			 */
			else if ((aStateFlags & nsIWebProgressListenerConstants.STATE_TRANSFERRING) != 0)
			{
			}
			#endregion STATE_TRANSFERRING

			#region STATE_STOP
			/* This flag indicates the completion of a request.
			 * The aStatus parameter to onStateChange() indicates the final status of the request.
			 */
			else if ((aStateFlags & nsIWebProgressListenerConstants.STATE_STOP) != 0)
			{
				if (stateIsNetwork)
				{
					if (aStatus == 0)
					{
						// navigating to a unrenderable file (.zip, .exe, etc.) causes the request pending;
						// also an OnStateChange call with aStatus:804B0004(NS_BINDING_RETARGETED) has been generated previously.
						if (!request.IsPending)
						{
							// kill any cached document and raise DocumentCompleted event
							OnDocumentCompleted(new GeckoDocumentCompletedEventArgs(destUri, domWindow));

							// clear progress bar
							OnProgressChanged(new GeckoProgressEventArgs(100, 100));
						}
					}
					else
					{
						OnNavigationError(new GeckoNavigationErrorEventArgs(request.Name, domWindow, aStatus));
					}
				}

				if (stateIsRequest)
				{
					if ((aStatus & 0xff0000) == ((GeckoError.NS_ERROR_MODULE_SECURITY + GeckoError.NS_ERROR_MODULE_BASE_OFFSET) << 16))
					{
						var ea = new GeckoNSSErrorEventArgs(destUri, aStatus);
						OnNSSError(ea);
						if (ea.Handled)
						{
							aRequest.Cancel(GeckoError.NS_BINDING_ABORTED);
						}
					}

					if (aStatus == GeckoError.NS_BINDING_RETARGETED)
					{
						GeckoRetargetedEventArgs ea = new GeckoRetargetedEventArgs(destUri, domWindow, request);
						OnRetargeted(ea);
					}
				}
			}
			#endregion STATE_STOP

			if (domWindow!=null)
			{
				domWindow.Dispose();
			}
		}

		#region IGeckoWebBrowser

		public GeckoDocument Document
		{
			get 
			{
				if (_webBrowser == null)
					return null;

				nsIWebBrowserChrome chromeBrowser = this;
				nsIWebBrowser browser = chromeBrowser.GetWebBrowserAttribute();
				var domWindow = browser.GetContentDOMWindowAttribute();
				var domDocument = domWindow.GetDocumentAttribute();
				Marshal.ReleaseComObject(domWindow);
				return GeckoDomDocument.CreateDomDocumentWraper(domDocument) as GeckoDocument;
			}
		}

		public GeckoWindow Window
		{
            get
            {
                if (_webBrowser == null)
                    return null;

                nsIWebBrowserChrome chromeBrowser = this;
                nsIWebBrowser browser = chromeBrowser.GetWebBrowserAttribute();

                if (_Window != null)
                {
                    var domWindow = browser.GetContentDOMWindowAttribute();
                    if (_Window.DomWindow == domWindow)
                        return _Window;
                    _Window.Dispose();
                }
                _Window = browser.GetContentDOMWindowAttribute().Wrap(x => new GeckoWindow(x));
                return _Window;
            }
		}

		public bool IsDisposed
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Navigates to the specified URL.
		/// In order to find out when Navigate has finished attach a handler to NavigateFinishedNotifier.NavigateFinished.
		/// </summary>
		/// <param name="url">The url to navigate to.</param>
		public void Navigate(string url)
		{
			Navigate(url, 0, null, null, null);
		}

		/// <summary>
		/// Navigates to the specified URL using the given load flags.
		/// In order to find out when Navigate has finished attach a handler to NavigateFinishedNotifier.NavigateFinished.
		/// </summary>
		/// <param name="url">The url to navigate to.  If the url is empty or null, the browser does not navigate and the method returns false.</param>
		/// <param name="loadFlags">Flags which specify how the page is loaded.</param>
		public bool Navigate(string url, GeckoLoadFlags loadFlags)
		{
			return Navigate(url, loadFlags, null, null, null);
		}

		/// <summary>
		///  Navigates to the specified URL using the given load flags, referrer and post data
		///  In order to find out when Navigate has finished attach a handler to NavigateFinishedNotifier.NavigateFinished.
		/// </summary>
		/// <param name="url">The url to navigate to.  If the url is empty or null, the browser does not navigate and the method returns false.</param>
		/// <param name="loadFlags">Flags which specify how the page is loaded.</param>
		/// <param name="referrer">The referring URL, or null.</param>
		/// <param name="postData">post data and headers, or null</param>
		/// <returns>true if Navigate started. false otherwise.</returns>
		public bool Navigate(string url, GeckoLoadFlags loadFlags, string referrer, MimeInputStream postData)
		{
			return Navigate(url, loadFlags, referrer, postData, null);
		}

		/// <summary>
		///  Navigates to the specified URL using the given load flags, referrer and post data
		///  In order to find out when Navigate has finished attach a handler to NavigateFinishedNotifier.NavigateFinished.
		/// </summary>
		/// <param name="url">The url to navigate to.  If the url is empty or null, the browser does not navigate and the method returns false.</param>
		/// <param name="loadFlags">Flags which specify how the page is loaded.</param>
		/// <param name="referrer">The referring URL, or null.</param>
		/// <param name="postData">post data and headers, or null</param>
		/// <param name="headers">headers, or null</param>
		/// <returns>true if Navigate started. false otherwise.</returns>
		public bool Navigate(string url, GeckoLoadFlags loadFlags, string referrer, MimeInputStream postData, MimeInputStream headers)
		{
			if (string.IsNullOrEmpty(url))
				return false;

			// added these from http://code.google.com/p/geckofx/issues/detail?id=5 so that it will work even if browser isn't currently shown
			//if (!IsHandleCreated) CreateHandle();
			//if (IsBusy) this.Stop();


		//	if (!IsHandleCreated)
		//		throw new InvalidOperationException("Cannot call Navigate() before the window handle is created.");

			// WebNav.LoadURI throws an exception if we try to open a file that doesn't exist...
			Uri created;
			if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out created) && created.IsAbsoluteUri && created.IsFile)
			{
				if (!File.Exists(created.LocalPath) && !Directory.Exists(created.LocalPath))
					return false;
			}

			nsIURI referrerUri = null;
			if (!string.IsNullOrEmpty(referrer))
			{
				//referrerUri = Xpcom.GetService<nsIIOService>("@mozilla.org/network/io-service;1").NewURI(new nsAUTF8String(referrer), null, null);
				referrerUri = IOService.CreateNsIUri(referrer);
			}


			_webNav.LoadURI(url, (uint)loadFlags, referrerUri, postData != null ? postData._inputStream : null, headers != null ? headers._inputStream : null);

			return true;
		}

		public bool GoBack()
		{
			bool ok;
			try
			{
				_webNav.GoBack();
				ok = true;
			}
			catch ( Exception )
			{
				ok = false;
			}
			return ok;

		}

		public bool GoForward()
		{
			bool ok;
			try
			{
				_webNav.GoForward();
				ok = true;
			}
			catch (Exception)
			{
				ok = false;
			}
			return ok;
		}

		public bool Reload()
		{
			bool ok;
			try
			{
				_webNav.Reload(0);
				ok = true;
			}
			catch (Exception)
			{
				ok = false;
			}
			return ok;
		}

		/// <summary>
		/// UI platform independent call function from UI thread
		/// </summary>
		/// <param name="action"></param>
		public void UserInterfaceThreadInvoke(Action action)
		{
			if ( Dispatcher.CheckAccess() )
			{
				action();
			}
			else
			{
				Dispatcher.Invoke( action );
			}
		}

		/// <summary>
		/// UI platform independent call function from UI thread
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public T UserInterfaceThreadInvoke<T>(Func<T> func)
		{
			if (Dispatcher.CheckAccess())
			{
				return func();
			}
			return (T)Dispatcher.Invoke(func);
		}

		

		#endregion

		protected override void OnGotMouseCapture(System.Windows.Input.MouseEventArgs e)
		{
			base.OnGotMouseCapture(e);
		}


		protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
		{
			if ( _baseWindow != null )
			{
				_baseWindow.SetPositionAndSize(0, 0, (int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height, true);
			}
			
			base.OnRenderSizeChanged(sizeInfo);
		}
	}
}
