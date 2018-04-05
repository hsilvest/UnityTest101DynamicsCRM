using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Plugin.Account
{
    public class PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            var service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                var entity = (Entity)context.InputParameters["Target"];

                if (entity.LogicalName == "contact")
                {
                    var accountRef = entity.GetAttributeValue<EntityReference>("accountid");

                    if (accountRef == null)
                    {
                        throw new InvalidPluginExecutionException("Please, provide a account for the contact");
                    }
                    else
                    {
                        var accountEntity = service.Retrieve("account", accountRef.Id, new ColumnSet("accountnumber"));

                        entity["firstname"] = $"{accountEntity.GetAttributeValue<string>("accountnumber")}-{entity["firstname"]}";
                    }
                }
            }
        }
    }
}
