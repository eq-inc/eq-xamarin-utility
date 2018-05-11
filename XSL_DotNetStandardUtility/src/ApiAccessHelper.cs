using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eq.Utility
{
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

        public virtual TRes Execute()
        {
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

            TRes result = default(TRes);

            if (responseMessage != null)
            {
                responseMessage.Wait();
                if (responseMessage.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent entity = responseMessage.Result.Content;
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

        public async Task<TRes> ExecuteAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = null;

            foreach(KeyValuePair<string, List<string>> kvPair in mRequestHeaderDic)
            {
                foreach(string value in kvPair.Value)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(kvPair.Key, value);
                }
            }

            switch (Method)
            {
                case MethodType.Get:
                    responseMessage = await httpClient.GetAsync(Url);
                    break;
                case MethodType.Post:
                    {
                        TReq entity = RequestEntity;
                        string entityStr = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                        StringContent entityContent = new StringContent(entityStr, Encoding.UTF8, "application/json");

                        responseMessage = await httpClient.PostAsync(Url, entityContent);
                    }
                    break;
            }

            TRes result = default(TRes);

            if(responseMessage != null)
            {
                if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent entity = responseMessage.Content;
                    if(entity != null)
                    {
                        string entityStr = await entity.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(entityStr))
                        {
                            result = Newtonsoft.Json.JsonConvert.DeserializeObject<TRes>(entityStr);
                        }
                    }
                }
            }

            return result;
        }
    }
}
