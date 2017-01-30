using Models;
using System.Collections.Generic;


namespace Services
{
    public interface IDocumentRepository : IEntityRepository<Document>
    {
        IEnumerable<Document> Search(DocumentSearchModel searchModel);
    }
}
