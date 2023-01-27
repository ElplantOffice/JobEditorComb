using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace JobEditor.Import
{
	public class VTC_ConfigSteelGrade
	{
		[XmlAttribute("Name")]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute("Thickness")]
		public double Thickness
		{
			get;
			set;
		}

		[XmlAttribute("Value")]
		public int Value
		{
			get;
			set;
		}

		public VTC_ConfigSteelGrade()
		{
		}
	}
}