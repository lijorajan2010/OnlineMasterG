using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
	public class TypeValidator
	{
		#region Constructors
		public TypeValidator()
		{
		}
		#endregion

		#region Methods
		public static bool IsCreditCard(string cardTypeCode, string cardNumber)
		{
			byte[] number = new byte[16]; // number to validate

			// Remove non-digits
			int len = 0;
			for (int i = 0; i < cardNumber.Length; i++)
			{
				if (char.IsDigit(cardNumber, i))
				{
					if (len == 16) return false; // number has too many digits
					number[len++] = byte.Parse(Convert.ToString(cardNumber[i]));
				}
			}

			// Validate based on card type, first if tests length, second tests prefix
			switch (cardTypeCode)
			{
				// MasterCard
				case "116":
					if (len != 16)
						return false;
					if (number[0] != 5 || number[1] == 0 || number[1] > 5)
						return false;
					break;

				// Visa
				case "64":
					if (len != 16 && len != 13)
						return false;
					if (number[0] != 4)
						return false;
					break;

				// American
				case "65":
					if (len != 15)
						return false;
					if (number[0] != 3 || (number[1] != 4 && number[1] != 7))
						return false;
					break;

				// Discover
				case "66":
					if (len != 16)
						return false;
					if (number[0] != 6 || number[1] != 0 || number[2] != 1 || number[3] != 1)
						return false;
					break;

				// Diners
				case "63":
					if (len != 14)
						return false;
					if (number[0] != 3 || (number[1] != 0 && number[1] != 6 && number[1] != 8)
						|| number[1] == 0 && number[2] > 5)
						return false;
					break;
			}

			// Use Luhn Algorithm to validate
			int sum = 0;
			for (int i = len - 1; i >= 0; i--)
			{
				if (i % 2 == len % 2)
				{
					int n = number[i] * 2;
					sum += (n / 10) + (n % 10);
				}
				else
					sum += number[i];
			}
			return (sum % 10 == 0);
		}

		public static bool IsDate(string s)
		{
			DateTime d;

			if (s.Trim() == "")
				return false;
			else
			{
				try
				{
					string prefix = s;

					// If has spaces in the middle, cut them off..
					if (prefix.IndexOf(" ") != -1)
						prefix = prefix.Substring(0, s.IndexOf(" "));

					// Check agains dd/mm/yyyy
					if (Regex.Match(prefix, @"^\d{1,2}/\d{1,2}/\d{4}$").Success)
						d = DateTime.Parse(s);
					else if (IsAlphaDate(s))
						d = DateTime.Parse(s);
					else
						return false;

					// Check for smalldatetime
					if (d.Year < 1900 || d.Year > 2078)
						return false;

					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		public static bool IsAlphaDate(string s)
		{
			string[] parts = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			// Three parts
			if (parts.Length < 3)
				return false;

			string day = parts[0].Trim();
			string month = parts[1].Trim();
			string year = parts[2].Trim();

			// Numerid day
			if (!IsNumeric(day))
				return false;

			// Numeric year
			if (!IsNumeric(year))
				return false;

			// Day range
			if (Convert.ToInt32(day) < 1 || Convert.ToInt32(day) > 31)
				return false;

			// Month name
			string[] monthNames = (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames);
			bool foundMonth = false;

			foreach (string m in monthNames)
			{
				if (m.ToLower() == month.ToLower())
				{
					foundMonth = true;
					break;
				}
			}

			if (!foundMonth)
				return false;

			return true;
		}

		public static bool IsMonthYear(string s)
		{
			if (s.Trim() == "")
				return false;

			string[] t = s.Split('/');

			if (t.Length != 2)
				return false;

			if (!TypeValidator.IsNumeric(t[0]) || !TypeValidator.IsNumeric(t[1]))
				return false;

			if (Convert.ToInt32(t[0]) < 1 || Convert.ToInt32(t[0]) > 12)
				return false;

			if (t[1].Length == 4 && (Convert.ToInt32(t[1]) < 1900 || Convert.ToInt32(t[0]) > 2100))
				return false;

			return true;
		}

		public static bool IsTime(string s)
		{
			if (s.Trim() == "")
				return false;
			else
			{
				try
				{
					// I comment this line, because the program use al the text to convert.
					// If has spaces in the middle, cut them off..
					string prefix = s;

					if (prefix.IndexOf(" ") != -1)
						prefix = prefix.Substring(0, s.IndexOf(" "));

					// Check against hh:mm
					if (Regex.Match(prefix, @"^\d{1,2}:\d{2}$").Success)
						DateTime.Parse(s);
					else
						return false;

					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		public static bool IsNumeric(string s)
		{
			double d;

			return Double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out d);
		}

		public static bool IsPercent(string s)
		{
			string t = s.Replace("%", "");

			return IsNumeric(t);
		}

		public static bool IsEmail(string s)
		{
			/*
			if (s == null) return false;
			if (s.IndexOf("@") == -1) return false;
			if (s.IndexOf(".") == -1) return false;
			if (s.IndexOf("@.") != -1) return false;

			// if email has more than one @, it's invalid too
			if (s.IndexOf("@") != s.LastIndexOf("@")) return false;

			return true;*/

			try
			{
				var addr = new MailAddress(s);

				return addr.Address == s;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsInt(string s)
		{
			if (s == null || s.Length == 0)
				return false;

			foreach (char c in s)
			{
				if (!Char.IsNumber(c))
					return false;
			}

			return true;
		}

		public static bool IsZip(string s, string country)
		{
			string auxCountry = country != null ? country : "";

			switch (auxCountry.ToUpper())
			{
				case "USA":
				case "UNITED STATES":
					return IsZipFromUS(s);
				case "CAN":
				case "CANADA":
					return IsZipFromCanada(s);
				default:
					return true;
			}
		}

		public static bool IsZipFromUS(string s)
		{
			return Regex.Match(s, @"(?<zip>\d{5})(?:-(?<suffix>\d{4}))?").Success;
		}

		public static bool IsZipFromCanada(string s)
		{
			return Regex.Match(s, @"([A-Za-z]\d[A-Za-z](| |-)\d[A-Za-z]\d)$").Success;
		}

		public static bool IsAlphaNumeric(string s)
		{
			return Regex.Match(s, @"^[a-zA-Z0-9]+$").Success;
		}

		public static bool IsPhone(string s)
		{
			foreach (char c in s.ToCharArray())
			{
				if ((@"0123456789+-/\.() ").IndexOf(c) == -1)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Validates password format.
		/// Password must contain between 8 and 16 characters with at least one alphabetic, one numeric value and 
		/// one special character with at least one of the alphabetic characters capitalized.
		/// Chars Allowed:
		/// - Upper Letters -> A to Z (include Ñ but not include marked letters)
		/// - Lower Letters -> a to z (include ñ but not include marked letters)
		/// - Numbers -> 0 a 9
		/// - Chars -> ! @ # $ %  & * (  ) _ - + =
		/// </summary>
		public static bool IsValidPassword(string s)
		{
			if (s.Trim() == "")
				return false;

			// Can't contain spaces.
			if (s.IndexOf(" ") != -1)
				return false;

			// Validate special chars and length.
			// string pattern = @"^(?=([a-z]|[0-9]|!|@|#|\$|%|&|\*|\(|\)|_|-|\+|=)*(!|@|#|\$|%|&|\*|\(|\)|_|-|\+|=)).{8,16}$";
			string pattern = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*(!|@|#|\$|%|&|\*|\(|\)|_|-|\+|=)).{8,16}$";
			return (Regex.Match(s, pattern).Value == s);
		}

		/// <summary>
		/// Validates login format.
		/// The Login ID must be between 6 and 40 characters and can contain only these special characters: _ . - @
		/// Chars Allowed:
		/// - Upper Letters -> A to Z (not include marked letters)
		/// - Lower Letters -> a to z (not include marked letters)
		/// - Numbers -> 0 a 9
		/// - Dots -> (.)
		/// - Hyphens -> (-)
		/// - Underscores -> (_)
		/// - Ats -> (@)            
		/// </summary>
		public static bool IsValidLogin(string s)
		{
			return IsValidLogin(s, 6);
		}

		public static bool IsValidLogin(string s, int minLength)
		{
			if (s.Trim() == "")
				return false;

			// Can't contain spaces.
			if (s.IndexOf(" ") != -1)
				return false;

			// Validate special chars and length.
			string pattern = @"([a-z]|[0-9]|@|-|_|\.){" + minLength.ToString() + ",40}";

			return (Regex.Match(s, pattern, RegexOptions.IgnoreCase).Value == s);
		}


		/// <summary>
		/// Validates Promo Code format.
		/// The Promo Code must be between up to 8 characters and can contain only letters and/or numbers
		/// Chars Allowed:
		/// - Upper Letters -> A to Z (not include marked letters)
		/// - Lower Letters -> a to z (not include marked letters)
		/// - Numbers -> 0 a 9
		/// </summary>
		public static bool IsValidPromoCode(string s)
		{
			if (s.Trim() == "")
				return false;

			// Can't contain spaces.
			if (s.IndexOf(" ") != -1)
				return false;

			// Validate special chars and length.
			string pattern = @"([a-z]|[0-9]){1,16}";

			return (Regex.Match(s, pattern, RegexOptions.IgnoreCase).Value == s);
		}

		/// <summary>
		/// Service Agreement Prefix format.
		/// The Service Agreement Prefix must have 4 characters and can contain only letters
		/// Chars Allowed:
		/// - Upper Letters -> A to Z (not include marked letters)
		/// - Lower Letters -> a to z (not include marked letters)
		/// </summary>
		public static bool IsValidServiceAgreementPrefix(string s)
		{
			if (s.Trim() == "")
				return false;

			// Can't contain spaces.
			if (s.IndexOf(" ") != -1)
				return false;

			// Validate special chars and length.
			string pattern = @"([a-z]){2,4}";

			return (Regex.Match(s, pattern, RegexOptions.IgnoreCase).Value == s);
		}
		#endregion
	}
}