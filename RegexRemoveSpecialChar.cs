public static string RemoveSpecialCharacters(string str)
{
	return Regex.Replace(str, "[^a-zA-Z0-9_ ]+", "", RegexOptions.Compiled);
}