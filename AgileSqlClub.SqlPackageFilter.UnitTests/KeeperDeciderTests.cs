﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dac.Model;
using Moq;
using NUnit.Framework;
using AgileSqlClub.SqlPackageFilter;

namespace AgileSqlClub.SqlPackageFilter.UnitTests
{
    [TestFixture]
    public class KeeperDeciderTests
    {
 
        [Test]
        public void Create_Step_Does_Not_Call_Keep_Rule()
        {
            var keepRule = new Mock<FilterRule>();
            keepRule.Setup(p => p.Operation()).Returns(FilterOperation.Keep);
            keepRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Callback(() => Assert.Fail("Rule should not have been called"));

            var decider = new KeeperDecider(new List<FilterRule>() { keepRule.Object });
            decider.ShouldRemoveFromPlan(null, StepType.Create);

        }

        [Test]
        public void Drop_Step_Does_Call_Keep_Rule()
        {
            var keepRule = new Mock<FilterRule>();
            keepRule.Setup(p => p.Operation()).Returns(FilterOperation.Keep);
            keepRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Returns(true);

            var decider = new KeeperDecider(new List<FilterRule>() { keepRule.Object });
            var result = decider.ShouldRemoveFromPlan(null, StepType.Drop);

            Assert.IsTrue(result);
        }

        [Test]
        public void Create_Step_Does_Call_Ignore_Rule()
        {
            var ignoreRule = new Mock<FilterRule>();
            ignoreRule.Setup(p => p.Operation()).Returns(FilterOperation.Ignore);
            ignoreRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Returns(true);

            var decider = new KeeperDecider(new List<FilterRule>() { ignoreRule.Object });
            var result = decider.ShouldRemoveFromPlan(null, StepType.Create);

            Assert.IsTrue(result);
        }


        [Test]
        public void Drop_Step_Does_Call_Ignore_Rule()
        {
            var ignoreRule = new Mock<FilterRule>();
            ignoreRule.Setup(p => p.Operation()).Returns(FilterOperation.Ignore);
            ignoreRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Returns(true);

            var decider = new KeeperDecider(new List<FilterRule>() { ignoreRule.Object });
            var result = decider.ShouldRemoveFromPlan(null, StepType.Drop);

            Assert.IsTrue(result);
        }


        [Test]
        public void Alter_Step_Does_Call_Ignore_Rule()
        {
            var ignoreRule = new Mock<FilterRule>();
            ignoreRule.Setup(p => p.Operation()).Returns(FilterOperation.Ignore);
            ignoreRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Returns(true);

            var decider = new KeeperDecider(new List<FilterRule>() { ignoreRule.Object });
            var result = decider.ShouldRemoveFromPlan(null, StepType.Alter);

            Assert.IsTrue(result);
        }

        [Test]
        public void All_Rules_Ignored_For_Other_Steps()
        {
            var ignoreRule = new Mock<FilterRule>();
            ignoreRule.Setup(p => p.Operation()).Returns(FilterOperation.Ignore);
            ignoreRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Callback(() => Assert.Fail("Ignore Rule should not have been called")); ;


            var keepRule = new Mock<FilterRule>();
            keepRule.Setup(p => p.Operation()).Returns(FilterOperation.Keep);
            keepRule.Setup(p => p.Matches(It.IsAny<TSqlObject>())).Callback(() => Assert.Fail("Keep Rule should not have been called"));

            var decider = new KeeperDecider(new List<FilterRule>() { ignoreRule.Object, keepRule.Object});
            var result = decider.ShouldRemoveFromPlan(null, StepType.Other);

            Assert.IsFalse(result);
        }
    }
}
