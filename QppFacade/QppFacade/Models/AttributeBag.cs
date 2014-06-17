using System.Collections.Generic;
using System.IO;
using com.quark.qpp.core.attribute.service.constants;

namespace QppFacade
{
    public interface IAttributeBag
    {
        Dictionary<object, object> Attributes {get;}
    }

    public static class AssetExtensions
    {
        private static T With<T>(T asset, object attributeId, object attributeValue) where T : IAttributeBag
        {
            if (false == asset.Attributes.ContainsKey(attributeId))
                asset.Attributes.Add(attributeId, attributeValue);
            asset.Attributes[attributeId] = attributeValue;
            return asset;
        }

        public static T With<T>(this T asset, long attributeId, object attributeValue) where T : IAttributeBag
        {
            With(asset, attributeId as object, attributeValue);
            return asset;
        }

        public static T With<T>(this T asset, string attributeName, object attributeValue) where T : IAttributeBag
        {
            With(asset, attributeName as object, attributeValue);
            return asset;
        }
        
    }

    public class FileAsset : IAttributeBag
    {
        public long Id { get; set; }
        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();
        private readonly string _filePath;

        public Dictionary<object, object> Attributes
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
            this.With(DefaultAttributes.NAME, Path.GetFileName(filePath))
                .With(DefaultAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(DefaultAttributes.FILE_EXTENSION, Path.GetExtension(filePath));
        }

        public FileAsset()
        {
        }
    }
}