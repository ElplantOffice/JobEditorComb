using ProductLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace JobEditor.Common
{
	public class ShapeInfoView : INotifyPropertyChanged
	{
		private ShapeProperties shapeProperties;

		private const string shapeNormal = "#FF007694";

		private const string shapeInfoNormal = "#FF6C9AA7";

		private const string shapeHighLight = "#FFFFFFFF";

		private const string hmiBackGround = "#FF015369";

		private ProductLib.Shape shape;

		private UIElementCollection drawshape;

		private bool highlight;

		private ulong stacknr;

		private ulong productsdone;

		private SolidColorBrush infoColor;

		public UIElementCollection DrawShape
		{
			get
			{
				return this.drawshape;
			}
			set
			{
				this.drawshape = value;
				this.onPropertyChanged("DrawShape");
			}
		}

		public bool Highlight
		{
			get
			{
				return this.highlight;
			}
			set
			{
				this.highlight = value;
				this.UpdateShapeView(this.highlight);
				this.InfoColor = this.GetShapeInfoTextColor(this.Highlight);
				this.onPropertyChanged("DrawShape");
			}
		}

		public string Info
		{
			get
			{
				return string.Concat(this.stacknr.ToString(), "#", this.productsdone.ToString());
			}
		}

		public SolidColorBrush InfoColor
		{
			get
			{
				return this.infoColor;
			}
			set
			{
				this.infoColor = value;
				this.onPropertyChanged("InfoColor");
			}
		}

		public ulong ProductsDone
		{
			get
			{
				return this.productsdone;
			}
			set
			{
				this.productsdone = value;
				this.onPropertyChanged("Info");
			}
		}

		public ProductLib.Shape Shape
		{
			get
			{
				return this.shape;
			}
			set
			{
				this.shape = value;
				this.InfoColor = this.GetShapeInfoTextColor(this.highlight);
				this.UpdateShapeView(this.highlight);
				this.onPropertyChanged("Shape");
			}
		}

		public ulong StackNr
		{
			get
			{
				return this.stacknr;
			}
			set
			{
				this.stacknr = value;
				this.onPropertyChanged("Info");
			}
		}

		public ShapeInfoView(ProductLib.Shape shape)
		{
			this.DrawShape = new UIElementCollection();
			this.UpdateView(shape);
		}

		public ShapeInfoView(ProductLib.Shape shape, ShapeProperties sp)
		{
			this.DrawShape = new UIElementCollection();
			this.shapeProperties = sp;
			this.UpdateView(shape);
		}

		public List<UIElement> GetShapeDrawing(out double length, bool highLight)
		{
			Color color;
			CutSeqDraw cutSeqDraw = new CutSeqDraw();
			if (this.shapeProperties == null)
			{
				this.shapeProperties = new ShapeProperties(this.Shape);
				if (highLight)
				{
					PolygonProperties polygonProperties = this.shapeProperties.PolygonProperties;
					PolygonProperties polygonProperty = this.shapeProperties.PolygonProperties;
					Color color1 = ((SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFFFFFFF")).Color;
					color = color1;
					polygonProperty.FillColor = color1;
					polygonProperties.StrokeColor = color;
				}
				else
				{
					PolygonProperties polygonProperties1 = this.shapeProperties.PolygonProperties;
					PolygonProperties polygonProperty1 = this.shapeProperties.PolygonProperties;
					Color color2 = ((SolidColorBrush)(new BrushConverter()).ConvertFrom("#FF007694")).Color;
					color = color2;
					polygonProperty1.FillColor = color2;
					polygonProperties1.StrokeColor = color;
				}
				this.shapeProperties.PolygonProperties.ScallingFactor = 7;
				this.shapeProperties.PolygonProperties.StrokeTicknees = 1;
				this.shapeProperties.PolygonProperties.DistanceBetweenParts = 5;
				this.shapeProperties.HoleProperties.FillColor = ((SolidColorBrush)(new BrushConverter()).ConvertFrom("#FF015369")).Color;
				this.shapeProperties.HoleProperties.StrokeThickness = 0;
				this.shapeProperties.HoleProperties.Diameter = 4;
				this.shapeProperties.SlotProperties.FillColor = ((SolidColorBrush)(new BrushConverter()).ConvertFrom("#FF015369")).Color;
				this.shapeProperties.SlotProperties.StrokeThickness = 0;
				this.shapeProperties.SlotProperties.Height = 4;
				this.shapeProperties.SlotProperties.Width = 50;
				this.shapeProperties.SlotProperties.VerticalDistance = 10;
				this.shapeProperties.StepLapProperties.IsEnabled = false;
			}
			CutSeqShapeHole cutSeqShapeHole = new CutSeqShapeHole(this.shapeProperties.HoleProperties);
			cutSeqShapeHole.NumberOfHoles = this.Shape.HolesInfo.NumberOfHoles;
			cutSeqShapeHole.MeasuringType = this.Shape.HolesInfo.MeasuringType;
			CutSeqShapeSlot cutSeqShapeSlot = new CutSeqShapeSlot(this.shapeProperties.SlotProperties);
			cutSeqShapeSlot.NumberOfSlotes = this.Shape.SlotsInfo.NumberOfSlots;
			cutSeqShapeSlot.MeasuringType = this.Shape.SlotsInfo.MeasuringType;
			CutSeqShapeCenter cutSeqShapeCenter = new CutSeqShapeCenter(this.shapeProperties.CenterProperties);
			cutSeqShapeCenter.MeasuringType = this.Shape.CentersInfo.MeasuringType;
			IList<CutSeqShape> cutSeqShapes = cutSeqDraw.GetCutSeqShapes(new List<ProductLib.Shape>()
			{
				this.Shape
			}, this.shapeProperties);
			cutSeqShapes[0].ShapeHole = cutSeqShapeHole;
			cutSeqShapes[0].ShapeSlot = cutSeqShapeSlot;
			cutSeqShapes[0].ShapeCenter = cutSeqShapeCenter;
			if (this.shapeProperties.TipCutProperties.IsEnabled)
			{
				cutSeqShapes[0].TipCatStart = this.Shape.ShapeParts.First<ShapePart>().TipCut.TipCutON;
				int num = 0;
				while (num < cutSeqShapes[0].Parts.Count)
				{
					if (cutSeqShapes[0].Parts[num].SourceIndex != this.Shape.ShapeParts.Count - 1)
					{
						num++;
					}
					else
					{
						cutSeqShapes[0].TipCatEnd = this.Shape.ShapeParts.Last<ShapePart>().TipCut.TipCutON;
						break;
					}
				}
			}
			if (this.shapeProperties.StepLapProperties.IsEnabled)
			{
				for (int i = 0; i < this.Shape.ShapeParts.Count; i++)
				{
					cutSeqShapes[0].Parts[cutSeqShapes[0].Parts[i].SourceIndex].ESLType = this.Shape.ShapeParts[i].StepLap.Type;
				}
			}
			length = this.shapeProperties.PolygonProperties.Length;
			return cutSeqDraw.GetCutSeqDrawData(cutSeqShapes, this.shapeProperties);
		}

		public SolidColorBrush GetShapeInfoTextColor(bool highLight)
		{
			if (highLight)
			{
				return (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFFFFFFF");
			}
			return (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FF6C9AA7");
		}

		protected void onPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public void UpdateShapeView(bool highLight)
		{
			double num;
			if (this.DrawShape != null)
			{
				this.DrawShape.ShapeElements = this.GetShapeDrawing(out num, highLight);
				this.DrawShape.ShapeLength = num + 14;
			}
		}

		public void UpdateShapeView()
		{
			double num;
			if (this.DrawShape != null)
			{
				this.DrawShape.ShapeElements = this.GetShapeDrawing(out num, false);
				this.DrawShape.ShapeLength = num;
			}
		}

		public void UpdateView(ProductLib.Shape shape)
		{
			this.Shape = shape;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}