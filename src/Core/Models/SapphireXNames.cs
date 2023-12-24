// -----------------------------------------------------------------------
// <copyright file="SapphireXNames.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    using System.Xml.Linq;

    public static class SapphireXNames
    {
        public const string DomainNamespaceName = "urn:vfi-sapphire:np.domain.2001-07-01";
        public static readonly XName DomainNamespace = XNamespace.Xmlns + "domain";

        public static readonly XName Credential = XName.Get("credential", DomainNamespaceName);
        public static readonly XName Plus = XName.Get("PLUs", DomainNamespaceName);
        public static readonly XName PluSelect = XName.Get("PLUSelect", DomainNamespaceName);
        public static readonly XName PosConfig = XName.Get("posConfig", DomainNamespaceName);

        public static readonly XName Plu = XName.Get("PLU", DomainNamespaceName);
        public static readonly XName Flag = XName.Get("flag", DomainNamespaceName);
        public static readonly XName IdCheck = XName.Get("idCheck", DomainNamespaceName);
        public static readonly XName TaxRate = XName.Get("taxRate", DomainNamespaceName);

        public static readonly XName Department = XName.Get("department");
        public static readonly XName AgeValidation = XName.Get("ageValidation");
    }
}
