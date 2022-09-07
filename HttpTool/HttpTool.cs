using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ZTools;

namespace ZTools
{
    /// <summary>
    /// HTTP 工具类
    /// </summary>
    public class HttpTool : MonoBehaviour
    {
        /// <summary>
        /// 实例化
        /// </summary>
        private static HttpTool instance = null;
        private static object oLock = new object();
        public static HttpTool Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (oLock)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "HttpTool";
                        HttpTool httpTool = obj.AddComponent<HttpTool>();
                        HttpTool.instance = httpTool;
                    }
                }
                return instance;
            }
        }
        int timeOut = 10;
        /// <summary>
        /// 设置当前超时时间
        /// </summary>
        public int TimeOut
        {
            set { timeOut = value; }
            get { return timeOut; }
        }
        string Token = "";
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="token"></param>
        public void SetHeader(string token)
        {
            Token = token;
        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void Get<T>(string url, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            StartCoroutine(GetRequest<T>(url, callback, needToken));
        }
        private IEnumerator GetRequest<T>(string url, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                //设置header
                this.SetHeader(webRequest, needToken);
                webRequest.timeout = timeOut;
                yield return webRequest.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
#elif UNITY_2017_1_OR_NEWER
                if (webRequest.isHttpError || webRequest.isNetworkError)
#endif
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    if (null != callback)
                    {
                        string requestText = webRequest.downloadHandler.text;
                        Debug.Log("url = " + url + "\n返回值 = " + requestText + "\nToken = " + needToken);
                        ResponseResult<T> requestData = JsonMapper.ToObject<ResponseResult<T>>(requestText);
                        if (requestData == null)
                        {
                            Debug.LogError("get请求的值为空");
                        }
                        else
                        {
                            callback(requestData);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="jsonString">为json字符串，post提交的数据包为json</param>
        /// <param name="callback">数据返回</param>
        public void Post<T>(string url, dynamic jsonString, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            StartCoroutine(PostRequest<T>(url, jsonString, callback, needToken));
        }
        private IEnumerator PostRequest<T>(string url, dynamic jsonString, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonMapper.ToJson(jsonString));
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.timeout = timeOut;
                this.SetHeader(webRequest, needToken);
                yield return webRequest.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
#elif UNITY_2017_1_OR_NEWER
                if (webRequest.isHttpError || webRequest.isNetworkError)
#endif
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    if (callback != null)
                    {
                        string requestText = webRequest.downloadHandler.text;
                        ResponseResult<T> requestData = JsonMapper.ToObject<ResponseResult<T>>(requestText);
                        Debug.Log("url = " + url + "\njsonString = " + jsonString + "\n返回值 = " + requestText + "\nToken = " + needToken);
                        if (null == requestData)
                        {
                            Debug.LogError("数据返回值为空");
                        }
                        else
                        {
                            callback(requestData);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="jsonString">为json字符串，post提交的数据包为json</param>
        /// <param name="callback">数据返回</param>
        public void Put<T>(string url, dynamic jsonString, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            StartCoroutine(PutRequest(url, jsonString, callback, needToken));
        }
        private IEnumerator PutRequest<T>(string url, dynamic jsonString, Action<ResponseResult<T>> callback, bool needToken = true)
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonMapper.ToJson(jsonString));
            using (UnityWebRequest webRequest = UnityWebRequest.Put(url, bodyRaw))
            {
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.timeout = timeOut;
                this.SetHeader(webRequest, needToken);
                yield return webRequest.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
#elif UNITY_2017_1_OR_NEWER
                if (webRequest.isHttpError || webRequest.isNetworkError)
#endif
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    string requestText = webRequest.downloadHandler.text;
                    ResponseResult<T> requestData = JsonMapper.ToObject<ResponseResult<T>>(requestText);
                    Debug.Log("url = " + url + "\njsonString = " + jsonString + "\n返回值 = " + requestText + "\nToken = " + needToken);
                    if (null == requestData)
                    {
                        Debug.LogError("数据返回值为空");
                    }
                    else
                    {
                        callback(requestData);
                    }
                }
            }
        }
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="path">下载路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fun">回调，可获得下载进度</param>
        /// <param name="needToken">是否需要token</param>
        public void DownloadFile(string url, string path, string fileName, Action<float> fun, bool needToken = true)
        {
            StartCoroutine(DownloadRequestData(url, path, fileName, fun, needToken));
        }
        IEnumerator DownloadRequestData(string url, string path, string fileName, Action<float> fun, bool needToken = true)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                string filePath = Path.Combine(path, fileName);
                webRequest.downloadHandler = new DownloadHandlerFile(filePath);
                this.SetHeader(webRequest, needToken);
                webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    if (fun != null)
                    {
                        fun(webRequest.downloadProgress);
                    }
                    yield return null;
                }
                fun(1);
            }
        }
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="receiveFunction"></param>
        public void DownloadFile(string url, string path, Action<float> fun, bool needToken = true)
        {
            StartCoroutine(DownloadRequestData(url, path, fun, needToken));
        }
        IEnumerator DownloadRequestData(string url, string path, Action<float> fun, bool needToken = true)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                webRequest.downloadHandler = new DownloadHandlerFile(path);
                this.SetHeader(webRequest, needToken);
                webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    if (fun != null)
                    {
                        fun(webRequest.downloadProgress);
                    }
                    yield return null;
                }
                fun(1);
            }
        }
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="webRequest"></param>
        private void SetHeader(UnityWebRequest webRequest, bool needToken = true)
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (needToken)
            {
                webRequest.SetRequestHeader("Authorization", Token);
            }
        }
    }
}
