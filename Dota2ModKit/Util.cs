﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKExtract;

namespace Dota2ModKit {
	public static class Util {

		public static string getDotaDir() {
			string dotaDir = "";
			// Auto-find the dota path.
			Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine;
			try {
				regKey =
					regKey.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 570");
				if (regKey != null) {
					string dir = regKey.GetValue("InstallLocation").ToString();
					dotaDir = dir;
				}
			} catch (Exception) {

			}

			if (dotaDir != "") {
				return dotaDir;
			}

			// try another way to auto-get the dir
			string p1 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
			string p2 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

			p1 = Path.Combine(p1, "steam", "steamapps", "common", "dota 2 beta");
			p2 = Path.Combine(p2, "steam", "steamapps", "common", "dota 2 beta");

			if (Directory.Exists(p1)) {
				dotaDir = p1;
			} else if (Directory.Exists(p2)) {
				dotaDir = p2;
			}

			// try another
			p1 = Path.Combine(p1, "steam", "steamapps", "common", "dota 2");
			p2 = Path.Combine(p2, "steam", "steamapps", "common", "dota 2");

			if (Directory.Exists(p1)) {
				dotaDir = p1;
			} else if (Directory.Exists(p2)) {
				dotaDir = p2;
			}

			return dotaDir;
		}

		public static bool hasSameDrives(string path1, string path2) {
			// D2ModKit must be ran from the same drive as dota or else things will break.
			char path1Drive = path1[0];
			char path2Drive = path2[0];
			if (path1Drive != path2Drive) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Determines a text file's encoding by analyzing its byte order mark (BOM).
		/// Defaults to ASCII when detection of the text file's endianness fails.
		/// </summary>
		/// <param name="filename">The text file to analyze.</param>
		/// <returns>The detected encoding.</returns>
		public static Encoding GetEncoding(string filename) {
			// Read the BOM
			var bom = new byte[4];
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				file.Read(bom, 0, 4);
			}

			// Analyze the BOM
			if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
			if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
			if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
			if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
			return Encoding.ASCII;
		}

		public static List<string> getFiles(string directory, string searchPattern) {
		List<string> allFiles = new List<string>();
		string[] exts = searchPattern.Split(';');
		for (int i = 0; i < exts.Count(); i++) {
			string[] foundFiles = Directory.GetFiles(directory, exts[i], SearchOption.AllDirectories);
			for (int j = 0; j < foundFiles.Count(); j++) {
				allFiles.Add(foundFiles[j]);
			}
		}
		return allFiles;
	}

	public static string[] getRGB() {
			string[] rgb = new string[3];
			ColorDialog color = new ColorDialog();
			color.AnyColor = true;
			color.AllowFullOpen = true;
			DialogResult re = color.ShowDialog();
			if (re == DialogResult.OK) {
				Color picked = color.Color;
				rgb[0] = picked.R.ToString();
				rgb[1] = picked.G.ToString();
				rgb[2] = picked.B.ToString();
			} else {
				return null;
			}
			return rgb;
		}

		public static string incrementVers(string vers, int add) {
			//Debug.WriteLine("input: " + vers);
			// check for new Vers
			string[] numStrings = vers.Split('.');
			int thousands = Int32.Parse(numStrings[0]) * 1000;
			int hundreds = Int32.Parse(numStrings[1]) * 100;
			int tens = Int32.Parse(numStrings[2]) * 10;
			int ones = Int32.Parse(numStrings[3]);
			int num = thousands + hundreds + tens + ones + add;

			//Debug.WriteLine("new num: " + num);
			int newThousands = num / 1000;
			int newHundreds = (num - newThousands * 1000) / 100;
			int newTens = (num - newThousands * 1000 - newHundreds * 100) / 10;
			int newOnes = num - newThousands * 1000 - newHundreds * 100 - newTens * 10;
			string newVers = newThousands + "." + newHundreds + "." + newTens + "." + newOnes;
			//Debug.WriteLine("New vers: " + newVers);
			return newVers;
		}

		internal static System.Drawing.Image imageUrlToObj(string url) {
			WebClient wc = new WebClient();
			byte[] bytes = wc.DownloadData("http://localhost/image.gif");
			MemoryStream ms = new MemoryStream(bytes);
			System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

			return img;
		}

				/*string[] versionSplit = version.Split('.');
		string newVers = "v";
		for (int i = 0; i < versionSplit.Count(); i++) {
			if (i == versionSplit.Count() - 1) {
				continue;
			}
			newVers += versionSplit[i] + ".";
		}
		newVers = newVers.Substring(0, newVers.Length - 1);*/

	}
}
