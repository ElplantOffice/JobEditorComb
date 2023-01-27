
using System;
using System.Windows;
using System.Windows.Input;

namespace CustomControlLibrary
{
  public class ScrollViewer : System.Windows.Controls.ScrollViewer
  {
    static ScrollViewer()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ScrollViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ScrollViewer)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.ManipulationBoundaryFeedback += new EventHandler<ManipulationBoundaryFeedbackEventArgs>(this.OnManipulationBoundaryFeedback);
    }

    private void OnManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
    {
      e.Handled = true;
    }
  }
}
