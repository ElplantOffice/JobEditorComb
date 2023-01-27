
using Patterns.EventAggregator;
using System.Windows;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class Image : System.Windows.Controls.Image
  {
    public static readonly DependencyProperty SourceFileNameProperty = DependencyProperty.Register(nameof (SourceFileName), typeof (string), typeof (Image), new PropertyMetadata((object) null, new PropertyChangedCallback(Image.SetImageSource)));
    public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(nameof (SourcePath), typeof (string), typeof (Image), new PropertyMetadata((object) "Images/", new PropertyChangedCallback(Image.SetImageSource)));

    static Image()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Image), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Image)));
    }

    public string SourceFileName
    {
      get
      {
        return (string) this.GetValue(Image.SourceFileNameProperty);
      }
      set
      {
        this.SetValue(Image.SourceFileNameProperty, (object) value);
      }
    }

    public string SourcePath
    {
      get
      {
        return (string) this.GetValue(Image.SourcePathProperty);
      }
      set
      {
        this.SetValue(Image.SourcePathProperty, (object) value);
      }
    }

    public static void SetImageSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      Image image = dependencyObject as Image;
      ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
      if (string.IsNullOrEmpty(image.SourceFileName))
      {
        image.Source = (ImageSource) null;
      }
      else
      {
        try
        {
          image.Source = (ImageSource) new FileResources().Data.Get<ImageSource>(image.SourceFileName);
          if (image.Source != null)
            return;
          string text = image.SourcePath + image.SourceFileName;
          if (!imageSourceConverter.IsValid((object) text))
            return;
          image.Source = (ImageSource) imageSourceConverter.ConvertFromString(text);
        }
        catch
        {
        }
      }
    }
  }
}
