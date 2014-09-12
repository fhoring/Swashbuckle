﻿using Newtonsoft.Json.Linq;
using System;
using NUnit.Framework;
using Swashbuckle.Dummy.Controllers;
using System.Collections.Generic;
using Swashbuckle.Application;
using Swashbuckle.Configuration;
using Swashbuckle.Dummy.SwaggerExtensions;

namespace Swashbuckle.Tests.Swagger2
{
    [TestFixture]
    public class CoreTests : HttpMessageHandlerTestFixture<SwaggerDocsHandler>
    {
        private Swagger20Config _swaggerConfig;

        public CoreTests()
            : base("swagger/docs/{apiVersion}")
        { }

        [SetUp]
        public void SetUp()
        {
            _swaggerConfig = new Swagger20Config();
            _swaggerConfig.ApiVersion("1.0").Title("Test API");

            Configuration.SetSwaggerConfig(_swaggerConfig);
        }

        [Test]
        public void It_should_indicate_swagger_version_2_0()
        {
            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");

            Assert.AreEqual("2.0", swagger["swagger"].ToString());
        }

        [Test]
        public void It_should_provide_info_version_and_title()
        {
            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");

            var info = swagger["info"];
            Assert.IsNotNull(info);

            var expected = JObject.FromObject(new
                {
                    version = "1.0",
                    title = "Test API",
                });
            Assert.AreEqual(expected.ToString(), info.ToString());
        }

        [Test]
        public void It_should_provide_host_base_path_and_schemes()
        {
            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");

            var host = swagger["host"];
            Assert.IsNotNull(host);
            Assert.AreEqual("tempuri.org", host.ToString());

            var basePath = swagger["basePath"];
            Assert.IsNotNull(basePath);
            Assert.AreEqual("/", basePath.ToString());

            var schemes = swagger["schemes"];
            Assert.IsNotNull(schemes);

            var expected = JArray.FromObject(new object[] { "http" });
            Assert.AreEqual(expected.ToString(), schemes.ToString());
        }

        [Test]
        public void It_should_provide_a_description_for_each_path_in_the_api()
        {
            AddDefaultRoutesFor(new[] { typeof(ProductsController), typeof(CustomersController) });

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var paths = swagger["paths"];

            var expected = JObject.FromObject(new Dictionary<string, object>
                {
                    {
                        "/products", new
                        {
                            get = new
                            {
                                operationId = "Products.GetAllByType",
                                consumes = new object[]{},
                                produces = new []{ "application/json", "text/json", "application/xml", "text/xml" },
                                parameters = new []
                                {
                                    new
                                    {
                                        name = "type",
                                        @in = "query",
                                        required = true,
                                        type = "string"
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new
                                        {
                                            schema = new
                                            {
                                                items = JObject.Parse("{ $ref: \"#/definitions/Product\" }"),
                                                type = "array"
                                            }
                                        }
                                    }
                                },
                                deprecated = false
                            },
                            post = new
                            {
                                operationId = "Products.Create",
                                consumes = new []{ "application/json", "text/json", "application/xml", "text/xml", "application/x-www-form-urlencoded" },
                                produces = new []{ "application/json", "text/json", "application/xml", "text/xml" },
                                parameters = new []
                                {
                                    new
                                    {
                                        name = "product",
                                        @in = "body",
                                        required = true,
                                        schema = JObject.Parse("{ $ref: \"#/definitions/Product\" }")
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new
                                        {
                                            schema = new
                                            {
                                                format = "int32",
                                                type = "integer"
                                            }
                                        }
                                    }
                                },
                                deprecated = false
                            }
                        }
                    },
                    {
                        "/products/{id}", new
                        {
                            get = new
                            {
                                operationId = "Products.GetById",
                                consumes = new object[]{},
                                produces = new []{ "application/json", "text/json", "application/xml", "text/xml" },
                                parameters = new []
                                {
                                    new
                                    {
                                        name = "id",
                                        @in = "path",
                                        required = true,
                                        type = "integer",
                                        format = "int32"
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new
                                        {
                                            schema = JObject.Parse("{ $ref: \"#/definitions/Product\" }")
                                        }
                                    }
                                },
                                deprecated = false
                            }
                        }
                    },
                    {
                        "/customers", new
                        {
                            post = new
                            {
                                operationId = "Customers.Create",
                                consumes = new []{ "application/json", "text/json", "application/xml", "text/xml", "application/x-www-form-urlencoded" },
                                produces = new []{ "application/json", "text/json", "application/xml", "text/xml" },
                                parameters = new []
                                {
                                    new
                                    {
                                        name = "customer",
                                        @in = "body",
                                        required = true,
                                        schema = JObject.Parse("{ $ref: \"#/definitions/Customer\" }")
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new
                                        {
                                            schema = new
                                            {
                                                format = "int32",
                                                type = "integer"
                                            }
                                        }
                                    }
                                },
                                deprecated = false
                            }
                        }
                    },
                    {
                        "/customers/{id}", new
                        {
                            put = new
                            {
                                operationId = "Customers.Update",
                                consumes = new []{ "application/json", "text/json", "application/xml", "text/xml", "application/x-www-form-urlencoded" },
                                produces = new string[]{},
                                parameters = new object []
                                {
                                    new
                                    {
                                        name = "id",
                                        @in = "path",
                                        required = true,
                                        type = "integer",
                                        format = "int32",
                                    },
                                    new
                                    {
                                        name = "customer",
                                        @in = "body",
                                        required = true,
                                        schema = JObject.Parse("{ $ref: \"#/definitions/Customer\" }")
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new { }
                                    }
                                },
                                deprecated = false
                            },
                            delete = new
                            {
                                operationId = "Customers.Delete",
                                consumes = new string[]{},
                                produces = new string[]{},
                                parameters = new []
                                {
                                    new
                                    {
                                        name = "id",
                                        @in = "path",
                                        required = true,
                                        type = "integer",
                                        format = "int32"
                                    }
                                },
                                responses = new Dictionary<string, object>
                                {
                                    {
                                        "200", new { }
                                    }
                                },
                                deprecated = false
                            }
                        }
                    }
                });

            Assert.AreEqual(expected.ToString(), paths.ToString());
        }


