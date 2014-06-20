using System.Collections.Generic;
using System.IO;
using System.Linq;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class FileAsset : IAttributeBag
    {
        public long Id { get; set; }
        private readonly ISet<IHaveNameAndId> _attributes = new HashSet<IHaveNameAndId>();
        private readonly string _filePath;

        public IHaveNameAndId this[IHaveNameAndId index]
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

        public ISet<IHaveNameAndId> Attributes
        {
            get { return _attributes; }
        }

        public virtual Stream Content {
            get
            {
                return File.Open(_filePath, FileMode.Open, FileAccess.Read);
            }
        }

        public FileAsset(string filePath)
        {
            _filePath = filePath;
            this.With(PhoenixAttributes.NAME,Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME,Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION,Path.GetExtension(filePath));
        }

        public FileAsset()
        {
        }
    }
}