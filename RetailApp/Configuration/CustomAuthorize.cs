using System.Net;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RetailApp.Configuration;

public class CustomAuthorize : AuthorizeAttribute
{
    public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
    {
        //base.OnAuthorization(actionContext);
        IEnumerable<string> apiKeyHeaderValues = null;
        if (actionContext.Request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
        {
            var token = apiKeyHeaderValues.First();
            return ;
           // var decToken = Utils.Decrypt(token);
            //var userInfo = decToken.Split(new[] { "##" }, StringSplitOptions.None);
            //if (userInfo.Count() > 0)
            //{
            //    var userEmail = userInfo[0];
            //    //using (var db = new MERContext())
            //    //{
            //    //    var loginUser = db.Users.Include("Role").FirstOrDefault(u => u.Email.ToLower() == userEmail);
            //    //    if (loginUser != null)
            //    //    {
            //    //        return;
            //    //    }
            //    //}
            //}
        }

        //invalid token
        //HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
        //HttpContext.Current.Response.Write("401-NotAuthorized");
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        return;

    }

}
