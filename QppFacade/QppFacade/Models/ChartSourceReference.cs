using System.Collections.Generic;
using System.Linq;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class ChartSourceReference : AttributeBag, IReference<FileAsset>
    {
        public FileAsset AssetModel { get; private set; }

        public long RelationType
        {
            get { return 1001; }
        }

        private readonly IDictionary<IAttribute, object> _attributes = new Dictionary<IAttribute, object>();

        public ChartSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}