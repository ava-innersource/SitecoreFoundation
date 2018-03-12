using Sitecore.Data;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SF.Foundation.API;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.GetImportContext;
using System.Xml.Serialization;
using System.Web;
using Sitecore.Pipelines;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.Import;
using System.Reflection;
using System.Net.Http;
using System.Net;

namespace SF.Foundation.Resources
{


    public class CreativeExchangeController : ServicesApiController
    {
        [HttpGet]
        public string Import(string homePageId, string database = "master")
        {
            try
            {

                var db = Sitecore.Data.Database.GetDatabase(database);
                //var coreDb = Sitecore.Data.Database.GetDatabase("core");
                //switch to master?
                using (new Sitecore.Data.DatabaseSwitcher(db))
                {
                    var item = db.GetItem(new Sitecore.Data.ID(homePageId));

                    //trying to fake out to use folder parametre
                    //rbStorageType={2292EC3E-D6C2-4FFF-A994-41E6A61287C0}
                    //AS it's required for on the of the processors.
                    //Unfortuently the Request collection is normally read only. But we can hack it.
                    // reflect to readonly property
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    // make collection editable
                    isreadonly.SetValue(HttpContext.Current.Request.Form, false, null);
                    //Probably shouldn't do this, but more convenient than forcing this to be passed as a parameter.
                    HttpContext.Current.Request.Form.Add("rbStorageType", "{2292EC3E-D6C2-4FFF-A994-41E6A61287C0}");

                    GetImportContextArgs args = new GetImportContextArgs()
                    {
                        ImportContext = new Sitecore.XA.Feature.CreativeExchange.Models.Import.ImportContext(),
                        Item = item,
                        HttpContext = HttpContext.Current
                    };

                    CorePipeline.Run("ceImport.getImportContext", args);

                    var storageInit = args.ImportContext.StorageServiceDefinition.InstantiateImportStorage();
                    var importMethod = storageInit.GetImportMethod();

                    ImportArgs importArg = new ImportArgs()
                    {
                        ImportContext = args.ImportContext,
                        HttpContext = args.HttpContext,
                        Messages = args.Messages
                    };

                    importArg.ImportContext.Database = db;

                    CorePipeline.Run("ceImport.import", importArg);

                    var sb = new StringBuilder();

                    foreach (var message in importArg.Messages)
                    {
                        sb.Append(message.Message).Append(Environment.NewLine);
                    }

                    sb.Append("Import Complete");

                    return sb.ToString();
                }

            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }





    }
}
