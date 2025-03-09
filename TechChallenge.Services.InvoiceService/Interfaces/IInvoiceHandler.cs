﻿using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Interfaces;
public interface IInvoiceHandler
{
    long CreateInvoice(Invoice invoice);
    EvaluationResponse EvaluateAsync(long invoiceId);
    Task SaveDocument(long invoiceId, MemoryStream memoryStream);
}