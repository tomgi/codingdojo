namespace QppFacade
{
    public class Relation : AttributeBag
    {
        private Relation(AssetModel assetModel, long relationType)
        {
            AssetModel = assetModel;
            RelationType = relationType;
        }

        public AssetModel AssetModel { get; private set; }
        public long RelationType { get; private set; }

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
    }
}