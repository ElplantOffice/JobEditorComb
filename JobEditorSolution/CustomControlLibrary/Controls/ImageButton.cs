

using Patterns.EventAggregator;
using System.Windows;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class ImageButton : System.Windows.Controls.Button
  {
    public static readonly DependencyProperty SourceFileNameProperty = DependencyProperty.Register(nameof (SourceFileName), typeof (string), typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.SetSource)));
    public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(nameof (SourcePath), typeof (string), typeof (ImageButton), new PropertyMetadata((object) "Images/", new PropertyChangedCallback(ImageButton.SetSource)));
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (ImageSource), typeof (ImageButton), new PropertyMetadata((PropertyChangedCallback) null));

    static ImageButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ImageButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ImageButton)));
    }

    public string SourceFileName
    {
      get
      {
        return (string) this.GetValue(ImageButton.SourceFileNameProperty);
      }
      set
      {
        this.SetValue(ImageButton.SourceFileNameProperty, (object) value);
      }
    }

    public string SourcePath
    {
      get
      {
        return (string) this.GetValue(ImageButton.SourcePathProperty);
      }
      set
      {
        this.SetValue(ImageButton.SourcePathProperty, (object) value);
      }
    }

    public ImageSource Source
    {
      get
      {
        return (ImageSource) this.GetValue(ImageButton.SourceProperty);
      }
      set
      {
        this.SetValue(ImageButton.SourceProperty, (object) value);
      }
    }

    public static void SetSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = dependencyObject as ImageButton;
      ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
      if (string.IsNullOrEmpty(imageButton.SourceFileName))
      {
        imageButton.Source = (ImageSource) null;
      }
      else
      {
        try
        {
          imageButton.Source = (ImageSource) new FileResources().Data.Get<ImageSource>(imageButton.SourceFileName);
          if (imageButton.Source != null)
            return;
          string text = imageButton.SourcePath + imageButton.SourceFileName;
          if (!imageSourceConverter.IsValid((object) text))
            return;
          imageButton.Source = (ImageSource) imageSourceConverter.ConvertFromString(text);
        }
        catch
        {
        }
      }
    }
  }
}
