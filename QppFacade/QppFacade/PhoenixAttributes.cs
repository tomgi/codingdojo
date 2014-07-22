using System;
using System.Collections.Generic;
using System.Reflection;
using com.quark.qpp.core.attribute.service.constants;
using IHS.Phoenix.QPP;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace QppFacade
{
    public static partial class PhoenixAttributes
    {
        public static Dictionary<string, object> ByName { get; private set; }
        public static Dictionary<long, object> ById { get; private set; }

        public static Dictionary<long, bool> ModifiableAttributes { get; private set; }

        public static void Init(Func<IEnumerable<Attribute>> getQppAttributes)
        {
            ModifiableAttributes = new Dictionary<long, bool>();
            ByName = new Dictionary<string, object>();
            ById = new Dictionary<long, object>();

            foreach (var qppAttribute in getQppAttributes())
            {
                object attribute = null;
                ModifiableAttributes[qppAttribute.id] = qppAttribute.constraintsChangeable;
                if (qppAttribute.valueType == AttributeValueTypes.TEXT)
                {
                    attribute = new PhoenixAttribute<string>(qppAttribute.id, qppAttribute.name) { Type = AttributeValueTypes.TEXT };
                }
                else if (qppAttribute.valueType == AttributeValueTypes.NUMERIC)
                {
                    attribute = new PhoenixAttribute<long>(qppAttribute.id, qppAttribute.name) { Type = AttributeValueTypes.NUMERIC };
                }
                else if (qppAttribute.valueType == AttributeValueTypes.BOOLEAN)
                {
                    attribute = new PhoenixAttribute<bool>(qppAttribute.id, qppAttribute.name) { Type = AttributeValueTypes.BOOLEAN };
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DATETIME)
                {
                    attribute = new PhoenixAttribute<DateTime>(qppAttribute.id, qppAttribute.name) { Type = AttributeValueTypes.DATETIME };
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DOMAIN)
                {
                    attribute = new PhoenixAttribute<PhoenixValue>(qppAttribute.id, qppAttribute.name){ Type = AttributeValueTypes.DOMAIN };
                }
                if (attribute != null)
                {
                    ByName[qppAttribute.name] = attribute;
                    ById[qppAttribute.id] = attribute;
                }
            }

            //var builder = new StringBuilder();

            foreach (var fieldInfo in typeof(PhoenixAttributes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attributePlaceholder = (PhoenixValue) fieldInfo.GetValue(null);
                object attribute = null;
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

                //if (attribute != null)
                //    builder.AppendLine(string.Format("public static IAttribute<{0}> {1} = new AttributePlaceholder<{0}> {{Id={2},Name=\"{3}\"}}",
                //        GetAttributeType(attribute),
                //        fieldInfo.Name,
                //        attribute.Id,
                //        attribute.Name));
            }
            //Debug.Write(builder.ToString());
        }
    }

    public static partial class PhoenixAttributes
    {
        public static PhoenixAttribute<string> APP_STUDIO_ISSUE_URN = new PhoenixAttribute<string>(180, "App Studio Issue URN");
        public static PhoenixAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_ID = new PhoenixAttribute<long>(414, "Article Component Reference Asset Id");
        public static PhoenixAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_MAJ_VERSION = new PhoenixAttribute<long>(415, "Article Component Reference Asset Major Version");
        public static PhoenixAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_MIN_VERSION = new PhoenixAttribute<long>(416, "Article Component Reference Asset Minor version");
        public static PhoenixAttribute<long> ARTICLE_COMPONENT_ID = new PhoenixAttribute<long>(305, "Article Component id");
        public static PhoenixAttribute<long> ARTICLE_ID = new PhoenixAttribute<long>(413, "Article Id");
        public static PhoenixAttribute<string> ASSET_CHECKSUM = new PhoenixAttribute<string>(407, "Asset Checksum");
        public static PhoenixAttribute<long> ATTACHED_COMPONENT_ID = new PhoenixAttribute<long>(412, "Attached Component Id");
        public static PhoenixAttribute<long> ATTACHED_LAYOUT_ID = new PhoenixAttribute<long>(401, "Attached Layout Uid");
        public static PhoenixAttribute<string> ATTACHMENT_CHECKSUM = new PhoenixAttribute<string>(408, "Attachment Checksum");
        public static PhoenixAttribute<string> AUXILIARY_DATA = new PhoenixAttribute<string>(409, "Auxillary Data");
        public static PhoenixAttribute<long> BOX_ID = new PhoenixAttribute<long>(402, "Box Uid");
        public static PhoenixAttribute<long> CHARACTER_COUNT = new PhoenixAttribute<long>(356, "Character count");
        public static PhoenixAttribute<DateTime> CHECK_OUT_DATE_TIME = new PhoenixAttribute<DateTime>(17, "Check Out Date and Time");
        public static PhoenixAttribute<string> CHECKED_OUT_APPLICATION = new PhoenixAttribute<string>(34, "Checkedout Application");
        public static PhoenixAttribute<PhoenixValue> CHECKED_OUT_BY = new PhoenixAttribute<PhoenixValue>(14, "Checked out by");
        public static PhoenixAttribute<long> CHECKED_OUT_DURATION = new PhoenixAttribute<long>(35, "Checked out duration");
        public static PhoenixAttribute<string> CHECKED_OUT_FILE_PATH = new PhoenixAttribute<string>(16, "Checked out file path");
        public static PhoenixAttribute<string> CHECKED_OUT_MACHINE_NAME = new PhoenixAttribute<string>(15, "Checked out machine name");
        public static PhoenixAttribute<PhoenixValue> COLLECTION = new PhoenixAttribute<PhoenixValue>(55, "Collection");
        public static PhoenixAttribute<string> COLLECTION_PATH = new PhoenixAttribute<string>(57, "Collection Path");
        public static PhoenixAttribute<PhoenixValue> COLLECTION_TEMPLATE = new PhoenixAttribute<PhoenixValue>(58, "Collection Template");
        public static PhoenixAttribute<long> COLOR_DEPTH = new PhoenixAttribute<long>(204, "Color depth");
        public static PhoenixAttribute<PhoenixValue> COLOR_SPACE = new PhoenixAttribute<PhoenixValue>(205, "Color space");
        public static PhoenixAttribute<string> COMPONENT_NAME = new PhoenixAttribute<string>(303, "Component name");
        public static PhoenixAttribute<long> COMPONENT_POSITION = new PhoenixAttribute<long>(302, "Component position");
        public static PhoenixAttribute<PhoenixValue> CONTENT_CREATOR = new PhoenixAttribute<PhoenixValue>(32, "Content creator");
        public static PhoenixAttribute<PhoenixValue> CONTENT_TYPE = new PhoenixAttribute<PhoenixValue>(62, "Content Type");
        public static PhoenixAttribute<string> CONTENT_TYPE_HIERARCHY = new PhoenixAttribute<string>(63, "Content Type Hierarchy");
        public static PhoenixAttribute<DateTime> CREATED = new PhoenixAttribute<DateTime>(3, "Created");
        public static PhoenixAttribute<PhoenixValue> CREATOR = new PhoenixAttribute<PhoenixValue>(5, "Creator");
        public static PhoenixAttribute<bool> DEPENDENT_ON_COLLECTION_RESOURCES = new PhoenixAttribute<bool>(418, "Dependent On Collection Resources");
        public static PhoenixAttribute<string> DEVICE_NAME = new PhoenixAttribute<string>(158, "Device Name");
        public static PhoenixAttribute<string> DITA_AUDIENCE = new PhoenixAttribute<string>(452, "Audience (DITA)");
        public static PhoenixAttribute<string> DITA_AUTHOR = new PhoenixAttribute<string>(453, "Author (DITA)");
        public static PhoenixAttribute<string> DITA_BRAND = new PhoenixAttribute<string>(454, "Brand (DITA)");
        public static PhoenixAttribute<string> DITA_CATEGORY = new PhoenixAttribute<string>(455, "Category (DITA)");
        public static PhoenixAttribute<string> DITA_ID = new PhoenixAttribute<string>(456, "Id (DITA)");
        public static PhoenixAttribute<string> DITA_IMPORTANCE = new PhoenixAttribute<string>(466, "Importance (DITA)");
        public static PhoenixAttribute<string> DITA_KEYWORDS = new PhoenixAttribute<string>(457, "Keywords (DITA)");
        public static PhoenixAttribute<string> DITA_LANGUAGE = new PhoenixAttribute<string>(464, "Language (DITA)");
        public static PhoenixAttribute<string> DITA_NAVIGATION_TITLE = new PhoenixAttribute<string>(458, "Navigation Title (DITA)");
        public static PhoenixAttribute<string> DITA_OTHER_PROPERTIES = new PhoenixAttribute<string>(465, "Other Properties (DITA)");
        public static PhoenixAttribute<string> DITA_PLATFORM = new PhoenixAttribute<string>(459, "Platform (DITA)");
        public static PhoenixAttribute<string> DITA_PRODUCT_NAME = new PhoenixAttribute<string>(460, "Product Name (DITA)");
        public static PhoenixAttribute<string> DITA_PUBLISHING_CONTENT = new PhoenixAttribute<string>(463, "Publishing Intent (DITA)");
        public static PhoenixAttribute<string> DITA_SEARCH_TITLE = new PhoenixAttribute<string>(461, "Search Title (DITA)");
        public static PhoenixAttribute<string> DITA_TITLE = new PhoenixAttribute<string>(462, "Title (DITA)");
        public static PhoenixAttribute<string> FILE_EXTENSION = new PhoenixAttribute<string>(11, "File extension");
        public static PhoenixAttribute<string> FILE_PATH = new PhoenixAttribute<string>(8, "File path");
        public static PhoenixAttribute<long> FILE_SIZE = new PhoenixAttribute<long>(12, "File size");
        public static PhoenixAttribute<string> FIRST_PAGE = new PhoenixAttribute<string>(51, "First page");
        public static PhoenixAttribute<string> GLOBAL_ID = new PhoenixAttribute<string>(66, "Global ID");
        public static PhoenixAttribute<bool> HAS_CHILDREN = new PhoenixAttribute<bool>(65, "Has Children");
        public static PhoenixAttribute<long> ID = new PhoenixAttribute<long>(1, "Id");
        public static PhoenixAttribute<long> INDESIGN_OBJECT_UID = new PhoenixAttribute<long>(419, "InDesign Object UID");
        public static PhoenixAttribute<PhoenixValue> INDEXING_STATUS = new PhoenixAttribute<PhoenixValue>(29, "Indexing status");
        public static PhoenixAttribute<string> IPTC_BY_LINE = new PhoenixAttribute<string>(213, "By-line (IPTC)");
        public static PhoenixAttribute<string> IPTC_BY_LINE_TITLE = new PhoenixAttribute<string>(214, "By-line Title (IPTC)");
        public static PhoenixAttribute<string> IPTC_CAPTION = new PhoenixAttribute<string>(223, "Caption (IPTC)");
        public static PhoenixAttribute<string> IPTC_CATEGORY = new PhoenixAttribute<string>(208, "Category (IPTC)");
        public static PhoenixAttribute<string> IPTC_CITY = new PhoenixAttribute<string>(215, "City (IPTC)");
        public static PhoenixAttribute<string> IPTC_COPYRIGHT_NOTICE = new PhoenixAttribute<string>(222, "Copyright Notice (IPTC)");
        public static PhoenixAttribute<string> IPTC_COUNTRY = new PhoenixAttribute<string>(217, "Country (IPTC)");
        public static PhoenixAttribute<string> IPTC_CREDIT = new PhoenixAttribute<string>(220, "Credit (IPTC)");
        public static PhoenixAttribute<string> IPTC_HEADLINE = new PhoenixAttribute<string>(219, "Headline (IPTC)");
        public static PhoenixAttribute<string> IPTC_KEYWORDS = new PhoenixAttribute<string>(210, "Keywords (IPTC)");
        public static PhoenixAttribute<string> IPTC_OBJECT_NAME = new PhoenixAttribute<string>(206, "Object Name (IPTC)");
        public static PhoenixAttribute<string> IPTC_ORIGINAL_TRANSMISSION_REFERENCE = new PhoenixAttribute<string>(218, "Original Transmission Reference (IPTC)");
        public static PhoenixAttribute<string> IPTC_PROVINCE = new PhoenixAttribute<string>(216, "Province (IPTC)");
        public static PhoenixAttribute<string> IPTC_SOURCE = new PhoenixAttribute<string>(221, "Source (IPTC)");
        public static PhoenixAttribute<string> IPTC_SPECIAL_INSTRUCTIONS = new PhoenixAttribute<string>(211, "Special Instructions (IPTC)");
        public static PhoenixAttribute<string> IPTC_SUPPLEMENTAL_CATEGORIES = new PhoenixAttribute<string>(209, "Supplemental Categories (IPTC)");
        public static PhoenixAttribute<string> IPTC_URGENCY = new PhoenixAttribute<string>(207, "Urgency (IPTC)");
        public static PhoenixAttribute<string> IPTC_WRITER = new PhoenixAttribute<string>(224, "Writer (IPTC)");
        public static PhoenixAttribute<bool> IS_CHECKED_OUT = new PhoenixAttribute<bool>(13, "Is checked out");
        public static PhoenixAttribute<bool> IS_COLLECTED = new PhoenixAttribute<bool>(18, "Is collected");
        public static PhoenixAttribute<bool> IS_COLLECTION_TEMPLATE = new PhoenixAttribute<bool>(59, "Is Template");
        public static PhoenixAttribute<bool> IS_PLACEHOLDER = new PhoenixAttribute<bool>(30, "Is placeholder");
        public static PhoenixAttribute<PhoenixValue> ISSUE = new PhoenixAttribute<PhoenixValue>(26, "Issue");
        public static PhoenixAttribute<string> ITEM_NAME = new PhoenixAttribute<string>(410, "Item Name");
        public static PhoenixAttribute<string> ITEM_TYPE = new PhoenixAttribute<string>(411, "Item Type");
        public static PhoenixAttribute<PhoenixValue> JOB_JACKET = new PhoenixAttribute<PhoenixValue>(44, "Job jacket");
        public static PhoenixAttribute<string> JOB_TICKET_TEMPLATE_NAME = new PhoenixAttribute<string>(45, "Job ticket template name");
        public static PhoenixAttribute<DateTime> LAST_CONTENT_UPDATE = new PhoenixAttribute<DateTime>(19, "Last content update");
        public static PhoenixAttribute<PhoenixValue> LAST_CONTENT_UPDATED_BY = new PhoenixAttribute<PhoenixValue>(20, "Last content updated by");
        public static PhoenixAttribute<DateTime> LAST_GEOMETRY_UPDATE = new PhoenixAttribute<DateTime>(21, "Last geometry update");
        public static PhoenixAttribute<DateTime> LAST_JOB_JACKET_MODIFIED = new PhoenixAttribute<DateTime>(10, "Job jacket last modified");
        public static PhoenixAttribute<DateTime> LAST_MODIFIED = new PhoenixAttribute<DateTime>(4, "Last modified");
        public static PhoenixAttribute<PhoenixValue> LAST_MODIFIER = new PhoenixAttribute<PhoenixValue>(6, "Last modifier");
        public static PhoenixAttribute<string> LAST_PAGE = new PhoenixAttribute<string>(52, "Last page");
        public static PhoenixAttribute<bool> LAYOUT_GEOMETRY_DIFFERS = new PhoenixAttribute<bool>(151, "Layout Geometry Differs");
        public static PhoenixAttribute<string> LAYOUT_NAME = new PhoenixAttribute<string>(157, "Layout Name");
        public static PhoenixAttribute<long> LINE_COUNT = new PhoenixAttribute<long>(354, "Line count");
        public static PhoenixAttribute<string> MAC_CREATOR_TYPE = new PhoenixAttribute<string>(47, "Mac creator type");
        public static PhoenixAttribute<string> MAC_OS_TYPE = new PhoenixAttribute<string>(46, "Mac OS type");
        public static PhoenixAttribute<long> MAJOR_VERSION = new PhoenixAttribute<long>(7, "Major version");
        public static PhoenixAttribute<string> MIME_TYPE = new PhoenixAttribute<string>(48, "Mime type");
        public static PhoenixAttribute<long> MINOR_VERSION = new PhoenixAttribute<long>(61, "Minor version");
        public static PhoenixAttribute<string> NAME = new PhoenixAttribute<string>(2, "Name");
        public static PhoenixAttribute<long> NUMBER_OF_LAYOUTS = new PhoenixAttribute<long>(60, "Number of Layouts");
        public static PhoenixAttribute<long> NUMBER_OF_PAGES = new PhoenixAttribute<long>(43, "Number of pages");
        public static PhoenixAttribute<PhoenixValue> ORIENTATION = new PhoenixAttribute<PhoenixValue>(153, "Orientation");
        public static PhoenixAttribute<string> ORIGINAL_FILENAME = new PhoenixAttribute<string>(33, "Original filename");
        public static PhoenixAttribute<long> PAGE_INDEX = new PhoenixAttribute<long>(404, "Page Index");
        public static PhoenixAttribute<string> PAGE_NAME = new PhoenixAttribute<string>(403, "Page Name");
        public static PhoenixAttribute<long> PIXEL_HEIGHT = new PhoenixAttribute<long>(202, "Pixel height");
        public static PhoenixAttribute<long> PIXEL_WIDTH = new PhoenixAttribute<long>(201, "Pixel width");
        public static PhoenixAttribute<string> RELATION_STATUS = new PhoenixAttribute<string>(64, "Relationship Status");
        public static PhoenixAttribute<PhoenixValue> REPOSITORY = new PhoenixAttribute<PhoenixValue>(28, "Repository");
        public static PhoenixAttribute<PhoenixValue> REPOSITORY_TYPE = new PhoenixAttribute<PhoenixValue>(27, "Repository type");
        public static PhoenixAttribute<long> RESOLUTION = new PhoenixAttribute<long>(203, "Resolution");
        public static PhoenixAttribute<string> REVISION_COMMENTS = new PhoenixAttribute<string>(31, "Revision comments");
        public static PhoenixAttribute<PhoenixValue> ROUTED_TO = new PhoenixAttribute<PhoenixValue>(25, "Routed to");
        public static PhoenixAttribute<PhoenixValue> STATUS = new PhoenixAttribute<PhoenixValue>(24, "Status");
        public static PhoenixAttribute<PhoenixValue> STORY_DIRECTION = new PhoenixAttribute<PhoenixValue>(154, "Story Direction");
        public static PhoenixAttribute<PhoenixValue> TEXT_INDEXING_STATUS = new PhoenixAttribute<PhoenixValue>(53, "Text Indexing status");
        public static PhoenixAttribute<string> TEXT_PREVIEW = new PhoenixAttribute<string>(352, "Text preview");
        public static PhoenixAttribute<long> WORD_COUNT = new PhoenixAttribute<long>(353, "Word count");
        public static PhoenixAttribute<PhoenixValue> WORKFLOW = new PhoenixAttribute<PhoenixValue>(54, "Workflow");
        public static PhoenixAttribute<string> XPATH = new PhoenixAttribute<string>(417, "XPath");
        public static PhoenixAttribute<long> SourceDocumentId = new PhoenixAttribute<long>(504, "Source Document Id");
        public static PhoenixAttribute<DateTime> PublishedDate = new PhoenixAttribute<DateTime>(505, "Published Date");
        public static PhoenixAttribute<bool> UserSetPublishedDate = new PhoenixAttribute<bool>(506, "User-Set Published Date");
        public static PhoenixAttribute<DateTime> UserProposedPublishedDate = new PhoenixAttribute<DateTime>(509, "User-Proposed Published Date");
        public static PhoenixAttribute<bool> DontUpdatePublishedDate = new PhoenixAttribute<bool>(507, "Don't Update Published Date");
        public static PhoenixAttribute<bool> HiddenUpdate = new PhoenixAttribute<bool>(508, "Hidden Update");
        public static PhoenixAttribute<string> InRangeName = new PhoenixAttribute<string>(515, "InRangeName");
        public static PhoenixAttribute<string> InRangeValue = new PhoenixAttribute<string>(516, "InRangeValue");
        public static PhoenixAttribute<string> OutSheet = new PhoenixAttribute<string>(517, "OutSheet");
        public static PhoenixAttribute<string> OutRange = new PhoenixAttribute<string>(518, "OutRange");
        public static PhoenixAttribute<string> OutChartName = new PhoenixAttribute<string>(519, "OutChartName");
        public static PhoenixAttribute<string> OutImageName = new PhoenixAttribute<string>(520, "OutImageName");
        public static PhoenixAttribute<string> PdfBusinessLine = new PhoenixAttribute<string>(620, "PDF Business Line");
        public static PhoenixAttribute<string> PdfReportType = new PhoenixAttribute<string>(621, "PDF Report Type");
        public static PhoenixAttribute<string> PdfReportTitle = new PhoenixAttribute<string>(622, "PDF Report Title");
        public static PhoenixAttribute<string> PdfSubheading = new PhoenixAttribute<string>(623, "PDF Subheading");
        public static PhoenixAttribute<string> PdfDate = new PhoenixAttribute<string>(624, "PDF Date");
        public static PhoenixAttribute<string> PdfAddressBar = new PhoenixAttribute<string>(625, "PDF Address Bar");
        public static PhoenixAttribute<string> UploaderVersion = new PhoenixAttribute<string>(501, "Uploader Version");
        public static PhoenixAttribute<long> AnalystWhoSentForReviewId = new PhoenixAttribute<long>(502, "Analyst who sent for review id");
        public static PhoenixAttribute<long> EditorWhoSentBackToAnalystId = new PhoenixAttribute<long>(503, "Editor who sent back to Analyst id");
        public static PhoenixAttribute<string> PublishFailReason = new PhoenixAttribute<string>(510, "Publishing fail reason");
        public static PhoenixAttribute<DateTime> DataRefreshStart = new PhoenixAttribute<DateTime>(511, "Data refresh started");
        public static PhoenixAttribute<DateTime> DataRefreshEnd = new PhoenixAttribute<DateTime>(512, "Data refresh ended");
        public static PhoenixAttribute<DateTime> DataRefreshExcelPull = new PhoenixAttribute<DateTime>(513, "Data refresh excels pulled");
        public static PhoenixAttribute<string> DataRefreshFailReason = new PhoenixAttribute<string>(514, "Data refresh fail reason");
        public static PhoenixAttribute<string> ChemicalProductAndMarket = new PhoenixAttribute<string>(635, "Chemical Product and Market(Taxonomy)");
        public static PhoenixAttribute<long> BrandTaxonomy = new PhoenixAttribute<long>(628, "Brand(Taxonomy)");
        public static PhoenixAttribute<string> DomainsTaxonomy = new PhoenixAttribute<string>(627, "Domain(Taxonomy)");
        public static PhoenixAttribute<long> ContentTypeTaxonomy = new PhoenixAttribute<long>(629, "Content Type(Taxonomy)");
        public static PhoenixAttribute<long> FileClassificationTaxonomy = new PhoenixAttribute<long>(630, "File Classification(Taxonomy)");
        public static PhoenixAttribute<long> PublishFrequencyTaxonomy = new PhoenixAttribute<long>(631, "Publish Frequency(Taxonomy)");
        public static PhoenixAttribute<string> AuthorizationCode = new PhoenixAttribute<string>(634, "Authorization Code");
        public static PhoenixAttribute<string> Authors = new PhoenixAttribute<string>(636, "Authors");
        public static PhoenixAttribute<string> DomainUrl = new PhoenixAttribute<string>(638, "Domain Url");
        public static PhoenixAttribute<string> HardCopyPublicationDate = new PhoenixAttribute<string>(639, "Hardcopy Publication Date");
        public static PhoenixAttribute<string> BrandDisplayName = new PhoenixAttribute<string>(632, "Brand");
        public static PhoenixAttribute<string> DomainsDisplayName = new PhoenixAttribute<string>(633, "Domains");

    }
}