
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomControlLibrary
{
  public class TilePanel : ItemsControl
  {
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(nameof (Rows), typeof (int), typeof (TilePanel), new PropertyMetadata((object) 2));
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof (Columns), typeof (int), typeof (TilePanel), new PropertyMetadata((object) 2));
    public static readonly DependencyProperty TileWidthProperty = DependencyProperty.Register(nameof (TileWidth), typeof (int), typeof (TilePanel), new PropertyMetadata((object) 105));
    public static readonly DependencyProperty TileHeightProperty = DependencyProperty.Register(nameof (TileHeight), typeof (int), typeof (TilePanel), new PropertyMetadata((object) 105));
    public static readonly DependencyProperty TileMarginProperty = DependencyProperty.Register(nameof (TileMargin), typeof (int), typeof (TilePanel), new PropertyMetadata((object) 7));
    public static readonly DependencyProperty TileBackgroundProperty = DependencyProperty.Register(nameof (TileBackground), typeof (string), typeof (TilePanel), new PropertyMetadata((object) "#15FFFFFF"));

    static TilePanel()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TilePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TilePanel)));
    }

    public int Rows
    {
      get
      {
        return (int) this.GetValue(TilePanel.RowsProperty);
      }
      set
      {
        this.SetValue(TilePanel.RowsProperty, (object) value);
      }
    }

    public int Columns
    {
      get
      {
        return (int) this.GetValue(TilePanel.ColumnsProperty);
      }
      set
      {
        this.SetValue(TilePanel.ColumnsProperty, (object) value);
      }
    }

    public int TileWidth
    {
      get
      {
        return (int) this.GetValue(TilePanel.TileWidthProperty);
      }
      set
      {
        this.SetValue(TilePanel.TileWidthProperty, (object) value);
      }
    }

    public int TileHeight
    {
      get
      {
        return (int) this.GetValue(TilePanel.TileHeightProperty);
      }
      set
      {
        this.SetValue(TilePanel.TileHeightProperty, (object) value);
      }
    }

    public int TileMargin
    {
      get
      {
        return (int) this.GetValue(TilePanel.TileMarginProperty);
      }
      set
      {
        this.SetValue(TilePanel.TileMarginProperty, (object) value);
      }
    }

    public string TileBackground
    {
      get
      {
        return (string) this.GetValue(TilePanel.TileBackgroundProperty);
      }
      set
      {
        this.SetValue(TilePanel.TileBackgroundProperty, (object) value);
      }
    }

    private Rectangle NewTileRect()
    {
      Rectangle rectangle = new Rectangle();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      SolidColorBrush solidColorBrush2 = (SolidColorBrush) new BrushConverter().ConvertFrom((object) this.TileBackground);
      SolidColorBrush solidColorBrush3 = new SolidColorBrush();
      SolidColorBrush background = this.Background as SolidColorBrush;
      if (background != null)
      {
        Color color = solidColorBrush2.Color;
        int a1 = (int) color.A;
        color = background.Color;
        int a2 = (int) color.A;
        int num1 = (int) (byte) (a1 & a2);
        color = solidColorBrush2.Color;
        int r1 = (int) color.R;
        color = background.Color;
        int r2 = (int) color.R;
        int num2 = (int) (byte) (r1 & r2);
        color = solidColorBrush2.Color;
        int g1 = (int) color.G;
        color = background.Color;
        int g2 = (int) color.G;
        int num3 = (int) (byte) (g1 & g2);
        color = solidColorBrush2.Color;
        int b1 = (int) color.B;
        color = background.Color;
        int b2 = (int) color.B;
        int num4 = (int) (byte) (b1 & b2);
        SolidColorBrush solidColorBrush4 = new SolidColorBrush(Color.FromArgb((byte) num1, (byte) num2, (byte) num3, (byte) num4));
        rectangle.Fill = (Brush) solidColorBrush4;
      }
      else
        rectangle.Fill = (Brush) solidColorBrush2;
      rectangle.Margin = new Thickness((double) this.TileMargin, (double) this.TileMargin, (double) this.TileMargin, (double) this.TileMargin);
      return rectangle;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      UniformGrid templateChild = this.GetTemplateChild("PART_GridTileBackground") as UniformGrid;
      templateChild.Rows = this.Rows;
      templateChild.Columns = this.Columns;
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
          templateChild.Children.Add((UIElement) this.NewTileRect());
      }
      foreach (TileButton child in (this.GetTemplateChild("PART_GridTileButton") as UniformGrid).Children)
      {
        child.Width = (double) this.TileWidth;
        child.Height = (double) this.TileHeight;
        child.Margin = new Thickness((double) this.TileMargin, (double) this.TileMargin, (double) this.TileMargin, (double) this.TileMargin);
      }
    }
  }
}