        [Test]
        public void It_should_support_config_to_include_additional_info_properties()
        {
            _swaggerConfig.ApiVersion("1.0")
                .Title("Test API")
                .Description("A test API")
                .TermsOfService("Test terms")
                .Contact(c => c
                    .Name("Joe Test")
                    .Url("http://tempuri.org/contact")
                    .Email("joe.test@tempuri.org"))
                .License(c => c
                    .Name("Test License")
                    .Url("http://tempuri.org/license"));

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");

            var info = swagger["info"];
            Assert.IsNotNull(info);

            var expected = JObject.FromObject(new
                {
                    version = "1.0",
                    title = "Test API",
                    description = "A test API",
                    termsOfService = "Test terms",
                    contact = new
                    {
                        name = "Joe Test",
                        url = "http://tempuri.org/contact",
                        email = "joe.test@tempuri.org"
                    },
                    license = new
                    {
                        name = "Test License",
                        url = "http://tempuri.org/license"
                    }
                });
            Assert.AreEqual(expected.ToString(), info.ToString());
        }

        [Test]
        public void It_should_support_config_to_customize_the_host_name()
        {
            _swaggerConfig.HostName((req) => "foobar.com");

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");

            var host = swagger["host"];
            Assert.IsNotNull(host);
            Assert.AreEqual("foobar.com", host.ToString());
        }

        [Test]
        public void It_should_mark_an_operation_deprecated_if_the_action_is_obsolete()
        {
            AddDefaultRouteFor<ObsoleteActionsController>();

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var putOp = swagger["paths"]["/obsoleteactions/{id}"]["put"];
            var deleteOp = swagger["paths"]["/obsoleteactions/{id}"]["delete"];

            Assert.IsFalse((bool)putOp["deprecated"]);
            Assert.IsTrue((bool)deleteOp["deprecated"]);
        }

        [Test]
        public void It_should_support_config_to_post_modify_the_document()
        {
            _swaggerConfig.DocumentFilter<ApplyVendorExtensions>();

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var xProp = swagger["x-foo"];

            Assert.IsNotNull(xProp);
            Assert.AreEqual("bar", xProp.ToString());
        }

        [Test]
        public void It_should_support_config_to_post_modify_operations()
        {
            AddDefaultRouteFor<ProductsController>();

            _swaggerConfig.OperationFilter<AddDefaultResponse>();

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var getDefaultResponse = swagger["paths"]["/products"]["get"]["responses"]["default"];
            var postDefaultResponse = swagger["paths"]["/products"]["post"]["responses"]["default"];

            Assert.IsNotNull(getDefaultResponse);
            Assert.IsNotNull(postDefaultResponse);
        }

        [Test]
        public void It_should_handle_additional_route_parameters()
        {
            // i.e. route params that are not included in the action signature
            AddCustomRouteFor<ProductsController>("{apiVersion}/products");

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var getParams = swagger["paths"]["/{apiVersion}/products"]["get"]["parameters"];

            var expected = JArray.FromObject(new []
                {
                    new
                    {
                        name = "type",
                        @in = "query",
                        required = true,
                        type = "string"
                    },
                    new
                    {
                        name = "apiVersion",
                        @in = "path",
                        required = true,
                        type = "string"
                    }
                });

            Assert.AreEqual(expected.ToString(), getParams.ToString());
        }

        [Test]
        public void It_should_handle_attribute_routes()
        {
            AddAttributeRoutes();

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
            var path = swagger["paths"]["/subscriptions/{id}/cancel"];
            Assert.IsNotNull(path);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void It_should_error_on_multiple_actions_with_same_path_and_method()
        {
            AddDefaultRouteFor<UnsupportedActionsController>();

            var swagger = Get<JObject>("http://tempuri.org/swagger/docs/1.0");
        }
    }
}