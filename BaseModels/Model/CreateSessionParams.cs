using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Models
{
    /// <summary>
    /// Session creation params
    /// </summary>
    [Serializable]
    public class CreateSessionParams
    {
        /// <summary>
        /// Size of data block (in bytes)
        /// </summary>
        [Required]
        public int ChunkSize { get; set; }

        /// <summary>
        /// Total file size (in bytes)
        /// </summary>
        [Required]
        public long TotalSize { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        [Required]
        public string FileName { get; set; }


        public string ?dir { get; set; }
        public bool dontChangeFileNameWithGUID { get; set; } = false;
    }

    public class UploadFileChunk
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }

}