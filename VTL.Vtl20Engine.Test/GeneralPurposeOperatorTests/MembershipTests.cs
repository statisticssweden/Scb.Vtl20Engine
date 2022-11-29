using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.GeneralPurposeOperatorTests
{
    [TestClass]
    public class MembershipTests
    {
        [TestMethod]
        public void Membership_GetComponentNamesMeasure()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "RefDate",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "MeasName",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Value1",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Value2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                }, new SimpleDataPointContainer())
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var res = sut.Execute("d <- DS_1#Value1", new Operand[] {ds_1}).FirstOrDefault(r => r.Persistant);

            var components = res.GetComponentNames();
            Assert.AreEqual(3, components.Length);
            Assert.AreEqual("RefDate", components[0]);
            Assert.AreEqual("MeasName", components[1]);
            Assert.AreEqual("Value1", components[2]);

            var identifiers = res.GetIdentifierNames();
            Assert.AreEqual(2, identifiers.Length);
            Assert.AreEqual("RefDate", identifiers[0]);
            Assert.AreEqual("MeasName", identifiers[1]);

            var measures = res.GetMeasureNames();
            Assert.AreEqual(1, measures.Length);
            Assert.AreEqual("Value1", measures[0]);
        }

        [TestMethod]
        public void Membership_GetComponentNamesIdentifier()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "RefDate",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "MeasName",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Value1",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Value2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                }, new SimpleDataPointContainer())
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var res = sut.Execute("d <- DS_1#MeasName", new Operand[] { ds_1 }).FirstOrDefault(r => r.Persistant);

            var components = res.GetComponentNames();
            Assert.AreEqual(3, components.Length);
            Assert.AreEqual("RefDate", components[0]);
            Assert.AreEqual("MeasName", components[1]);
            Assert.AreEqual("string_var", components[2]);

            var identifiers = res.GetIdentifierNames();
            Assert.AreEqual(2, identifiers.Length);
            Assert.AreEqual("RefDate", identifiers[0]);
            Assert.AreEqual("MeasName", identifiers[1]);

            var measures = res.GetMeasureNames();
            Assert.AreEqual(1, measures.Length);
            Assert.AreEqual("string_var", measures[0]);
        }
    }
}
