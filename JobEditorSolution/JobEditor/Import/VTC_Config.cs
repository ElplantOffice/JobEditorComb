using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;

namespace JobEditor.Import
{
	[XmlRoot("Convert")]
	public class VTC_Config
	{
		public const string defaultFilename = "JobEditor.Convert.xml";

		[XmlArray("SteelGrades")]
		[XmlArrayItem("SteelGrade")]
		public List<VTC_ConfigSteelGrade> SteelGrades = new List<VTC_ConfigSteelGrade>();

		[XmlAttribute("EnableHeightMeasurement")]
		public bool EnableHeightMeasurement
		{
			get;
			set;
		}

		[XmlAttribute("OverCut")]
		public double OverCut
		{
			get;
			set;
		}

		public VTC_Config()
		{
		}

		public static VTC_Config Load(string fullFilename = "JobEditor.Convert.xml")
		{
			VTC_Config vTCConfig = null;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VTC_Config));
			StreamReader streamReader = null;
			try
			{
				try
				{
					streamReader = new StreamReader(fullFilename);
					vTCConfig = (VTC_Config)xmlSerializer.Deserialize(streamReader);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(string.Concat("Could not read convert file!\nException: ", exception.ToString()));
				}
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			return vTCConfig;
		}
	}
}