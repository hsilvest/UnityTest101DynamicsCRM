using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;

namespace Plugin.Account.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidPluginExecutionException))]
        public void ContactPreCreatePlugin_ContactWithoutAccount_ShouldThrowAnException()
        {
            var fakedContext = new XrmFakedContext();

            var target = new Entity("contact") { Id = Guid.NewGuid() };

            //Execute our plugin against a target that doesn't contains the account
            var fakedPlugin = fakedContext.ExecutePluginWithTarget<PreCreate>(target);

            //Because of the ExpectedException annotation, if this  
            //test doenst thrown an exception the test will fail
        }

        [TestMethod]
        public void ContactPreCreatePlugin_ContactWithAccount_ContactNameShouldBeAConcatenationOfAccountNumberAndContactName()
        {
            var fakedContext = new XrmFakedContext();

            var accountId = Guid.NewGuid();

            fakedContext.Initialize(new Entity("account")
            {
                Id = accountId,
                ["accountnumber"] = "1"
            });

            var target = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["firstname"] = "Henrique",
                ["accountid"] = new EntityReference("account", accountId)
            };

            //Execute our plugin against a target that doesn't contains the account
            var fakedPlugin = fakedContext.ExecutePluginWithTarget<PreCreate>(target);

            //Assert that the target contains a new attribute
            Assert.IsTrue(target.GetAttributeValue<string>("firstname").Equals("1-Henrique"));
        }
    }
}
