using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.dto;

namespace QppFacade
{
    public sealed class AssetModel
    {
        public long Id { get; set; }

        private readonly List<Relation> _relations = new List<Relation>();
        private readonly AttributeBag _attributeBag;

        private AssetModel(IEnumerable<AttributeValue> attributes)
        {
            _attributeBag = new AttributeBag(attributes);
        }

        public IReadOnlyCollection<Relation> Relations
        {
            get { return _relations; }
        }

        public IReadOnlyCollection<AttributeValue> AttributeValues
        {
            get { return _attributeBag; }
        }

        public AssetModel WithRelation(Relation relation)
        {
            _relations.Add(relation);
            return this;
        }

        public AssetModel With<TValue>(PhoenixAttribute<TValue> attribute, TValue value)
        {
            _attributeBag.Set(attribute, value);
            return this;
        }

        public TValue Get<TValue>(PhoenixAttribute<TValue> value)
        {
            return _attributeBag.Get(value);
        }

        public Asset ToAsset()
        {
            return new Asset
            {
                assetId = Id,
                attributeValues = _attributeBag.ToArray()
            };
        }

        public static AssetModel FromFile(string filePath)
        {
            return new AssetModel(Enumerable.Empty<AttributeValue>())
                .With(PhoenixAttributes.NAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION, Path.GetExtension(filePath));
        }

        public static AssetModel FromAsset(Asset asset)
        {
            return new AssetModel(asset.attributeValues) {Id = asset.assetId};
        }
    }
}