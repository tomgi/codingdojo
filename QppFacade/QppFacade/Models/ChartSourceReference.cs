using System.Collections.Generic;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class ChartSourceReference : IReference<FileAsset>, IAttributeBag
    {
        public FileAsset AssetModel { get; private set; }
        public long RelationType
        {
            get
            {
                return 1001;
            }
        }
        private readonly ISet<IHaveNameAndId> _attributes = new HashSet<IHaveNameAndId>();
        public ISet<IHaveNameAndId> Attributes
        {
            get { return _attributes; }
        }

        public ChartSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}