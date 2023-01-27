
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace CustomControlLibrary
{
  public class RichTextBox : System.Windows.Controls.RichTextBox
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (RichTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RichTextBox.OnTextPropertyChanged), new CoerceValueCallback(RichTextBox.CoerceTextProperty), true, UpdateSourceTrigger.LostFocus));
    public static readonly DependencyProperty TextFormatterProperty = DependencyProperty.Register(nameof (TextFormatter), typeof (ITextFormatter), typeof (RichTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) new RtfFormatter(), new PropertyChangedCallback(RichTextBox.OnTextFormatterPropertyChanged)));
    public static readonly DependencyProperty DocumentFileNameProperty = DependencyProperty.Register(nameof (DocumentFileName), typeof (string), typeof (RichTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(RichTextBox.SetDocumentSource)));
    public static readonly DependencyProperty DocumentPathProperty = DependencyProperty.Register(nameof (DocumentPath), typeof (string), typeof (RichTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(RichTextBox.SetDocumentSource)));
    public static readonly DependencyProperty DocumentSourceProperty = DependencyProperty.Register(nameof (DocumentSource), typeof (string), typeof (RichTextBox), new PropertyMetadata((object) null, new PropertyChangedCallback(RichTextBox.LoadDocumentSource)));
    private bool _preventDocumentUpdate;
    private bool _preventTextUpdate;

    public RichTextBox()
    {
    }

    public RichTextBox(FlowDocument document)
      : base(document)
    {
    }

    public string Text
    {
      get
      {
        return (string) this.GetValue(RichTextBox.TextProperty);
      }
      set
      {
        this.SetValue(RichTextBox.TextProperty, (object) value);
      }
    }

    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((RichTextBox) d).UpdateDocumentFromText();
    }

    private static object CoerceTextProperty(DependencyObject d, object value)
    {
      return value ?? (object) "";
    }

    public ITextFormatter TextFormatter
    {
      get
      {
        return (ITextFormatter) this.GetValue(RichTextBox.TextFormatterProperty);
      }
      set
      {
        this.SetValue(RichTextBox.TextFormatterProperty, (object) value);
      }
    }

    private static void OnTextFormatterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as RichTextBox)?.OnTextFormatterPropertyChanged((ITextFormatter) e.OldValue, (ITextFormatter) e.NewValue);
    }

    protected virtual void OnTextFormatterPropertyChanged(ITextFormatter oldValue, ITextFormatter newValue)
    {
      this.UpdateTextFromDocument();
    }

    public string DocumentFileName
    {
      get
      {
        return (string) this.GetValue(RichTextBox.DocumentFileNameProperty);
      }
      set
      {
        this.SetValue(RichTextBox.DocumentFileNameProperty, (object) value);
      }
    }

    public string DocumentPath
    {
      get
      {
        return (string) this.GetValue(RichTextBox.DocumentPathProperty);
      }
      set
      {
        this.SetValue(RichTextBox.DocumentPathProperty, (object) value);
      }
    }

    public string DocumentSource
    {
      get
      {
        return (string) this.GetValue(RichTextBox.DocumentSourceProperty);
      }
      set
      {
        this.SetValue(RichTextBox.DocumentSourceProperty, (object) value);
      }
    }

    public static void SetDocumentSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      RichTextBox richTextBox = dependencyObject as RichTextBox;
      if (richTextBox == null)
        return;
      richTextBox.DocumentSource = richTextBox.DocumentPath + "/" + richTextBox.DocumentFileName;
      RichTextBox.ReadFile(richTextBox.DocumentSource, richTextBox.Document);
    }

    public static void LoadDocumentSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      RichTextBox richTextBox = dependencyObject as RichTextBox;
      if (richTextBox == null)
        return;
      RichTextBox.ReadFile(richTextBox.DocumentSource, richTextBox.Document);
    }

    private static void ReadFile(string inFilename, FlowDocument inFlowDocument)
    {
      if (!File.Exists(inFilename))
        return;
      TextRange textRange = new TextRange(inFlowDocument.ContentStart, inFlowDocument.ContentEnd);
      FileStream fileStream1 = new FileStream(inFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
      FileStream fileStream2 = fileStream1;
      string rtf = DataFormats.Rtf;
      textRange.Load((Stream) fileStream2, rtf);
      fileStream1.Close();
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
      base.OnTextChanged(e);
      this.UpdateTextFromDocument();
    }

    private void UpdateTextFromDocument()
    {
      if (this._preventTextUpdate)
        return;
      this._preventDocumentUpdate = true;
      this.Text = this.TextFormatter.GetText(this.Document);
      this._preventDocumentUpdate = false;
    }

    private void UpdateDocumentFromText()
    {
      if (this._preventDocumentUpdate)
        return;
      this._preventTextUpdate = true;
      this.TextFormatter.SetText(this.Document, this.Text);
      this._preventTextUpdate = false;
    }

    public void Clear()
    {
      this.Document.Blocks.Clear();
    }

    public override void BeginInit()
    {
      base.BeginInit();
      this._preventTextUpdate = true;
      this._preventDocumentUpdate = true;
    }

    public override void EndInit()
    {
      base.EndInit();
      this._preventTextUpdate = false;
      this._preventDocumentUpdate = false;
      if (!string.IsNullOrEmpty(this.Text))
        this.UpdateDocumentFromText();
      else
        this.UpdateTextFromDocument();
    }
  }
}
