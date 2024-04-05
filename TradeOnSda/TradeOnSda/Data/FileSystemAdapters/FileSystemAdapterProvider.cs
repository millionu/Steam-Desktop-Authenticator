using System;

namespace TradeOnSda.Data.FileSystemAdapters;

public class FileSystemAdapterProvider
{
    private readonly IFileSystemAdapter _fileSystemAdapter;

    public FileSystemAdapterProvider()
    {
        if (OperatingSystem.IsMacOS())
        {
            _fileSystemAdapter = new MacFileSystemAdapter();
            return;
        }

        if (OperatingSystem.IsWindows())
        {
            _fileSystemAdapter = new WindowsFileSystemAdapter();
            return;
        }

        throw new NotSupportedException("��֧�� macOS �� Windows");
    }

    public IFileSystemAdapter GetAdapter()
    {
        return _fileSystemAdapter;
    }
}