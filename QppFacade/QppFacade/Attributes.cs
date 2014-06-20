using System;
using System.Collections.Generic;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace QppFacade
{
    public class AttributePlaceholder : IHaveNameAndId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public void InitFromAttributeValue(AttributeValue value)
        {
            throw new NotImplementedException();
        }

        public AttributeValue ToAttributeValue()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUpdated()
        {
            throw new NotImplementedException();
        }

        public IHaveNameAndId New()
        {
            throw new NotImplementedException();
        }
    }

    public static partial class PhoenixAttributes
    {
        public static Dictionary<string, BaseAttribute> ByName { get; private set; }

        public static Dictionary<long, BaseAttribute> ById { get; private set; }

        private static bool Initialized = false;

        public static void Init(
            Func<IEnumerable<Attribute>> getQppAttributes,
            Func<int, IEnumerable<DomainValue>> getDomainValues,
            Func<string, long> getCollectionValues)
        {
            if (Initialized)
                return;
            Initialized = true;
            ByName = new Dictionary<string, BaseAttribute>();
            ById = new Dictionary<long, BaseAttribute>();

            foreach (var qppAttribute in getQppAttributes())
            {
                BaseAttribute attribute = null;
                if (qppAttribute.valueType == AttributeValueTypes.TEXT)
                {
                    attribute = new TextAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.NUMERIC)
                {
                    attribute = new NumAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.BOOLEAN)
                {
                    attribute = new BoolAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DATETIME)
                {
                    attribute = new DateTimeAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DOMAIN)
                {
                    if (qppAttribute.id == DefaultAttributes.COLLECTION)
                        attribute = new CollectionAttr(qppAttribute, getCollectionValues);
                    else
                        attribute = new DomainAttr(qppAttribute, getDomainValues);
                }
                if (attribute != null)
                {
                    ByName[qppAttribute.name] = attribute;
                    ById[qppAttribute.id] = attribute;
                }
            }

            foreach (var fieldInfo in typeof(PhoenixAttributes).GetFields())
            {
                var attributePlaceholder = (AttributePlaceholder) fieldInfo.GetValue(null);
                BaseAttribute attribute = null;
                if (attributePlaceholder.Id != 0)
                {
                    if (ById.ContainsKey(attributePlaceholder.Id))
                        attribute = ById[attributePlaceholder.Id];
                }
                else
                {
                    if (ByName.ContainsKey(attributePlaceholder.Name))
                        attribute = ByName[attributePlaceholder.Name];
                }
                if(attribute!=null)
                    fieldInfo.SetValue(null,attribute);
            }
        }
    }

    public static partial class PhoenixAttributes
    {
        public static IHaveNameAndId APP_STUDIO_ISSUE_URN = new AttributePlaceholder { Id = 180L };
        public static IHaveNameAndId ARTICLE_COMP_REFRENCE_ASSET_ID = new AttributePlaceholder { Id = 0x19eL };
        public static IHaveNameAndId ARTICLE_COMP_REFRENCE_ASSET_MAJ_VERSION = new AttributePlaceholder { Id = 0x19fL };
        public static IHaveNameAndId ARTICLE_COMP_REFRENCE_ASSET_MIN_VERSION = new AttributePlaceholder { Id = 0x1a0L };
        public static IHaveNameAndId ARTICLE_COMPONENT_ID = new AttributePlaceholder { Id = 0x131L };
        public static IHaveNameAndId ARTICLE_ID = new AttributePlaceholder { Id = 0x19dL };
        public static IHaveNameAndId ASSET_CHECKSUM = new AttributePlaceholder { Id = 0x197L };
        public static IHaveNameAndId ASSIGNED_LENGTH = new AttributePlaceholder { Id = 0x166L };
        public static IHaveNameAndId ATTACHED_COMPONENT_ID = new AttributePlaceholder { Id = 0x19cL };
        public static IHaveNameAndId ATTACHED_LAYOUT_ID = new AttributePlaceholder { Id = 0x191L };
        public static IHaveNameAndId ATTACHMENT_CHECKSUM = new AttributePlaceholder { Id = 0x198L };
        public static IHaveNameAndId AUXILIARY_DATA = new AttributePlaceholder { Id = 0x199L };
        public static IHaveNameAndId BOX_HEIGHT = new AttributePlaceholder { Id = 0x196L };
        public static IHaveNameAndId BOX_ID = new AttributePlaceholder { Id = 0x192L };
        public static IHaveNameAndId BOX_WIDTH = new AttributePlaceholder { Id = 0x195L };
        public static IHaveNameAndId CHARACTER_COUNT = new AttributePlaceholder { Id = 0x164L };
        public static IHaveNameAndId CHECK_OUT_DATE_TIME = new AttributePlaceholder { Id = 0x11L };
        public static IHaveNameAndId CHECKED_OUT_APPLICATION = new AttributePlaceholder { Id = 0x22L };
        public static IHaveNameAndId CHECKED_OUT_BY = new AttributePlaceholder { Id = 14L };
        public static IHaveNameAndId CHECKED_OUT_DURATION = new AttributePlaceholder { Id = 0x23L };
        public static IHaveNameAndId CHECKED_OUT_FILE_PATH = new AttributePlaceholder { Id = 0x10L };
        public static IHaveNameAndId CHECKED_OUT_MACHINE_NAME = new AttributePlaceholder { Id = 15L };
        public static IHaveNameAndId COLLECTION = new AttributePlaceholder { Id = 0x37L };
        public static IHaveNameAndId COLLECTION_PATH = new AttributePlaceholder { Id = 0x39L };
        public static IHaveNameAndId COLLECTION_TEMPLATE = new AttributePlaceholder { Id = 0x3aL };
        public static IHaveNameAndId COLOR_DEPTH = new AttributePlaceholder { Id = 0xccL };
        public static IHaveNameAndId COLOR_SPACE = new AttributePlaceholder { Id = 0xcdL };
        public static IHaveNameAndId COLUMN_WIDTH = new AttributePlaceholder { Id = 0x163L };
        public static IHaveNameAndId COMPONENT_NAME = new AttributePlaceholder { Id = 0x12fL };
        public static IHaveNameAndId COMPONENT_POSITION = new AttributePlaceholder { Id = 0x12eL };
        public static IHaveNameAndId CONTENT_CREATOR = new AttributePlaceholder { Id = 0x20L };
        public static IHaveNameAndId CONTENT_TYPE = new AttributePlaceholder { Id = 0x3eL };
        public static IHaveNameAndId CONTENT_TYPE_HIERARCHY = new AttributePlaceholder { Id = 0x3fL };
        public static IHaveNameAndId CREATED = new AttributePlaceholder { Id = 3L };
        public static IHaveNameAndId CREATOR = new AttributePlaceholder { Id = 5L };
        public static IHaveNameAndId CURRENT_BILLABLE_HOURS = new AttributePlaceholder { Id = 0x24L };
        public static IHaveNameAndId CURRENT_LENGTH = new AttributePlaceholder { Id = 0x165L };
        public static IHaveNameAndId DEPENDENT_ON_COLLECTION_RESOURCES = new AttributePlaceholder { Id = 0x1a2L };
        public static IHaveNameAndId DEVICE_NAME = new AttributePlaceholder { Id = 0x9eL };
        public static IHaveNameAndId DITA_AUDIENCE = new AttributePlaceholder { Id = 0x1c4L };
        public static IHaveNameAndId DITA_AUTHOR = new AttributePlaceholder { Id = 0x1c5L };
        public static IHaveNameAndId DITA_BRAND = new AttributePlaceholder { Id = 0x1c6L };
        public static IHaveNameAndId DITA_CATEGORY = new AttributePlaceholder { Id = 0x1c7L };
        public static IHaveNameAndId DITA_ID = new AttributePlaceholder { Id = 0x1c8L };
        public static IHaveNameAndId DITA_IMPORTANCE = new AttributePlaceholder { Id = 0x1d2L };
        public static IHaveNameAndId DITA_KEYWORDS = new AttributePlaceholder { Id = 0x1c9L };
        public static IHaveNameAndId DITA_LANGUAGE = new AttributePlaceholder { Id = 0x1d0L };
        public static IHaveNameAndId DITA_NAVIGATION_TITLE = new AttributePlaceholder { Id = 0x1caL };
        public static IHaveNameAndId DITA_OTHER_PROPERTIES = new AttributePlaceholder { Id = 0x1d1L };
        public static IHaveNameAndId DITA_PLATFORM = new AttributePlaceholder { Id = 0x1cbL };
        public static IHaveNameAndId DITA_PRODUCT_NAME = new AttributePlaceholder { Id = 460L };
        public static IHaveNameAndId DITA_PUBLISHING_CONTENT = new AttributePlaceholder { Id = 0x1cfL };
        public static IHaveNameAndId DITA_SEARCH_TITLE = new AttributePlaceholder { Id = 0x1cdL };
        public static IHaveNameAndId DITA_TITLE = new AttributePlaceholder { Id = 0x1ceL };
        public static IHaveNameAndId DUE_DATE = new AttributePlaceholder { Id = 0x29L };
        public static IHaveNameAndId DUE_TIME = new AttributePlaceholder { Id = 0x2aL };
        public static IHaveNameAndId FILE_EXTENSION = new AttributePlaceholder { Id = 11L };
        public static IHaveNameAndId FILE_PATH = new AttributePlaceholder { Id = 8L };
        public static IHaveNameAndId FILE_SIZE = new AttributePlaceholder { Id = 12L };
        public static IHaveNameAndId FIRST_PAGE = new AttributePlaceholder { Id = 0x33L };
        public static IHaveNameAndId GLOBAL_ID = new AttributePlaceholder { Id = 0x42L };
        public static IHaveNameAndId HAS_CHILDREN = new AttributePlaceholder { Id = 0x41L };
        public static IHaveNameAndId ID = new AttributePlaceholder { Id = 1L };
        public static IHaveNameAndId INDESIGN_OBJECT_UID = new AttributePlaceholder { Id = 0x1a3L };
        public static IHaveNameAndId INDEXING_STATUS = new AttributePlaceholder { Id = 0x1dL };
        public static IHaveNameAndId IPTC_BY_LINE = new AttributePlaceholder { Id = 0xd5L };
        public static IHaveNameAndId IPTC_BY_LINE_TITLE = new AttributePlaceholder { Id = 0xd6L };
        public static IHaveNameAndId IPTC_CAPTION = new AttributePlaceholder { Id = 0xdfL };
        public static IHaveNameAndId IPTC_CATEGORY = new AttributePlaceholder { Id = 0xd0L };
        public static IHaveNameAndId IPTC_CITY = new AttributePlaceholder { Id = 0xd7L };
        public static IHaveNameAndId IPTC_COPYRIGHT_NOTICE = new AttributePlaceholder { Id = 0xdeL };
        public static IHaveNameAndId IPTC_COUNTRY = new AttributePlaceholder { Id = 0xd9L };
        public static IHaveNameAndId IPTC_CREDIT = new AttributePlaceholder { Id = 220L };
        public static IHaveNameAndId IPTC_DATE_CREATED = new AttributePlaceholder { Id = 0xd4L };
        public static IHaveNameAndId IPTC_HEADLINE = new AttributePlaceholder { Id = 0xdbL };
        public static IHaveNameAndId IPTC_KEYWORDS = new AttributePlaceholder { Id = 210L };
        public static IHaveNameAndId IPTC_OBJECT_NAME = new AttributePlaceholder { Id = 0xceL };
        public static IHaveNameAndId IPTC_ORIGINAL_TRANSMISSION_REFERENCE = new AttributePlaceholder { Id = 0xdaL };
        public static IHaveNameAndId IPTC_PROVINCE = new AttributePlaceholder { Id = 0xd8L };
        public static IHaveNameAndId IPTC_SOURCE = new AttributePlaceholder { Id = 0xddL };
        public static IHaveNameAndId IPTC_SPECIAL_INSTRUCTIONS = new AttributePlaceholder { Id = 0xd3L };
        public static IHaveNameAndId IPTC_SUPPLEMENTAL_CATEGORIES = new AttributePlaceholder { Id = 0xd1L };
        public static IHaveNameAndId IPTC_URGENCY = new AttributePlaceholder { Id = 0xcfL };
        public static IHaveNameAndId IPTC_WRITER = new AttributePlaceholder { Id = 0xe0L };
        public static IHaveNameAndId IS_CHECKED_OUT = new AttributePlaceholder { Id = 13L };
        public static IHaveNameAndId IS_COLLECTED = new AttributePlaceholder { Id = 0x12L };
        public static IHaveNameAndId IS_COLLECTION_TEMPLATE = new AttributePlaceholder { Id = 0x3bL };
        public static IHaveNameAndId IS_PLACEHOLDER = new AttributePlaceholder { Id = 30L };
        public static IHaveNameAndId ISSUE = new AttributePlaceholder { Id = 0x1aL };
        public static IHaveNameAndId ITEM_NAME = new AttributePlaceholder { Id = 410L };
        public static IHaveNameAndId ITEM_TYPE = new AttributePlaceholder { Id = 0x19bL };
        public static IHaveNameAndId JOB_JACKET = new AttributePlaceholder { Id = 0x2cL };
        public static IHaveNameAndId JOB_TICKET_TEMPLATE_NAME = new AttributePlaceholder { Id = 0x2dL };
        public static IHaveNameAndId LAST_CONTENT_UPDATE = new AttributePlaceholder { Id = 0x13L };
        public static IHaveNameAndId LAST_CONTENT_UPDATED_BY = new AttributePlaceholder { Id = 20L };
        public static IHaveNameAndId LAST_GEOMETRY_UPDATE = new AttributePlaceholder { Id = 0x15L };
        public static IHaveNameAndId LAST_JOB_JACKET_MODIFIED = new AttributePlaceholder { Id = 10L };
        public static IHaveNameAndId LAST_MODIFIED = new AttributePlaceholder { Id = 4L };
        public static IHaveNameAndId LAST_MODIFIER = new AttributePlaceholder { Id = 6L };
        public static IHaveNameAndId LAST_PAGE = new AttributePlaceholder { Id = 0x34L };
        public static IHaveNameAndId LAYOUT_GEOMETRY_DIFFERS = new AttributePlaceholder { Id = 0x97L };
        public static IHaveNameAndId LAYOUT_NAME = new AttributePlaceholder { Id = 0x9dL };
        public static IHaveNameAndId LINE_COUNT = new AttributePlaceholder { Id = 0x162L };
        public static IHaveNameAndId MAC_CREATOR_TYPE = new AttributePlaceholder { Id = 0x2fL };
        public static IHaveNameAndId MAC_OS_TYPE = new AttributePlaceholder { Id = 0x2eL };
        public static IHaveNameAndId MAJOR_VERSION = new AttributePlaceholder { Id = 7L };
        public static IHaveNameAndId MIME_TYPE = new AttributePlaceholder { Id = 0x30L };
        public static IHaveNameAndId MINOR_VERSION = new AttributePlaceholder { Id = 0x3dL };
        public static IHaveNameAndId NAME = new AttributePlaceholder { Id = 2L };
        public static IHaveNameAndId NUMBER_OF_LAYOUTS = new AttributePlaceholder { Id = 60L };
        public static IHaveNameAndId NUMBER_OF_PAGES = new AttributePlaceholder { Id = 0x2bL };
        public static IHaveNameAndId ORIENTATION = new AttributePlaceholder { Id = 0x99L };
        public static IHaveNameAndId ORIGINAL_FILENAME = new AttributePlaceholder { Id = 0x21L };
        public static IHaveNameAndId PAGE_HEIGHT = new AttributePlaceholder { Id = 0x9cL };
        public static IHaveNameAndId PAGE_INDEX = new AttributePlaceholder { Id = 0x194L };
        public static IHaveNameAndId PAGE_NAME = new AttributePlaceholder { Id = 0x193L };
        public static IHaveNameAndId PAGE_WIDTH = new AttributePlaceholder { Id = 0x9bL };
        public static IHaveNameAndId PIXEL_HEIGHT = new AttributePlaceholder { Id = 0xcaL };
        public static IHaveNameAndId PIXEL_WIDTH = new AttributePlaceholder { Id = 0xc9L };
        public static IHaveNameAndId RELATION_STATUS = new AttributePlaceholder { Id = 0x40L };
        public static IHaveNameAndId REPOSITORY = new AttributePlaceholder { Id = 0x1cL };
        public static IHaveNameAndId REPOSITORY_TYPE = new AttributePlaceholder { Id = 0x1bL };
        public static IHaveNameAndId RESOLUTION = new AttributePlaceholder { Id = 0xcbL };
        public static IHaveNameAndId REVISION_COMMENTS = new AttributePlaceholder { Id = 0x1fL };
        public static IHaveNameAndId ROUTED_TO = new AttributePlaceholder { Id = 0x19L };
        public static IHaveNameAndId STATUS = new AttributePlaceholder { Id = 0x18L };
        public static IHaveNameAndId STORY_DIRECTION = new AttributePlaceholder { Id = 0x9aL };
        public static IHaveNameAndId TEXT_INDEXING_STATUS = new AttributePlaceholder { Id = 0x35L };
        public static IHaveNameAndId TEXT_PREVIEW = new AttributePlaceholder { Id = 0x160L };
        public static IHaveNameAndId TOTAL_BILLABLE_HOURS = new AttributePlaceholder { Id = 0x25L };
        public static IHaveNameAndId WORD_COUNT = new AttributePlaceholder { Id = 0x161L };
        public static IHaveNameAndId WORKFLOW = new AttributePlaceholder { Id = 0x36L };
        public static IHaveNameAndId XPATH = new AttributePlaceholder { Id = 0x1a1L };
        public static IHaveNameAndId SourceDocumentId = new AttributePlaceholder { Name = "Source Document Id" };
        public static IHaveNameAndId GeographyId = new AttributePlaceholder { Name = "Geography Id" };
        public static IHaveNameAndId Geography = new AttributePlaceholder { Name = "Geography" };
        public static IHaveNameAndId PublishedDate = new AttributePlaceholder { Name = "Published Date" };
        public static IHaveNameAndId UserSetPublishedDate = new AttributePlaceholder { Name = "User-Set Published Date" };
        public static IHaveNameAndId UserProposedPublishedDate = new AttributePlaceholder { Name = "User-Proposed Published Date" };
        public static IHaveNameAndId DontUpdatePublishedDate = new AttributePlaceholder { Name = "Don't Update Published Date" };
        public static IHaveNameAndId HiddenUpdate = new AttributePlaceholder { Name = "Hidden Update" };
        public static IHaveNameAndId InRangeName = new AttributePlaceholder { Name = "InRangeName" };
        public static IHaveNameAndId InRangeValue = new AttributePlaceholder { Name = "InRangeValue" };
        public static IHaveNameAndId OutSheet = new AttributePlaceholder { Name = "OutSheet" };
        public static IHaveNameAndId OutRange = new AttributePlaceholder { Name = "OutRange" };
        public static IHaveNameAndId OutChartName = new AttributePlaceholder { Name = "OutChartName" };
        public static IHaveNameAndId OutImageName = new AttributePlaceholder { Name = "OutImageName" };
        public static IHaveNameAndId PdfBusinessLine = new AttributePlaceholder { Name = "PDF Business Line" };
        public static IHaveNameAndId PdfReportType = new AttributePlaceholder { Name = "PDF Report Type" };
        public static IHaveNameAndId PdfReportTitle = new AttributePlaceholder { Name = "PDF Report Title" };
        public static IHaveNameAndId PdfSubheading = new AttributePlaceholder { Name = "PDF Subheading" };
        public static IHaveNameAndId PdfDate = new AttributePlaceholder { Name = "PDF Date" };
        public static IHaveNameAndId PdfAddressBar = new AttributePlaceholder { Name = "PDF Address Bar" };
        public static IHaveNameAndId Location = new AttributePlaceholder { Name = "Location" };
        public static IHaveNameAndId UploaderVersion = new AttributePlaceholder { Name = "Uploader Version" };
        public static IHaveNameAndId AnalystWhoSentForReviewId = new AttributePlaceholder { Name = "Analyst who sent for review id" };
        public static IHaveNameAndId EditorWhoSentBackToAnalystId = new AttributePlaceholder { Name = "Editor who sent back to Analyst id" };
        public static IHaveNameAndId PublishFailReason = new AttributePlaceholder { Name = "Publishing fail reason" };
        public static IHaveNameAndId DataRefreshStart = new AttributePlaceholder { Name = "Data refresh started" };
        public static IHaveNameAndId DataRefreshEnd = new AttributePlaceholder { Name = "Data refresh ended" };
        public static IHaveNameAndId DataRefreshExcelPull = new AttributePlaceholder { Name = "Data refresh excels pulled" };
        public static IHaveNameAndId DataRefreshFailReason = new AttributePlaceholder { Name = "Data refresh fail reason" };
        public static IHaveNameAndId ChemicalProductAndMarket = new AttributePlaceholder { Name = "Chemical Product and Market(Taxonomy)" };
        public static IHaveNameAndId BrandTaxonomy = new AttributePlaceholder { Name = "Brand(Taxonomy)" };
        public static IHaveNameAndId DomainsTaxonomy = new AttributePlaceholder { Name = "Domain(Taxonomy)" };
        public static IHaveNameAndId ContentTypeTaxonomy = new AttributePlaceholder { Name = "Content Type(Taxonomy)" };
        public static IHaveNameAndId FileClassificationTaxonomy = new AttributePlaceholder { Name = "File Classification(Taxonomy)" };
        public static IHaveNameAndId PublishFrequencyTaxonomy = new AttributePlaceholder { Name = "Publish Frequency(Taxonomy)" };
        public static IHaveNameAndId AuthorizationCode = new AttributePlaceholder { Name = "Authorization Code" };
        public static IHaveNameAndId Authors = new AttributePlaceholder { Name = "Authors" };
        public static IHaveNameAndId DomainUrl = new AttributePlaceholder { Name = "Domain Url" };
        public static IHaveNameAndId HardCopyPublicationDate = new AttributePlaceholder { Name = "Hardcopy Publication Date" };
        public static IHaveNameAndId BrandDisplayName = new AttributePlaceholder { Name = "Brand" };
        public static IHaveNameAndId DomainsDisplayName = new AttributePlaceholder { Name = "Domains" };
    }
}