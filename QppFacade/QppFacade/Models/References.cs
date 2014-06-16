using System.Collections.Generic;
using System.IO;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.relation.service.constants;

namespace QppFacade
{

    public interface IReference<out TAssetModel> where TAssetModel : FileAsset
    {
        TAssetModel AssetModel { get; }
    }

    public class XmlReference<TAssetModel> : IReference<TAssetModel> where TAssetModel : FileAsset
    {
        public TAssetModel AssetModel { get; set; }

        public long RelationType {
            get
            {
                return DefaultRelationTypes.XML_COMP_REFERENCE;
            }
        }

        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        private void With(object attributeId, object attributeValue)
        {
            if (false == Attributes.ContainsKey(attributeId))
                Attributes.Add(attributeId, attributeValue);
            Attributes[attributeId] = attributeValue;
        }
        public XmlReference<TAssetModel> With(long attributeId, object attributeValue)
        {
            With(attributeId as object, attributeValue);
            return this;
        }
        public XmlReference<TAssetModel> With(string attributeName, object attributeValue)
        {
            With(attributeName as object, attributeValue);
            return this;
        }

        public XmlReference(string xPath, TAssetModel assetModel)
        {
            AssetModel = assetModel;
            With(DefaultAttributes.XPATH,xPath);
        }
        public XmlReference(TAssetModel assetModel)
        {
            AssetModel = assetModel;
        }
    }


    public class TableSourceReference : IReference<FileAsset>
    {
        public FileAsset AssetModel { get; private set; }
        public long RelationType
        {
            get
            {
                return 1000;
            }
        }
        private readonly Dictionary<object, object> _attributes = new Dictionary<object, object>();

        public Dictionary<object, object> Attributes
        {
            get { return _attributes; }
        }

        private void With(object attributeId, object attributeValue)
        {
            if (false == Attributes.ContainsKey(attributeId))
                Attributes.Add(attributeId, attributeValue);
            Attributes[attributeId] = attributeValue;
        }
        public TableSourceReference With(long attributeId, object attributeValue)
        {
            With(attributeId as object, attributeValue);
            return this;
        }
        public TableSourceReference With(string attributeName, object attributeValue)
        {
            With(attributeName as object, attributeValue);
            return this;
        }

        public TableSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}