using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QppFacade
{
    public class AssetModel : AttributeBag
    {
        public long Id { get; set; }

        public Func<Stream> OpenContentStream { get; private set; }

        public readonly List<Relation> Relations = new List<Relation>();

        public void WithContentDo(Action<Stream> streamAction)
        {
            using (var stream = OpenContentStream())
            {
                streamAction(stream);
            }
        }

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
            return new AssetModel
            {
                OpenContentStream = () => File.Open(filePath, FileMode.Open, FileAccess.Read)
            }
                .With(PhoenixAttributes.NAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.ORIGINAL_FILENAME, Path.GetFileName(filePath))
                .With(PhoenixAttributes.FILE_EXTENSION, Path.GetExtension(filePath));
        }
    }
}