﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoT_Deathbet;
using GoT_Deathbet.Controllers;

namespace GoT_Deathbet.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
