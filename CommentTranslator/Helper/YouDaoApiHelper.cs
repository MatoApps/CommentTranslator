using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommentTranslator.Model;
using Newtonsoft.Json;

namespace CommentTranslator.Helper
{
    internal class YouDaoApiHelper
    {
        const string _appKey = "75766d8fc97f34a3";
        const string _from = "auto";
        const string _to = "auto";
        const string _appSecret = "rFkTqsDws1bCoETcxSL7afG33emwJdr5";

        public static async Task<YouDaoTranslationData> GetWordsAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);

            WebRequest translationWebRequest = WebRequest.Create(requestUrl);

            var response = await translationWebRequest.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException(), Encoding.GetEncoding("utf-8")))
                {
                    string result = reader.ReadToEnd();
                    var youDaoTranslationResponse = JsonConvert.DeserializeObject<YouDaoTranslationResponse>(result);

                    return new YouDaoTranslationData { ResultDetail = result, YouDaoTranslation = youDaoTranslationResponse };
                }
            }
        }

        private static string GetRequestUrl(string queryText)
        {
            string salt = DateTime.Now.Millisecond.ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = _appKey + queryText + salt + _appSecret;
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");

            var requestUrl = string.Format(
                "http://openapi.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}",
                _appKey,
                queryText,
                _from, _to, sign, salt);

            return requestUrl;
        }
    }

    public class YouDaoTranslationData
    {
        public YouDaoTranslationResponse YouDaoTranslation { get; set; }
        public string ResultDetail { get; set; }
    }
}
