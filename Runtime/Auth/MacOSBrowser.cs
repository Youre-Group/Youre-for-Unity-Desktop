using Utils;

namespace Auth
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using System.Threading.Tasks;
    
    using System;
    using System.Diagnostics;
    using UnityEngine;
    using WebView2Forms;
    using System.IO;
    using WebView2;
    using System.Threading;
    using YourePlugin;
    using Debug = UnityEngine.Debug;
    public class MacOSBrowser : Browser
    {
        private bool _connected = false;
        private string _targetUrl;
        private SocketServer _socketServer;
        
        protected override void Launch(string url)
        {
            _targetUrl = url;
            CreateWebView();
            SendClientData();
        }

        
        
        private void SendClientData()
        {
            Console.WriteLine("_socketServer Start!");
            _socketServer = new SocketServer();
            _socketServer.Connected += OnConnected;
            _socketServer.Disconnected += OnDisconnected;
            _socketServer.MessageReceived += OnMessage;
            _socketServer.Start();
        }
        
        private void StartMacOSWebViewProcess()
        {
            try
            {
                string commandArguments = $"\"{WebViewProcessBinPath}\" --args \"{_targetUrl}\"";
               // Process.Start("killall", "YoureLogin");
                Process.Start("open", commandArguments);
                Debug.Log($"Successfully launched WebViewProcessBinPath: {WebViewProcessBinPath}");
                Debug.Log($"Successfully launched _targetUrl: {_targetUrl}");
                Debug.Log(_targetUrl);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to launch app: {e.Message}");
            }
        }
        
        public void CreateWebView()
        {
            Application.runInBackground = true;
            StartMacOSWebViewProcess();
        }

        private static string WebViewProcessBinPath
        {
            get
            {
            #if UNITY_EDITOR
                        return Path.GetFullPath(@"Packages/Youre-for-Unity-Desktop/macOSWebView/YoureLogin.app");
            #else
                        return Application.dataPath + @"\YoureLogin.app";
            #endif
            }
        }

        private void OnConnected()
        {
            Debug.Log("OnConnected");
        }
        
        private void OnMessage(string inputString)
        {
            Youre.LogDebug("OnMessage:" + inputString);
            if (!string.IsNullOrEmpty(inputString))
            {
                if (inputString.Contains("keycloak_callback"))
                {
                    string url = inputString.Substring(4);
                    OnAuthReply(url);
                }
            }
        }
        
        private void OnDisconnected()
        {
            Youre.LogDebug("Auth process disconnected");
            OnAuthReply("");
            DestroyWebView();
        }
        
        public override void Dismiss()
        {
            DestroyWebView();
        }

        public void DestroyWebView()
        {
            try
            {
                _socketServer.MessageReceived -= OnMessage;
                _socketServer.Connected -= OnConnected;
                _socketServer.Disconnected -= OnDisconnected;
                _connected = false;
                Process.Start("killall", "YoureLogin");
                Debug.Log($"Successfully stopped: YoureLogin");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to stop app: {e.Message}");
            }

        }
        

    }
    
    
    
}