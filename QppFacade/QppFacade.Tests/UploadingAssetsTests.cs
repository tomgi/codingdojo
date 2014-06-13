using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Castle.Windsor;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.constants;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;
using Quark.CMSAdapters.QPP.UI;

namespace QppFacade.Tests
{
    [Subject(typeof (Type), "Category")]
    public class when_uploading_asset_on_the_platform
    {
        private Establish context = () =>
        {
            _container = new WindsorContainer();
            _container.Install(new QppFacadeWindsorInstaller());

            _sut = _container.Resolve<Qpp>();
        };


        private Because of = () =>
            _assetId = _sut.Upload(new Dictionary<object,object>
            {
                {DefaultAttributes.CONTENT_TYPE,"Chemical Report"},
                {DefaultAttributes.NAME,"acetic acid2"},
                {DefaultAttributes.WORKFLOW,"Default Workflow"},
                {DefaultAttributes.STATUS, "Default"},
                {DefaultAttributes.COLLECTION,"Home/Test"},
                {DefaultAttributes.FILE_EXTENSION,"txt"},
                {DefaultAttributes.ORIGINAL_FILENAME,"acetic acid2"},
               }
            , GenerateStreamFromString("content")
            );

            

        private It should_upload_asset_properly = () =>
        {
            Asset asset = _sut.Get(_assetId);
        };

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

    }
}
