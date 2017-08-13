﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKM.V3.Models;
using SKM.V3;
using System.Collections.Generic;
using Cryptolens.SKM.Auth;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Cryptolens.SKM.PaymentForms;
using Cryptolens.SKM.Models;

namespace SKM_Test.Experimental
{
    [TestClass]
    public class PaymentFormTests
    {
        [TestMethod]
        public void PaymentFormCreateSessionTest()
        {
            var result = PaymentForm.CreateSession(new CreateSessionModel { Expires = 80000, PaymentFormId = 3, Price = 100, Currency = "SEK", Heading = "Artem", ProductName = "product name" },
                "WyI0NzgiLCJtSEdCQUtqTDhIWjdZVGNlcHN4OUlaRDRmaG40QzQvNDM3MGorUGpYIl0=");


        }


    }


}
