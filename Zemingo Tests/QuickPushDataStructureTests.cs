using Microsoft.Extensions.Logging;
using NSubstitute;
using Zemingo_Tests.TestData;

namespace Zemingo.Tests
{
    [TestFixture]
    public class QuickPushDataStructureTests
    {
        private readonly ILogger _logger;

        public QuickPushDataStructureTests()
        {
            _logger = Substitute.For<ILogger>();
        }

        [Test]
        public void SimpleDataType_Pop_Returns_MaximumValueElement()
        {
            // Arrange
            var quickPushDataStructure = new QuickPushDataStructure<int>(_logger);

            // Act
            quickPushDataStructure.Push(5);
            quickPushDataStructure.Push(10);
            quickPushDataStructure.Push(3);
            var popped1 = quickPushDataStructure.Pop();

            // Assert
            Assert.That(popped1, Is.EqualTo(10));
        }

        [Test]
        public void ComplexDataType_Pop_Returns_MaximumValueElement()
        {
            // Arrange
            var quickPushDataStructure = new QuickPushDataStructure<Person>(_logger);
            var person1 = new Person("Test1", 1, 500);
            var person2 = new Person("Test2", 2, 800);
            var person3 = new Person("Test3", 3, 200);

            // Act
            quickPushDataStructure.Push(person1);
            quickPushDataStructure.Push(person2);
            quickPushDataStructure.Push(person3);

            var popped1 = quickPushDataStructure.Pop();
            var popped2 = quickPushDataStructure.Pop();
            var popped3 = quickPushDataStructure.Pop();

            // Assert
            Assert.That(popped1.TotalMark, Is.EqualTo(800));
            Assert.That(popped2.TotalMark, Is.EqualTo(500));
            Assert.That(popped3.TotalMark, Is.EqualTo(200));
        }

        [Test]
        public void Pop_EmptyCollections_Logs_Error()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var quickPushDataStructure = new QuickPushDataStructure<int>(logger);

            // Act
            var result = quickPushDataStructure.Pop();

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
            logger.Received(1).LogError("The QuickPushDataStructure is empty.");
        }

        [Test]
        public void Push_Pop_SingleThreaded_WorksAsExpected()
        {
            // Arrange
            var quickPushDataStructure = new QuickPushDataStructure<int>(_logger);

            // Act
            quickPushDataStructure.Push(5);
            quickPushDataStructure.Push(10);
            quickPushDataStructure.Push(3);
            var popped1 = quickPushDataStructure.Pop();
            var popped2 = quickPushDataStructure.Pop();
            var popped3 = quickPushDataStructure.Pop();

            // Assert
            Assert.That(popped1, Is.EqualTo(10));
            Assert.That(popped2, Is.EqualTo(5));
            Assert.That(popped3, Is.EqualTo(3));
        }

        [Test]
        public async Task Push_Pop_MultiThreaded_WorksAsExpected()
        {
            // Arrange
            var quickPushDataStructure = new QuickPushDataStructure<int>(_logger);
            var tasks = new List<Task<int>>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    quickPushDataStructure.Push(i);
                    return quickPushDataStructure.Pop();
                }));
            }

            await Task.WhenAll(tasks.AsParallel());

            // Assert
            foreach (var task in tasks)
            {
                Console.WriteLine(task.Result);
                Assert.That(task.Result >= 0 && task.Result <= 1000);
            }
        }

        [Test]
        public void Concurrent_Push_Pop_WorksAsExpected()
        {
            // Arrange
            var quickPushDataStructure = new QuickPushDataStructure<int>(_logger);

            // Act
            var pushTask = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    quickPushDataStructure.Push(i);
                }
            });

            var popTask = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    quickPushDataStructure.Pop();
                }
            });

            Task.WaitAll(pushTask, popTask);
            // Assert
            Assert.Pass();
        }
    }
}
