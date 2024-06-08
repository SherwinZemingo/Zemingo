using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zemingo;
using Zemingo_Tests.TestData;

namespace Zemingo_Tests
{
    [TestFixture]
    internal class QuickPopDataStructureTests
    {

        private readonly ILogger _logger;

        public QuickPopDataStructureTests()
        {
            _logger = Substitute.For<ILogger>();
        }

        [Test]
        public void SimpleDataType_Pop_Returns_MaximumValueElement()
        {
            // Arrange
            var QuickPopDataStructure = new QuickPopDataStructure<int>(_logger);

            // Act
            QuickPopDataStructure.Push(5);
            QuickPopDataStructure.Push(10);
            QuickPopDataStructure.Push(3);
            var popped1 = QuickPopDataStructure.Pop();

            // Assert
            Assert.That(popped1, Is.EqualTo(10));
        }

        [Test]
        public void ComplexDataType_Pop_Returns_MaximumValueElement()
        {
            // Arrange
            var QuickPopDataStructure = new QuickPopDataStructure<Person>(_logger);
            var person1 = new Person("Test1", 1, 500);
            var person2 = new Person("Test2", 2, 800);
            var person3 = new Person("Test3", 3, 200);

            // Act
            QuickPopDataStructure.Push(person1);
            QuickPopDataStructure.Push(person2);
            QuickPopDataStructure.Push(person3);

            var popped1 = QuickPopDataStructure.Pop();
            var popped2 = QuickPopDataStructure.Pop();
            var popped3 = QuickPopDataStructure.Pop();

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
            var QuickPopDataStructure = new QuickPopDataStructure<int>(logger);

            // Act
            var result = QuickPopDataStructure.Pop();

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
            logger.Received(1).LogError("The QuickPopDataStructure is empty.");
        }

        [Test]
        public void Push_Pop_SingleThreaded_WorksAsExpected()
        {
            // Arrange
            var QuickPopDataStructure = new QuickPopDataStructure<int>(_logger);

            // Act
            QuickPopDataStructure.Push(5);
            QuickPopDataStructure.Push(10);
            QuickPopDataStructure.Push(3);
            var popped1 = QuickPopDataStructure.Pop();
            var popped2 = QuickPopDataStructure.Pop();
            var popped3 = QuickPopDataStructure.Pop();

            // Assert
            Assert.That(popped1, Is.EqualTo(10));
            Assert.That(popped2, Is.EqualTo(5));
            Assert.That(popped3, Is.EqualTo(3));
        }

        [Test]
        public async Task Push_Pop_MultiThreaded_WorksAsExpected()
        {
            // Arrange
            var QuickPopDataStructure = new QuickPopDataStructure<int>(_logger);
            var tasks = new List<Task<int>>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    QuickPopDataStructure.Push(i);
                    return QuickPopDataStructure.Pop();
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
            var QuickPopDataStructure = new QuickPopDataStructure<int>(_logger);

            // Act
            var pushTask = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    QuickPopDataStructure.Push(i);
                }
            });

            var popTask = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    QuickPopDataStructure.Pop();
                }
            });

            Task.WaitAll(pushTask, popTask);
            // Assert
            Assert.Pass();
        }
    }
}
