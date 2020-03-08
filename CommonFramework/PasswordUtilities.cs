using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace OnlineMasterG.CommonFramework
{
	public class PasswordUtilities
	{
		#region Constants
		private static readonly char[] LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		private static readonly char[] NUMBERS = "1234567890".ToCharArray();
		private static readonly char[] SYMBOLS = "!@#$%&*()_-+=".ToCharArray();
		#endregion

		#region Class Members
		private static RNGCryptoServiceProvider _Random = new RNGCryptoServiceProvider();
		private static byte[] _bytes = new byte[4];
		#endregion

		#region Members
		int m_minimumLength, m_maximumLength;
		bool m_includeUpper, m_includeLower, m_includeNumber, m_includeSpecial;
		string[] m_characterTypes;
		#endregion

		#region Enums
		enum CharacterType
		{
			Uppercase,
			Lowercase,
			Special,
			Number
		}
		#endregion

		#region Properties
		public bool IncludeUpper
		{
			get
			{
				return m_includeUpper;
			}
			set
			{
				m_includeUpper = value;
			}
		}

		public bool IncludeLower
		{
			get
			{
				return m_includeLower;
			}
			set
			{
				m_includeLower = value;
			}
		}

		public bool IncludeNumber
		{
			get
			{
				return m_includeNumber;
			}
			set
			{
				m_includeNumber = value;
			}
		}

		public bool IncludeSpecial
		{
			get
			{
				return m_includeSpecial;
			}
			set
			{
				m_includeSpecial = value;
			}
		}

		public int MinimumLength
		{
			get
			{
				return m_minimumLength;
			}
			set
			{
				if (value > m_maximumLength)
				{
					throw new ArgumentOutOfRangeException("MinimumLength must be greater than MaximumLength");
				}
				m_minimumLength = value;
			}
		}

		public int MaximumLength
		{
			get
			{
				return m_maximumLength;
			}
			set
			{
				if (value < m_minimumLength)
				{
					throw new ArgumentOutOfRangeException("MaximumLength must be greater than MinimumLength");
				}
				m_maximumLength = value;
			}
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance for generating alphanumeric passwords.
		/// </summary>
		public PasswordUtilities()
		{
			m_includeSpecial = false;
			m_includeNumber = true;
			m_includeUpper = true;
			m_includeLower = true;
		}

		/// <summary>
		/// Creates an instance for generating passwords including the selected character types.
		/// </summary>
		public PasswordUtilities(bool includeSpecial, bool includeNumber, bool includeUpper, bool includeLower)
			: this()
		{
			m_includeNumber = includeNumber;
			m_includeSpecial = includeSpecial;
			m_includeUpper = includeUpper;
			m_includeLower = includeLower;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Randomly creates a password with size between minimum and maximum values.
		/// </summary>
		/// <returns>A random string of characters.</returns>
		public string GeneratePassword(int minimumLength, int maximumLength)
		{
			m_characterTypes = GetCharacterTypes();

			StringBuilder password = new StringBuilder(maximumLength);

			//Get a random length for the password.
			int currentPasswordLength = maximumLength;//RandomUtilities.Next(m_maximumLength);

			//Only allow for passwords greater than or equal to the minimum length.
			/*if (currentPasswordLength < m_minimumLength)
			{
				currentPasswordLength = m_minimumLength;
			}*/

			//Generate the password
			for (int i = 0; i < currentPasswordLength; i++)
			{
				password.Append(GetCharacter());
			}

			return password.ToString();
		}

		/// <summary>
		/// Randomly creates a password with a length of 6 characters.
		/// </summary>
		/// <returns>A random string of characters.</returns>
		public string GeneratePassword()
		{
			return this.GeneratePassword(6, 6);
		}

		/// <summary>
		/// Determines which character types should be used to generate
		/// the current password.
		/// </summary>
		/// <returns>A string[] of character that should be used to generate the current password.</returns>
		private string[] GetCharacterTypes()
		{
			ArrayList characterTypes = new ArrayList();
			foreach (string characterType in Enum.GetNames(typeof(CharacterType)))
			{
				CharacterType currentType = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
				bool addType = false;
				switch (currentType)
				{
					case CharacterType.Lowercase:
						addType = IncludeLower;
						break;
					case CharacterType.Number:
						addType = IncludeNumber;
						break;
					case CharacterType.Special:
						addType = IncludeSpecial;
						break;
					case CharacterType.Uppercase:
						addType = IncludeUpper;
						break;
				}
				if (addType)
				{
					characterTypes.Add(characterType);
				}
			}
			return (string[])characterTypes.ToArray(typeof(string));
		}

		/// <summary>
		/// Randomly determines a character type to return from the 
		/// available CharacterType enum.
		/// </summary>
		/// <returns>The string character to append to the password.</returns>
		private string GetCharacter()
		{
			string characterType = m_characterTypes[PasswordUtilities.Next(m_characterTypes.Length)];
			CharacterType typeToGet = (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);
			switch (typeToGet)
			{
				case CharacterType.Lowercase:
					return LETTERS[PasswordUtilities.Next(LETTERS.Length)].ToString().ToLower();
				case CharacterType.Uppercase:
					return LETTERS[PasswordUtilities.Next(LETTERS.Length)].ToString().ToUpper();
				case CharacterType.Number:
					return NUMBERS[PasswordUtilities.Next(NUMBERS.Length)].ToString();
				case CharacterType.Special:
					return SYMBOLS[PasswordUtilities.Next(SYMBOLS.Length)].ToString();
			}
			return null;
		}

		/// <summary>
		/// Randomly selects an int value
		/// </summary>
		/// <returns>A randomly selected int value.</returns>
		public static int Next(int max)
		{
			if (max <= 0)
			{
				throw new ArgumentOutOfRangeException("max");
			}
			_Random.GetBytes(_bytes);
			int value = BitConverter.ToInt32(_bytes, 0) % max;
			if (value < 0)
			{
				value = -value;
			}
			return value;
		}

		#region Ecryption
		public static string CreateHashNoSalt(string plainText)
		{
			if (plainText.Trim().Length == 0)
				return "";
			else
				return Cryptographer.CreateHash("hashprovider_nosalt", plainText);
		}

		public static string CreateHash(string plainText)
		{
			if (plainText.Trim().Length == 0)
				return "";
			else
				return Cryptographer.CreateHash("hashprovider", plainText);
		}

		public static bool CompareHash(string plainText, string hashedText)
		{
			return Cryptographer.CompareHash("hashprovider", plainText, hashedText);
		}
		#endregion

		#endregion
	}
}