
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CustomControlLibrary
{
  public static class NumericTextBoxBehavior
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (NumericTextBoxBehavior), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(NumericTextBoxBehavior.OnEnabledStateChanged)));

    public static bool GetIsEnabled(DependencyObject source)
    {
      return (bool) source.GetValue(NumericTextBoxBehavior.IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject source, bool value)
    {
      source.SetValue(NumericTextBoxBehavior.IsEnabledProperty, (object) value);
    }

    private static void OnEnabledStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (textBox == null)
        return;
      textBox.PreviewTextInput -= new TextCompositionEventHandler(NumericTextBoxBehavior.tbb_PreviewTextInput);
      DataObject.RemovePastingHandler((DependencyObject) textBox, new DataObjectPastingEventHandler(NumericTextBoxBehavior.OnClipboardPaste));
      if ((e.NewValue == null || !(e.NewValue.GetType() == typeof (bool)) ? 0 : ((bool) e.NewValue ? 1 : 0)) == 0)
        return;
      textBox.PreviewTextInput += new TextCompositionEventHandler(NumericTextBoxBehavior.tbb_PreviewTextInput);
      DataObject.AddPastingHandler((DependencyObject) textBox, new DataObjectPastingEventHandler(NumericTextBoxBehavior.OnClipboardPaste));
    }

    private static void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
    {
      TextBox tb = sender as TextBox;
      string data = e.SourceDataObject.GetData(e.FormatToApply) as string;
      if (tb == null || string.IsNullOrEmpty(data) || NumericTextBoxBehavior.Validate(tb, data))
        return;
      e.CancelCommand();
    }

    private static void tbb_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox tb = sender as TextBox;
      if (tb == null || NumericTextBoxBehavior.Validate(tb, e.Text))
        return;
      e.Handled = true;
    }

    private static bool Validate(TextBox tb, string newContent)
    {
      string empty = string.Empty;
      string input;
      if (!string.IsNullOrEmpty(tb.SelectedText))
      {
        string str1 = tb.Text.Substring(0, tb.SelectionStart);
        string str2 = tb.Text.Substring(tb.SelectionStart + tb.SelectionLength, tb.Text.Length - (tb.SelectionStart + tb.SelectionLength));
        string str3 = newContent;
        string str4 = str2;
        input = str1 + str3 + str4;
      }
      else
      {
        string str1 = tb.Text.Substring(0, tb.CaretIndex);
        string str2 = tb.Text.Substring(tb.CaretIndex, tb.Text.Length - tb.CaretIndex);
        string str3 = newContent;
        string str4 = str2;
        input = str1 + str3 + str4;
      }
      return new Regex("^([-+]?)(\\d*)([,.]?)(\\d*)$").IsMatch(input);
    }
  }
}
