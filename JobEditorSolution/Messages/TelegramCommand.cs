
namespace Messages
{
  public enum TelegramCommand
  {
    None = 0,
    Create = 1,
    Created = 2,
    Dispose = 3,
    Disposed = 4,
    Show = 201, // 0x000000C9
    Visible = 202, // 0x000000CA
    Hide = 203, // 0x000000CB
    Hidden = 204, // 0x000000CC
    CreateQueue = 301, // 0x0000012D
    TranslationReply = 401, // 0x00000191
  }
}
