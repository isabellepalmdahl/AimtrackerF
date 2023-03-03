using NUnit.Framework;
using Aimtracker.Repositories;
using System;
using Aimtracker.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Aimtracker.Models;

namespace AimtrackerTDD
{
    public class FiltreringDatumTest
    {

        private readonly SessionViewModel _sessionViewModel;
        private static List<TrainingSession> shootings1 = new();
        private static List<TrainingSession> shootings2 = new();
        private static List<TrainingSession> shootings3 = new();
        private static List<TrainingSession> shootings4 = new();
        private static List<TrainingSession> shootings5 = new();
        public FiltreringDatumTest()
        {
            _sessionViewModel = new();
        }
        [SetUp]
        public void Setup()
        {
            #region Fill shootings lists
            //shooting1
            for (int i = 0; i < 10; i++)
            {
                shootings1.Add(new TrainingSession
                {
                    HitStatistic = 0
                });
            }
            //shooting2
            for (int i = 1; i <= 10; i++)
            {
                shootings2.Add(new TrainingSession
                {
                    HitStatistic = i * 10
                });
            }
            //shooting3
            for (int i = 1; i <= 10; i++)
            {
                shootings3.Add(new TrainingSession
                {
                    HitStatistic = 60
                });
            }
            //shooting5
            for (int i = 1; i <= 10; i++)
            {
                shootings5.Add(new TrainingSession
                {
                    HitStatistic = i * 10 / 2
                }) ;
            }
            #endregion
        }
        public static IEnumerable<TestCaseData> shootings
        {
            get {
                yield return new TestCaseData(shootings1).Returns(0);
                yield return new TestCaseData(shootings2).Returns(55);
                yield return new TestCaseData(shootings3).Returns(60);
                yield return new TestCaseData(shootings4).Returns(0);
                yield return new TestCaseData(shootings5).Returns(27.5);
            }
        }
        public static IEnumerable<TestCaseData> shootingsPercent
        {
            get
            {
                yield return new TestCaseData(shootings2,shootings3).Returns(-8.3);
                yield return new TestCaseData(shootings3, shootings2).Returns(9.1);
            }
        }
        [TestCaseSource("shootings")]
        public double TestCalcHitStatistic(List<TrainingSession> shootings)
        {
            return _sessionViewModel.CalcHitStatistic(shootings);
        }
        [TestCaseSource("shootingsPercent")]
        public double TestCalcPercent(List<TrainingSession> share, List<TrainingSession> whole)
        {
            return _sessionViewModel.CalcPercent(_sessionViewModel.CalcHitStatistic(share), _sessionViewModel.CalcHitStatistic(whole));
        }
    }
}