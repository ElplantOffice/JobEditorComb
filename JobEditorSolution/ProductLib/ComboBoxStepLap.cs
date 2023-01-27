
using System;
using System.Windows.Controls;

namespace ProductLib
{
  public class ComboBoxStepLap : ComboBox
  {
    public ComboBoxStepLap()
    {
      this.Items.Add((object) Enum.GetNames(typeof (EStepLapType)));
    }
  }
}
