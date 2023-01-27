using JobEditor.Properties;
using ProductLib;
using System;

namespace JobEditor.Model
{
	public class EditorModel
	{
		private SequenceMaker seqMaker;

		public SequenceMaker SeqMaker
		{
			get
			{
				return this.seqMaker;
			}
			set
			{
				this.seqMaker = value;
			}
		}

		public EditorModel()
		{
			SequenceMakerDefinition sequenceMakerDefinition = new SequenceMakerDefinition(Settings.Default.SequenceMakerFile);
			this.seqMaker = sequenceMakerDefinition.SequenceMakerData;
		}
	}
}