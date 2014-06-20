using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;
using com.quark.qpp.common.constants;
using com.quark.qpp.core.attribute.service.remote;
using com.quark.qpp.core.content.service.remote;
using com.quark.qpp.core.privilege.service.remote;
using com.quark.qpp.core.relation.service.remote;
using com.quark.qpp.core.workflow.service.remote;

namespace IHS.Phoenix.QPP
{
    public class PhoenixValuesInitializer
    {
        public static void Initialize(Func<string, SoapHttpClientProtocol> getService)
        {
            _getService = getService;

            Init<AttributeService>(typeof (CustomAttributes), ServiceNames.ATTRIBUTE_SERVICE, GetAllAttributes);
            Init<ContentStructureService>(typeof (CustomContentTypes), ServiceNames.CONTENT_STRUCTURE_SERVICE, GetAllContentTypes);
            Init<PrivilegeService>(typeof (CustomPrivilegeGroups), ServiceNames.PRIVILEGE_SERVICE, GetAllPrivilegeGroups);
            Init<PrivilegeService>(typeof (CustomPrivileges), ServiceNames.PRIVILEGE_SERVICE, GetAllPrivileges);
            Init<RelationService>(typeof (CustomRelations), ServiceNames.RELATION_SERVICE, GetAllRelations);
            Init<UserClassService>(typeof (CustomRoles), ServiceNames.USER_CLASS_SERVICE, GetAllRoles);
            Init<WorkflowService>(typeof (CustomStatuses), ServiceNames.WORKFLOW_SERVICE, GetAllStatuses);
            Init<WorkflowService>(typeof (CustomWorkflows), ServiceNames.WORKFLOW_SERVICE, GetAllWorkflows);

            IsInitialized = true;
        }

        private static Func<string, SoapHttpClientProtocol> _getService;

        public static bool IsInitialized { get; private set; }

        private static void Init<T>(Type valuesType, string serviceName, Func<T, IEnumerable<PhoenixValue>> getValues)
            where T : SoapHttpClientProtocol
        {
            var service = (T) _getService(serviceName);
            var values = getValues(service);

            Replace(valuesType, values);
        }

        private static IEnumerable<PhoenixValue> GetAllAttributes(AttributeService attributeService)
        {
            return attributeService
                .getAllAttributes()
                .Select(a => new PhoenixValue(a.id, a.name));
        }

        private static IEnumerable<PhoenixValue> GetAllContentTypes(ContentStructureService contentStructureService)
        {
            return contentStructureService
                .getAllContentTypes()
                .Select(ct => new PhoenixValue(ct.id, ct.name));
        }

        private static IEnumerable<PhoenixValue> GetAllStatuses(WorkflowService workflowService)
        {
            return workflowService
                .getAllWorkflows()
                .SelectMany(w => w.statuses)
                .Select(s => new PhoenixValue(s.id, s.name));
        }

        private static IEnumerable<PhoenixValue> GetAllWorkflows(WorkflowService workflowService)
        {
            return workflowService
                .getAllWorkflows()
                .Select(w => new PhoenixValue(w.id, w.name));
        }

        private static IEnumerable<PhoenixValue> GetAllRoles(UserClassService userClassService)
        {
            return userClassService
                .getAllUserClasses()
                .Select(uc => new PhoenixValue(uc.id, uc.name));
        }

        private static IEnumerable<PhoenixValue> GetAllRelations(RelationService relationService)
        {
            return relationService
                .getAllRelationTypes()
                .Select(rt => new PhoenixValue(rt.id, rt.name));
        }

        private static IEnumerable<PhoenixValue> GetAllPrivileges(PrivilegeService privilegeService)
        {
            return privilegeService
                .getAllPrivilegeDefs()
                .Select(p => new PhoenixValue(p.id, p.name));
        }

        private static IEnumerable<PhoenixValue> GetAllPrivilegeGroups(PrivilegeService privilegeService)
        {
            return privilegeService
                .getAllPrivilegeGroupDefs()
                .Select(pg => new PhoenixValue(pg.id, pg.name));
        }

        private static void Replace(Type type, IEnumerable<PhoenixValue> values)
        {
            var dictionary = ToDictionary(values);

            foreach (var defaultField in type
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(FieldHasDefaultId))
            {
                var value = (PhoenixValue) defaultField.GetValue(null);
                if (dictionary.ContainsKey(value))
                {
                    var actualValue = dictionary[value];
                    if (actualValue != null)
                        defaultField.SetValue(null, actualValue);
                }
                else
                    Debug.WriteLine("Cannot find " + value + " on QPP");
            }
        }

        private static bool FieldHasDefaultId(FieldInfo field)
        {
            var value = (PhoenixValue) field.GetValue(null);
            return value < 0;
        }

        private static Dictionary<string, PhoenixValue> ToDictionary(IEnumerable<PhoenixValue> values)
        {
            return values
                .GroupBy(v => (string) v)
                // ignores duplicates
                .Select(grouping => new {grouping.Key, First = grouping.First()})
                .ToDictionary(arg => arg.Key, arg => arg.First);
        }
    }
}