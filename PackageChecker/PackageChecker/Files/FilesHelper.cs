﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace PackageChecker.Files
{
	public static class FilesHelper
	{
		public static string PickZipDialog()
		{
			using (var fileDialog = new OpenFileDialog())
			{
				fileDialog.Filter = "Zip Archive|*.zip";
				fileDialog.CheckPathExists = true;
				fileDialog.Multiselect = false;
				DialogResult result = fileDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
				{
					return fileDialog.FileName;
				}

				return string.Empty;
			}
		}

		public static string PickFolderDialog()
		{
			using (var fileDialog = new FolderBrowserDialog())
			{
				DialogResult result = fileDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.SelectedPath))
				{
					return fileDialog.SelectedPath;
				}

				return string.Empty;
			}
		}

		public static bool IsFolder(string path)
		{
			FileAttributes attributes = File.GetAttributes(path);
			return attributes.HasFlag(FileAttributes.Directory);
		}

		public static bool IsZipFile(string path)
		{
			if (IsFolder(path))
			{
				return false;
			}

			FileInfo info = new FileInfo(path);
			return !string.IsNullOrEmpty(info.Extension) && info.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase);
		}

		public static void OpenFileExplorer(string rootFolder, string relativePath)
		{
			string fullPath = Path.Combine(rootFolder, relativePath);
			fullPath = ReplaseAltSeparators(fullPath);

			if (!File.Exists(fullPath))
			{
				return;
			}

			const string explorerArgsFormat = "/select, \"{0}\"";
			string argument = string.Format(CultureInfo.InvariantCulture, explorerArgsFormat, fullPath);

			Process.Start("explorer.exe", argument);
		}

		public static string GetRelativePath(string fullPath, string rootPath)
		{
			if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				rootPath += Path.DirectorySeparatorChar;
			}
			Uri fileUri = new Uri(fullPath);
			Uri referenceUri = new Uri(rootPath);
			return referenceUri.MakeRelativeUri(fileUri).ToString().Replace("%20", " ");
		}

		public static string ReplaseAltSeparators(string path)
		{
			return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}
	}
}