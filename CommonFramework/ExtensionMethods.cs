using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
	public static class ExtensionMethods
	{
		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this object value)
		{
			if (value == null)
				return DBNull.Value;
			else
				return value;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this int value)
		{
			if (value <= 0)
				return DBNull.Value;
			else
				return value;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this int? value)
		{
			if (!value.HasValue || value.Value <= 0)
				return DBNull.Value;
			else
				return value;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this string value)
		{
			if (value == "")
				return DBNull.Value;
			else
				return value;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this DateTime value)
		{
			if (value == DateTime.MinValue)
				return DBNull.Value;
			else
				return value;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static object ToDBNull(this DateTime? value)
		{
			if (!value.HasValue)
				return DBNull.Value;
			else
				return value;
		}

		/// <summary>
		/// Returns an int that represents the current object
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough]
		public static int ToInt(this object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return 0;
			}
			else
			{
				if (value is decimal)
				{
					return Convert.ToInt32(value);
				}
				else
				{
					return ToInt(value.ToString());
				}
			}
		}

		/// <summary>
		/// Returns an int that represents the current object
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough]
		public static int ToInt(this string value)
		{
			int ret;
			int.TryParse(value, out ret);
			return ret;
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static int ToInt(this string value, int result)
		{
			int ret;
			return int.TryParse(value, out ret) ? ret : result;
		}

		/// <summary>
		/// Returns a decimal that represents the current object
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough]
		public static decimal ToDecimal(this object value)
		{
			if (value == null || value == DBNull.Value)
				return 0;
			else
				return ToDecimal(value.ToString());
		}

		/// <summary>
		/// Returns a decimal that represents the current object
		/// </summary>
		public static decimal ToDecimal(this string value)
		{
			decimal ret;
			Decimal.TryParse(value, out ret);
			return ret;
		}

		public static decimal ToDecimal(this string value, Decimal result)
		{
			decimal ret;
			return Decimal.TryParse(value, out ret) ? ret : result;
		}

		/// <summary>
		/// Returns a bool that represents the current object
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough]
		public static bool ToBool(this object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return false;
			}
			else
			{
				if (value is int)
					return Convert.ToBoolean(value);
				else
					return Convert.ToBoolean(value.ToString());
			}
		}

		/// <summary>
		/// Returns a bool that represents the current object
		/// </summary>
		public static bool ToBool(this string value)
		{
			if (TypeValidator.IsInt(value))
			{
				return Convert.ToBoolean(ToInt(value));
			}
			else
			{
				bool ret;
				bool.TryParse(value, out ret);
				return ret;
			}
		}

		/// <summary>
		/// Returns a DateTime that represents the current object
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough]
		public static DateTime ToDateTime(this object value)
		{
			if (value == null || value == DBNull.Value || value.ToString() == "" || value.ToString() == "From" || value.ToString() == "To")
				return DateTime.MinValue;
			else
				return Convert.ToDateTime(value);
		}

		public static DateTime ToDateTime(this string value, string dateFormatString)
		{
			var culture = dateFormatString.StartsWith("MM", StringComparison.OrdinalIgnoreCase) ? new CultureInfo("en-US") : new CultureInfo("es-AR");

			DateTime ret;
			DateTime.TryParse(value, culture, DateTimeStyles.None, out ret);
			return ret;
		}

		public static NameValueCollection ToNameValueCollection<TKey, TValue>(
			this IDictionary<TKey, TValue> dict)
		{
			var nameValueCollection = new NameValueCollection();

			foreach (var kvp in dict)
			{
				string value = null;
				if (kvp.Value != null)
					value = kvp.Value.ToString();

				nameValueCollection.Add(kvp.Key.ToString(), value);
			}

			return nameValueCollection;
		}

		public static string ToCultureCase(this string culture)
		{
			if (string.IsNullOrEmpty(culture) || culture.Length != 5)
				return culture;

			return culture.Substring(0, 2).ToLower() + "-" + culture.Substring(3, 2).ToUpper();
		}

		public static DataTable ToStringColumnsDataTable(this DataTable dataTable)
		{
			var dtCloned = dataTable.Clone();

			foreach (DataColumn column in dtCloned.Columns)
			{
				column.DataType = typeof(string);
			}

			foreach (DataRow row in dataTable.Rows)
			{
				dtCloned.ImportRow(row);
			}

			return dtCloned;
		}

		public static string ToStringLog(this Exception exception)
		{
			if (exception == null)
			{
				return "empty";
			}
			return string.Format
				("Exception: {0}\nMessage: {1}\nStack Trace: {2}\nInner {3}", exception.GetType(),
					exception.Message, exception.StackTrace, exception.InnerException.ToStringLog());
		}

		public static string ToSentence(this string input)
		{
			return new string(input.ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray());
		}

		public static List<string> ToList(this string input, char separator)
		{
			return input.Split(separator).Select(m => m.Trim()).ToList();
		}

		public static string ListProperties(this object obj)
		{
			var props = obj.GetType().GetProperties();
			var rows = props.Select(p => p.Name + ": " + p.GetValue(obj, null)).ToList();

			return string.Join(", ", rows);
		}

		public static Dictionary<string, string> GetStringFieldValues(this object obj)
		{
			return obj.GetType()
					  .GetFields(BindingFlags.Public | BindingFlags.Static)
					  .Where(f => f.FieldType == typeof(string))
					  .ToDictionary(f => f.Name,
									f => (string)f.GetValue(null));
		}

		public static KeyValuePair<string, string> GetStringHeadAndTail(this string obj)
		{
			var firstSpaceIndex = obj.IndexOf(" ", StringComparison.Ordinal);

			// If string has no spaces to split by, return empty tail
			if (firstSpaceIndex == -1)
				return new KeyValuePair<string, string>(obj, string.Empty);

			var head = obj.Substring(0, firstSpaceIndex).Trim();
			var tail = obj.Substring(firstSpaceIndex, obj.Length - firstSpaceIndex).Trim();
			var kvp = new KeyValuePair<string, string>(head, tail);

			return kvp;
		}

		public static bool ContainsColumns(this DataTable dataTable, string[] columns)
		{
			foreach (var column in columns)
			{
				var result = dataTable.Columns.Contains(column);

				if (!result) return false;
			}

			return true;
		}

		public static string FilterInput(this string input)
		{
			if (string.IsNullOrEmpty(input))
				return null;

			return input.Trim();
		}
	}
}