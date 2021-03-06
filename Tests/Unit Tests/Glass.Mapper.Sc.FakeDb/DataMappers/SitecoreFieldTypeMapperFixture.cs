using System;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldTypeMapperFixture 
    {

        #region Method - CanHandle

        [Test]
        public void CanHandle_TypeHasBeenLoadedByGlass_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldTypeMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_TypeHasNotBeenLoadedByGlass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreFieldTypeMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyFalse");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - GetField


        [Test]
        public void GetField_FieldContainsId_ReturnsConcreteType()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var config = new SitecoreFieldConfiguration();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);


                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = item.ID.ToString();
                }

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.AreEqual(item.ID.Guid, result.Id);
            }
        }

        [Test]
        public void GetField_FieldEmpty_ReturnsNull()
        {
            //Assign

            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var targetId = string.Empty;
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var config = new SitecoreFieldConfiguration();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);


                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = targetId.ToString();
                }

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetField_FieldRandomText_ReturnsNull()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var targetId = "some random text";
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var config = new SitecoreFieldConfiguration();
                var options = new GetItemOptionsParams();

                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);


                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = targetId.ToString();
                }

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.IsNull(result);

            }
        }

        //#endregion


        //#region Method - SetField

        [Test]
        public void SetField_ClassContainsId_IdSetInField()
        {
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];

                var config = new SitecoreFieldConfiguration();
                var options = new GetItemOptionsParams();


                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);

                var propertyValue = new Stub();
                propertyValue.Id = targetId;

                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, propertyValue, config, scContext);
                }
                //Assert
                Assert.AreEqual(targetId, Guid.Parse(item["Field"]));
            }
        }


        [Test]
        public void SetField_ClassContainsNoIdProperty_ThrowsException()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var options = new GetItemOptionsParams();

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);

                var propertyValue = new StubNoId();

                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    Assert.Throws<NotSupportedException>(() =>
                    {
                        mapper.SetField(field, propertyValue, config,
                            scContext);
                    });
                }

                //Assert
                Assert.AreEqual(string.Empty, item["Field"]);
            }
        }


        [Test]
        public void SetField_ContextDatabaseNull_ThrowsException()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var options = new GetItemOptionsParams();

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(null as Database, context);

                var propertyValue = new StubNoId();

                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    Assert.Throws<NullReferenceException>(() =>
                    {
                        mapper.SetField(field, propertyValue, config, scContext);
                    });
                }

                //Assert
                Assert.AreEqual(string.Empty, item["Field"]);
            }
        }

        [Test]
        public void SetField_ContextNull_ThrowsException()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

                var propertyValue = new StubNoId();

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    using (new ItemEditing(item, true))
                    {
                        Assert.Throws<ArgumentNullException>(() =>
                        {
                            mapper.SetField(field, propertyValue, config, null);
                        });
                    }
                }

                //Assert
                Assert.AreEqual(string.Empty, item["Field"]);
            }
        }

        [Test]
        public void SetField_ContextServiceNull_ThrowsException()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var options = new GetItemOptionsParams();

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));


                var propertyValue = new StubNoId();

                var scContext = new SitecoreDataMappingContext(null, item, null, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                using (new ItemEditing(item, true))
                {
                    Assert.Throws<NullReferenceException>(() =>
                    {
                        mapper.SetField(field, propertyValue, config, scContext);
                    });
                }

                //Assert
                Assert.AreEqual(string.Empty, item["Field"]);
            }
        }

        [Test]
        public void SetField_ClassContainsIdButItemMissing_ThrowsException()
        {
            //Assign
            var templateId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");

            using (Db database = new Db
            {
                new DbTemplate(new ID(templateId))
                {
                    new DbField("Field")
                    {
                        Type = "text"
                    }
                },

                new Sitecore.FakeDb.DbItem("Target", ID.NewID, new ID(templateId))
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var targetId = Guid.Parse("{11111111-A3F0-410E-8A6D-07FF3A1E78C3}");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields["Field"];
                var options = new GetItemOptionsParams();

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubContaining)));

                var service = new SitecoreService(database.Database, context);

                var propertyValue = new Stub();
                propertyValue.Id = targetId;

                var scContext = new SitecoreDataMappingContext(null, item, service, options);

                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                //Act
                Assert.Throws<NullReferenceException>(() =>
                {
                    mapper.SetField(field, propertyValue, config, scContext);
                });
            }
        }
        #endregion

        #region Stubs

        [SitecoreType]
        public class Stub
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        public class StubContaining : StubInterface
        {
            public Stub PropertyTrue { get; set; }
            public StubContaining PropertyFalse { get; set; }
            public StubNoId PropertyNoId { get; set; }
        }

        [SitecoreType]
        public class StubNoId
        {
        }

        public interface StubInterface
        {
            
        }

        #endregion



    }
}




