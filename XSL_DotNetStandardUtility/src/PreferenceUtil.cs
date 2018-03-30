using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace Eq.Utility
{
    public static class PreferenceUtil
    {
        private static readonly string DefaultPrefName = "pref.json";
        private static object sSemaphore = new object();
        private static IsolatedStorageFile sIsolatedStorageFile;
        private static Dictionary<string, object> sCachedDictionary = null;

        public static bool Read<T>(string key, out T value)
        {
            bool ret = false;

            lock (sSemaphore)
            {
                Initialize();

                value = default(T);
                if (sCachedDictionary.TryGetValue(key, out object tempObject))
                {
                    Type outParamType = typeof(T);

                    if (outParamType.Equals(typeof(object)) || outParamType.Equals(tempObject.GetType()))
                    {
                        value = (T)tempObject;
                        ret = true;
                    }
                }
            }

            return ret;
        }

        public static void Write<T>(string key, T value)
        {
            lock (sSemaphore)
            {
                Initialize();

                //if (!sCachedDictionary.TryAdd(key, value))
                {
                    sCachedDictionary.Remove(key);
                    sCachedDictionary.Add(key, value);
                }
            }
        }

        public static void Flush()
        {
            lock (sSemaphore)
            {
                if (sIsolatedStorageFile != null)
                {
                    try
                    {
                        IsolatedStorageFileStream stream = sIsolatedStorageFile.OpenFile(DefaultPrefName, System.IO.FileMode.Open);
                        byte[] raw = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(sCachedDictionary));
                        stream.Write(raw, 0, raw.Length);
                        stream.Flush();
                        stream.Close();
                    }
                    catch (FileNotFoundException)
                    {
                        // 処理なし
                    }
                }
            }
        }

        private static void Initialize()
        {
            if (sIsolatedStorageFile == null)
            {
                sIsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream stream = sIsolatedStorageFile.OpenFile(DefaultPrefName, System.IO.FileMode.OpenOrCreate);
                StreamReader reader = new StreamReader(stream);
                try
                {
                    sCachedDictionary = (Dictionary<string, object>)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(Dictionary<string, object>));
                }
                catch (JsonSerializationException)
                {
                    // 処理なし
                }
                catch (JsonReaderException)
                {
                    // 処理なし
                }
                finally
                {
                    if (sCachedDictionary == null)
                    {
                        sCachedDictionary = new Dictionary<string, object>();
                    }
                }
                reader.Close();
                stream.Close();
            }
        }

        private static void ClearCache()
        {
            lock (sSemaphore)
            {
                sCachedDictionary.Clear();
                if (sIsolatedStorageFile != null)
                {
                    sIsolatedStorageFile.Close();
                    sIsolatedStorageFile = null;
                }
            }
        }
    }
}
