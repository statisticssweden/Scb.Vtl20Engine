using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.ComparersTests
{
    [TestClass]
    public class DataPointComparerTest
    {
        private DataSetType _ds_1;

        [TestInitialize]
        public void TestSetup()
        {
            _ds_1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Value1",
                        Role = ComponentType.ComponentRole.Measure
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Population"),
                            new IntegerType(200)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(805)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(250)
                        }
                    )
                }));
        }

        [TestMethod]
        public void DataPointComparer_SortDatapoints()
        {
            _ds_1.SortComponents();

            Assert.AreEqual("MeasName", _ds_1.DataSetComponents[0].Name);
            Assert.AreEqual("RefDate", _ds_1.DataSetComponents[1].Name);
            Assert.AreEqual("Value1", _ds_1.DataSetComponents[2].Name);

            _ds_1.SortDataPoints(_ds_1.DataSetComponents.OrderBy(x => x.Name).
                Select(x => new OrderByName() { ComponentName = x.Name }).ToArray());

            using (var dataPointEnumerator = _ds_1.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)805, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void DataPointComparer_CompareBool()
        {
            var trueBool = new BooleanType(true);
            var falseBool = new BooleanType(false);
            var nullBool = new BooleanType(null);

            Assert.AreEqual(1, trueBool.CompareTo(nullBool));
            Assert.AreEqual(-1, nullBool.CompareTo(trueBool));
            Assert.AreEqual(1, falseBool.CompareTo(nullBool));
            Assert.AreEqual(-1, nullBool.CompareTo(falseBool));
            Assert.AreEqual(0, nullBool.CompareTo(nullBool));
            Assert.AreEqual(0, falseBool.CompareTo(falseBool));
            Assert.AreEqual(-1, falseBool.CompareTo(trueBool));
            Assert.AreEqual(1, trueBool.CompareTo(falseBool));
            Assert.AreEqual(0, trueBool.CompareTo(trueBool));
        }

        [TestMethod]
        public void DataPointComparer_CompareNumbers()
        {
            var one = new IntegerType(1);
            var minus = new IntegerType(-1);
            var none = new IntegerType(null);

            Assert.AreEqual(1, one.CompareTo(none));
            Assert.AreEqual(-1, none.CompareTo(one));
            Assert.AreEqual(1, minus.CompareTo(none));
            Assert.AreEqual(-1, none.CompareTo(minus));
            Assert.AreEqual(0, none.CompareTo(none));
            Assert.AreEqual(0, minus.CompareTo(minus));
            Assert.AreEqual(-1, minus.CompareTo(one));
            Assert.AreEqual(1, one.CompareTo(minus));
            Assert.AreEqual(0, one.CompareTo(one));
        }
    }
}
