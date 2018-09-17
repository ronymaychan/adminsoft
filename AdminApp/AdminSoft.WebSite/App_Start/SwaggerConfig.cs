using System.Web.Http;
using WebActivatorEx;
using AdminSoft.WebSite;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace AdminSoft.WebSite
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "AdminSoft.WebSite");
                        c.DocumentFilter<AuthTokenOperation>();
                        c.DescribeAllEnumsAsStrings();
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }

    class AuthTokenOperation : IDocumentFilter
    {

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/api/TokenAuth/Authenticate", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string> { "TokenAuth" },
                    consumes = new List<string> {
                        "application/x-www-form-urlencoded",
                        "application/json",
                        "text/json",
                        "application/xml",
                        "text/xml"
                    },
                    parameters = new List<Parameter> {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = true,
                            @in = "formData",

                        },
                        new Parameter
                        {
                            type = "string",
                            name = "password",
                            required = true,
                            @in = "formData"
                        }
                    },
                    responses = new Dictionary<string, Response>()
                    {
                        {
                            "200",
                            new Response() {
                                description = "OK",
                                schema = new Schema(){
                                    type = typeof(object).Name.ToLower()
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
