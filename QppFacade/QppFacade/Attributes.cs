using System;
using System.Collections.Generic;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using com.quark.qpp.core.attribute.service.dto;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace QppFacade
{
    public class AttributePlaceholder : IAttribute
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public object FromAttributeValue(AttributeValue value)
        {
            throw new NotImplementedException();
        }

        public AttributeValue ToAttributeValue(object value)
        {
            throw new NotImplementedException();
        }

        public bool CanBeUpdated()
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
            System.Func<IEnumerable<Attribute>> getQppAttributes,
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

            foreach (var fieldInfo in typeof (PhoenixAttributes).GetFields())
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
                if (attribute != null)
                    fieldInfo.SetValue(null, attribute);
            }
        }
    }

    public static partial class PhoenixAttributes
    {
        public static IAttribute APP_STUDIO_ISSUE_URN = new AttributePlaceholder {Id = 180L};
        public static IAttribute ARTICLE_COMP_REFRENCE_ASSET_ID = new AttributePlaceholder {Id = 0x19eL};
        public static IAttribute ARTICLE_COMP_REFRENCE_ASSET_MAJ_VERSION = new AttributePlaceholder {Id = 0x19fL};
        public static IAttribute ARTICLE_COMP_REFRENCE_ASSET_MIN_VERSION = new AttributePlaceholder {Id = 0x1a0L};
        public static IAttribute ARTICLE_COMPONENT_ID = new AttributePlaceholder {Id = 0x131L};
        public static IAttribute ARTICLE_ID = new AttributePlaceholder {Id = 0x19dL};
        public static IAttribute ASSET_CHECKSUM = new AttributePlaceholder {Id = 0x197L};
        public static IAttribute ASSIGNED_LENGTH = new AttributePlaceholder {Id = 0x166L};
        public static IAttribute ATTACHED_COMPONENT_ID = new AttributePlaceholder {Id = 0x19cL};
        public static IAttribute ATTACHED_LAYOUT_ID = new AttributePlaceholder {Id = 0x191L};
        public static IAttribute ATTACHMENT_CHECKSUM = new AttributePlaceholder {Id = 0x198L};
        public static IAttribute AUXILIARY_DATA = new AttributePlaceholder {Id = 0x199L};
        public static IAttribute BOX_HEIGHT = new AttributePlaceholder {Id = 0x196L};
        public static IAttribute BOX_ID = new AttributePlaceholder {Id = 0x192L};
        public static IAttribute BOX_WIDTH = new AttributePlaceholder {Id = 0x195L};
        public static IAttribute CHARACTER_COUNT = new AttributePlaceholder {Id = 0x164L};
        public static IAttribute CHECK_OUT_DATE_TIME = new AttributePlaceholder {Id = 0x11L};
        public static IAttribute CHECKED_OUT_APPLICATION = new AttributePlaceholder {Id = 0x22L};
        public static IAttribute CHECKED_OUT_BY = new AttributePlaceholder {Id = 14L};
        public static IAttribute CHECKED_OUT_DURATION = new AttributePlaceholder {Id = 0x23L};
        public static IAttribute CHECKED_OUT_FILE_PATH = new AttributePlaceholder {Id = 0x10L};
        public static IAttribute CHECKED_OUT_MACHINE_NAME = new AttributePlaceholder {Id = 15L};
        public static IAttribute COLLECTION = new AttributePlaceholder {Id = 0x37L};
        public static IAttribute COLLECTION_PATH = new AttributePlaceholder {Id = 0x39L};
        public static IAttribute COLLECTION_TEMPLATE = new AttributePlaceholder {Id = 0x3aL};
        public static IAttribute COLOR_DEPTH = new AttributePlaceholder {Id = 0xccL};
        public static IAttribute COLOR_SPACE = new AttributePlaceholder {Id = 0xcdL};
        public static IAttribute COLUMN_WIDTH = new AttributePlaceholder {Id = 0x163L};
        public static IAttribute COMPONENT_NAME = new AttributePlaceholder {Id = 0x12fL};
        public static IAttribute COMPONENT_POSITION = new AttributePlaceholder {Id = 0x12eL};
        public static IAttribute CONTENT_CREATOR = new AttributePlaceholder {Id = 0x20L};
        public static IAttribute CONTENT_TYPE = new AttributePlaceholder {Id = 0x3eL};
        public static IAttribute CONTENT_TYPE_HIERARCHY = new AttributePlaceholder {Id = 0x3fL};
        public static IAttribute CREATED = new AttributePlaceholder {Id = 3L};
        public static IAttribute CREATOR = new AttributePlaceholder {Id = 5L};
        public static IAttribute CURRENT_BILLABLE_HOURS = new AttributePlaceholder {Id = 0x24L};
        public static IAttribute CURRENT_LENGTH = new AttributePlaceholder {Id = 0x165L};
        public static IAttribute DEPENDENT_ON_COLLECTION_RESOURCES = new AttributePlaceholder {Id = 0x1a2L};
        public static IAttribute DEVICE_NAME = new AttributePlaceholder {Id = 0x9eL};
        public static IAttribute DITA_AUDIENCE = new AttributePlaceholder {Id = 0x1c4L};
        public static IAttribute DITA_AUTHOR = new AttributePlaceholder {Id = 0x1c5L};
        public static IAttribute DITA_BRAND = new AttributePlaceholder {Id = 0x1c6L};
        public static IAttribute DITA_CATEGORY = new AttributePlaceholder {Id = 0x1c7L};
        public static IAttribute DITA_ID = new AttributePlaceholder {Id = 0x1c8L};
        public static IAttribute DITA_IMPORTANCE = new AttributePlaceholder {Id = 0x1d2L};
        public static IAttribute DITA_KEYWORDS = new AttributePlaceholder {Id = 0x1c9L};
        public static IAttribute DITA_LANGUAGE = new AttributePlaceholder {Id = 0x1d0L};
        public static IAttribute DITA_NAVIGATION_TITLE = new AttributePlaceholder {Id = 0x1caL};
        public static IAttribute DITA_OTHER_PROPERTIES = new AttributePlaceholder {Id = 0x1d1L};
        public static IAttribute DITA_PLATFORM = new AttributePlaceholder {Id = 0x1cbL};
        public static IAttribute DITA_PRODUCT_NAME = new AttributePlaceholder {Id = 460L};
        public static IAttribute DITA_PUBLISHING_CONTENT = new AttributePlaceholder {Id = 0x1cfL};
        public static IAttribute DITA_SEARCH_TITLE = new AttributePlaceholder {Id = 0x1cdL};
        public static IAttribute DITA_TITLE = new AttributePlaceholder {Id = 0x1ceL};
        public static IAttribute DUE_DATE = new AttributePlaceholder {Id = 0x29L};
        public static IAttribute DUE_TIME = new AttributePlaceholder {Id = 0x2aL};
        public static IAttribute FILE_EXTENSION = new AttributePlaceholder {Id = 11L};
        public static IAttribute FILE_PATH = new AttributePlaceholder {Id = 8L};
        public static IAttribute FILE_SIZE = new AttributePlaceholder {Id = 12L};
        public static IAttribute FIRST_PAGE = new AttributePlaceholder {Id = 0x33L};
        public static IAttribute GLOBAL_ID = new AttributePlaceholder {Id = 0x42L};
        public static IAttribute HAS_CHILDREN = new AttributePlaceholder {Id = 0x41L};
        public static IAttribute ID = new AttributePlaceholder {Id = 1L};
        public static IAttribute INDESIGN_OBJECT_UID = new AttributePlaceholder {Id = 0x1a3L};
        public static IAttribute INDEXING_STATUS = new AttributePlaceholder {Id = 0x1dL};
        public static IAttribute IPTC_BY_LINE = new AttributePlaceholder {Id = 0xd5L};
        public static IAttribute IPTC_BY_LINE_TITLE = new AttributePlaceholder {Id = 0xd6L};
        public static IAttribute IPTC_CAPTION = new AttributePlaceholder {Id = 0xdfL};
        public static IAttribute IPTC_CATEGORY = new AttributePlaceholder {Id = 0xd0L};
        public static IAttribute IPTC_CITY = new AttributePlaceholder {Id = 0xd7L};
        public static IAttribute IPTC_COPYRIGHT_NOTICE = new AttributePlaceholder {Id = 0xdeL};
        public static IAttribute IPTC_COUNTRY = new AttributePlaceholder {Id = 0xd9L};
        public static IAttribute IPTC_CREDIT = new AttributePlaceholder {Id = 220L};
        public static IAttribute IPTC_DATE_CREATED = new AttributePlaceholder {Id = 0xd4L};
        public static IAttribute IPTC_HEADLINE = new AttributePlaceholder {Id = 0xdbL};
        public static IAttribute IPTC_KEYWORDS = new AttributePlaceholder {Id = 210L};
        public static IAttribute IPTC_OBJECT_NAME = new AttributePlaceholder {Id = 0xceL};
        public static IAttribute IPTC_ORIGINAL_TRANSMISSION_REFERENCE = new AttributePlaceholder {Id = 0xdaL};
        public static IAttribute IPTC_PROVINCE = new AttributePlaceholder {Id = 0xd8L};
        public static IAttribute IPTC_SOURCE = new AttributePlaceholder {Id = 0xddL};
        public static IAttribute IPTC_SPECIAL_INSTRUCTIONS = new AttributePlaceholder {Id = 0xd3L};
        public static IAttribute IPTC_SUPPLEMENTAL_CATEGORIES = new AttributePlaceholder {Id = 0xd1L};
        public static IAttribute IPTC_URGENCY = new AttributePlaceholder {Id = 0xcfL};
        public static IAttribute IPTC_WRITER = new AttributePlaceholder {Id = 0xe0L};
        public static IAttribute IS_CHECKED_OUT = new AttributePlaceholder {Id = 13L};
        public static IAttribute IS_COLLECTED = new AttributePlaceholder {Id = 0x12L};
        public static IAttribute IS_COLLECTION_TEMPLATE = new AttributePlaceholder {Id = 0x3bL};
        public static IAttribute IS_PLACEHOLDER = new AttributePlaceholder {Id = 30L};
        public static IAttribute ISSUE = new AttributePlaceholder {Id = 0x1aL};
        public static IAttribute ITEM_NAME = new AttributePlaceholder {Id = 410L};
        public static IAttribute ITEM_TYPE = new AttributePlaceholder {Id = 0x19bL};
        public static IAttribute JOB_JACKET = new AttributePlaceholder {Id = 0x2cL};
        public static IAttribute JOB_TICKET_TEMPLATE_NAME = new AttributePlaceholder {Id = 0x2dL};
        public static IAttribute LAST_CONTENT_UPDATE = new AttributePlaceholder {Id = 0x13L};
        public static IAttribute LAST_CONTENT_UPDATED_BY = new AttributePlaceholder {Id = 20L};
        public static IAttribute LAST_GEOMETRY_UPDATE = new AttributePlaceholder {Id = 0x15L};
        public static IAttribute LAST_JOB_JACKET_MODIFIED = new AttributePlaceholder {Id = 10L};
        public static IAttribute LAST_MODIFIED = new AttributePlaceholder {Id = 4L};
        public static IAttribute LAST_MODIFIER = new AttributePlaceholder {Id = 6L};
        public static IAttribute LAST_PAGE = new AttributePlaceholder {Id = 0x34L};
        public static IAttribute LAYOUT_GEOMETRY_DIFFERS = new AttributePlaceholder {Id = 0x97L};
        public static IAttribute LAYOUT_NAME = new AttributePlaceholder {Id = 0x9dL};
        public static IAttribute LINE_COUNT = new AttributePlaceholder {Id = 0x162L};
        public static IAttribute MAC_CREATOR_TYPE = new AttributePlaceholder {Id = 0x2fL};
        public static IAttribute MAC_OS_TYPE = new AttributePlaceholder {Id = 0x2eL};
        public static IAttribute MAJOR_VERSION = new AttributePlaceholder {Id = 7L};
        public static IAttribute MIME_TYPE = new AttributePlaceholder {Id = 0x30L};
        public static IAttribute MINOR_VERSION = new AttributePlaceholder {Id = 0x3dL};
        public static IAttribute NAME = new AttributePlaceholder {Id = 2L};
        public static IAttribute NUMBER_OF_LAYOUTS = new AttributePlaceholder {Id = 60L};
        public static IAttribute NUMBER_OF_PAGES = new AttributePlaceholder {Id = 0x2bL};
        public static IAttribute ORIENTATION = new AttributePlaceholder {Id = 0x99L};
        public static IAttribute ORIGINAL_FILENAME = new AttributePlaceholder {Id = 0x21L};
        public static IAttribute PAGE_HEIGHT = new AttributePlaceholder {Id = 0x9cL};
        public static IAttribute PAGE_INDEX = new AttributePlaceholder {Id = 0x194L};
        public static IAttribute PAGE_NAME = new AttributePlaceholder {Id = 0x193L};
        public static IAttribute PAGE_WIDTH = new AttributePlaceholder {Id = 0x9bL};
        public static IAttribute PIXEL_HEIGHT = new AttributePlaceholder {Id = 0xcaL};
        public static IAttribute PIXEL_WIDTH = new AttributePlaceholder {Id = 0xc9L};
        public static IAttribute RELATION_STATUS = new AttributePlaceholder {Id = 0x40L};
        public static IAttribute REPOSITORY = new AttributePlaceholder {Id = 0x1cL};
        public static IAttribute REPOSITORY_TYPE = new AttributePlaceholder {Id = 0x1bL};
        public static IAttribute RESOLUTION = new AttributePlaceholder {Id = 0xcbL};
        public static IAttribute REVISION_COMMENTS = new AttributePlaceholder {Id = 0x1fL};
        public static IAttribute ROUTED_TO = new AttributePlaceholder {Id = 0x19L};
        public static IAttribute STATUS = new AttributePlaceholder {Id = 0x18L};
        public static IAttribute STORY_DIRECTION = new AttributePlaceholder {Id = 0x9aL};
        public static IAttribute TEXT_INDEXING_STATUS = new AttributePlaceholder {Id = 0x35L};
        public static IAttribute TEXT_PREVIEW = new AttributePlaceholder {Id = 0x160L};
        public static IAttribute TOTAL_BILLABLE_HOURS = new AttributePlaceholder {Id = 0x25L};
        public static IAttribute WORD_COUNT = new AttributePlaceholder {Id = 0x161L};
        public static IAttribute WORKFLOW = new AttributePlaceholder {Id = 0x36L};
        public static IAttribute XPATH = new AttributePlaceholder {Id = 0x1a1L};
        public static IAttribute SourceDocumentId = new AttributePlaceholder {Name = "Source Document Id"};
        public static IAttribute GeographyId = new AttributePlaceholder {Name = "Geography Id"};
        public static IAttribute Geography = new AttributePlaceholder {Name = "Geography"};
        public static IAttribute PublishedDate = new AttributePlaceholder {Name = "Published Date"};
        public static IAttribute UserSetPublishedDate = new AttributePlaceholder {Name = "User-Set Published Date"};
        public static IAttribute UserProposedPublishedDate = new AttributePlaceholder {Name = "User-Proposed Published Date"};
        public static IAttribute DontUpdatePublishedDate = new AttributePlaceholder {Name = "Don't Update Published Date"};
        public static IAttribute HiddenUpdate = new AttributePlaceholder {Name = "Hidden Update"};
        public static IAttribute InRangeName = new AttributePlaceholder {Name = "InRangeName"};
        public static IAttribute InRangeValue = new AttributePlaceholder {Name = "InRangeValue"};
        public static IAttribute OutSheet = new AttributePlaceholder {Name = "OutSheet"};
        public static IAttribute OutRange = new AttributePlaceholder {Name = "OutRange"};
        public static IAttribute OutChartName = new AttributePlaceholder {Name = "OutChartName"};
        public static IAttribute OutImageName = new AttributePlaceholder {Name = "OutImageName"};
        public static IAttribute PdfBusinessLine = new AttributePlaceholder {Name = "PDF Business Line"};
        public static IAttribute PdfReportType = new AttributePlaceholder {Name = "PDF Report Type"};
        public static IAttribute PdfReportTitle = new AttributePlaceholder {Name = "PDF Report Title"};
        public static IAttribute PdfSubheading = new AttributePlaceholder {Name = "PDF Subheading"};
        public static IAttribute PdfDate = new AttributePlaceholder {Name = "PDF Date"};
        public static IAttribute PdfAddressBar = new AttributePlaceholder {Name = "PDF Address Bar"};
        public static IAttribute Location = new AttributePlaceholder {Name = "Location"};
        public static IAttribute UploaderVersion = new AttributePlaceholder {Name = "Uploader Version"};
        public static IAttribute AnalystWhoSentForReviewId = new AttributePlaceholder {Name = "Analyst who sent for review id"};
        public static IAttribute EditorWhoSentBackToAnalystId = new AttributePlaceholder {Name = "Editor who sent back to Analyst id"};
        public static IAttribute PublishFailReason = new AttributePlaceholder {Name = "Publishing fail reason"};
        public static IAttribute DataRefreshStart = new AttributePlaceholder {Name = "Data refresh started"};
        public static IAttribute DataRefreshEnd = new AttributePlaceholder {Name = "Data refresh ended"};
        public static IAttribute DataRefreshExcelPull = new AttributePlaceholder {Name = "Data refresh excels pulled"};
        public static IAttribute DataRefreshFailReason = new AttributePlaceholder {Name = "Data refresh fail reason"};
        public static IAttribute ChemicalProductAndMarket = new AttributePlaceholder {Name = "Chemical Product and Market(Taxonomy)"};
        public static IAttribute BrandTaxonomy = new AttributePlaceholder {Name = "Brand(Taxonomy)"};
        public static IAttribute DomainsTaxonomy = new AttributePlaceholder {Name = "Domain(Taxonomy)"};
        public static IAttribute ContentTypeTaxonomy = new AttributePlaceholder {Name = "Content Type(Taxonomy)"};
        public static IAttribute FileClassificationTaxonomy = new AttributePlaceholder {Name = "File Classification(Taxonomy)"};
        public static IAttribute PublishFrequencyTaxonomy = new AttributePlaceholder {Name = "Publish Frequency(Taxonomy)"};
        public static IAttribute AuthorizationCode = new AttributePlaceholder {Name = "Authorization Code"};
        public static IAttribute Authors = new AttributePlaceholder {Name = "Authors"};
        public static IAttribute DomainUrl = new AttributePlaceholder {Name = "Domain Url"};
        public static IAttribute HardCopyPublicationDate = new AttributePlaceholder {Name = "Hardcopy Publication Date"};
        public static IAttribute BrandDisplayName = new AttributePlaceholder {Name = "Brand"};
        public static IAttribute DomainsDisplayName = new AttributePlaceholder {Name = "Domains"};
    }
}