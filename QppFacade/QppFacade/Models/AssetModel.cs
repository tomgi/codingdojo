using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.quark.qpp.core.asset.service.dto;

namespace QppFacade
{
    public class AssetModel : AttributeBag
    {
        public long Id { get; set; }

        public readonly List<Relation> Relations = new List<Relation>();

        public IEnumerable<Relation> RelationsOfType(long relationType)
        {
            return Relations
                .Where(r => r.RelationType == relationType);
        }

        public AssetModel WithRelation(Relation relation)
        {
            Relations.Add(relation);
            return this;
        }

        public static AssetModel FromFile(string filePath)
        {
            return new AssetModel()
                .With(PhoenixAttributes.NAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION, Path.GetExtension(filePath));
        }

        public Asset ToAsset()
        {
            return new Asset
            {
                assetId = Id,
                attributeValues = GimmeAttributeValues()
            };
        }
    }
}