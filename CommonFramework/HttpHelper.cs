using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
	public static class HttpHelper
	{
		public static string HttpPost(string uri, string parameters)
		{
			// parameters: name1=value1&name2=value2	
			WebRequest webRequest = WebRequest.Create(uri);
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.Method = "POST";

			byte[] bytes = Encoding.ASCII.GetBytes(parameters);
			Stream os = null;

			try
			{
				webRequest.ContentLength = bytes.Length;
				os = webRequest.GetRequestStream();
				os.Write(bytes, 0, bytes.Length);
			}
			catch (WebException ex)
			{
				throw (ex);
			}
			finally
			{
				if (os != null)
				{
					os.Close();
				}
			}

			try
			{
				WebResponse webResponse = webRequest.GetResponse();

				if (webResponse == null)
					return null;

				StreamReader sr = new StreamReader(webResponse.GetResponseStream());

				return sr.ReadToEnd().Trim();
			}
			catch (WebException ex)
			{
				throw (ex);
			}
		}

		public static string SafeUrlEncode(string url)
		{
			var urlEncode = HttpUtility.UrlEncode(url);

			return urlEncode != null ? urlEncode.Replace("'", "%27") : string.Empty;
		}

		public static string SafeUrlDecode(string url)
		{
			return string.IsNullOrEmpty(url) ? string.Empty : HttpUtility.UrlDecode(url.Replace("+", "%2B"));
		}

		public static string GetClientIpAddress()
		{
			var ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (string.IsNullOrEmpty(ipaddress))
				ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

			return ipaddress;
		}

		public static string HttpGet(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.Method = "GET";
			string urlResponse = string.Empty;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				urlResponse = reader.ReadToEnd();
			}

			return urlResponse;
		}

	}
}