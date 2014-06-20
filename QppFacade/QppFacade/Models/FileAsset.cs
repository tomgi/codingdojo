using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class FileAsset : IAttributeBag
    {
        public long Id { get; set; }
        private readonly ISet<IAttribute> _attributes = new HashSet<IAttribute>();
        private readonly string _filePath;

        public IAttribute this[IAttribute index]
        {
            get
            {
                var attributeValue = Attributes.FirstOrDefault(attr => attr.Equals(index));
                if (attributeValue == null)
                {
                    attributeValue = index.New();
                    Attributes.Add(attributeValue);
                }
                return attributeValue;
            }
        }

        public ISet<IAttribute> Attributes
        {
            get { return _attributes; }
        }

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