// -----------------------------------------------------------------------
// <copyright file="ModelConversionTests.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using VerifoneCommander.PriceBookManager.Core.Models;
    using Xunit;

    public class ModelConversionTests
    {
        [Fact]
        public void ConvertXmlToPlu_AllPropertiesMissing()
        {
            var document = XDocument.Parse(@"
<domain:PLUs page=""1"" ofPages=""1"" xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"" xmlns:vs=""urn:vfi-sapphire:vs.2001-10-01"" xmlns:base=""urn:vfi-sapphire:base.2001-10-01"">
  <domain:PLU>
  </domain:PLU>
</domain:PLUs>");

            var element = document.Descendants(SapphireXNames.Plu).First();
            var plu = ModelConverter.ConvertXmlToPlu(element);
            Assert.NotNull(plu);
            Assert.Equal(0, plu.Ean13);
            Assert.Equal(0, plu.Modifier);
            Assert.Equal(string.Empty, plu.Description);
            Assert.Equal(0, plu.DepartmentId);

            Assert.NotNull(plu.FeeIds);
            Assert.Single(plu.FeeIds);
            Assert.True(plu.FeeIds.Contains(0));

            Assert.Equal(0, plu.ProductCodeId);
            Assert.Equal(0, plu.Price);

            Assert.NotNull(plu.FlagIds);
            Assert.Equal(2, plu.FlagIds.Count);
            Assert.True(plu.FlagIds.Contains(1));
            Assert.True(plu.FlagIds.Contains(5));

            Assert.NotNull(plu.TaxRateIds);
            Assert.Empty(plu.TaxRateIds);

            Assert.NotNull(plu.AgeValidationIds);
            Assert.Empty(plu.AgeValidationIds);

            Assert.Equal(1, plu.SellUnit);
            Assert.Equal(0, plu.TaxableRebateAmount);
            Assert.Equal(0, plu.MaxQuantityPerTransaction);
        }

        [Fact]
        public void ConvertXmlToPlu_SomePropertiesMissing()
        {
            var document = XDocument.Parse(@"
<domain:PLUs page=""1"" ofPages=""1"" xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"" xmlns:vs=""urn:vfi-sapphire:vs.2001-10-01"" xmlns:base=""urn:vfi-sapphire:base.2001-10-01"">
  <domain:PLU>
    <upc>00000000985239</upc>
    <upcModifier>000</upcModifier>
    <description>Kinder Joy</description>
    <department>6</department>
    <fees>
      <fee>0</fee>
    </fees>
    <pcode>400</pcode>
    <price>1.99</price>
    <flags>
      <domain:flag sysid=""1""/>
      <domain:flag sysid=""4""/>
      <domain:flag sysid=""5""/>
    </flags>
    <SellUnit>1.000</SellUnit>
    <taxableRebate>
      <amount>0.00</amount>
    </taxableRebate>
    <maxQtyPerTrans>0.00</maxQtyPerTrans>
  </domain:PLU>
</domain:PLUs>");

            var element = document.Descendants(SapphireXNames.Plu).First();
            var plu = ModelConverter.ConvertXmlToPlu(element);
            Assert.NotNull(plu);
            Assert.Equal(985239, plu.Ean13);
            Assert.Equal(0, plu.Modifier);
            Assert.Equal("Kinder Joy", plu.Description);
            Assert.Equal(6, plu.DepartmentId);

            Assert.NotNull(plu.FeeIds);
            Assert.Single(plu.FeeIds);
            Assert.Equal(0, plu.FeeIds.First());

            Assert.Equal(400, plu.ProductCodeId);
            Assert.Equal(1.99, plu.Price);

            Assert.NotNull(plu.FlagIds);
            Assert.Equal(3, plu.FlagIds.Count);
            Assert.True(plu.FlagIds.Contains(1));
            Assert.True(plu.FlagIds.Contains(4));
            Assert.True(plu.FlagIds.Contains(5));

            Assert.NotNull(plu.TaxRateIds);
            Assert.Empty(plu.TaxRateIds);

            Assert.NotNull(plu.AgeValidationIds);
            Assert.Empty(plu.AgeValidationIds);

            Assert.Equal(1, plu.SellUnit);
            Assert.Equal(0, plu.TaxableRebateAmount);
            Assert.Equal(0, plu.MaxQuantityPerTransaction);
        }

        [Fact]
        public void ConvertXmlToPlu_NoPropertiesMissing()
        {
            var document = XDocument.Parse(@"
<domain:PLUs page=""1"" ofPages=""1"" xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"" xmlns:vs=""urn:vfi-sapphire:vs.2001-10-01"" xmlns:base=""urn:vfi-sapphire:base.2001-10-01"">
  <domain:PLU>
    <upc>00000000985239</upc>
    <upcModifier>000</upcModifier>
    <description>Kinder Joy</description>
    <department>6</department>
    <fees>
      <fee>0</fee>
    </fees>
    <pcode>400</pcode>
    <price>1.99</price>
    <flags>
      <domain:flag sysid=""1""/>
      <domain:flag sysid=""4""/>
      <domain:flag sysid=""5""/>
    </flags>
    <taxRates>
      <domain:taxRate sysid=""1""/>
    </taxRates>
    <idChecks>
      <domain:idCheck sysid=""1""/>
    </idChecks>
    <SellUnit>1.000</SellUnit>
    <taxableRebate>
      <amount>0.00</amount>
    </taxableRebate>
    <maxQtyPerTrans>0.00</maxQtyPerTrans>
  </domain:PLU>
</domain:PLUs>");

            var element = document.Descendants(SapphireXNames.Plu).First();
            var plu = ModelConverter.ConvertXmlToPlu(element);
            Assert.NotNull(plu);
            Assert.Equal(985239, plu.Ean13);
            Assert.Equal(0, plu.Modifier);
            Assert.Equal("Kinder Joy", plu.Description);
            Assert.Equal(6, plu.DepartmentId);

            Assert.NotNull(plu.FeeIds);
            Assert.Single(plu.FeeIds);
            Assert.Equal(0, plu.FeeIds.First());

            Assert.Equal(400, plu.ProductCodeId);
            Assert.Equal(1.99, plu.Price);

            Assert.NotNull(plu.FlagIds);
            Assert.Equal(3, plu.FlagIds.Count);
            Assert.True(plu.FlagIds.Contains(1));
            Assert.True(plu.FlagIds.Contains(4));
            Assert.True(plu.FlagIds.Contains(5));

            Assert.NotNull(plu.TaxRateIds);
            Assert.Single(plu.TaxRateIds);
            Assert.True(plu.TaxRateIds.Contains(1));

            Assert.NotNull(plu.AgeValidationIds);
            Assert.Single(plu.AgeValidationIds);
            Assert.True(plu.AgeValidationIds.Contains(1));

            Assert.Equal(1, plu.SellUnit);
            Assert.Equal(0, plu.TaxableRebateAmount);
            Assert.Equal(0, plu.MaxQuantityPerTransaction);
        }

        [Fact]
        public void ConvertPluToXml()
        {
            var element = new XElement(
                SapphireXNames.Plus,
                new XAttribute(SapphireXNames.DomainNamespace, SapphireXNames.DomainNamespaceName),
                ModelConverter.ConvertPluToXml(new Plu
                {
                    Ean13 = 12345,
                    Modifier = 12,
                    Description = "Kinder Joy",
                    DepartmentId = 7,
                    FeeIds = new HashSet<int> { 1 },
                    ProductCodeId = 400,
                    Price = 1.99,
                    FlagIds = new HashSet<int> { 1, 3, 5, 7 },
                    TaxRateIds = new HashSet<int> { 1, 3 },
                    AgeValidationIds = new HashSet<int> { 1, 5 },
                    SellUnit = 1,
                    TaxableRebateAmount = 2,
                    MaxQuantityPerTransaction = 1,
                }));

            var expectedString = @"
<domain:PLUs xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"">
  <domain:PLU>
    <upc>00000000012345</upc>
    <upcModifier>012</upcModifier>
    <description>Kinder Joy</description>
    <department>7</department>
    <fees>
      <fee>1</fee>
    </fees>
    <pcode>400</pcode>
    <price>1.99</price>
    <flags>
      <domain:flag sysid=""1"" />
      <domain:flag sysid=""3"" />
      <domain:flag sysid=""5"" />
      <domain:flag sysid=""7"" />
    </flags>
    <taxRates>
      <domain:taxRate sysid=""1"" />
      <domain:taxRate sysid=""3"" />
    </taxRates>
    <idChecks>
      <domain:idCheck sysid=""1"" />
      <domain:idCheck sysid=""5"" />
    </idChecks>
    <SellUnit>1.00</SellUnit>
    <taxableRebate>
      <amount>2.00</amount>
    </taxableRebate>
    <maxQtyPerTrans>1.00</maxQtyPerTrans>
  </domain:PLU>
</domain:PLUs>";

            Assert.Equal(expectedString.Trim(), element.ToString());
        }

        [Fact]
        public void ConvertXmlToDepartment_AllPropertiesMissing()
        {
            var document = XDocument.Parse(@"
<domain:posConfig xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"">
  <departments>
    <department>
    </department>
  </departments>
</domain:posConfig>");

            var element = document.Descendants(SapphireXNames.Department).First();
            var department = ModelConverter.ConvertXmlToDepartment(element);
            Assert.NotNull(department);
            Assert.Equal(0, department.SystemId);
            Assert.Equal(string.Empty, department.Name);
            Assert.False(department.AllowFoodStamps);
            Assert.Equal(0, department.ProductCodeId);

            Assert.NotNull(department.TaxRateIds);
            Assert.Empty(department.TaxRateIds);

            Assert.NotNull(department.AgeValidationIds);
            Assert.Empty(department.AgeValidationIds);
        }

        [Fact]
        public void ConvertXmlToDepartment_NoPropertiesMissing()
        {
            var document = XDocument.Parse(@"
<domain:posConfig xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"">
  <departments>
    <department sysid=""4"" name=""ALCOHOL"" minAmt=""0.00"" maxAmt=""0.00"" isAllowFS=""1"" isNegative=""0"" isFuel=""0"" isAllowFQ=""0"" isAllowSD=""0"" prohibitDisc=""0"" isBL1=""0"" isBL2=""0"" isMoneyOrder=""0"" isSNPromptReqd=""0"">
      <category sysid=""0""/>
      <prodCode sysid=""400""/>
      <fees/>
      <taxes>
        <tax sysid=""1""/>
      </taxes>
      <ageValidns>
        <ageValidn sysid=""1""/>
      </ageValidns>
      <blueLaws/>
      <maxQtyPerTrans>0.00</maxQtyPerTrans>
    </department>
  </departments>
</domain:posConfig>");

            var element = document.Descendants(SapphireXNames.Department).First();
            var department = ModelConverter.ConvertXmlToDepartment(element);
            Assert.NotNull(department);
            Assert.Equal(4, department.SystemId);
            Assert.Equal("ALCOHOL", department.Name);
            Assert.True(department.AllowFoodStamps);
            Assert.Equal(400, department.ProductCodeId);

            Assert.NotNull(department.TaxRateIds);
            Assert.Single(department.TaxRateIds);
            Assert.True(department.TaxRateIds.Contains(1));

            Assert.NotNull(department.AgeValidationIds);
            Assert.Single(department.AgeValidationIds);
            Assert.True(department.AgeValidationIds.Contains(1));
        }
    }
}
