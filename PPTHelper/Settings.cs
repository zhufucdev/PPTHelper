using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace PPTHelper
{
	class Settings
	{
		public bool fix
		{
			get; set;
		}
		public bool keep
		{
			get; set;
		}

		public void Save()
		{
			if (!File.Exists(Store))
			{
				Directory.CreateDirectory(AppDir);
			}
			using (StreamWriter writer = File.CreateText(Store))
			{
				writer.Write(JsonConvert.SerializeObject(this));
			}
		}

		public static Settings Default
		{
			get
			{
				if (_settings == null)
					loadSettings();
				return _settings;
			}
		}

		private static Settings _settings;
		private static void loadSettings()
		{
			if (File.Exists(Store))
			{
				using (StreamReader reader = new StreamReader(Store))
				{
					_settings = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd());
				}
			}
			else
			{
				// Use default
				_settings = new Settings()
				{
					keep = true,
					fix = false
				};
			}
		}

		static String Store
		{
			get => Path.Combine(AppDir, "config.json");
		}

		static string AppDir
		{
			get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WordHelper");
		}
	}
}
