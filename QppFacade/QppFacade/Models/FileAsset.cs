using System;
using System.Collections.Generic;
using System.IO;
using com.quark.qpp.core.attribute.service.dto;

namespace QppFacade
{
    public class FileAsset : AttributeBag
    {
        public long Id { get; set; }
        private readonly string _filePath;

        protected virtual Stream CreateStream()
        {
            return File.Open(_filePath, FileMode.Open, FileAccess.Read);
        }

        public FileAsset(string filePath)
        {
            _filePath = filePath;
            this.With(PhoenixAttributes.NAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION, Path.GetExtension(filePath));
        }

        public FileAsset()
        {
        }

        public void WithContentDo(Action<Stream> streamAction)
        {
            using (var stream = CreateStream())
            {
                streamAction(stream);
            }
        }
    }
}