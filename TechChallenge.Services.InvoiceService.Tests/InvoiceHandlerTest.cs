using Microsoft.Extensions.Logging;
using TechChallenge.Common.DTO.Contexts;
using TechChallenge.Common.Repositories.Interfaces;
using TechChallenge.Services.InvoiceService.Handler;
using TechChallenge.Services.InvoiceService.Interfaces;
using TechChallenge.Services.InvoiceService.Tests.Factories;
using TechChallenge.Services.InvoiceService.Tests.Mocks;

namespace TechChallenge.Services.InvoiceService.Tests;

public class InvoiceHandlerTest
{
    private InvoiceContext invoiceContext;
    private IInvoiceRepository invoiceRepository;
    private LoggerFactory loggerFactory;
    private IInvoiceHandler invoiceHandler;
    private IFileHandler fileHandler;
    private IRiskLevelServiceClient riskLevelServiceClient;

    [SetUp]
    public void Setup()
    {
        loggerFactory = new LoggerFactory();
        var logger = loggerFactory.CreateLogger<InvoiceHandler>();
        invoiceContext = TestConnectionFactory.CreateInvoiceContext();
        invoiceRepository = RepositoryFactory.CreateInvoiceRepository(invoiceContext);
        
        fileHandler = new FileHandlerMock();
        riskLevelServiceClient = new RiskLevelServiceClientMock();
        invoiceHandler = new InvoiceHandler(logger, fileHandler, riskLevelServiceClient, invoiceRepository);
    }

    [TearDown]
    public void TearDown() 
    {
        invoiceContext.Dispose();
        loggerFactory.Dispose();
    }

    [Test]
    public async Task IllegalUploadAsync()
    {
        using(var memoryStream = new MemoryStream()) 
        {
            try 
            {
                await invoiceHandler.SaveDocument(10000, memoryStream);
                Assert.False(true);
            }
            catch(ArgumentException ex) 
            {
                Assert.IsTrue(true);
            }
        }        
    }

    [Test]
    public async Task IllegalEvalution()
    {
        try 
        {
            await invoiceHandler.Evaluate(1000);
        }
        catch(ArgumentException ex) 
        {
            Assert.IsTrue(true);
        }
    }
}