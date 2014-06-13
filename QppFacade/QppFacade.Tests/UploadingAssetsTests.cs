using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
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
            _assetId = _sut.Upload(
                new FileAsset("asset.txt")
                    .With(DefaultAttributes.CONTENT_TYPE, "Chemical Report")
                    .With(DefaultAttributes.NAME, "acetic acid2")
                    .With(DefaultAttributes.WORKFLOW, "Default Workflow")
                    .With(DefaultAttributes.STATUS, "Default")
                    .With(DefaultAttributes.COLLECTION, "Home/Test")
                    .With(DefaultAttributes.FILE_EXTENSION, "txt")
                    .With(DefaultAttributes.ORIGINAL_FILENAME, "acetic acid2")
                    .With(DefaultAttributes.DITA_TITLE, "acetic acid2")
                );

        private It should_upload_asset_properly = () =>
        {
            var file = _sut.GetFile(_assetId);
            file.With(DefaultAttributes.DITA_TITLE, "dupa");
            _sut.UpdateFile(file);
            _fileUpdated = _sut.GetFile(_assetId);
            _fileUpdated.Attributes[DefaultAttributes.DITA_TITLE].ShouldEqual("dupa");
        };

        private It should_map_things_nicely = () =>
        {
            var model = new DatabaseModel()
            {
                Id = _fileUpdated.Id,
                DitaTitle = (string) _fileUpdated.Attributes[DefaultAttributes.DITA_TITLE],
                Name = (string) _fileUpdated.Attributes[DefaultAttributes.NAME]
            };
            Mapper.CreateMap<FileAsset, DatabaseModel>()
                  .ForMember(dest => dest.DitaTitle, opts => opts.MapFrom(fileAsset => fileAsset.Attributes[DefaultAttributes.DITA_TITLE]))
                  .ForMember(dest => dest.Name, opts => opts.MapFrom(fileAsset => fileAsset.Attributes[DefaultAttributes.NAME]));

            var modelAutoMapper = Mapper.Map<FileAsset,DatabaseModel>(_fileUpdated);
            modelAutoMapper.Id.ShouldEqual(_fileUpdated.Id);
        };

        private Cleanup after = () => _sut.Delete(_assetId);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static FileAsset _fileUpdated;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

    }

    public class DatabaseModel
    {
        public long Id { get; set; }
        public string DitaTitle { get; set; }
        public string Name { get; set; }
    }
}
