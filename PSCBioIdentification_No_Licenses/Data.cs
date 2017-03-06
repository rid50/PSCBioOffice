using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using System.IO;
using Neurotec.Images;
using System.Data;
//using System.Data.SQLite;
using Neurotec.Biometrics;
using PSCBioIdentification.Properties;

namespace PSCBioIdentification
{
	public class Record
	{
		#region Private fields

		private string id;
		byte[] template;

		#endregion

		#region Internal constructor

		internal Record(string id, byte[] template)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (template == null) throw new ArgumentNullException("template");
			this.id = id;
			this.template = template;
		}

		#endregion

		#region Public properties

		public string Id
		{
			get
			{
				return id;
			}
		}

		public byte[] Template
		{
			get
			{
				return template;
			}
		}

		#endregion
	}
/*
	public class Database : IDisposable
	{
		#region Public types

		public class RecordCollection: ReadOnlyCollection<Record>
		{
			#region Private fields

			private Database owner;
			private List<Record>[] itemsByG;

			#endregion

			#region Internal constructor

			internal RecordCollection(Database owner)
				: base(new List<Record>())
			{
				this.owner = owner;
				itemsByG = new List<Record>[256];
				for (int i = 0; i != 256; i++)
				{
					itemsByG[i] = new List<Record>();
				}

				SQLiteCommand cmd = new SQLiteCommand("SELECT TemplateId, Template FROM Templates",
					owner.connection);

				SQLiteDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					string templateId = reader.GetString(0);

					long templateSize = reader.GetBytes(1, 0, null, 0, 0);
					byte[] templateData = new byte[(int)templateSize];
					reader.GetBytes(1, 0, templateData, 0, (int)templateSize);

					Record record = new Record(templateId, templateData);
					Items.Add(record);
					itemsByG[NFRecord.GetG(templateData)].Add(record);
				}

				cmd.Dispose();
			}

			#endregion

			#region Public methods

			public Record Add(string id, byte[] template, NImage image)
			{
				Record record = new Record(id, template);

				string query = "INSERT INTO Templates (TemplateId, Template) VALUES (@templateId, @template)";

				SQLiteCommand insertCmd = new SQLiteCommand(query, owner.connection);
				insertCmd.Parameters.AddWithValue("@templateId", id);
				insertCmd.Parameters.AddWithValue("@template", template);
				insertCmd.ExecuteNonQuery();

				SQLiteCommand lastIdCmd = new SQLiteCommand("SELECT last_insert_rowid()", owner.connection);
				int rowId = Convert.ToInt32(lastIdCmd.ExecuteScalar());

				if (image != null)
				{
					MemoryStream imageStream = new MemoryStream();
					Png.SaveImage(image, imageStream);
					byte[] imageData = imageStream.ToArray();

					SQLiteCommand insertImageCmd = owner.connection.CreateCommand();
					insertImageCmd.CommandText = "INSERT INTO Images(Id, TemplateId, BiometricType, RecordIndex, GenIndex, FrameIndex, Image) VALUES(NULL, @templateId, @biometricType, 0, 0, 0, @image)";
					insertImageCmd.Parameters.AddWithValue("@templateId", rowId);
					insertImageCmd.Parameters.AddWithValue("@biometricType", (int)NBiometricType.Fingerprint);
					insertImageCmd.Parameters.AddWithValue("@image", imageData);
					insertImageCmd.ExecuteNonQuery();
				}

				Items.Add(record);
				itemsByG[NFRecord.GetG(template)].Add(record);
				return record;
			}

			public void Clear()
			{
				SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Templates", owner.connection);
				cmd.ExecuteNonQuery();
				cmd = new SQLiteCommand("DELETE FROM Images", owner.connection);
				cmd.ExecuteNonQuery();

				for (int i = 0; i != 256; i++)
				{
					itemsByG[i].Clear();
				}
				Items.Clear();
			}

			public IEnumerable<Record> GetEnumerator(byte[] template)
			{
				int g = NFRecord.GetG(template);
				int[] gs = new int[256];
				gs[0] = g;
				int count = 1;
				for (int i = 1; i != 256; i++)
				{
					int g1 = g + i;
					int g2 = g - i;
					if (g1 <= 255)
					{
						gs[count++] = g1;
					}
					if (g2 >= 0)
					{
						gs[count++] = g2;
					}
				}
				for (int i = 0; i != count; i++)
				{
					foreach (Record record in itemsByG[gs[i]])
					{
						yield return record;
					}
				}
			}

			#endregion

			#region Public properties

			public Record this[string id]
			{
				get
				{
					foreach(Record record in Items)
					{
						if (record.Id == id) return record;
					}
					return null;
				}
			}

			#endregion
		}

		#endregion

		#region Private fields

		private string fileName;
		private RecordCollection records;
		private SQLiteConnection connection = null;

		#endregion

		#region Public constructor

		public Database()
		{
			string localFileName = "Fingers.db";
			try
			{
				this.fileName = Path.Combine(Helpers.GetUserLocalDataDir("FingersSample"), localFileName);
			}
			catch
			{
				this.fileName = localFileName;
			}

			if (!File.Exists(fileName))
			{
				SQLiteConnection conn = new SQLiteConnection("Version=3;New=True;Compress=False;Data Source=" + fileName);
				conn.Open();

				SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE Templates (Id INTEGER PRIMARY KEY, TemplateId TEXT NOT NULL, Template BLOB NOT NULL, Thumbnail BLOB)", conn);
				cmd.ExecuteNonQuery();

				cmd = new SQLiteCommand("CREATE TABLE Images (Id INTEGER PRIMARY KEY, TemplateId INTEGER NOT NULL, BiometricType INTEGER NOT NULL, " +
					"RecordIndex INTEGER NOT NULL, GenIndex INTEGER NOT NULL, FrameIndex INTEGER NOT NULL, Image BLOB NOT NULL)", conn);
				cmd.ExecuteNonQuery();

				connection = conn;
			}
			else
			{
				connection = new SQLiteConnection("Version=3;New=False;Compress=False;Data Source=" + fileName);
				connection.Open();
			}

			records = new RecordCollection(this);
		}

		#endregion

		#region Destructor

		~Database()
		{
			Dispose();
		}

		#endregion

		#region Public properties

		public RecordCollection Records
		{
			get
			{
				return records;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
		}

		#endregion
	}
*/
    static class Data
    {
        #region Public static fields

        //public static Database Database;
        public static NFExtractor NFExtractor;
        public static NMatcher NMatcher;

        #endregion

        #region Public static methods

        public static void ResetSetting(string name)
        {
            Settings settings = Settings.Default;
            PropertyInfo pi = typeof(Settings).GetProperty(name);
            DefaultSettingValueAttribute dsva = (DefaultSettingValueAttribute)pi.GetCustomAttributes(typeof(DefaultSettingValueAttribute), false)[0];
            pi.SetValue(settings, TypeDescriptor.GetConverter(pi.PropertyType).ConvertFromInvariantString(dsva.Value), null);
        }

        public static void UpdateNfe()
        {
            Settings settings = Settings.Default;
            try { NFExtractor.UseQuality = settings.NfeUseQuality; }
            catch { }
            try { NFExtractor.QualityThreshold = settings.NfeQualityThreshold; }
            catch { }
            try { NFExtractor.Mode = settings.NfeMode; }
            catch { }
            try { NFExtractor.TemplateSize = settings.NfeTemplateSize; }
            catch { }
            try { NFExtractor.ReturnedImage = settings.NfeReturnedImage; }
            catch { }
            try { NFExtractor.GeneralizationThreshold = settings.NfeGeneralizationThreshold; }
            catch { }
            try { NFExtractor.GeneralizationMaximalRotation = settings.NfeGeneralizationMaximalRotation; }
            catch { }
        }

        public static void UpdateNfeSettings()
        {
            Settings settings = Settings.Default;
            settings.NfeUseQuality = NFExtractor.UseQuality;
            settings.NfeQualityThreshold = NFExtractor.QualityThreshold;
            settings.NfeMode = NFExtractor.Mode;
            settings.NfeTemplateSize = NFExtractor.TemplateSize;
            settings.NfeReturnedImage = NFExtractor.ReturnedImage;
            settings.NfeGeneralizationThreshold = NFExtractor.GeneralizationThreshold;
            settings.NfeGeneralizationMaximalRotation = NFExtractor.GeneralizationMaximalRotation;
        }

        public static void UpdateNM()
        {
            Settings settings = Settings.Default;
            try { NMatcher.FingersNfmMode = settings.NMMode; }
            catch { }
            try { NMatcher.MatchingThreshold = settings.NMMatchingThreshold; }
            catch { }
            try { NMatcher.FingersNfmMaximalRotation = settings.NMMaximalRotation; }
            catch { }
        }

        public static void UpdateNMSettings()
        {
            Settings settings = Settings.Default;
            settings.NMMode = NMatcher.FingersNfmMode;
            settings.NMMatchingThreshold = NMatcher.MatchingThreshold;
            settings.NMMaximalRotation = NMatcher.FingersNfmMaximalRotation;
        }

        public static string SizeToString(int size)
        {
            string value;
            int rem;
            size = Math.DivRem(size, 1024, out rem);
            value = rem + " byte(s)";
            if (size != 0)
            {
                size = Math.DivRem(size, 1024, out rem);
                value = rem + " KB " + value;
                if (size != 0)
                {
                    size = Math.DivRem(size, 1024, out rem);
                    value = rem + " MB " + value;
                    if (size != 0)
                    {
                        size = Math.DivRem(size, 1024, out rem);
                        value = rem + " GB " + value;
                        if (size != 0)
                        {
                            value = size + " TB " + value;
                        }
                    }
                }
            }
            return value;
        }

        public static string TimeToString(TimeSpan time)
        {
            long t = time.Ticks / 10;
            string value;
            long rem;
            t = Math.DivRem(t, 1000, out rem);
            value = rem + " mks";
            if (t != 0)
            {
                t = Math.DivRem(t, 1000, out rem);
                value = rem + " ms " + value;
                if (t != 0)
                {
                    t = Math.DivRem(t, 60, out rem);
                    value = rem + " s " + value;
                    if (t != 0)
                    {
                        t = Math.DivRem(t, 60, out rem);
                        value = rem + " m " + value;
                        if (t != 0)
                        {
                            t = Math.DivRem(t, 24, out rem);
                            value = rem + " h " + value;
                            if (t != 0)
                            {
                                value = t + " d " + value;
                            }
                        }
                    }
                }
            }
            return value;
        }

        #endregion
    }

	public struct MatchingResult
	{
		#region Private fields

		private Record record;
		private int score;

		#endregion

		#region Public constructor

		public MatchingResult(Record record, int score)
		{
			if (record == null) throw new ArgumentNullException("record");
			this.record = record;
			this.score = score;
		}

		#endregion

		#region Public properties

		public Record Record
		{
			get
			{
				return record;
			}
		}

		public int Score
		{
			get
			{
				return score;
			}
		}

		public string RecordId
		{
			get
			{
				return record.Id;
			}
		}

		#endregion
	}
}
