
using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace ModelLibrary
{
  public class TestPlcModel : ModelBase
  {
        private List<ISubscription<Telegram>> _subscriptions = new List<ISubscription<Telegram>>();
        private ModelAttributedEventType<string> AxisWindow;
    private ModelAttributedEventType<string> AxisLabel;
    private ModelAttributedEventType<string> AxisButton;
    private ModelAttributedEventType<double> AxisField;
    private ModelAttributedEventType<bool> AxisCheckBox;
    private double AxisPos;

    public TestPlcModel(UIProtoType uiProtoType)
    {
      this.AxisWindow = new ModelAttributedEventType<string>((IModel) this, uiProtoType.Model.Address, (Expression<Func<ModelAttributedEventType<string>>>) (() => this.AxisWindow));
      this.AxisLabel = new ModelAttributedEventType<string>((IModel) this, uiProtoType.Model.Address, (Expression<Func<ModelAttributedEventType<string>>>) (() => this.AxisLabel));
      this.AxisButton = new ModelAttributedEventType<string>((IModel) this, uiProtoType.Model.Address, (Expression<Func<ModelAttributedEventType<string>>>) (() => this.AxisButton));
      this.AxisField = new ModelAttributedEventType<double>((IModel) this, uiProtoType.Model.Address, (Expression<Func<ModelAttributedEventType<double>>>) (() => this.AxisField));
      this.AxisCheckBox = new ModelAttributedEventType<bool>((IModel) this, uiProtoType.Model.Address, (Expression<Func<ModelAttributedEventType<bool>>>) (() => this.AxisCheckBox));
      this.AxisWindow.RegisterAsPublisher();
      this.AxisLabel.RegisterAsPublisher();
      this.AxisField.RegisterRelay();
      this.AxisButton.RegisterRelay();
      this.AxisCheckBox.RegisterRelay();
      this.AxisLabel.ContentText = "Test X";
      this.AxisField.IsEnabled = true;
      this.AxisButton.IsEnabled = true;
      this.AxisCheckBox.IsEnabled = true;
      this.AxisWindow.ContentText = (string) null;
      this.AxisWindow.Visibility = UiElementDataVisibility.Visible;
    }

    public void AxisPositionChange(Telegram telegram)
    {
      this.AxisWindow.ContentText = nameof (AxisPositionChange);
      this.AxisPos = (telegram.Value as UiElementData<double>).Value;
    }

    public void AxisMoveButtonClick(Telegram telegram)
    {
      this.AxisWindow.ContentText = nameof (AxisMoveButtonClick);
      this.AxisField.Value = this.AxisPos + 1000.0;
      this.AxisPos = this.AxisField.Value;
      this.AxisButton.ContentText = "Moved";
    }

    public void AxisCheckBoxClick(Telegram telegram)
    {
      UiElementData<bool> uiElementData = telegram.Value as UiElementData<bool>;
      this.AxisCheckBox.Value = uiElementData.Value;
      this.AxisWindow.ContentText = !uiElementData.Value ? "AxisCheckBox Value Changed, Value=False" : "AxisCheckBox Value Changed, Value=True";
      Thread.Sleep(2000);
      this.AxisCheckBox.Value = false;
      this.AxisWindow.ContentText = "AxisCheckBoxClicked, Value Set Back to False";
    }
  }
}
