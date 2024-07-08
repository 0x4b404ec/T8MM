    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace T8MM.Utils;

public static class FileUtils
{
	public static void WriteJsonObjectInFile(string path, object obj)
	{
		var json = JsonConvert.SerializeObject(obj);
		WriteAllText(path, json, Encoding.UTF8);
	}
	
	public static T? ReadJsonObjectInFile<T>(string path) where T : class
	{
		if (!File.Exists(path)) return null;
		var json = ReadAllText(path, Encoding.UTF8);
		return JsonConvert.DeserializeObject<T>(json);
	}
	
	public static void WriteEncryptJsonObjectInFile(string path, object obj)
	{
		var json = JsonConvert.SerializeObject(obj);
#if DEBUG
		WriteAllText(path, json, Encoding.UTF8);
#else
		var content = LocalCryptography.Encryp(json);
		WriteAllText(path, content, Encoding.UTF8);
#endif
	}
	
	public static T? ReadDecryptJsonObjectInFile<T>(string path) where T : class
	{
		// if (!File.Exists(path)) return default(T);
		if (!File.Exists(path)) return null;
		var json = ReadAllText(path, Encoding.UTF8);
#if DEBUG
		return JsonConvert.DeserializeObject<T>(json);
#else
		var content = LocalCryptography.Decrypt(json);
		return JsonConvert.DeserializeObject<T>(content);
#endif
	}
	
	public static bool CreateDirectoryIfNotExists(string directoryPath)
	{
		bool result = !Directory.Exists(directoryPath);
		if (result)
		{
			try
			{
				Directory.CreateDirectory(directoryPath);
			}
			catch (IOException ex)
			{
				throw ex;
			}
		}
		return result;
	}

	public static void DeleteFileIfExists(string filePath)
	{
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	public static string ReadAllText(string path, Encoding encoding)
	{
		using var streamReader = new StreamReader(path, encoding);
		return streamReader.ReadToEnd();
	}
	
	public static void WriteAllText(string path, string contents, Encoding encoding)
	{
		using var streamWriter = new StreamWriter(path, false, encoding);
		streamWriter.Write(contents);
	}

	public static string[]? GetFileNameListInPath(string path, string pattern = "")
	{
		if (!Directory.Exists(path)) return null;
		return Directory.GetFiles(path, pattern);
	}

	public static FileInfo[]? GetFileInfoListInPath(string path, string pattern = "")
	{
		if (!Directory.Exists(path)) return null;
		DirectoryInfo root = new DirectoryInfo(path);
		return root.GetFiles(pattern);
	}
}