using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.quark.qpp.core.attribute.service.constants;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public interface IAttributeBag
    {
        ISet<IHaveNameAndId> Attributes { get; }
    }

    public static class AssetExtensions
    {
        public static T With<T>(this T asset, IHaveNameAndId attributeId) where T : IAttributeBag
        {
            if (asset.Attributes.Contains(attributeId))
                asset.Attributes.Remove(attributeId);
            asset.Attributes.Add(attributeId);
            return asset;
        }        
    }

    public class FileAsset : IAttributeBag
    {
        public long Id { get; set; }
        private readonly ISet<IHaveNameAndId> _attributes = new HashSet<IHaveNameAndId>();
        private readonly string _filePath;

        public object this[IHaveNameAndId index]
        {
            get
            {
                return Attributes.First(elem => Equals(elem, index)).Value;
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
            this.With(PhoenixAttributes.NAME.WithValue(Path.GetFileName(filePath)))
                .With(PhoenixAttributes.ORIGINAL_FILENAME.WithValue(Path.GetFileName(filePath)))
                .With(PhoenixAttributes.FILE_EXTENSION.WithValue(Path.GetExtension(filePath)));
        }

        public FileAsset()
        {
        }
    }
}