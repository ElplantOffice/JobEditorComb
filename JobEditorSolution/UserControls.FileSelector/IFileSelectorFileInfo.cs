
using System;
using System.Windows.Media;

namespace UserControls.FileSelector
{
  public interface IFileSelectorFileInfo
  {
    string FileInfoUid { get; }

    string ViewFileName { get; }

    string ViewFileDate { get; }

    string ViewFileSize { get; }

    ImageSource ViewFileImage { get; }

    bool IsSelected { get; set; }

    string FileName { get; }

    string FilePath { get; }

    string FullFileName { get; }

    DateTime FileDate { get; }

    long FileSize { get; }

    ImageSource FileImage { get; }
  }
}
