﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/YEFIErrorStructure/xml/schemas/version2.0")]
public partial class CommonErrorStructure
{
    
    private string raisedByField;
    
    private string codeField;
    
    private string errorTextField;
    
    private string[] insertField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public string RaisedBy
    {
        get
        {
            return this.raisedByField;
        }
        set
        {
            this.raisedByField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public string ErrorText
    {
        get
        {
            return this.errorTextField;
        }
        set
        {
            this.errorTextField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Insert", Order=3)]
    public string[] Insert
    {
        get
        {
            return this.insertField;
        }
        set
        {
            this.insertField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/PersonProfileCommonTypes/xml/schemas/version2.0")]
public partial class PersonNameBodyStructure
{
    
    private string[] titleField;
    
    private string[] prefixField;
    
    private string firstNameField;
    
    private string secondNameField;
    
    private string thirdNameField;
    
    private string fourthNameField;
    
    private string lastNameField;
    
    private string[] suffixField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Title", Order=0)]
    public string[] Title
    {
        get
        {
            return this.titleField;
        }
        set
        {
            this.titleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Prefix", Order=1)]
    public string[] Prefix
    {
        get
        {
            return this.prefixField;
        }
        set
        {
            this.prefixField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public string SecondName
    {
        get
        {
            return this.secondNameField;
        }
        set
        {
            this.secondNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    public string ThirdName
    {
        get
        {
            return this.thirdNameField;
        }
        set
        {
            this.thirdNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=5)]
    public string FourthName
    {
        get
        {
            return this.fourthNameField;
        }
        set
        {
            this.fourthNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=6)]
    public string LastName
    {
        get
        {
            return this.lastNameField;
        }
        set
        {
            this.lastNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Suffix", Order=7)]
    public string[] Suffix
    {
        get
        {
            return this.suffixField;
        }
        set
        {
            this.suffixField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/PersonProfileCommonTypes/xml/schemas/version2.0")]
public partial class PersonNameDetailsStructure
{
    
    private object itemField;
    
    private LanguageType languageField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PersonFullName", typeof(string), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("PersonNameBody", typeof(PersonNameBodyStructure), Order=0)]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public LanguageType language
    {
        get
        {
            return this.languageField;
        }
        set
        {
            this.languageField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/CommonTypes/xml/schemas/version2.0")]
public enum LanguageType
{
    
    /// <remarks/>
    EN,
    
    /// <remarks/>
    AR,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/PersonProfileCommonTypes/xml/schemas/version2.0")]
public partial class NationalIdentifierSummaryStructure
{
    
    private string nationalIDField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public string NationalID
    {
        get
        {
            return this.nationalIDField;
        }
        set
        {
            this.nationalIDField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/GosiEmploymentStatus/xml/schemas/version2.0")]
public partial class ContributorStructure
{
    
    private NationalIdentifierSummaryStructure contributorIDField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public NationalIdentifierSummaryStructure ContributorID
    {
        get
        {
            return this.contributorIDField;
        }
        set
        {
            this.contributorIDField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/GosiEmploymentStatus/xml/schemas/version2.0")]
public partial class EmploymentStatusStructure
{
    
    private ContributorStructure contributorField;
    
    private PersonNameDetailsStructure contributorNameField;
    
    private int contributorStatusField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public ContributorStructure Contributor
    {
        get
        {
            return this.contributorField;
        }
        set
        {
            this.contributorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public PersonNameDetailsStructure ContributorName
    {
        get
        {
            return this.contributorNameField;
        }
        set
        {
            this.contributorNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public int ContributorStatus
    {
        get
        {
            return this.contributorStatusField;
        }
        set
        {
            this.contributorStatusField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yesser.gov.sa/SocialInsurance/EmploymentStatus/EmploymentStatusService/ver" +
    "sion/2.0")]
public partial class getEmploymentStatusResponseObject
{
    
    private object itemField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("commonErrorElement", typeof(CommonErrorStructure), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("getEmploymentStatusResponse", typeof(EmploymentStatusStructure), Order=0)]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IGOSIEmploymentStatusService")]
public interface IGOSIEmploymentStatusService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGOSIEmploymentStatusService/GetEmploymentStatus", ReplyAction="http://tempuri.org/IGOSIEmploymentStatusService/GetEmploymentStatusResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(CommonErrorStructure), Action="http://yesser.gov.sa/SocialInsurance/EmploymentStatus/EmploymentStatusService/ver" +
        "sion/2.0/getEmploymentStatus", Name="commonErrorElement")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<getEmploymentStatusResponseObject> GetEmploymentStatusAsync(string NationalID);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGOSIEmploymentStatusService/GetEmploymentStatusMultiple", ReplyAction="http://tempuri.org/IGOSIEmploymentStatusService/GetEmploymentStatusMultipleRespon" +
        "se")]
    [System.ServiceModel.FaultContractAttribute(typeof(CommonErrorStructure), Action="http://yesser.gov.sa/SocialInsurance/EmploymentStatus/EmploymentStatusService/ver" +
        "sion/2.0/getEmploymentStatusMultiple", Name="commonErrorElement")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<getEmploymentStatusMultipleResponseObject> GetEmploymentStatusMultipleAsync(string[] NationalIDs);
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yesser.gov.sa/SocialInsurance/EmploymentStatus/EmploymentStatusService/ver" +
    "sion/2.0")]
public partial class getEmploymentStatusMultipleResponseObject
{
    
    private object itemField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("commonErrorElement", typeof(CommonErrorStructure), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("getEmploymentStatusMultipleResponse", typeof(MultipleEmploymentStatusStructure), Order=0)]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://yefi.gov.sa/GosiEmploymentStatus/xml/schemas/version2.0")]
public partial class MultipleEmploymentStatusStructure
{
    
    private EmploymentStatusStructure[] employmentStatusStructureField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("EmploymentStatusStructure", Order=0)]
    public EmploymentStatusStructure[] EmploymentStatusStructure
    {
        get
        {
            return this.employmentStatusStructureField;
        }
        set
        {
            this.employmentStatusStructureField = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface IGOSIEmploymentStatusServiceChannel : IGOSIEmploymentStatusService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class GOSIEmploymentStatusServiceClient : System.ServiceModel.ClientBase<IGOSIEmploymentStatusService>, IGOSIEmploymentStatusService
{
    
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
    
    public GOSIEmploymentStatusServiceClient() : 
            base(GOSIEmploymentStatusServiceClient.GetDefaultBinding(), GOSIEmploymentStatusServiceClient.GetDefaultEndpointAddress())
    {
        this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IGOSIEmploymentStatusService.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public GOSIEmploymentStatusServiceClient(EndpointConfiguration endpointConfiguration) : 
            base(GOSIEmploymentStatusServiceClient.GetBindingForEndpoint(endpointConfiguration), GOSIEmploymentStatusServiceClient.GetEndpointAddress(endpointConfiguration))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public GOSIEmploymentStatusServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
            base(GOSIEmploymentStatusServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public GOSIEmploymentStatusServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(GOSIEmploymentStatusServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public GOSIEmploymentStatusServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public System.Threading.Tasks.Task<getEmploymentStatusResponseObject> GetEmploymentStatusAsync(string NationalID)
    {
        return base.Channel.GetEmploymentStatusAsync(NationalID);
    }
    
    public System.Threading.Tasks.Task<getEmploymentStatusMultipleResponseObject> GetEmploymentStatusMultipleAsync(string[] NationalIDs)
    {
        return base.Channel.GetEmploymentStatusMultipleAsync(NationalIDs);
    }
    
    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }
    
    private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IGOSIEmploymentStatusService))
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IGOSIEmploymentStatusService))
        {
            return new System.ServiceModel.EndpointAddress("http://localhost/GSBExpress/SocialInsurance/GOSIEmploymentStatus/2.0/EmploymentSt" +
                    "atusService.svc");
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    private static System.ServiceModel.Channels.Binding GetDefaultBinding()
    {
        return GOSIEmploymentStatusServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IGOSIEmploymentStatusService);
    }
    
    private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
    {
        return GOSIEmploymentStatusServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IGOSIEmploymentStatusService);
    }
    
    public enum EndpointConfiguration
    {
        
        BasicHttpBinding_IGOSIEmploymentStatusService,
    }
}
