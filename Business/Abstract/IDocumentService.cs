using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Results;

namespace Business.Abstract
{
    public interface IDocumentService
    {
        Task<Result> InsertDocuments(int userId, List<string> docPaths);

    }
}
