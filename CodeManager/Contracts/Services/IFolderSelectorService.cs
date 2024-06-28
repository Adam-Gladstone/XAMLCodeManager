﻿namespace CodeManager.Contracts.Services;

public interface IFolderSelectorService
{
    string Folder
    {
        get;
    }

    Task InitializeAsync();

    Task SetFolderAsync(string folder);

    Task SetRequestedFolderAsync();
}
