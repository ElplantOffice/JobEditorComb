
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class CustomGrid : Grid
  {
    private static double _minimumCellSize = 50.0;
    public static readonly DependencyProperty CellHeightProperty = DependencyProperty.Register(nameof (CellHeight), typeof (double), typeof (CustomGrid), (PropertyMetadata) new UIPropertyMetadata((object) CustomGrid._minimumCellSize, new PropertyChangedCallback(CustomGrid.VisualPropertyChangedCallback)), new ValidateValueCallback(CustomGrid.CellSizeValidateCallback));
    public static readonly DependencyProperty CellWidthProperty = DependencyProperty.Register(nameof (CellWidth), typeof (double), typeof (CustomGrid), (PropertyMetadata) new UIPropertyMetadata((object) CustomGrid._minimumCellSize, new PropertyChangedCallback(CustomGrid.VisualPropertyChangedCallback)), new ValidateValueCallback(CustomGrid.CellSizeValidateCallback));
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (CustomGrid), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Transparent, new PropertyChangedCallback(CustomGrid.VisualPropertyChangedCallback)));
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (CustomGrid), (PropertyMetadata) new UIPropertyMetadata((object) 14, new PropertyChangedCallback(CustomGrid.VisualPropertyChangedCallback)));
    public static readonly DependencyProperty DashStyleProperty = DependencyProperty.Register(nameof (DashStyle), typeof (DashStyle), typeof (CustomGrid), (PropertyMetadata) new UIPropertyMetadata((object) new DashStyle(), new PropertyChangedCallback(CustomGrid.VisualPropertyChangedCallback)));

    public double CellHeight
    {
      get
      {
        return (double) this.GetValue(CustomGrid.CellHeightProperty);
      }
      set
      {
        this.SetValue(CustomGrid.CellHeightProperty, (object) value);
      }
    }

    public double CellWidth
    {
      get
      {
        return (double) this.GetValue(CustomGrid.CellWidthProperty);
      }
      set
      {
        this.SetValue(CustomGrid.CellWidthProperty, (object) value);
      }
    }

    public Brush Stroke
    {
      get
      {
        return (Brush) this.GetValue(CustomGrid.StrokeProperty);
      }
      set
      {
        this.SetValue(CustomGrid.StrokeProperty, (object) value);
      }
    }

    public double StrokeThickness
    {
      get
      {
        return (double) this.GetValue(CustomGrid.StrokeThicknessProperty);
      }
      set
      {
        this.SetValue(CustomGrid.StrokeThicknessProperty, (object) value);
      }
    }

    public DashStyle DashStyle
    {
      get
      {
        return (DashStyle) this.GetValue(CustomGrid.DashStyleProperty);
      }
      set
      {
        this.SetValue(CustomGrid.DashStyleProperty, (object) value);
      }
    }

    private static void VisualPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      (o as CustomGrid).InvalidateVisual();
    }

    private static bool CellSizeValidateCallback(object target)
    {
      return (double) target >= CustomGrid._minimumCellSize;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      int num1 = (int) Math.Ceiling(this.RenderSize.Height / this.CellHeight);
      int num2 = (int) Math.Ceiling(this.RenderSize.Width / this.CellWidth);
      double y1 = 0.0;
      double x1 = 0.0;
      Pen pen = new Pen(this.Stroke, this.StrokeThickness);
      pen.DashStyle = this.DashStyle;
      for (int index1 = 0; index1 <= num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          drawingContext.DrawLine(pen, new Point(x1, y1), new Point(x1, this.CellHeight + y1));
          y1 += this.CellHeight;
        }
        x1 += this.CellWidth;
        y1 = 0.0;
      }
      double x2 = 0.0;
      double y2 = 0.0;
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
        {
          drawingContext.DrawLine(pen, new Point(x2, y2), new Point(this.CellWidth + x2, y2));
          x2 += this.CellWidth;
        }
        y2 += this.CellHeight;
        x2 = 0.0;
      }
    }
  }
}
