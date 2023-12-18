// -----------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;

    public class FileSystem : IFileSystem
    {
        private readonly ILogger<FileSystem> logger;

        public FileSystem(
            ILogger<FileSystem> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void FileWriteAllText(
            string path,
            string content)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            path = Path.Combine(App.AppDataFolderPath, path);

            this.logger.LogInformation("Writing content to {path}", path);
            File.WriteAllText(path, content);
        }
    }
}
