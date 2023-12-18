// -----------------------------------------------------------------------
// <copyright file="IFileSystem.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    public interface IFileSystem
    {
        void FileWriteAllText(string path, string content);
    }
}
