using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eq.Utility
{
    public delegate HttpResponseMessage Mock(object req);

    public static class MockProvider
    {
        internal static Mock sMock = null;

        public static void EnableTestMode(Mock mock)
        {
#if DEBUG
            sMock = mock;
#endif
        }
    }

    public class ApiAccessHelper<TReq, TRes>
    {
        public enum MethodType
        {
            Get,
            Post
        }

        public string Url { get; set; }
        public MethodType Method { get; set; }
        public TReq RequestEntity { get; set; }

        private Dictionary<string, List<string>> mRequestHeaderDic = new Dictionary<string, List<string>>();

        public void AppendHeader(string headerName, string value)
        {
            List<string> valueList = null;
            if(!mRequestHeaderDic.TryGetValue(headerName.ToLower(), out valueList))
            {
                valueList = new List<string>();
                mRequestHeaderDic[headerName] = valueList;
            }

            valueList.Add(value);
        }

        public void RemoveHeader(string headerName, string value)
        {
            List<string> valueList = null;
            if (mRequestHeaderDic.TryGetValue(headerName.ToLower(), out valueList))
            {
                valueList.Remove(value);
                if(valueList.Count == 0)
                {
                    mRequestHeaderDic.Remove(headerName);
                }
            }
        }

        public virtual HttpResponseMessage ExecuteRaw()
        {
#if DEBUG
            if(MockProvider.sMock != null)
            {
                return MockProvider.sMock(RequestEntity);
            }
#endif
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> responseMessage = null;

            foreach (KeyValuePair<string, List<string>> kvPair in mRequestHeaderDic)
            {
                foreach (string value in kvPair.Value)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(kvPair.Key, value);
                }
            }

            switch (Method)
            {
                case MethodType.Get:
                    responseMessage = httpClient.GetAsync(Url);
                    break;
                case MethodType.Post:
                    {
                        TReq entity = RequestEntity;
                        string entityStr = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                        StringContent entityContent = new StringContent(entityStr, Encoding.UTF8, "application/json");

                        responseMessage = httpClient.PostAsync(Url, entityContent);
                    }
                    break;
            }

            if(responseMessage != null)
            {
                responseMessage.Wait();
            }

            return responseMessage.Result;
        }

        public Task<HttpResponseMessage> ExecuteRawAsync()
        {
#if DEBUG
            if (MockProvider.sMock != null)
            {
                return new Task<HttpResponseMessage>(
                    delegate()
                    {
                        return MockProvider.sMock(RequestEntity);
                    }
                );
            }
#endif
            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> responseMessage = null;

            foreach (KeyValuePair<string, List<string>> kvPair in mRequestHeaderDic)
            {
                foreach (string value in kvPair.Value)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(kvPair.Key, value);
                }
            }

            switch (Method)
            {
                case MethodType.Get:
                    responseMessage = httpClient.GetAsync(Url);
                    break;
                case MethodType.Post:
                    {
                        TReq entity = RequestEntity;
                        string entityStr = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                        StringContent entityContent = new StringContent(entityStr, Encoding.UTF8, "application/json");

                        responseMessage = httpClient.PostAsync(Url, entityContent);
                    }
                    break;
            }

            return responseMessage;
        }

        public virtual TRes Execute()
        {
            HttpResponseMessage responseMessage = ExecuteRaw();
            TRes result = default(TRes);

            if (responseMessage != null)
            {
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent entity = responseMessage.Content;
                    if (entity != null)
                    {
                        Task<string> entityStr = entity.ReadAsStringAsync();
                        entityStr.Wait();
                        if (!string.IsNullOrEmpty(entityStr.Result))
                        {
                            result = Newtonsoft.Json.JsonConvert.DeserializeObject<TRes>(entityStr.Result);
                        }
                    }
                }
            }

            return result;
        }
    }
}
