using System.Linq;
using com.quark.qpp.core.asset.service.dto;

namespace QppFacade
{
    public sealed class Relation 
    {
        private readonly AttributeBag _attributeBag = new AttributeBag();

        private Relation(AssetModel assetModel, long relationType)
        {
            AssetModel = assetModel;
            RelationType = relationType;
        }

        public AssetModel AssetModel { get; private set; }
        public long RelationType { get; private set; }

        public Relation With<TValue>(PhoenixAttribute<TValue> attribute, TValue value)
        {
            _attributeBag.Set(attribute, value);
            return this;
        }

        public static RelationBuilder To(AssetModel assetModel)
        {
            return new RelationBuilder(assetModel);
        }

        public class RelationBuilder
        {
            private readonly AssetModel _assetModel;

            internal RelationBuilder(AssetModel assetModel)
            {
                _assetModel = assetModel;
            }

            public Relation OfType(long relationType)
            {
                return new Relation(_assetModel, relationType);
            }
        }

        public AssetRelation ToAssetRelation(long parentAssetId)
        {
            return new AssetRelation
            {
                childAssetId = AssetModel.Id,
                parentAssetId = parentAssetId,
                relationAttributes = _attributeBag.ToArray(),
                relationTypeId = RelationType,
            };
        }
    }
}