// See https://aka.ms/new-console-template for more information
using DemoApp;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

Console.WriteLine(new string('-', 21 * 3));
Console.Write(new string('-', 21));
Console.Write(" VTL engine demo app ");
Console.WriteLine(new string('-', 21));
Console.WriteLine(new string('-', 21 * 3));
Console.WriteLine("");
Console.WriteLine("");

// Create a data container factory that creates suitable data storage
// based on provided data amount
var dataContainerFactory = new SimpleDataContainerFactory();

// Create an instance of VTL eninge
var engine = new VTL.Vtl20Engine.VtlEngine(dataContainerFactory);

// Create sample data sets for demo app
Operand[] heap = MakeDemoDataSets();

var vtlCode = "";
var init = true;

do
{
    try
    {
        if (init)
        {
            init = false;
        }
        else
        {
            // Execute parser
            heap = engine.Execute(vtlCode, heap);
        }

        foreach (var op in heap)
        {
            Console.WriteLine(op.Alias);

            // GetValue performs calculation
            var opValue = op.GetValue();

            if (opValue is ScalarType scalar)
            {
                Console.WriteLine(scalar);
            }

            // Print current state of data variables
            if (opValue is DataSetType dataSet)
            {
                Console.Write(" ");
                foreach (var component in dataSet.DataSetComponents)
                {
                    Console.Write($"{component.Name,13}");
                }
                Console.WriteLine();
                Console.WriteLine(new string('-', dataSet.DataSetComponents.Length * 13 + 2));

                foreach (var dataPoint in dataSet.DataPoints)
                {
                    Console.Write("|");
                    foreach (var scalarValue in dataPoint)
                    {
                        Console.Write($"{scalarValue,13}");
                    }
                    Console.Write("|");
                    Console.WriteLine();
                }
                Console.WriteLine(new string('-', dataSet.DataSetComponents.Length * 13 + 2));
            }
            Console.WriteLine();
        }
    }
    catch(Exception e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine();
    }
    Console.WriteLine("Enter VTL code: ");
    vtlCode = Console.ReadLine();
}
while (!string.IsNullOrWhiteSpace(vtlCode));

// Hard code your sample data
static Operand[] MakeDemoDataSets()
{
    return new Operand[]
    {
    new Operand
    {
        Alias = "DS_1",
        Data = DemoComponent.MakeDataSet(new List<DemoComponent>
        {
            new DemoComponent(typeof(StringType))
            {
                Name = "Date",
                Role = ComponentType.ComponentRole.Identifier
            },
            new DemoComponent(typeof(StringType))
            {
                Name = "Meas. Name",
                Role = ComponentType.ComponentRole.Identifier
            },
            new DemoComponent(typeof(IntegerType))
            {
                Name = "Meas. Value",
                Role = ComponentType.ComponentRole.Measure
            }
        }, new SimpleDataPointContainer(new HashSet<DataPointType>
        {
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2013"),
                    new StringType("Population"),
                    new IntegerType(200)
                }),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2013"),
                    new StringType("Gross Prod."),
                    new IntegerType(800)
                }),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2014"),
                    new StringType("Population"),
                    new IntegerType(250)
                }),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2014"),
                    new StringType("Gross Prod."),
                    new IntegerType(1000)
                }
            )
        }))
    },

    new Operand
    {
        Alias = "DS_2",
        Data = DemoComponent.MakeDataSet(new List<DemoComponent>
        {
            new DemoComponent(typeof(StringType))
            {
                Name = "Date",
                Role = ComponentType.ComponentRole.Identifier
            },
            new DemoComponent(typeof(StringType))
            {
                Name = "Meas. Name",
                Role = ComponentType.ComponentRole.Identifier
            },
            new DemoComponent(typeof(IntegerType))
            {
                Name = "Meas. Value",
                Role = ComponentType.ComponentRole.Measure
            }
        },
        new SimpleDataPointContainer(new HashSet<DataPointType>
        {
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2013"),
                    new StringType("Population"),
                    new IntegerType(300)
                }
            ),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2013"),
                    new StringType("Gross Prod."),
                    new IntegerType(900)
                }
            ),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2014"),
                    new StringType("Population"),
                    new IntegerType(350)
                }
            ),
            new DataPointType
            (
                new ScalarType[]
                {
                    new StringType("2014"),
                    new StringType("Gross Prod."),
                    new IntegerType(1000)
                }
            )
        }))
    }
    };
}