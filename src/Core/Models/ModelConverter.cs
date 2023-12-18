// -----------------------------------------------------------------------
// <copyright file="ModelConverter.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public static class ModelConverter
    {
        public static Plu ConvertXmlToPlu(XElement element)
        {
            _ = element ?? throw new ArgumentNullException(nameof(element));
            EnsureEqualXName(element, SapphireXNames.Plu);

            var plu = new Plu();
            plu.Ean13 = ParseAsLong(GetElement(element, "upc")?.Value, 0);
            plu.Modifier = ParseAsInt(GetElement(element, "upcModifier")?.Value, 0);
            plu.Description = GetElement(element, "description")?.Value ?? string.Empty;
            plu.DepartmentId = ParseAsInt(GetElement(element, "department")?.Value, 0);
            plu.ProductCodeId = ParseAsInt(GetElement(element, "pcode")?.Value, 0);
            plu.Price = ParseAsDouble(GetElement(element, "price")?.Value, 0);
            plu.SellUnit = ParseAsDouble(GetElement(element, "SellUnit")?.Value, 1);
            plu.MaxQuantityPerTransaction = ParseAsDouble(GetElement(element, "maxQtyPerTrans")?.Value, 0);
            plu.TaxableRebateAmount = ParseAsDouble(GetElement(element, "taxableRebate", "amount")?.Value, 0);

            var feesElement = GetElement(element, "fees");
            if (feesElement != null)
            {
                plu.FeeIds = feesElement.Elements("fee")
                    .Where(x => int.TryParse(x.Value, out int _))
                    .Select(x => int.Parse(x.Value))
                    .ToHashSet();
            }

            var flagsElement = GetElement(element, "flags");
            if (flagsElement != null)
            {
                plu.FlagIds = flagsElement.Elements(SapphireXNames.Flag)
                    .Where(x => int.TryParse(x.Attribute("sysid")?.Value, out int _))
                    .Select(x => int.Parse(x.Attribute("sysid").Value))
                    .ToHashSet();
            }

            var taxRatesElement = GetElement(element, "taxRates");
            if (taxRatesElement != null)
            {
                plu.TaxRateIds = taxRatesElement.Elements(SapphireXNames.TaxRate)
                    .Where(x => int.TryParse(x.Attribute("sysid")?.Value, out int _))
                    .Select(x => int.Parse(x.Attribute("sysid").Value))
                    .ToHashSet();
            }

            var idChecksElement = GetElement(element, "idChecks");
            if (idChecksElement != null)
            {
                plu.AgeValidationIds = idChecksElement.Elements(SapphireXNames.IdCheck)
                    .Where(x => int.TryParse(x.Attribute("sysid")?.Value, out int _))
                    .Select(x => int.Parse(x.Attribute("sysid").Value))
                    .ToHashSet();
            }

            return plu;
        }

        public static XElement ConvertPluToXml(Plu plu)
        {
            _ = plu ?? throw new ArgumentNullException(nameof(plu));

            return new XElement(
                SapphireXNames.Plu,
                new XElement(
                    "upc",
                    plu.Ean13.ToString("D14")),
                new XElement(
                    "upcModifier",
                    plu.Modifier.ToString("D3")),
                new XElement(
                    "description",
                    plu.Description),
                new XElement(
                    "department",
                    plu.DepartmentId),
                new XElement(
                    "fees",
                    plu.FeeIds.Select(x => new XElement("fee", x))),
                new XElement(
                    "pcode",
                    plu.ProductCodeId),
                new XElement(
                    "price",
                    plu.Price.ToString("F2")),
                new XElement(
                    "flags",
                    plu.FlagIds.Select(x => new XElement(SapphireXNames.Flag, new XAttribute("sysid", x)))),
                new XElement(
                    "taxRates",
                    plu.TaxRateIds.Select(x => new XElement(SapphireXNames.TaxRate, new XAttribute("sysid", x)))),
                new XElement(
                    "idChecks",
                    plu.AgeValidationIds.Select(x => new XElement(SapphireXNames.IdCheck, new XAttribute("sysid", x)))),
                new XElement(
                    "SellUnit",
                    plu.SellUnit.ToString("F2")),
                new XElement(
                    "taxableRebate",
                    new XElement(
                        "amount",
                        plu.TaxableRebateAmount.ToString("F2"))),
                new XElement(
                    "maxQtyPerTrans",
                    plu.MaxQuantityPerTransaction.ToString("F2")));
        }

        public static Department ConvertXmlToDepartment(XElement element)
        {
            _ = element ?? throw new ArgumentNullException(nameof(element));
            EnsureEqualXName(element, SapphireXNames.Department);

            var department = new Department();
            department.SystemId = ParseAsInt(element.Attribute("sysid")?.Value, 0);
            department.Name = element.Attribute("name")?.Value ?? string.Empty;
            department.AllowFoodStamps = ParseAsBool(element.Attribute("isAllowFS")?.Value, false);
            department.ProductCodeId = ParseAsInt(GetElement(element, "prodCode")?.Attribute("sysid")?.Value, 0);

            var taxesElement = GetElement(element, "taxes");
            if (taxesElement != null)
            {
                department.TaxRateIds = taxesElement.Elements("tax")
                    .Where(x => int.TryParse(x.Attribute("sysid")?.Value, out int _))
                    .Select(x => int.Parse(x.Attribute("sysid").Value))
                    .ToHashSet();
            }

            var ageValidnsElement = GetElement(element, "ageValidns");
            if (ageValidnsElement != null)
            {
                department.AgeValidationIds = ageValidnsElement.Elements("ageValidn")
                    .Where(x => int.TryParse(x.Attribute("sysid")?.Value, out int _))
                    .Select(x => int.Parse(x.Attribute("sysid").Value))
                    .ToHashSet();
            }

            return department;
        }

        public static TaxRate ConvertXmlToTaxRate(XElement element)
        {
            _ = element ?? throw new ArgumentNullException(nameof(element));
            EnsureEqualXName(element, "taxRate");

            var taxRate = new TaxRate();
            taxRate.SystemId = ParseAsInt(element.Attribute("sysid")?.Value, 0);
            taxRate.Name = element.Attribute("name")?.Value ?? string.Empty;
            taxRate.Rate = ParseAsDouble(GetElement(element, "taxProperties")?.Attribute("rate")?.Value, 0);

            return taxRate;
        }

        public static AgeValidation ConvertXmlToAgeValidation(XElement element)
        {
            _ = element ?? throw new ArgumentNullException(nameof(element));
            EnsureEqualXName(element, SapphireXNames.AgeValidation);

            var ageValidation = new AgeValidation();
            ageValidation.SystemId = ParseAsInt(element.Attribute("sysid")?.Value, 0);
            ageValidation.Name = element.Attribute("name")?.Value ?? string.Empty;

            return ageValidation;
        }

        private static void EnsureEqualXName(XElement element, XName xName)
        {
            if (!xName.Equals(element.Name))
            {
                throw new ArgumentException($"Element name must be '{xName}'", nameof(element));
            }
        }

        private static XElement GetElement(XElement parentElement, params XName[] childElementXNames)
        {
            _ = parentElement ?? throw new ArgumentNullException(nameof(parentElement));
            _ = childElementXNames ?? throw new ArgumentNullException(nameof(childElementXNames));

            string path = string.Empty;
            XElement resultElement = parentElement;

            foreach (var childElementXName in childElementXNames)
            {
                path += "/" + childElementXName.Namespace;
                resultElement = resultElement.Element(childElementXName);

                if (resultElement == null)
                {
                    return null;
                }
            }

            return resultElement;
        }

        private static long ParseAsLong(string input, long defaultValue)
        {
            if (long.TryParse(input, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        private static int ParseAsInt(string input, int defaultValue)
        {
            if (int.TryParse(input, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        private static double ParseAsDouble(string input, double defaultValue)
        {
            if (double.TryParse(input, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        private static bool ParseAsBool(string input, bool defaultValue)
        {
            if ("0".Equals(input))
            {
                return false;
            }
            else if ("1".Equals(input))
            {
                return true;
            }
            else
            {
                if (bool.TryParse(input, out var result))
                {
                    return result;
                }
            }

            return defaultValue;
        }
    }
}
