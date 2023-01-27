

using Patterns.EventAggregator;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class TileButton : RepeatButton
  {
    public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register(nameof (ContentText), typeof (string), typeof (TileButton), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ContentTextFontSizeProperty = DependencyProperty.Register(nameof (ContentTextFontSize), typeof (double), typeof (TileButton), new PropertyMetadata((object) 14.0));
    public static readonly DependencyProperty ContentDataProperty = DependencyProperty.Register(nameof (ContentData), typeof (int), typeof (TileButton), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty ContentDataFontSizeProperty = DependencyProperty.Register(nameof (ContentDataFontSize), typeof (double), typeof (TileButton), new PropertyMetadata((object) 22.0));
    public static readonly DependencyProperty ContentDataVisibilityProperty = DependencyProperty.Register(nameof (ContentDataVisibility), typeof (Visibility), typeof (TileButton), new PropertyMetadata((object) Visibility.Hidden));
    public static readonly DependencyProperty BackGroundVisibilityProperty = DependencyProperty.Register(nameof (BackGroundVisibility), typeof (bool), typeof (TileButton), new PropertyMetadata((object) true, new PropertyChangedCallback(TileButton.BackGroundVisibilityOnChange)));
    public static readonly DependencyProperty UseBackGroundVisibilityProperty = DependencyProperty.Register(nameof (UseBackGroundVisibility), typeof (bool), typeof (TileButton), new PropertyMetadata((object) true, new PropertyChangedCallback(TileButton.UseBackGroundVisibilityOnChange)));
    public static readonly DependencyProperty ImageBackGroundProperty = DependencyProperty.Register(nameof (ImageBackGround), typeof (Color), typeof (TileButton), new PropertyMetadata((object) Colors.Transparent, new PropertyChangedCallback(TileButton.ImageBackGroundOnChange)));
    public static readonly DependencyProperty ContentImageSourceFileNameProperty = DependencyProperty.Register(nameof (ContentImageSourceFileName), typeof (string), typeof (TileButton), new PropertyMetadata((object) null, new PropertyChangedCallback(TileButton.SetContentImageSource)));
    public static readonly DependencyProperty ContentImageSourcePathProperty = DependencyProperty.Register(nameof (ContentImageSourcePath), typeof (string), typeof (TileButton), new PropertyMetadata((object) "Images/", new PropertyChangedCallback(TileButton.SetContentImageSource)));
    public static readonly DependencyProperty ContentImageSourceProperty = DependencyProperty.Register(nameof (ContentImageSource), typeof (ImageSource), typeof (TileButton), new PropertyMetadata((PropertyChangedCallback) null));

    static TileButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TileButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TileButton)));
    }

    public string ContentText
    {
      get
      {
        return (string) this.GetValue(TileButton.ContentTextProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentTextProperty, (object) value);
      }
    }

    public double ContentTextFontSize
    {
      get
      {
        return (double) this.GetValue(TileButton.ContentTextFontSizeProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentTextFontSizeProperty, (object) value);
      }
    }

    public int ContentData
    {
      get
      {
        return (int) this.GetValue(TileButton.ContentDataProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentDataProperty, (object) value);
      }
    }

    public double ContentDataFontSize
    {
      get
      {
        return (double) this.GetValue(TileButton.ContentDataFontSizeProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentDataFontSizeProperty, (object) value);
      }
    }

    public Visibility ContentDataVisibility
    {
      get
      {
        return (Visibility) this.GetValue(TileButton.ContentDataVisibilityProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentDataVisibilityProperty, (object) value);
      }
    }

    public static void SetBackgroundVisibility(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      TileButton tileButton1 = dependencyObject as TileButton;
      if (tileButton1.UseBackGroundVisibility)
      {
        if (!tileButton1.BackGroundVisibility)
        {
          tileButton1.Background = (Brush) null;
        }
        else
        {
          TileButton tileButton2 = tileButton1;
          tileButton2.Background = (Brush) new SolidColorBrush(tileButton2.ImageBackGround);
        }
      }
      else
      {
        TileButton tileButton2 = tileButton1;
        tileButton2.Background = (Brush) new SolidColorBrush(tileButton2.ImageBackGround);
      }
    }

    public bool BackGroundVisibility
    {
      get
      {
        return (bool) this.GetValue(TileButton.BackGroundVisibilityProperty);
      }
      set
      {
        this.SetValue(TileButton.BackGroundVisibilityProperty, (object) value);
      }
    }

    public static void BackGroundVisibilityOnChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      TileButton.SetBackgroundVisibility(dependencyObject, e);
    }

    public bool UseBackGroundVisibility
    {
      get
      {
        return (bool) this.GetValue(TileButton.UseBackGroundVisibilityProperty);
      }
      set
      {
        this.SetValue(TileButton.UseBackGroundVisibilityProperty, (object) value);
      }
    }

    public static void UseBackGroundVisibilityOnChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      TileButton.SetBackgroundVisibility(dependencyObject, e);
    }

    public Color ImageBackGround
    {
      get
      {
        return (Color) this.GetValue(TileButton.ImageBackGroundProperty);
      }
      set
      {
        this.SetValue(TileButton.ImageBackGroundProperty, (object) value);
      }
    }

    public static void ImageBackGroundOnChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      TileButton.SetBackgroundVisibility(dependencyObject, e);
    }

    public string ContentImageSourceFileName
    {
      get
      {
        return (string) this.GetValue(TileButton.ContentImageSourceFileNameProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentImageSourceFileNameProperty, (object) value);
      }
    }

    public string ContentImageSourcePath
    {
      get
      {
        return (string) this.GetValue(TileButton.ContentImageSourcePathProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentImageSourcePathProperty, (object) value);
      }
    }

    public ImageSource ContentImageSource
    {
      get
      {
        return (ImageSource) this.GetValue(TileButton.ContentImageSourceProperty);
      }
      set
      {
        this.SetValue(TileButton.ContentImageSourceProperty, (object) value);
      }
    }

    public static void SetContentImageSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      TileButton tileButton = dependencyObject as TileButton;
      ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
      if (string.IsNullOrEmpty(tileButton.ContentImageSourceFileName))
      {
        tileButton.ContentImageSource = (ImageSource) null;
      }
      else
      {
        try
        {
          tileButton.ContentImageSource = (ImageSource) new FileResources().Data.Get<ImageSource>(tileButton.ContentImageSourceFileName);
          if (tileButton.ContentImageSource != null)
            return;
          string text = tileButton.ContentImageSourcePath + tileButton.ContentImageSourceFileName;
          if (!imageSourceConverter.IsValid((object) text))
            return;
          tileButton.ContentImageSource = (ImageSource) imageSourceConverter.ConvertFromString(text);
        }
        catch
        {
        }
      }
    }
  }
}
