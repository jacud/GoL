using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Gol.Core.Controls.Models;
using GoL.Model;

namespace GoL.Tests
{
    [TestClass]
    [Story(
        AsA = "Как разработчик игры Жизнь",
        IWant = "я хочу проверить выполнение некоторых правил",
        SoThat = "это позволит не сломать функционал при рефакторинге,....")]
    public class GoLTest
    {

        /// <summary>
        /// Ширина вселенной.
        /// </summary>
        private const int DefaultFieldWidth = 3;

        /// <summary>
        /// Высота вселенной.
        /// </summary>
        private const int DefaultFieldHeight = 3;

        private ClassicGameOfLife game;

        [TestMethod]
        public void TestDestinyOfLonolyLife()
        {
            this.When(_ => _.NewUniverseWithOnlyOneLifeCreatedAndAppearedNewGeeneration())
                .Then(_ => _.NumberOfLifesShouldBeEqual(0))
                .BDDfy();
        }

        private void NewUniverseWithOnlyOneLifeCreatedAndAppearedNewGeeneration()
        {
            var universeGrid = new bool[DefaultFieldWidth, DefaultFieldHeight]
            {
                {false, false, false},
                {false, true, false},
                {false, false, false}
            };
            var grid = new SingleGenerationGrid<bool>(universeGrid, Guid.NewGuid());
            this.game = new ClassicGameOfLife(grid);
            this.game.RewindGenerationForward();
        }

        private void NumberOfLifesShouldBeEqual(int n)
        {
            int numberOfLifes = 0;
            for (var i = 0; i < DefaultFieldWidth; i++)
            {
                for (var j = 0; j < DefaultFieldHeight; j++)
                {
                    if (this.game.CurrentGeneration[i, j] == true)
                    {
                        numberOfLifes++;
                    }
                }
            }

            Assert.AreEqual(n, numberOfLifes);
        }

        [TestMethod]
        public void TestCreationOfLife()
        {
            this.When(_ => _.NewUniverseWithThreeLifeCreatedAndAppearedNewGeeneration())
                .Then(_ => _.NumberOfLifesShouldBeEqual(4))
                .BDDfy();
        }

        private void NewUniverseWithThreeLifeCreatedAndAppearedNewGeeneration()
        {
            var universeGrid = new bool[DefaultFieldWidth, DefaultFieldHeight]
            {
                {true, true, false},
                {false, true, false},
                {false, false, false}
            };
            var grid = new SingleGenerationGrid<bool>(universeGrid, Guid.NewGuid());
            this.game = new ClassicGameOfLife(grid);
            this.game.RewindGenerationForward();
        }

        [TestMethod]
        public void TestLifeAndDeath()
        {
            this.When(_ => _.NewUniverseWithThreeLifeInLineCreatedAndAppearedNewGeeneration())
                .Then(_ => _.NumberOfLifesShouldBeEqual(3))
                .BDDfy();
        }

        private void NewUniverseWithThreeLifeInLineCreatedAndAppearedNewGeeneration()
        {
            var universeGrid = new bool[DefaultFieldWidth, DefaultFieldHeight]
            {
                {false, false, false},
                {true, true, true},
                {false, false, false}
            };
            var grid = new SingleGenerationGrid<bool>(universeGrid, Guid.NewGuid());
            this.game = new ClassicGameOfLife(grid);
            this.game.RewindGenerationForward();
        }

        [TestMethod]
        public void TestDeathWhenAlotOfLife()
        {
            this.When(_ => _.NewUniverseWithThreeLifeInLineCreatedAndAppearedNewGeeneration())
                .Then(_ => _.NumberOfLifesShouldBeEqual(3))
                .BDDfy();
        }

        private void NewUniverseWithTooManyLifeCreatedAndAppearedNewGeeneration()
        {
            var universeGrid = new bool[DefaultFieldWidth, DefaultFieldHeight]
            {
                {true, true, false},
                {true, true, false},
                {false, false, false}
            };
            var grid = new SingleGenerationGrid<bool>(universeGrid, Guid.NewGuid());
            this.game = new ClassicGameOfLife(grid);
            this.game.RewindGenerationForward();
        }
    }
}
