
namespace Models
{
  public class MenuTreeItemCommand : ITreeElementCommand<UiElementData<string>>
  {
    private ModelAttributedEventType<string> infoCommand;

    public MenuTreeItemCommand(ModelAttributedEventType<string> infoCommand = null)
    {
      this.infoCommand = infoCommand;
    }

    public bool Execute(UiElementData<string> item)
    {
      if (!item.IsEnabled)
        return false;
      this.infoCommand.Value = string.Format("Menu Command: {0}", (object) item.ContentText);
      return true;
    }
  }
}
