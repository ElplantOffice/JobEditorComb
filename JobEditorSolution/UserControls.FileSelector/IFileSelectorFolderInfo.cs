
using System;

namespace UserControls.FileSelector
{
  public interface IFileSelectorFolderInfo
  {
    string FolderInfoUid { get; }

    string ViewFolderName { get; }

    string ViewFolderDate { get; }

    string FolderName { get; }

    string FolderPath { get; }

    string FullFolderName { get; }

    DateTime FolderDate { get; }
  }
}
