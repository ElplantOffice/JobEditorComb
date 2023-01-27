using JobEditor.Common;
using ProductLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobEditor.Views.ProductData
{
	public class TipCutView : ProductDataViewBase, ICloneable
	{
		private bool tipCutON;

		private double height;

		private double overCut;

		private bool doubleCut;

		public bool DoubleCut
		{
			get
			{
				return this.doubleCut;
			}
			set
			{
				this.doubleCut = value;
				base.RaisePropertyChanged("DoubleCut", true);
			}
		}

		public bool DoubleCut_Valid
		{
			get
			{
				return base.GetValidState("DoubleCut");
			}
			set
			{
				base.SetValidState("DoubleCut", value);
			}
		}

		public double Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				base.RaisePropertyChanged("Height", true);
			}
		}

		public bool Height_Valid
		{
			get
			{
				return base.GetValidState("Height");
			}
			set
			{
				base.SetValidState("Height", value);
			}
		}

		public double OverCut
		{
			get
			{
				return this.overCut;
			}
			set
			{
				this.overCut = value;
				base.RaisePropertyChanged("OverCut", true);
			}
		}

		public bool OverCut_Valid
		{
			get
			{
				return base.GetValidState("OverCut");
			}
			set
			{
				base.SetValidState("OverCut", value);
			}
		}

		private JobEditor.Views.ProductData.ShapePartView ShapePartView
		{
			get
			{
				JobEditor.Views.ProductData.ShapePartView shapePartView;
				if (base.ProductView != null)
				{
					using (IEnumerator<ShapeView> enumerator = base.ProductView.SelectedLayerView.ShapeViews.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ShapeView current = enumerator.Current;
							if (current.ShapePartViews == null)
							{
								continue;
							}
							List<JobEditor.Views.ProductData.ShapePartView>.Enumerator enumerator1 = current.ShapePartViews.GetEnumerator();
							try
							{
								while (enumerator1.MoveNext())
								{
									JobEditor.Views.ProductData.ShapePartView current1 = enumerator1.Current;
									if (current1.TipCutView != this)
									{
										continue;
									}
									shapePartView = current1;
									return shapePartView;
								}
							}
							finally
							{
								((IDisposable)enumerator1).Dispose();
							}
						}
						return null;
					}
					return shapePartView;
				}
				return null;
			}
		}

		public ProductLib.TipCut TipCut
		{
			get
			{
				ProductLib.TipCut tipCut = new ProductLib.TipCut();
				tipCut.TipCutON = this.TipCutON;
				tipCut.Height = this.Height;
				tipCut.OverCut = this.OverCut;
				tipCut.DoubleCut = this.DoubleCut;
				return tipCut;
			}
			set
			{
				ProductLib.TipCut tipCut = value;
				if (tipCut == null)
				{
					this.Clear();
					return;
				}
				this.TipCutON = tipCut.TipCutON;
				this.Height = tipCut.Height;
				this.OverCut = tipCut.OverCut;
				this.DoubleCut = tipCut.DoubleCut;
			}
		}

		public bool TipCutON
		{
			get
			{
				return this.tipCutON;
			}
			set
			{
				this.tipCutON = value;
				base.RaisePropertyChanged("TipCutON", true);
				base.RaisePropertyChanged("Type", true);
			}
		}

		public bool TipCutON_Valid
		{
			get
			{
				return base.GetValidState("TipCutON");
			}
			set
			{
				base.SetValidState("TipCutON", value);
			}
		}

		public ETipCutType Type
		{
			get
			{
				EFeature feature;
				ETipCutType eTipCutType = ETipCutType.TipCutNone;
				JobEditor.Views.ProductData.ShapePartView shapePartView = this.ShapePartView;
				if (shapePartView != null)
				{
					if (shapePartView.Id == 0)
					{
						if (!this.TipCutON)
						{
							feature = shapePartView.Feature;
							if (feature == EFeature.Cut45)
							{
								eTipCutType = ETipCutType.TipCut45LeftOff;
							}
							else
							{
								eTipCutType = feature == EFeature.Cut135 ? ETipCutType.TipCut135LeftOff : ETipCutType.TipCutNone;
							}
						}
						else
						{
							feature = shapePartView.Feature;
							if (feature == EFeature.Cut45)
							{
								eTipCutType = ETipCutType.TipCut45LeftOn;
							}
							else
							{
								eTipCutType = feature == EFeature.Cut135 ? ETipCutType.TipCut135LeftOn : ETipCutType.TipCutNone;
							}
						}
					}
					else if (!this.TipCutON)
					{
						feature = shapePartView.Feature;
						if (feature == EFeature.Cut45)
						{
							eTipCutType = ETipCutType.TipCut45RightOff;
						}
						else
						{
							eTipCutType = feature == EFeature.Cut135 ? ETipCutType.TipCut135RightOff : ETipCutType.TipCutNone;
						}
					}
					else
					{
						feature = shapePartView.Feature;
						if (feature == EFeature.Cut45)
						{
							eTipCutType = ETipCutType.TipCut45RightOn;
						}
						else
						{
							eTipCutType = feature == EFeature.Cut135 ? ETipCutType.TipCut135RightOn : ETipCutType.TipCutNone;
						}
					}
				}
				return eTipCutType;
			}
			set
			{
				switch ((int)value)
				{
					case 0:
					case 1:
					case 4:
					case 5:
					{
						this.TipCutON = true;
						return;
					}
					case 2:
					case 3:
					case 6:
					case 7:
					case 8:
					{
						this.TipCutON = false;
						return;
					}
					default:
					{
						return;
					}
				}
			}
		}

		public TipCutView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.TipCut tipCut = null)
		{
			base.ProductView = productView;
			this.TipCut = tipCut;
		}

		public void Clear()
		{
			this.TipCutON = false;
			this.Height = 0;
			this.OverCut = 0;
			this.DoubleCut = false;
		}

		public object Clone()
		{
			TipCutView tipCutView = new TipCutView(base.ProductView, null);
			tipCutView.CloneValidationDataFrom(this);
			tipCutView.tipCutON = this.tipCutON;
			tipCutView.height = this.height;
			tipCutView.overCut = this.overCut;
			tipCutView.doubleCut = this.doubleCut;
			return tipCutView;
		}

		public void CloneDataFrom(TipCutView tipCutView)
		{
			this.TipCutON = tipCutView.TipCutON;
			this.Height = tipCutView.Height;
			this.OverCut = tipCutView.OverCut;
			this.DoubleCut = tipCutView.DoubleCut;
		}
	}
}