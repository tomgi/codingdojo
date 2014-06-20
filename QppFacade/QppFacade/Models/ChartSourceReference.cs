﻿using System.Collections.Generic;
using System.Linq;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;

namespace QppFacade
{
    public class ChartSourceReference : IReference<FileAsset>, IAttributeBag
    {
        public FileAsset AssetModel { get; private set; }

        public long RelationType
        {
            get { return 1001; }
        }

        private readonly ISet<IAttribute> _attributes = new HashSet<IAttribute>();

        public ISet<IAttribute> Attributes
        {
            get { return _attributes; }
        }

        public IAttribute this[IAttribute index]
        {
            get
            {
                var attributeValue = Attributes.FirstOrDefault(attr => attr.Equals(index));
                if (attributeValue == null)
                {
                    attributeValue = index.New();
                    Attributes.Add(attributeValue);
                }
                return attributeValue;
            }
        }

        public ChartSourceReference(FileAsset assetModel)
        {
            AssetModel = assetModel;
        }
    }
}