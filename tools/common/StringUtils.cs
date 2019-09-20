using System;
using System.Text;

namespace Xamarin.Utils {
	internal class StringUtils {
		static StringUtils ()
		{
			PlatformID pid = Environment.OSVersion.Platform;
			if (((int)pid != 128 && pid != PlatformID.Unix && pid != PlatformID.MacOSX))
				shellQuoteChar = '"'; // Windows
			else
				shellQuoteChar = '\''; // !Windows
		}

		static char shellQuoteChar;
		static char[] mustQuoteCharacters = new char [] { ' ', '\'', ',', '$', '\\' };
		static char [] mustQuoteCharactersProcess = { ' ', '\\', '"' };

		public static string[] Quote (params string[] array)
		{
			if (array == null || array.Length == 0)
				return array;

			var rv = new string [array.Length];
			for (var i = 0; i < array.Length; i++)
				rv [i] = Quote (array [i]);
			return rv;
		}

		public static string Quote (string f)
		{
			if (String.IsNullOrEmpty (f))
				return f ?? String.Empty;

			if (f.IndexOfAny (mustQuoteCharacters) == -1)
				return f;

			var s = new StringBuilder ();

			s.Append (shellQuoteChar);
			foreach (var c in f) {
				if (c == '\'' || c == '"' || c == '\\')
					s.Append ('\\');

				s.Append (c);
			}
			s.Append (shellQuoteChar);

			return s.ToString ();
		}

		public static string [] QuoteForProcess (params string [] array)
		{
			if (array == null || array.Length == 0)
				return array;

			var rv = new string [array.Length];
			for (var i = 0; i < array.Length; i++)
				rv [i] = QuoteForProcess (array [i]);
			return rv;
		}

		// Quote input according to how System.Diagnostics.Process needs it quoted.
		public static string QuoteForProcess (string f)
		{
			if (String.IsNullOrEmpty (f))
				return f ?? String.Empty;

			if (f.IndexOfAny (mustQuoteCharactersProcess) == -1)
				return f;

			var s = new StringBuilder ();

			s.Append ('"');
			foreach (var c in f) {
				if (c == '"' || c == '\\')
					s.Append ('\\');

				s.Append (c);
			}
			s.Append ('"');

			return s.ToString ();
		}

		public static string Unquote (string input)
		{
			if (input == null || input.Length == 0 || input [0] != shellQuoteChar)
				return input;

			var builder = new StringBuilder ();
			for (int i = 1; i < input.Length - 1; i++) {
				char c = input [i];
				if (c == '\\') {
					builder.Append (input [i + 1]);
					i++;
					continue;
				}
				builder.Append (input [i]);
			}
			return builder.ToString ();
		}

		// Version.Parse requires, minimally, both major and minor parts.
		// However we want to accept `11` as `11.0`
		public static Version ParseVersion (string v)
		{
			int major;
			if (int.TryParse (v, out major))
				return new Version (major, 0);
			return Version.Parse (v);
		}

		public static string SanitizeObjectiveCName (string name)
		{
			StringBuilder sb = null;

			for (int i = 0; i < name.Length; i++) {
				var ch = name [i];
				switch (ch) {
				case '.':
				case '+':
				case '/':
				case '`':
				case '@':
				case '<':
				case '>':
				case '$':
				case '-':
					if (sb == null)
						sb = new StringBuilder (name, 0, i, name.Length);
					sb.Append ('_');
					break;
				default:
					if (sb != null)
						sb.Append (ch);
					break;
				}
			}

			if (sb != null)
				return sb.ToString ();

			return name;
		}
	}

	static class StringExtensions
	{
		internal static string [] SplitLines (this string s) => s.Split (new [] { Environment.NewLine }, StringSplitOptions.None);

		// Adds an element to an array and returns a new array with the added element.
		// The original array is not modified.
		// If the original array is null, a new array is also created, with just the new value.
		internal static T [] CopyAndAdd<T>(this T[] array, T value)
		{
			if (array == null || array.Length == 0)
				return new T [] { value };
			var tmpArray = array;
			Array.Resize (ref array, array.Length + 1);
			tmpArray[tmpArray.Length - 1] = value;
			return tmpArray;
		}
	}
}
