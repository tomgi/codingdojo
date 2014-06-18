using System.Collections.Generic;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class TableSourceReference : IReference<FileAsset>, IAttributeBag
    {
        public FileAsset AssetModel { get; private set; }
        public long RelationType
        {
            get
            {
                return 1000;
            }
        }
        private readonly ISet<IHaveNameAndId> _attributes = new HashSet<IHaveNameAndId>();
        public ISet<IHaveNameAndId> Attributes
        {
            get { return _attributes; }
        }

        public TableSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}