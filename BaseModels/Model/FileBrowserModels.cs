using System;
using System.Collections.Generic;
using ClientMsgs;

namespace Models
{
    /// <summary>
    /// Represents a file or directory item in the file browser
    /// </summary>
    [Serializable]
    public class FileBrowserItem
    {
        /// <summary>
        /// Name of the file or directory
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full path of the file or directory
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Whether this is a directory
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// File size in bytes (0 for directories)
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// File extension (empty for directories)
        /// </summary>
        public string Extension { get; set; }
    }

    /// <summary>
    /// Response for file browser list API
    /// </summary>
    [Serializable]
    public class FileBrowserResponse : BooleanResponse
    {
        /// <summary>
        /// Current directory path
        /// </summary>
        public string CurrentPath { get; set; }

        /// <summary>
        /// List of files and directories
        /// </summary>
        public List<FileBrowserItem> Items { get; set; } = new List<FileBrowserItem>();

        /// <summary>
        /// Parent directory path (null if at root)
        /// </summary>
        public string ParentPath { get; set; }
    }

    /// <summary>
    /// Request parameters for file browser API
    /// </summary>
    [Serializable]
    public class FileBrowserRequest
    {
        /// <summary>
        /// Path to browse (optional, defaults to root upload directory)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Whether to include hidden files
        /// </summary>
        public bool IncludeHidden { get; set; } = false;
    }
}